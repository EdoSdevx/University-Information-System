/**
 * AdminPages Module
 * Page templates for admin dashboard with admin-* prefix
 */

import { apiRequest } from '../core/ApiService.js';

let currentEditingUserId = null;
let currentEditingDepartmentId = null;
let currentEditingInstanceId = null;
let allDepartments = [];
let allTeachers = [];
let allCourses = [];

export const AdminPages = {
    dashboard: {
        render: () => `
            <div class="admin-breadcrumb">Home / Dashboard</div>
            <div class="admin-banner">
                <div class="admin-banner-title">System Dashboard</div>
                <div class="admin-banner-text">System is running normally. All services operational.</div>
            </div>
            <div class="admin-stats">
                <div class="admin-stat-card">
                    <div class="admin-stat-value">2,547</div>
                    <div class="admin-stat-label">Total Users</div>
                </div>
                <div class="admin-stat-card">
                    <div class="admin-stat-value">156</div>
                    <div class="admin-stat-label">Active Courses</div>
                </div>
                <div class="admin-stat-card">
                    <div class="admin-stat-value">98.5%</div>
                    <div class="admin-stat-label">System Uptime</div>
                </div>
                <div class="admin-stat-card">
                    <div class="admin-stat-value">42</div>
                    <div class="admin-stat-label">Pending Requests</div>
                </div>
            </div>
        `,
        afterRender: () => { }
    },

    users: {
        render: () => `
    <div class="admin-breadcrumb">Home / User Management</div>
    <div class="admin-section-header">User Management</div>
    
    <div class="admin-users-container">
        <div class="admin-users-header">
            <div class="admin-users-filters">
                <select id="roleFilter" class="admin-filter-select">
                    <option value="">All Roles</option>
                    <option value="Student">Students</option>
                    <option value="Teacher">Teachers</option>
                    <option value="Admin">Admins</option>
                </select>
                 <select id="departmentFilter" class="admin-filter-select">
                     <option value="">All Departments</option>
                </select>
                <input type="text" id="searchUsers" class="admin-search-input" placeholder="Search by name or email...">
            </div>
            <button class="admin-btn-primary" onclick="window.openCreateUserModal()">+ Create User</button>
        </div>

        <div class="admin-table-wrapper">
            <table class="admin-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Role</th>
                        <th>Department</th>
                        <th>Created</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody id="usersTable">
                    <tr><td colspan="7" style="text-align: center; padding: 20px;">Loading...</td></tr>
                </tbody>
            </table>
        </div>
    </div>
    
    <div class="admin-modal" id="userModal" style="display: none;">
        <div class="admin-modal-content">
            <div class="admin-modal-header">
                <h3 id="userModalTitle">Create User</h3>
                <button class="admin-modal-close" onclick="document.getElementById('userModal').style.display='none'">×</button>
            </div>
            <div class="admin-modal-body">
                <div class="admin-form-group">
                    <label>Email *</label>
                    <input type="email" id="userEmail" class="admin-input" placeholder="user@university.edu.tr">
                </div>
                <div class="admin-form-row">
                    <div class="admin-form-group">
                        <label>First Name *</label>
                        <input type="text" id="userFirstName" class="admin-input" placeholder="First name">
                    </div>
                    <div class="admin-form-group">
                        <label>Last Name *</label>
                        <input type="text" id="userLastName" class="admin-input" placeholder="Last name">
                    </div>
                </div>
                <div class="admin-form-row">
                    <div class="admin-form-group">
                        <label>Role *</label>
                        <select id="userRole" class="admin-input">
                            <option value="Student">Student</option>
                            <option value="Teacher">Teacher</option>
                            <option value="Admin">Admin</option>
                        </select>
                    </div>
                    <div class="admin-form-group">
                        <label>Department</label>
                        <select id="userDepartment" class="admin-input">
                            <option value="">Select department...</option>
                        </select>
                    </div>
                </div>
                <div class="admin-form-group" id="passwordGroup">
                    <label>Password *</label>
                    <input type="password" id="userPassword" class="admin-input" placeholder="Minimum 8 characters">
                </div>
            </div>
            <div class="admin-modal-footer">
                <button class="admin-btn-cancel" onclick="document.getElementById('userModal').style.display='none'">Cancel</button>
                <button class="admin-btn-primary" onclick="window.saveUser()">Save User</button>
            </div>
        </div>
    </div>
`,
        afterRender: async () => {
            await loadDepartmentsForDropdown();
            await loadUsers();

            document.getElementById('roleFilter').addEventListener('change', loadUsers);
            document.getElementById('departmentFilter').addEventListener('change', loadUsers);
            document.getElementById('searchUsers').addEventListener('input', debounce(loadUsers, 500));
        }
    },

    departments: {
        render: () => `
    <div class="admin-breadcrumb">Home / Department Management</div>
    <div class="admin-section-header">Department Management</div>
    
    <div class="admin-departments-container">
        <div class="admin-departments-header">
            <div class="admin-departments-info">
                <p id="departmentCount">Loading departments...</p>
            </div>
            <button class="admin-btn-primary" onclick="window.openCreateDepartmentModal()">+ Create Department</button>
        </div>

        <div class="admin-departments-grid" id="departmentsGrid">
            <div style="text-align: center; padding: 40px; grid-column: 1/-1;">Loading departments...</div>
        </div>
    </div>
    
    <div class="admin-modal" id="departmentModal" style="display: none;">
        <div class="admin-modal-content">
            <div class="admin-modal-header">
                <h3 id="departmentModalTitle">Create Department</h3>
                <button class="admin-modal-close" onclick="document.getElementById('departmentModal').style.display='none'">×</button>
            </div>
            <div class="admin-modal-body">
                <div class="admin-form-group">
                    <label>Department Name *</label>
                    <input type="text" id="deptName" class="admin-input" placeholder="Computer Engineering">
                </div>
                <div class="admin-form-group">
                    <label>Department Code *</label>
                    <input type="text" id="deptCode" class="admin-input" placeholder="CE">
                </div>
                <div class="admin-form-group">
                    <label>Department Email</label>
                    <input type="email" id="deptEmail" class="admin-input" placeholder="ce@university.edu.tr">
                </div>
                <div class="admin-form-group">
                    <label>Secretary Email</label>
                    <input type="email" id="deptSecretaryEmail" class="admin-input" placeholder="secretary.ce@university.edu.tr">
                </div>
                <div class="admin-form-row">
                    <div class="admin-form-group">
                        <label>Department Head Name</label>
                        <input type="text" id="deptHeadName" class="admin-input" placeholder="Prof. Dr. John Doe">
                    </div>
                    <div class="admin-form-group">
                        <label>Department Head Email</label>
                        <input type="email" id="deptHeadEmail" class="admin-input" placeholder="head.ce@university.edu.tr">
                    </div>
                </div>
            </div>
            <div class="admin-modal-footer">
                <button class="admin-btn-cancel" onclick="document.getElementById('departmentModal').style.display='none'">Cancel</button>
                <button class="admin-btn-primary" onclick="window.saveDepartment()">Save Department</button>
            </div>
        </div>
    </div>
`,
        afterRender: async () => {
            await loadDepartments();
        }
    },

    instances: {
        render: () => `
<div class="admin-breadcrumb">Home / Course Instance Management</div>
<div class="admin-section-header">Course Instances</div>

<div class="admin-instances-container">
    <div class="admin-instances-header">
        <div class="admin-instances-filters">
            <select id="instanceDepartmentFilter" class="admin-filter-select">
                <option value="">All Departments</option>
            </select>
            <input type="text" id="searchInstances" class="admin-search-input" placeholder="Search by course code or name...">
        </div>
        <button class="admin-btn-primary" onclick="window.openCreateInstanceModal()">+ Create Instance</button>
    </div>

    <div class="admin-table-wrapper">
        <table class="admin-table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Course Code</th>
                    <th>Course Name</th>
                    <th>Section</th>
                    <th>Instructor</th>
                    <th>Schedule</th>
                    <th>Enrolled</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody id="instancesTable">
                <tr><td colspan="8" style="text-align: center; padding: 20px;">Loading...</td></tr>
            </tbody>
        </table>
    </div>
</div>

<div class="admin-modal" id="instanceModal" style="display: none;">
    <div class="admin-modal-content">
        <div class="admin-modal-header">
            <h3 id="instanceModalTitle">Create Course Instance</h3>
            <button class="admin-modal-close" onclick="document.getElementById('instanceModal').style.display='none'">×</button>
        </div>
        <div class="admin-modal-body">
            <div class="admin-form-group">
                <label>Department *</label>
                <select id="instanceDepartment" class="admin-input">
                    <option value="">Select department...</option>
                </select>
            </div>
            <div class="admin-form-group">
                <label>Course *</label>
                <select id="instanceCourse" class="admin-input" disabled>
                    <option value="">Select department first...</option>
                </select>
            </div>
            <div class="admin-form-row">
                <div class="admin-form-group">
                    <label>Section *</label>
                    <input type="text" id="instanceSection" class="admin-input" placeholder="A">
                </div>
                <div class="admin-form-group">
                    <label>Capacity *</label>
                    <input type="number" id="instanceCapacity" class="admin-input" placeholder="40" min="1">
                </div>
            </div>
            <div class="admin-form-group">
                <label>Instructor *</label>
                <select id="instanceTeacher" class="admin-input">
                    <option value="">Select instructor...</option>
                </select>
            </div>
            <div class="admin-form-row">
                <div class="admin-form-group">
                    <label>Day 1 *</label>
                    <select id="instanceDay1" class="admin-input">
                        <option value="">Select day...</option>
                        <option value="0">Sunday</option>
                        <option value="1">Monday</option>
                        <option value="2">Tuesday</option>
                        <option value="3">Wednesday</option>
                        <option value="4">Thursday</option>
                        <option value="5">Friday</option>
                        <option value="6">Saturday</option>
                    </select>
                </div>
                <div class="admin-form-group">
                    <label>Day 2 (Optional)</label>
                    <select id="instanceDay2" class="admin-input">
                        <option value="">None</option>
                        <option value="0">Sunday</option>
                        <option value="1">Monday</option>
                        <option value="2">Tuesday</option>
                        <option value="3">Wednesday</option>
                        <option value="4">Thursday</option>
                        <option value="5">Friday</option>
                        <option value="6">Saturday</option>
                    </select>
                </div>
            </div>
            <div class="admin-form-row">
                <div class="admin-form-group">
                    <label>Start Time *</label>
                    <input type="time" id="instanceStartTime" class="admin-input">
                </div>
                <div class="admin-form-group">
                    <label>End Time *</label>
                    <input type="time" id="instanceEndTime" class="admin-input">
                </div>
            </div>
            <div class="admin-form-group">
                <label>Location</label>
                <input type="text" id="instanceLocation" class="admin-input" placeholder="Building A - Room 101">
            </div>
        </div>
        <div class="admin-modal-footer">
            <button class="admin-btn-cancel" onclick="document.getElementById('instanceModal').style.display='none'">Cancel</button>
            <button class="admin-btn-primary" onclick="window.saveInstance()">Save Instance</button>
        </div>
    </div>
</div>
`,
        afterRender: async () => {
            await loadInstanceData();
            await loadInstances();

            document.getElementById('instanceDepartmentFilter').addEventListener('change', loadInstances);
            document.getElementById('searchInstances').addEventListener('input', debounce(loadInstances, 500));
        }
    },

    courses: {
        render: () => `
            <div class="admin-breadcrumb">Home / Course Management</div>
            <div class="admin-section-header">Course Management</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">📚</div>
                <div class="placeholder-title">Course Management</div>
                <div class="placeholder-text">Create, edit, and manage all courses in the system.</div>
            </div>
        `,
        afterRender: () => { }
    },

    reports: {
        render: () => `
            <div class="admin-breadcrumb">Home / Reports</div>
            <div class="admin-section-header">System Reports</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">📊</div>
                <div class="placeholder-title">Reports</div>
                <div class="placeholder-text">View system reports, analytics, and statistics.</div>
            </div>
        `,
        afterRender: () => { }
    },

    profile: {
        render: () => `
            <div class="admin-breadcrumb">Home / My Profile</div>
            <div class="admin-section-header">Personal Information</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">👤</div>
                <div class="placeholder-title">My Profile</div>
                <div class="placeholder-text">View and update your personal information.</div>
            </div>
        `,
        afterRender: () => { }
    },

    settings: {
        render: () => `
            <div class="admin-breadcrumb">Home / System Settings</div>
            <div class="admin-section-header">System Settings</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">⚙️</div>
                <div class="placeholder-title">System Settings</div>
                <div class="placeholder-text">Configure system-wide settings and preferences.</div>
            </div>
        `,
        afterRender: () => { }
    },

    logs: {
        render: () => `
            <div class="admin-breadcrumb">Home / Activity Logs</div>
            <div class="admin-section-header">Activity Logs</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">📋</div>
                <div class="placeholder-title">Activity Logs</div>
                <div class="placeholder-text">View system activity, audit logs, and user actions.</div>
            </div>
        `,
        afterRender: () => { }
    }
};

