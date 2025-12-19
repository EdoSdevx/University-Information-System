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
let currentEditingCourseId = null;
let currentManageInstanceId = null;

export const AdminPages = {
    dashboard: {
        render: () => `
            <div class="admin-breadcrumb">Home / Dashboard</div>
            <div class="admin-banner">
                <div class="admin-banner-title">System Dashboard</div>
                <div class="admin-banner-text">System is running normally. All services operational.</div>
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

        <div id="usersPagination"></div>

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

            window.usersPaginationPagination = new Pagination('usersPagination', {
                pageSize: 10,
                onPageChange: (page, pageSize) => loadUsers(page, pageSize)
            });

            await loadDepartmentsForDropdown();
            await loadUsers();

            document.getElementById('roleFilter').addEventListener('change', () => {
                loadUsers(1, 10);
            });
            document.getElementById('departmentFilter').addEventListener('change', () => {
                loadUsers(1, 10);
            });
            document.getElementById('searchUsers').addEventListener('input', debounce(() => {
                loadUsers(1, 10);
            }, 500));
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
                            <tr><td colspan="7" style="text-align: center; padding: 20px;">Loading...</td></tr>
                        </tbody>
                    </table>
                </div>

                <div id="instancesPagination"></div>

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
                            <select id="instanceDepartment" class="admin-input"><option value="">Select department...</option></select>
                        </div>
                        <div class="admin-form-group">
                            <label>Course *</label>
                            <select id="instanceCourse" class="admin-input" disabled><option value="">Select department first...</option></select>
                        </div>
                        <div class="admin-form-row">
                            <div class="admin-form-group"><label>Section *</label><input type="text" id="instanceSection" class="admin-input" placeholder="A"></div>
                            <div class="admin-form-group"><label>Capacity *</label><input type="number" id="instanceCapacity" class="admin-input" placeholder="40" min="1"></div>
                        </div>
                        <div class="admin-form-group">
                            <label>Instructor *</label>
                            <select id="instanceTeacher" class="admin-input"><option value="">Select instructor...</option></select>
                        </div>
                        <div class="admin-form-row">
                            <div class="admin-form-group">
                                <label>Day 1 *</label>
                                <select id="instanceDay1" class="admin-input">
                                    <option value="">Select day...</option>
                                    <option value="0">Sunday</option><option value="1">Monday</option><option value="2">Tuesday</option><option value="3">Wednesday</option><option value="4">Thursday</option><option value="5">Friday</option><option value="6">Saturday</option>
                                </select>
                            </div>
                            <div class="admin-form-group">
                                <label>Day 2 (Optional)</label>
                                <select id="instanceDay2" class="admin-input">
                                    <option value="">None</option>
                                    <option value="0">Sunday</option><option value="1">Monday</option><option value="2">Tuesday</option><option value="3">Wednesday</option><option value="4">Thursday</option><option value="5">Friday</option><option value="6">Saturday</option>
                                </select>
                            </div>
                        </div>
                        <div class="admin-form-row">
                            <div class="admin-form-group"><label>Start Time *</label><input type="time" id="instanceStartTime" class="admin-input"></div>
                            <div class="admin-form-group"><label>End Time *</label><input type="time" id="instanceEndTime" class="admin-input"></div>
                        </div>
                        <div class="admin-form-group"><label>Location</label><input type="text" id="instanceLocation" class="admin-input" placeholder="Building A - Room 101"></div>
                    </div>
                    <div class="admin-modal-footer">
                        <button class="admin-btn-cancel" onclick="document.getElementById('instanceModal').style.display='none'">Cancel</button>
                        <button class="admin-btn-primary" onclick="window.saveInstance()">Save Instance</button>
                    </div>
                </div>
            </div>

            <div class="admin-modal" id="enrollmentModal" style="display: none;">
                <div class="admin-modal-content">
                    <div class="admin-modal-header">
                        <h3 id="enrollmentModalTitle">Manage Enrollments</h3>
                        <button class="admin-modal-close" onclick="document.getElementById('enrollmentModal').style.display='none'">×</button>
                    </div>
                    
                    <div class="admin-enrollment-top-bar">
                        <div class="admin-enrollment-input-group">
                            <input type="number" id="enrollStudentId" class="admin-enrollment-input" placeholder="Enter Student ID (e.g. 2024001)">
                            <button class="admin-btn-primary" onclick="window.adminEnrollStudent()">Add Student</button>
                        </div>
                    </div>

                    <div class="admin-enrollment-table-container">
                        <table class="admin-table">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th>Email</th>
                                    <th>Enrolled Date</th>
                                    <th>Status</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody id="enrollmentListTable">
                                <tr><td colspan="6" style="text-align: center; padding: 20px;">Loading students...</td></tr>
                            </tbody>
                        </table>

                        <div id="enrollmentPagination"></div>

                    </div>
                    
                    <div class="admin-modal-footer">
                        <button class="admin-btn-cancel" onclick="document.getElementById('enrollmentModal').style.display='none'">Close</button>
                    </div>
                </div>
            </div>
        `,
        afterRender: async () => {

            window.instancesPaginationPagination = new Pagination('instancesPagination', {
                pageSize: 10,
                onPageChange: (page, pageSize) => loadInstances(page, pageSize)
            });

            await loadInstanceData();
            await loadInstances(1, 10);

            document.getElementById('instanceDepartmentFilter').addEventListener('change', () => {
                loadInstances(1, 10);
            });
            document.getElementById('searchInstances').addEventListener('input', debounce(() => {
                loadInstances(1, 10);
            }, 500));
        }
    },

    courses: {
        render: () => `
    <div class="admin-breadcrumb">Home / Course Management</div>
    <div class="admin-section-header">Course Management</div>
    
    <div class="admin-courses-container">
        <div class="admin-courses-header">
            <div class="admin-courses-filters">
                <select id="courseDepartmentFilter" class="admin-filter-select">
                    <option value="">All Departments</option>
                </select>
                <input type="text" id="searchCourses" class="admin-search-input" placeholder="Search by code or name...">
            </div>
            <button class="admin-btn-primary" onclick="window.openCreateCourseModal()">+ Create Course</button>
        </div>

        <div class="admin-table-wrapper">
            <table class="admin-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Code</th>
                        <th>Name</th>
                        <th>Credits</th>
                        <th>Department</th>
                        <th>Prerequisite</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody id="coursesTable">
                    <tr><td colspan="7" style="text-align: center; padding: 20px;">Loading...</td></tr>
                </tbody>
            </table>
        </div>

        <div id="coursesPagination"></div>

    </div>
    
    <div class="admin-modal" id="courseModal" style="display: none;">
        <div class="admin-modal-content">
            <div class="admin-modal-header">
                <h3 id="courseModalTitle">Create Course</h3>
                <button class="admin-modal-close" onclick="document.getElementById('courseModal').style.display='none'">×</button>
            </div>
            <div class="admin-modal-body">
                <div class="admin-form-row">
                    <div class="admin-form-group">
                        <label>Course Code *</label>
                        <input type="text" id="courseCode" class="admin-input" placeholder="CS101">
                    </div>
                    <div class="admin-form-group">
                        <label>Credit Hours *</label>
                        <input type="number" id="courseCreditHours" class="admin-input" placeholder="3" min="1" max="10">
                    </div>
                </div>
                <div class="admin-form-group">
                    <label>Course Name *</label>
                    <input type="text" id="courseName" class="admin-input" placeholder="Introduction to Computer Science">
                </div>
                <div class="admin-form-group">
                    <label>Department *</label>
                    <select id="courseDepartment" class="admin-input">
                        <option value="">Select department...</option>
                    </select>
                </div>
                <div class="admin-form-group">
                    <label>Prerequisite Course (Optional)</label>
                    <select id="coursePrerequisite" class="admin-input" disabled>
                        <option value="">None</option>
                    </select>
                </div>
            </div>
            <div class="admin-modal-footer">
                <button class="admin-btn-cancel" onclick="document.getElementById('courseModal').style.display='none'">Cancel</button>
                <button class="admin-btn-primary" onclick="window.saveCourse()">Save Course</button>
            </div>
        </div>
    </div>
`,
        afterRender: async () => {

            window.coursesPaginationPagination = new Pagination('coursesPagination', {
                pageSize: 10,
                onPageChange: (page, pageSize) => loadCoursesForTable(page, pageSize)
            });

            await loadCourseData();
            await loadCoursesForTable(1, 10);

            document.getElementById('courseDepartmentFilter').addEventListener('change', () => {
                loadCoursesForTable(1, 10);
            });
            document.getElementById('searchCourses').addEventListener('input', debounce(() => {
                loadCoursesForTable(1, 10);
            }, 500));
        }
    },

    profile: {
        render: () => `
            <div class="admin-breadcrumb">Home / My Profile</div>
            <div class="admin-section-header">My Profile</div>

            <div class="admin-profile-container">
                
                <div class="admin-card">
                    <h3>Personal Information</h3>
                    <div class="admin-form-group">
                        <label>Full Name</label>
                        <input type="text" id="profileName" class="admin-input" readonly disabled>
                    </div>
                    <div class="admin-form-group">
                        <label>Email Address</label>
                        <input type="email" id="profileEmail" class="admin-input" readonly disabled>
                    </div>
                    <div class="admin-form-group">
                        <label>Role</label>
                        <input type="text" id="profileRole" class="admin-input" readonly disabled>
                    </div>
                    <div class="admin-form-group">
                        <label>Department</label>
                        <input type="text" id="profileDepartment" class="admin-input" readonly disabled>
                    </div>
                </div>

                <div class="admin-card">
                    <h3>Security Settings</h3>
                    <div class="admin-form-group">
                        <label>Current Password</label>
                        <input type="password" id="currentPassword" class="admin-input" placeholder="Enter current password">
                    </div>
                    <div class="admin-form-group">
                        <label>New Password</label>
                        <input type="password" id="newPassword" class="admin-input" placeholder="Minimum 8 characters">
                    </div>
                    <div class="admin-form-group">
                        <label>Confirm New Password</label>
                        <input type="password" id="confirmPassword" class="admin-input" placeholder="Repeat new password">
                    </div>
                    <div class="admin-card-footer">
                        <button class="admin-btn-primary" onclick="window.updateAdminPassword()">Update Password</button>
                    </div>
                </div>

            </div>
        `,
        afterRender: async () => {
            await loadAdminProfile();
        }
    },
};

