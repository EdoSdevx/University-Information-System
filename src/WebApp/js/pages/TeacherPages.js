/**
 * TeacherPages Module
 * Page templates for teacher dashboard
 */

export const TeacherPages = {
    dashboard: {
        render: () => `
            <div class="student-breadcrumb">Home / Dashboard</div>
            <div class="student-section-header">Teacher Dashboard</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">??</div>
                <div class="placeholder-title">Teacher Dashboard</div>
                <div class="placeholder-text">Manage your classes and student progress.</div>
            </div>
        `,
        afterRender: () => { }
    },

    classes: {
        render: () => `
            <div class="student-breadcrumb">Home / My Classes</div>
            <div class="student-section-header">My Classes</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">??</div>
                <div class="placeholder-title">My Classes</div>
                <div class="placeholder-text">View and manage your classes.</div>
            </div>
        `,
        afterRender: () => { }
    },

    grading: {
        render: () => `
            <div class="student-breadcrumb">Home / Grading</div>
            <div class="student-section-header">Grading</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">??</div>
                <div class="placeholder-title">Grading</div>
                <div class="placeholder-text">Enter and manage student grades.</div>
            </div>
        `,
        afterRender: () => { }
    },

    roster: {
        render: () => `
            <div class="student-breadcrumb">Home / Class Roster</div>
            <div class="student-section-header">Class Roster</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">??</div>
                <div class="placeholder-title">Class Roster</div>
                <div class="placeholder-text">View enrolled students in your classes.</div>
            </div>
        `,
        afterRender: () => { }
    },

    profile: {
        render: () => `
            <div class="student-breadcrumb">Home / My Profile</div>
            <div class="student-section-header">Personal Information</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">??</div>
                <div class="placeholder-title">My Profile</div>
                <div class="placeholder-text">View and update your personal information.</div>
            </div>
        `,
        afterRender: () => { }
    },

    settings: {
        render: () => `
            <div class="student-breadcrumb">Home / Account Settings</div>
            <div class="student-section-header">Settings</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">??</div>
                <div class="placeholder-title">Account Settings</div>
                <div class="placeholder-text">Manage your account preferences and security settings.</div>
            </div>
        `,
        afterRender: () => { }
    },

    help: {
        render: () => `
            <div class="student-breadcrumb">Home / Help & Support</div>
            <div class="student-section-header">Support Center</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">?</div>
                <div class="placeholder-title">Help & Support</div>
                <div class="placeholder-text">Get help and contact support for any issues.</div>
            </div>
        `,
        afterRender: () => { }
    }
};