// ==================== USER MANAGEMENT ====================


async function loadUsers() {
    const roleFilter = document.getElementById('roleFilter')?.value || '';
    const departmentId = document.getElementById('departmentFilter')?.value || '';
    const searchTerm = document.getElementById('searchUsers')?.value || '';

    const response = await apiRequest(`/user/admin/all?roleFilter=${roleFilter}&departmentId=${departmentId}&searchTerm=${searchTerm}&pageSize=100`);

    const tbody = document.getElementById('usersTable');

    if (!response.ok || !response.data) {
        tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px; color: red;">Failed to load users</td></tr>';
        return;
    }

    const users = response.data;

    if (users.length === 0) {
        tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px;">No users found</td></tr>';
        return;
    }

    tbody.innerHTML = users.map(user => {
        const roleName = getRoleName(user.role);
        return `
        <tr>
            <td>${user.id}</td>
            <td>${user.firstName} ${user.lastName}</td>
            <td>${user.email}</td>
            <td><span class="admin-role-badge ${roleName.toLowerCase()}">${roleName}</span></td>
            <td>${user.departmentName || 'N/A'}</td>
            <td>${new Date(user.createdAt).toLocaleDateString()}</td>
            <td>
                <button class="admin-action-btn edit" onclick="window.openEditUserModal(${user.id})">Edit</button>
                <button class="admin-action-btn delete" onclick="window.deleteUser(${user.id})">Delete</button>
            </td>
        </tr>
    `}).join('');
}

