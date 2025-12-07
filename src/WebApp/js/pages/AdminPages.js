/**
 * AdminPages Module
 * Page templates for admin dashboard with admin-* prefix
 */

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
            <div class="placeholder-page">
                <div class="placeholder-icon">👥</div>
                <div class="placeholder-title">User Management</div>
                <div class="placeholder-text">Manage system users, roles, and permissions.</div>
            </div>
        `,
        afterRender: () => { }
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