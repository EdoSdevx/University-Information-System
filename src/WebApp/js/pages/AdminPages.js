/**
 * AdminPages Module
 * Page templates for admin dashboard
 */

export const AdminPages = {
    dashboard: {
        render: () => `
            <div class="student-breadcrumb">Home / Dashboard</div>
            <div class="student-section-header">Admin Dashboard</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">👨‍💼</div>
                <div class="placeholder-title">Admin Dashboard</div>
                <div class="placeholder-text">System overview and management tools.</div>
            </div>
        `,
        afterRender: () => { }
    },

    users: {
        render: () => `
            <div class="student-breadcrumb">Home / User Management</div>
            <div class="student-section-header">User Management</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">👥</div>
                <div class="placeholder-title">User Management</div>
                <div class="placeholder-text">Manage system users and permissions.</div>
            </div>
        `,
        afterRender: () => { }
    },

    courses: {
        render: () => `
            <div class="student-breadcrumb">Home / Course Management</div>
            <div class="student-section-header">Course Management</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">📚</div>
                <div class="placeholder-title">Course Management</div>
                <div class="placeholder-text">Create and manage courses.</div>
            </div>
        `,
        afterRender: () => { }
    },

    reports: {
        render: () => `
            <div class="student-breadcrumb">Home / Reports</div>
            <div class="student-section-header">System Reports</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">📊</div>
                <div class="placeholder-title">Reports</div>
                <div class="placeholder-text">View system reports and analytics.</div>
            </div>
        `,
        afterRender: () => { }
    },

    profile: {
        render: () => `
            <div class="student-breadcrumb">Home / My Profile</div>
            <div class="student-section-header">Personal Information</div>
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
            <div class="student-breadcrumb">Home / System Settings</div>
            <div class="student-section-header">System Settings</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">⚙️</div>
                <div class="placeholder-title">System Settings</div>
                <div class="placeholder-text">Configure system-wide settings.</div>
            </div>
        `,
        afterRender: () => { }
    },

    logs: {
        render: () => `
            <div class="student-breadcrumb">Home / Activity Logs</div>
            <div class="student-section-header">Activity Logs</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">📋</div>
                <div class="placeholder-title">Activity Logs</div>
                <div class="placeholder-text">View system activity and audit logs.</div>
            </div>
        `,
        afterRender: () => { }
    }
};