// ==================== USER MANAGEMENT ====================


async function loadUsers(page = 1, pageSize = 10) {

    const tbody = document.getElementById('usersTable');
    if (!tbody) return;

    const roleFilter = document.getElementById('roleFilter')?.value || '';
    const departmentId = document.getElementById('departmentFilter')?.value || '';
    const searchTerm = document.getElementById('searchUsers')?.value || '';

    const response = await apiRequest(`/user/admin/all?roleFilter=${roleFilter}&departmentId=${departmentId}&searchTerm=${searchTerm}&pageIndex=${page}&pageSize=${pageSize}`);

    if (!response.ok || !response.data) {
        tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px; color: red;">Failed to load users</td></tr>';
        return;
    }

    const users = response.data || [];
    const totalCount = response.totalCount;

    if (window.usersPaginationPagination) {
        window.usersPaginationPagination.update(totalCount, page);
    }

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

    const response = await apiRequest(`/user/admin/${userId}`);

    if (response.ok && response.data) {

        const user = response.data;
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

async function loadInstances(page = 1, pageSize = 10) {
    const departmentId = document.getElementById('instanceDepartmentFilter')?.value || '';
    const searchTerm = document.getElementById('searchInstances')?.value || '';

    const response = await apiRequest(`/courseinstance/admin/all?departmentId=${departmentId}&searchTerm=${searchTerm}&pageIndex=${page}&pageSize=${pageSize}`);

    const tbody = document.getElementById('instancesTable');

    if (!response.ok || !response.data) {
        tbody.innerHTML = '<tr><td colspan="8" style="text-align: center; padding: 20px; color: red;">Failed to load instances</td></tr>';
        return;
    }

    const instances = response.data;
    const totalCount = response.totalCount;

    if (window.instancesPaginationPagination) {
        window.instancesPaginationPagination.update(totalCount, page);
    }

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
            <td><strong>${inst.courseCode}</strong></td>
            <td>${inst.courseName}</td>
            <td>${inst.section}</td>
            <td>${inst.teacherName}</td>
            <td>${schedule}</td>
            <td>${inst.currentEnrollmentCount}/${inst.capacity}</td>
            <td style="white-space: nowrap;">
                <button class="admin-action-btn info" onclick="window.openEnrollmentModal(${inst.courseInstanceId}, '${inst.courseCode}')">Students</button>
                <button class="admin-action-btn edit" onclick="window.openEditInstanceModal(${inst.courseInstanceId})">Edit</button>
                <button class="admin-action-btn delete" onclick="window.deleteInstance(${inst.courseInstanceId})">Delete</button>
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
        await loadTeachersByDepartment(e.target.value);
    });

    document.getElementById('instanceModal').style.display = 'flex';
};