async function loadDepartmentsForDropdown() {
    const response = await apiRequest('/department/admin/all?pageSize=100');

    if (response.ok && response.data) {
        allDepartments = response.data;
        populateDepartmentDropdown();
    }
}
function populateDepartmentDropdown() {

    const select = document.getElementById('userDepartment');
    if (select && allDepartments.length > 0) {
        select.innerHTML = '<option value="">Select department...</option>' +
            allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
    }

    const filterSelect = document.getElementById('departmentFilter');
    if (filterSelect && allDepartments.length > 0) {
        filterSelect.innerHTML = '<option value="">All Departments</option>' +
            allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
    }
}
window.openCreateUserModal = () => {
    currentEditingUserId = null;
    document.getElementById('userModalTitle').textContent = 'Create User';
    document.getElementById('userEmail').value = '';
    document.getElementById('userFirstName').value = '';
    document.getElementById('userLastName').value = '';
    document.getElementById('userRole').value = 'Student';
    document.getElementById('userDepartment').value = '';
    document.getElementById('userPassword').value = '';
    document.getElementById('passwordGroup').style.display = 'block';
    document.getElementById('userModal').style.display = 'flex';
};

window.openEditUserModal = async (userId) => {
    currentEditingUserId = userId;
    document.getElementById('userModalTitle').textContent = 'Edit User';
    document.getElementById('passwordGroup').style.display = 'none';

    populateDepartmentDropdown();

    const response = await apiRequest(`/user/admin/all?pageSize=100`);

    if (response.ok && response.data) {
        const user = response.data.find(u => u.id === userId);
        if (user) {
            document.getElementById('userEmail').value = user.email;
            document.getElementById('userFirstName').value = user.firstName;
            document.getElementById('userLastName').value = user.lastName;
            document.getElementById('userRole').value = getRoleName(user.role);
            document.getElementById('userDepartment').value = user.departmentId || '';
        }
    }

    document.getElementById('userModal').style.display = 'flex';
};