window.openEditInstanceModal = async (instanceId) => {
    currentEditingInstanceId = instanceId;
    document.getElementById('instanceModalTitle').textContent = 'Edit Course Instance';

    const deptSelect = document.getElementById('instanceDepartment');
    if (deptSelect && allDepartments.length > 0) {

        if (deptSelect.options.length <= 1) {
            deptSelect.innerHTML = '<option value="">Select department...</option>' +
                allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
        }
    }
    const response = await apiRequest(`/courseinstance/admin/detail/${instanceId}`);

    if (!response.ok || !response.data) {
        alert("Failed to load instance details.");
        return;
    }

    const inst = response.data;

    document.getElementById('instanceDepartment').value = inst.departmentId || '';

    if (inst.departmentId) {
        await Promise.all([
            loadCoursesByDepartment(inst.departmentId),
            loadTeachersByDepartment(inst.departmentId)
        ]);
    }

    document.getElementById('instanceCourse').value = inst.courseId || '';
    document.getElementById('instanceTeacher').value = inst.teacherId || '';
    document.getElementById('instanceSection').value = inst.section;
    document.getElementById('instanceCapacity').value = inst.capacity;

    document.getElementById('instanceDay1').value = inst.day1 !== null ? inst.day1 : '';
    document.getElementById('instanceDay2').value = (inst.day2 !== null && inst.day2 !== undefined) ? inst.day2 : '';
    document.getElementById('instanceStartTime').value = inst.startTime || '';
    document.getElementById('instanceEndTime').value = inst.endTime || '';
    document.getElementById('instanceLocation').value = inst.location || '';

    const deptSelectElement = document.getElementById('instanceDepartment');

    deptSelectElement.onchange = async (e) => {
        await loadCoursesByDepartment(e.target.value);
        await loadTeachersByDepartment(e.target.value);
    };

    document.getElementById('instanceModal').style.display = 'flex';
};

function loadTeachers() {
    const teacherSelect = document.getElementById('instanceTeacher');
    if (teacherSelect && allTeachers && allTeachers.length > 0) {
        teacherSelect.innerHTML = '<option value="">Select instructor...</option>' +
            allTeachers.map(t => `<option value="${t.id}">${t.firstName} ${t.lastName}</option>`).join('');
    }
}
async function loadTeachersByDepartment(departmentId) {
    if (!departmentId) {
        loadTeachers();
        return;
    }

    const response = await apiRequest(`/user/admin/all?roleFilter=Teacher&departmentId=${departmentId}&pageSize=100`);

    const teacherSelect = document.getElementById('instanceTeacher');

    if (response.ok && response.data) {
        const teachers = response.data;

        if (teachers.length === 0) {
            teacherSelect.innerHTML = '<option value="">No teachers in this department</option>';
        } else {
            teacherSelect.innerHTML = '<option value="">Select instructor...</option>' +
                teachers.map(t => `<option value="${t.id}">${t.firstName} ${t.lastName}</option>`).join('');
        }
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
// ==================== COURSE MANAGEMENT ====================

async function loadCourseData() {

    const [deptResponse, courseResponse] = await Promise.all([
        apiRequest('/department/admin/all?pageSize=100'),
        apiRequest('/course/admin/all?pageSize=1000') 
    ]);

    if (deptResponse.ok && deptResponse.data) {
        allDepartments = deptResponse.data;

        const filterSelect = document.getElementById('courseDepartmentFilter');
        if (filterSelect) {
            filterSelect.innerHTML = '<option value="">All Departments</option>' +
                allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
        }

        const modalSelect = document.getElementById('courseDepartment');
        if (modalSelect) {
            modalSelect.innerHTML = '<option value="">Select department...</option>' +
                allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
        }
    }
    if (courseResponse.ok && courseResponse.data) {
        allCourses = courseResponse.data.items || courseResponse.data;

        const prereqSelect = document.getElementById('coursePrerequisite');
        if (prereqSelect) {
            prereqSelect.innerHTML = '<option value="">None</option>' +
                allCourses.map(c => `<option value="${c.id}">${c.code} - ${c.name}</option>`).join('');
        }
    }
}

async function loadCoursesForTable(page = 1, pageSize = 10) {

    const tbody = document.getElementById('coursesTable');

    if (!tbody) {
        return;
    }

    const departmentId = document.getElementById('courseDepartmentFilter')?.value || '';
    const searchTerm = document.getElementById('searchCourses')?.value.toLowerCase() || '';

    tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px;">Loading...</td></tr>';

    let url = departmentId
        ? `/course/admin/by-department/${departmentId}?pageIndex=${page}&pageSize=${pageSize}`
        : `/course/admin/all?pageIndex=${page}&pageSize=${pageSize}`;

    if (searchTerm) {
        url += `&searchTerm=${encodeURIComponent(searchTerm)}`;
    }

    const response = await apiRequest(url);

    if (!response.ok || !response.data) {
        tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px; color: red;">Failed to load courses</td></tr>';
        return;
    }

    let courses = response.data.items || response.data;
    const totalCount = response.totalCount;

    if (window.coursesPaginationPagination) {
        window.coursesPaginationPagination.update(totalCount, page);
    }

    if (courses.length === 0) {
        tbody.innerHTML = '<tr><td colspan="7" style="text-align: center; padding: 20px;">No courses found</td></tr>';
        return;
    }

    const getDeptName = (id) => {
        const d = allDepartments.find(dept => dept.id === id);
        return d ? d.name : 'Unknown';
    };

    const getPrereqName = (id) => {

        if (!id) return '-';
        const c = allCourses.find(course => course.id === id);
        return c ? c.code : 'Unknown';
    };

    tbody.innerHTML = courses.map(c => `
        <tr>
            <td>${c.id}</td>
            <td><strong>${c.code}</strong></td>
            <td>${c.name}</td>
            <td>${c.creditHours || '-'}</td>
            <td>${getDeptName(c.departmentId)}</td>
            <td>${getPrereqName(c.prerequisiteCourseId)}</td>
            <td>
                <button class="admin-action-btn edit" onclick="window.openEditCourseModal(${c.id})">Edit</button>
                <button class="admin-action-btn delete" onclick="window.deleteCourse(${c.id})">Delete</button>
            </td>
        </tr>
    `).join('');
}

function populateCourseModalDropdowns(selectedDepartmentId = null, excludeCourseId = null) {
    const deptSelect = document.getElementById('courseDepartment');
    const prereqSelect = document.getElementById('coursePrerequisite');

    if (deptSelect && allDepartments.length > 0) {
        if (deptSelect.innerHTML.length < 50) {
            deptSelect.innerHTML = '<option value="">Select department...</option>' +
                allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
        }

        if (selectedDepartmentId) {
            deptSelect.value = selectedDepartmentId;
        }

        deptSelect.onchange = (e) => populateCourseModalDropdowns(e.target.value, excludeCourseId);
    }

    if (prereqSelect) {
        prereqSelect.innerHTML = '<option value="">None</option>';

        const currentDeptId = selectedDepartmentId || (deptSelect ? deptSelect.value : null);

        if (!currentDeptId) {
            prereqSelect.disabled = true;
            return;
        }

        const availableCourses = allCourses.filter(c =>
            c.departmentId == currentDeptId &&
            c.id !== excludeCourseId
        );

        if (availableCourses.length > 0) {
            prereqSelect.disabled = false;
            prereqSelect.innerHTML += availableCourses.map(c =>
                `<option value="${c.id}">${c.code} - ${c.name}</option>`
            ).join('');
        } else {
            prereqSelect.disabled = false; 
        }
    }
}

window.openCreateCourseModal = () => {
    currentEditingCourseId = null;
    document.getElementById('courseModalTitle').textContent = 'Create Course';

    document.getElementById('courseCode').value = '';
    document.getElementById('courseName').value = '';
    document.getElementById('courseCreditHours').value = '';

    document.getElementById('courseDepartment').innerHTML = '';
    document.getElementById('courseDepartment').value = '';
    document.getElementById('coursePrerequisite').value = '';

    populateCourseModalDropdowns(null, null);

    document.getElementById('courseModal').style.display = 'flex';
};

window.openEditCourseModal = (courseId) => {
    currentEditingCourseId = courseId;
    document.getElementById('courseModalTitle').textContent = 'Edit Course';

    const course = allCourses.find(c => c.id === courseId);

    if (course) {

        document.getElementById('courseCode').value = course.code;
        document.getElementById('courseName').value = course.name;
        document.getElementById('courseCreditHours').value = course.creditHours || '';

        const deptSelect = document.getElementById('courseDepartment');
        if (deptSelect && allDepartments.length > 0) {
            deptSelect.innerHTML = '<option value="">Select department...</option>' +
                allDepartments.map(d => `<option value="${d.id}">${d.name}</option>`).join('');
        }
        deptSelect.value = course.departmentId;

        populateCourseModalDropdowns(course.departmentId, course.id);

        document.getElementById('coursePrerequisite').value = course.prerequisiteCourseId || "";

        document.getElementById('courseModal').style.display = 'flex';
    } else {
        alert("Error: Course details not found.");
    }
};

window.saveCourse = async () => {

    const code = document.getElementById('courseCode').value.trim();
    const name = document.getElementById('courseName').value.trim();
    const creditHours = document.getElementById('courseCreditHours').value;
    const departmentId = document.getElementById('courseDepartment').value;
    const prerequisiteCourseId = document.getElementById('coursePrerequisite').value;

    if (!code || !name || !creditHours || !departmentId) {
        alert('Please fill in all required fields marked with *');
        return;
    }

    const payload = {
        code: code,
        name: name,
        creditHours: parseInt(creditHours),
        departmentId: parseInt(departmentId),
        prerequisiteCourseId: prerequisiteCourseId ? parseInt(prerequisiteCourseId) : null
    };

    let response;
    if (currentEditingCourseId) {
        response = await apiRequest(`/course/admin/edit/${currentEditingCourseId}`, 'PUT', payload);
    } else {
        response = await apiRequest('/course/admin/create', 'POST', payload);
    }

    if (response.ok) {
        alert(currentEditingCourseId ? 'Course updated successfully' : 'Course created successfully');
        document.getElementById('courseModal').style.display = 'none';

        await loadCourseData();
        await loadCoursesForTable();
    } else {
        alert(response.message || 'Operation failed');
    }
};

window.deleteCourse = async (courseId) => {
    if (!confirm('Are you sure you want to delete this course? This action cannot be undone if students are enrolled.')) return;

    const response = await apiRequest(`/course/admin/delete/${courseId}`, 'DELETE');

    if (response.ok) {
        alert('Course deleted successfully');
        await loadCourseData();
        await loadCoursesForTable();
    } else {
        alert(response.message || 'Delete failed. Ensure the course has no active instances.');
    }
};

window.openEnrollmentModal = async (instanceId, courseCode) => {
    currentManageInstanceId = instanceId;
    document.getElementById('enrollmentModalTitle').textContent = `Manage Enrollments: ${courseCode}`;
    document.getElementById('enrollmentModal').style.display = 'flex';
    document.getElementById('enrollStudentId').value = '';

    if (!window.enrollmentPaginationPagination) {
        window.enrollmentPaginationPagination = new Pagination('enrollmentPagination', {
            pageSize: 10,
            onPageChange: (page, pageSize) => loadClassEnrollments(currentManageInstanceId, page, pageSize)
        });
    }

    await loadClassEnrollments(instanceId, 1, 10);
};

async function loadClassEnrollments(instanceId, page = 1, pageSize = 10) {
    const tbody = document.getElementById('enrollmentListTable');
    tbody.innerHTML = '<tr><td colspan="6" style="text-align: center; padding: 20px;">Loading...</td></tr>';

    const response = await apiRequest(`/enrollment/admin/course/${instanceId}?pageIndex=${page}&pageSize=${pageSize}`);

    if (response.ok && response.data) {
        const students = response.data;
        const totalCount = response.totalCount;

        if (window.enrollmentPaginationPagination) {
            window.enrollmentPaginationPagination.update(totalCount, page);
        }
        if (students.length === 0) {
            tbody.innerHTML = '<tr><td colspan="6" style="text-align: center; padding: 20px;">No students enrolled yet.</td></tr>';
            return;
        }

        tbody.innerHTML = students.map(s => `
            <tr>
                <td>${s.studentId}</td>
                <td>${s.studentName}</td>
                <td>${s.studentEmail || '-'}</td>
                <td>${new Date(s.enrolledAt).toLocaleDateString()}</td>
                <td><span class="admin-enrollment-status">Active</span></td>
                <td>
                    <button class="admin-action-btn delete" onclick="window.adminDropStudent(${s.studentId})">Drop</button>
                </td>
            </tr>
        `).join('');
    } else {
        tbody.innerHTML = '<tr><td colspan="6" style="text-align: center; color: red;">Failed to load students.</td></tr>';
    }
}

window.adminEnrollStudent = async () => {
    const studentId = document.getElementById('enrollStudentId').value.trim();

    if (!studentId) {
        alert("Please enter a Student ID");
        return;
    }
    const response = await apiRequest('/enrollment/admin/enroll', 'POST', {
        studentId: parseInt(studentId),
        courseInstanceId: currentManageInstanceId
    });

    if (response.ok) {
        alert("Student enrolled successfully.");
        document.getElementById('enrollStudentId').value = '';
        await loadClassEnrollments(currentManageInstanceId);
        await loadInstances();
    } else {
        alert(response.message || "Failed to enroll student. Check ID or Capacity.");
    }
};

window.adminDropStudent = async (studentId) => {
    if (!confirm(`Are you sure you want to remove Student ID: ${studentId} from this course?`)) return;

    const response = await apiRequest('/enrollment/admin/drop', 'POST', {
        studentId: parseInt(studentId),
        courseInstanceId: currentManageInstanceId
    });

    if (response.ok) {
        await loadClassEnrollments(currentManageInstanceId);
        await loadInstances();
    } else {
        alert(response.message || "Failed to drop student.");
    }
};

// ==================== ADMIN PROFILE FUNCTIONS ====================

async function loadAdminProfile() {

    const response = await apiRequest(`/user/admin/profile`);

    if (response.ok && response.data) {
        const u = response.data;
        document.getElementById('profileName').value = `${u.firstName} ${u.lastName}`;
        document.getElementById('profileEmail').value = u.email;
        document.getElementById('profileRole').value = getRoleName(u.role);
        document.getElementById('profileDepartment').value = u.departmentName || 'System Administrator';
    }
}

window.updateAdminPassword = async () => {
    const currentPassword = document.getElementById('currentPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (!currentPassword || !newPassword || !confirmPassword) {
        alert("Please fill in all password fields.");
        return;
    }

    if (newPassword !== confirmPassword) {
        alert("New passwords do not match.");
        return;
    }

    if (newPassword.length < 8) {
        alert("New password must be at least 8 characters.");
        return;
    }

    const payload = {
        currentPassword: currentPassword,
        newPassword: newPassword
    };

    const response = await apiRequest('/user/change-password', 'POST', payload);

    if (response.ok) {
        alert("Password updated successfully.");
        document.getElementById('currentPassword').value = '';
        document.getElementById('newPassword').value = '';
        document.getElementById('confirmPassword').value = '';
    } else {
        alert(response.message || "Failed to update password.");
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


// ==================== PAGINATION UTILITY ====================

class Pagination {
    constructor(containerId, options = {}) {
        this.containerId = containerId;
        this.currentPage = 1;
        this.pageSize = options.pageSize || 10;
        this.totalItems = 0;
        this.totalPages = 0;
        this.onPageChange = options.onPageChange || (() => { });
        this.pageSizeOptions = options.pageSizeOptions || [10, 25, 50, 100];
    }

    update(totalItems, currentPage = 1) {
        this.totalItems = totalItems;
        this.currentPage = currentPage;
        this.totalPages = Math.ceil(totalItems / this.pageSize);
        this.render();
    }

    setPageSize(size) {
        this.pageSize = size;
        this.currentPage = 1;
        this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        this.onPageChange(this.currentPage, this.pageSize);
    }

    goToPage(page) {
        if (page < 1 || page > this.totalPages) return;
        this.currentPage = page;
        this.onPageChange(this.currentPage, this.pageSize);
    }

    render() {
        const container = document.getElementById(this.containerId);
        if (!container) return;

        if (this.totalItems === 0) {
            container.innerHTML = '';
            return;
        }

        const startItem = (this.currentPage - 1) * this.pageSize + 1;
        const endItem = Math.min(this.currentPage * this.pageSize, this.totalItems);

        let pageNumbers = [];
        const maxVisiblePages = 5;

        if (this.totalPages <= maxVisiblePages) {
            for (let i = 1; i <= this.totalPages; i++) {
                pageNumbers.push(i);
            }
        } else {
            if (this.currentPage <= 3) {
                pageNumbers = [1, 2, 3, 4, '...', this.totalPages];
            } else if (this.currentPage >= this.totalPages - 2) {
                pageNumbers = [1, '...', this.totalPages - 3, this.totalPages - 2, this.totalPages - 1, this.totalPages];
            } else {
                pageNumbers = [1, '...', this.currentPage - 1, this.currentPage, this.currentPage + 1, '...', this.totalPages];
            }
        }

        container.innerHTML = `
            <div class="admin-pagination">
                <div class="admin-pagination-info">
                    Showing <strong>${startItem}-${endItem}</strong> of <strong>${this.totalItems}</strong> items
                </div>
                <div class="admin-pagination-controls">
                    <select class="admin-pagination-size" onchange="window.${this.containerId}Pagination.setPageSize(Number(this.value))">
                        ${this.pageSizeOptions.map(size =>
            `<option value="${size}" ${size === this.pageSize ? 'selected' : ''}>${size} / page</option>`
        ).join('')}
                    </select>
                    <div class="admin-pagination-pages">
                        <button class="admin-pagination-btn" ${this.currentPage === 1 ? 'disabled' : ''} 
                            onclick="window.${this.containerId}Pagination.goToPage(${this.currentPage - 1})">
                            &laquo; Prev
                        </button>
                        ${pageNumbers.map(page =>
            page === '...'
                ? `<span class="admin-pagination-ellipsis">...</span>`
                : `<button class="admin-pagination-btn ${page === this.currentPage ? 'active' : ''}" 
                                    onclick="window.${this.containerId}Pagination.goToPage(${page})">${page}</button>`
        ).join('')}
                        <button class="admin-pagination-btn" ${this.currentPage === this.totalPages ? 'disabled' : ''} 
                            onclick="window.${this.containerId}Pagination.goToPage(${this.currentPage + 1})">
                            Next &raquo;
                        </button>
                    </div>
                </div>
            </div>
        `;
    }
}