window.saveUser = async () => {
    const email = document.getElementById('userEmail').value.trim();
    const firstName = document.getElementById('userFirstName').value.trim();
    const lastName = document.getElementById('userLastName').value.trim();
    const role = document.getElementById('userRole').value;
    const departmentId = document.getElementById('userDepartment').value;
    const password = document.getElementById('userPassword').value;

    if (!email || !firstName || !lastName) {
        alert('Please fill in all required fields');
        return;
    }

    if (!currentEditingUserId && (!password || password.length < 8)) {
        alert('Password must be at least 8 characters');
        return;
    }

    const payload = {
        email,
        firstName,
        lastName,
        role,
        departmentId: departmentId ? parseInt(departmentId) : null
    };

    let response;
    if (currentEditingUserId) {
        response = await apiRequest(`/user/admin/${currentEditingUserId}`, 'PUT', payload);
    } else {
        payload.password = password;
        response = await apiRequest('/user/admin/create', 'POST', payload);
    }

    if (response.ok) {
        alert(currentEditingUserId ? 'User updated successfully' : 'User created successfully');
        document.getElementById('userModal').style.display = 'none';
        await loadUsers();
    } else {
        alert(response.message || 'Operation failed');
    }
};

window.deleteUser = async (userId) => {
    if (!confirm('Are you sure you want to delete this user?')) return;

    const response = await apiRequest(`/user/admin/${userId}`, 'DELETE');

    if (response.ok) {
        alert('User deleted successfully');
        await loadUsers();
    } else {
        alert(response.message || 'Delete failed');
    }
};

// ==================== DEPARTMENT MANAGEMENT ====================

async function loadDepartments() {
    const response = await apiRequest('/department/admin/all?pageSize=100');

    const grid = document.getElementById('departmentsGrid');
    const countEl = document.getElementById('departmentCount');

    if (!response.ok || !response.data) {
        grid.innerHTML = '<div style="text-align: center; padding: 40px; grid-column: 1/-1; color: red;">Failed to load departments</div>';
        countEl.textContent = 'Error loading departments';
        return;
    }

    const departments = response.data;
    countEl.textContent = `Total: ${departments.length} departments`;

    if (departments.length === 0) {
        grid.innerHTML = '<div style="text-align: center; padding: 40px; grid-column: 1/-1;">No departments found</div>';
        return;
    }

    grid.innerHTML = departments.map(dept => `
        <div class="admin-department-card">
            <div class="admin-department-card-header">
                <div>
                    <div class="admin-department-code">${dept.code}</div>
                    <div class="admin-department-name">${dept.name}</div>
                </div>
            </div>
            <div class="admin-department-info">
                ${dept.email ? `<div><label>Email:</label> ${dept.email}</div>` : ''}
                ${dept.secretaryEmail ? `<div><label>Secretary:</label> ${dept.secretaryEmail}</div>` : ''}
                ${dept.departmentHeadName ? `<div><label>Head:</label> ${dept.departmentHeadName}</div>` : ''}
                ${dept.departmentHeadEmail ? `<div><label>Head Email:</label> ${dept.departmentHeadEmail}</div>` : ''}
            </div>
            <div class="admin-department-actions">
                <button class="admin-action-btn edit" onclick="window.openEditDepartmentModal(${dept.id})">Edit</button>
                <button class="admin-action-btn delete" onclick="window.deleteDepartment(${dept.id})">Delete</button>
            </div>
        </div>
    `).join('');

    allDepartments = departments;
}

window.openCreateDepartmentModal = () => {
    currentEditingDepartmentId = null;
    document.getElementById('departmentModalTitle').textContent = 'Create Department';
    document.getElementById('deptName').value = '';
    document.getElementById('deptCode').value = '';
    document.getElementById('deptEmail').value = '';
    document.getElementById('deptSecretaryEmail').value = '';
    document.getElementById('deptHeadName').value = '';
    document.getElementById('deptHeadEmail').value = '';
    document.getElementById('departmentModal').style.display = 'flex';
};

window.openEditDepartmentModal = (deptId) => {
    currentEditingDepartmentId = deptId;
    document.getElementById('departmentModalTitle').textContent = 'Edit Department';

    const dept = allDepartments.find(d => d.id === deptId);
    if (dept) {
        document.getElementById('deptName').value = dept.name;
        document.getElementById('deptCode').value = dept.code;
        document.getElementById('deptEmail').value = dept.email || '';
        document.getElementById('deptSecretaryEmail').value = dept.secretaryEmail || '';
        document.getElementById('deptHeadName').value = dept.departmentHeadName || '';
        document.getElementById('deptHeadEmail').value = dept.departmentHeadEmail || '';
    }

    document.getElementById('departmentModal').style.display = 'flex';
};

window.saveDepartment = async () => {
    const name = document.getElementById('deptName').value.trim();
    const code = document.getElementById('deptCode').value.trim();
    const email = document.getElementById('deptEmail').value.trim();
    const secretaryEmail = document.getElementById('deptSecretaryEmail').value.trim();
    const departmentHeadName = document.getElementById('deptHeadName').value.trim();
    const departmentHeadEmail = document.getElementById('deptHeadEmail').value.trim();

    if (!name || !code) {
        alert('Please fill in department name and code');
        return;
    }

    const payload = {
        name,
        code,
        email: email || null,
        secretaryEmail: secretaryEmail || null,
        departmentHeadName: departmentHeadName || null,
        departmentHeadEmail: departmentHeadEmail || null
    };

    let response;
    if (currentEditingDepartmentId) {
        response = await apiRequest(`/department/${currentEditingDepartmentId}`, 'PUT', payload);
    } else {
        response = await apiRequest('/department', 'POST', payload);
    }

    if (response.ok) {
        alert(currentEditingDepartmentId ? 'Department updated successfully' : 'Department created successfully');
        document.getElementById('departmentModal').style.display = 'none';
        await loadDepartments();
    } else {
        alert(response.message || 'Operation failed');
    }
};

window.deleteDepartment = async (deptId) => {
    if (!confirm('Are you sure you want to delete this department?')) return;

    const response = await apiRequest(`/department/${deptId}`, 'DELETE');

    if (response.ok) {
        alert('Department deleted successfully');
        await loadDepartments();
    } else {
        alert(response.message || 'Delete failed');
    }
};

// ==================== COURSE INSTANCE MANAGEMENT ====================

async function loadInstanceData() {
    const [deptResponse, teacherResponse] = await Promise.all([
        apiRequest('/department/admin/all?pageSize=100'),
        apiRequest('/user/admin/all?roleFilter=Teacher&pageSize=100')
    ]);

    if (deptResponse.ok && deptResponse.data) {
        allDepartments = deptResponse.data;
        const select = document.getElementById('instanceDepartmentFilter');
        if (select) {
            select.innerHTML = '<option value="">All Departments</option>' +
                allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
        }
    }

    if (teacherResponse.ok && teacherResponse.data) {
        allTeachers = teacherResponse.data;
    }
}

async function loadCoursesByDepartment(departmentId) {
    if (!departmentId) {
        document.getElementById('instanceCourse').disabled = true;
        document.getElementById('instanceCourse').innerHTML = '<option value="">Select department first...</option>';
        return;
    }

    const response = await apiRequest(`/course/admin/by-department/${departmentId}?pageSize=100`);

    const courseSelect = document.getElementById('instanceCourse');

    if (response.ok && response.data) {
        allCourses = response.data;

        if (allCourses.length === 0) {
            courseSelect.disabled = true;
            courseSelect.innerHTML = '<option value="">No courses in this department</option>';
        } else {
            courseSelect.disabled = false;
            courseSelect.innerHTML = '<option value="">Select course...</option>' +
                allCourses.map(c => `<option value="${c.id}">${c.code} - ${c.name}</option>`).join('');
        }
    } else {
        courseSelect.disabled = true;
        courseSelect.innerHTML = '<option value="">Failed to load courses</option>';
    }
}

async function loadInstances() {
    const departmentId = document.getElementById('instanceDepartmentFilter')?.value || '';
    const searchTerm = document.getElementById('searchInstances')?.value || '';

    const response = await apiRequest(`/courseinstance/admin/all?departmentId=${departmentId}&searchTerm=${searchTerm}&pageSize=100`);

    const tbody = document.getElementById('instancesTable');

    if (!response.ok || !response.data) {
        tbody.innerHTML = '<tr><td colspan="8" style="text-align: center; padding: 20px; color: red;">Failed to load instances</td></tr>';
        return;
    }

    const instances = response.data;

    if (instances.length === 0) {
        tbody.innerHTML = '<tr><td colspan="8" style="text-align: center; padding: 20px;">No instances found</td></tr>';
        return;
    }

    tbody.innerHTML = instances.map(inst => {
        const days = [];
        if (inst.day1 !== null) days.push(getDayName(inst.day1));
        if (inst.day2 !== null) days.push(getDayName(inst.day2));
        const schedule = days.length > 0 ? `${days.join(', ')} ${inst.startTime || ''}-${inst.endTime || ''}` : 'N/A';

        return `
        <tr>
            <td>${inst.courseInstanceId}</td>
            <td>${inst.courseCode}</td>
            <td>${inst.courseName}</td>
            <td>${inst.section}</td>
            <td>${inst.teacherName}</td>
            <td>${schedule}</td>
            <td>${inst.currentEnrollmentCount}/${inst.capacity}</td>
            <td>
                <button class="admin-action-btn edit" onclick="window.openEditInstanceModal(${inst.id})">Edit</button>
                <button class="admin-action-btn delete" onclick="window.deleteInstance(${inst.id})">Delete</button>
            </td>
        </tr>
    `}).join('');
}

window.openCreateInstanceModal = async () => {
    currentEditingInstanceId = null;
    document.getElementById('instanceModalTitle').textContent = 'Create Course Instance';


    const deptSelect = document.getElementById('instanceDepartment');
    if (deptSelect && allDepartments && allDepartments.length > 0) {
        deptSelect.innerHTML = '<option value="">Select department...</option>' +
            allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
    }

    loadTeachers();
 
    const deptSelectElement = document.getElementById('instanceDepartment');
    const newDeptSelect = deptSelectElement.cloneNode(true);
    deptSelectElement.parentNode.replaceChild(newDeptSelect, deptSelectElement);

    newDeptSelect.addEventListener('change', async (e) => {
        await loadCoursesByDepartment(e.target.value);
    });

    document.getElementById('instanceModal').style.display = 'flex';
};

window.openEditInstanceModal = async (instanceId) => {
    currentEditingInstanceId = instanceId;
    document.getElementById('instanceModalTitle').textContent = 'Edit Course Instance';

    const deptSelect = document.getElementById('instanceDepartment');
    if (deptSelect && allDepartments && allDepartments.length > 0) {
        deptSelect.innerHTML = '<option value="">Select department...</option>' +
            allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
    }

    loadTeachers();

    const response = await apiRequest(`/courseinstance/admin/all?pageSize=100`);

    if (response.ok && response.data) {
        const inst = response.data.find(i => i.id === instanceId);
        if (inst) {

            document.getElementById('instanceDepartment').value = inst.departmentId || '';

            if (inst.departmentId) {
                await loadCoursesByDepartment(inst.departmentId);
            }

            document.getElementById('instanceCourse').value = inst.courseId || '';
            document.getElementById('instanceSection').value = inst.section;
            document.getElementById('instanceCapacity').value = inst.capacity;
            document.getElementById('instanceTeacher').value = inst.teacherId || '';
            document.getElementById('instanceDay1').value = inst.day1 !== null ? inst.day1 : '';
            document.getElementById('instanceDay2').value = inst.day2 !== null ? inst.day2 : '';
            document.getElementById('instanceStartTime').value = inst.startTime || '';
            document.getElementById('instanceEndTime').value = inst.endTime || '';
            document.getElementById('instanceLocation').value = inst.location || '';
        }
    }

    const deptSelectElement = document.getElementById('instanceDepartment');
    const newDeptSelect = deptSelectElement.cloneNode(true);
    deptSelectElement.parentNode.replaceChild(newDeptSelect, deptSelectElement);

    newDeptSelect.addEventListener('change', async (e) => {
        await loadCoursesByDepartment(e.target.value);
    });

    document.getElementById('instanceModal').style.display = 'flex';
};

function loadTeachers() {
    const teacherSelect = document.getElementById('instanceTeacher');
    if (teacherSelect && allTeachers && allTeachers.length > 0) {
        teacherSelect.innerHTML = '<option value="">Select instructor...</option>' +
            allTeachers.map(t => `<option value="${t.id}">${t.firstName} ${t.lastName}</option>`).join('');
    }
}

window.saveInstance = async () => {
    const departmentId = document.getElementById('instanceDepartment').value;
    const courseId = document.getElementById('instanceCourse').value;
    const section = document.getElementById('instanceSection').value.trim();
    const capacity = document.getElementById('instanceCapacity').value;
    const teacherId = document.getElementById('instanceTeacher').value;
    const day1 = document.getElementById('instanceDay1').value;
    const day2 = document.getElementById('instanceDay2').value;
    const startTime = document.getElementById('instanceStartTime').value;
    const endTime = document.getElementById('instanceEndTime').value;
    const location = document.getElementById('instanceLocation').value.trim();

    if (!courseId || !section || !capacity || !teacherId || !day1 || !startTime || !endTime) {
        alert('Please fill in all required fields');
        return;
    }

    const payload = {
        academicYearId: 1,  
        courseId: parseInt(courseId),
        section,
        capacity: parseInt(capacity),
        teacherId: parseInt(teacherId),
        day1: parseInt(day1),
        day2: day2 ? parseInt(day2) : null,
        startTime,
        endTime,
        location: location || null
    };

    let response;
    if (currentEditingInstanceId) {
        response = await apiRequest(`/courseinstance/admin/${currentEditingInstanceId}`, 'PUT', payload);
    } else {
        response = await apiRequest('/courseinstance/admin/create', 'POST', payload);
    }

    if (response.ok) {
        alert(currentEditingInstanceId ? 'Instance updated successfully' : 'Instance created successfully');
        document.getElementById('instanceModal').style.display = 'none';
        await loadInstances();
    } else {
        alert(response.message || 'Operation failed');
    }
};

window.deleteInstance = async (instanceId) => {
    if (!confirm('Are you sure you want to delete this course instance?')) return;

    const response = await apiRequest(`/courseinstance/admin/${instanceId}`, 'DELETE');

    if (response.ok) {
        alert('Instance deleted successfully');
        await loadInstances();
    } else {
        alert(response.message || 'Delete failed');
    }
};

// ==================== UTILITY FUNCTIONS ====================

function getRoleName(roleNumber) {
    const roles = ['Admin', 'Student', 'Teacher'];
    return roles[roleNumber] || 'Unknown';
}

function getDayName(dayNumber) {
    const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return days[dayNumber] || '';
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}