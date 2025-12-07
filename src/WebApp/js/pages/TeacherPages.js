/**
 * TeacherPages Module
 * Page templates for teacher dashboard with teacher-* prefix
 */

export const TeacherPages = {
    dashboard: {
        render: () => `
            <div class="teacher-breadcrumb">Home / Dashboard</div>
            <div class="teacher-banner">
                <div class="teacher-banner-title">Welcome Back, Teacher</div>
                <div class="teacher-banner-text">You have 3 new assignments to grade and 12 pending student questions.</div>
            </div>
            <div class="teacher-stats">
                <div class="teacher-stat-card">
                    <div class="teacher-stat-value">4</div>
                    <div class="teacher-stat-label">Classes</div>
                </div>
                <div class="teacher-stat-card">
                    <div class="teacher-stat-value">127</div>
                    <div class="teacher-stat-label">Total Students</div>
                </div>
                <div class="teacher-stat-card">
                    <div class="teacher-stat-value">23</div>
                    <div class="teacher-stat-label">Pending Grades</div>
                </div>
                <div class="teacher-stat-card">
                    <div class="teacher-stat-value">12</div>
                    <div class="teacher-stat-label">Questions</div>
                </div>
            </div>
        `,
        afterRender: () => { }
    },

    classes: {
        render: () => `
            <div class="teacher-breadcrumb">Home / My Classes</div>
            <div class="teacher-section-header">My Classes</div>
            <div class="teacher-classes-grid">
                <div class="teacher-class-card">
                    <div class="teacher-class-header">
                        <div class="teacher-class-code">CS101</div>
                        <div class="teacher-class-name">Introduction to Programming</div>
                    </div>
                    <div class="teacher-class-body">
                        <div class="teacher-class-info">
                            <div><span class="teacher-label">Students:</span> 32</div>
                            <div><span class="teacher-label">Schedule:</span> MWF 10am</div>
                        </div>
                        <button class="teacher-btn">Manage Class</button>
                    </div>
                </div>
                <div class="teacher-class-card">
                    <div class="teacher-class-header">
                        <div class="teacher-class-code">MATH201</div>
                        <div class="teacher-class-name">Calculus II</div>
                    </div>
                    <div class="teacher-class-body">
                        <div class="teacher-class-info">
                            <div><span class="teacher-label">Students:</span> 28</div>
                            <div><span class="teacher-label">Schedule:</span> TTh 2pm</div>
                        </div>
                        <button class="teacher-btn">Manage Class</button>
                    </div>
                </div>
                <div class="teacher-class-card">
                    <div class="teacher-class-header">
                        <div class="teacher-class-code">ENG102</div>
                        <div class="teacher-class-name">Academic Writing</div>
                    </div>
                    <div class="teacher-class-body">
                        <div class="teacher-class-info">
                            <div><span class="teacher-label">Students:</span> 25</div>
                            <div><span class="teacher-label">Schedule:</span> MWF 2pm</div>
                        </div>
                        <button class="teacher-btn">Manage Class</button>
                    </div>
                </div>
                <div class="teacher-class-card">
                    <div class="teacher-class-header">
                        <div class="teacher-class-code">PHY101</div>
                        <div class="teacher-class-name">Physics I</div>
                    </div>
                    <div class="teacher-class-body">
                        <div class="teacher-class-info">
                            <div><span class="teacher-label">Students:</span> 30</div>
                            <div><span class="teacher-label">Schedule:</span> TTh 10am</div>
                        </div>
                        <button class="teacher-btn">Manage Class</button>
                    </div>
                </div>
            </div>
        `,
        afterRender: () => { }
    },

    grading: {
        render: () => `
            <div class="teacher-breadcrumb">Home / Grading</div>
            <div class="teacher-section-header">Grade Submissions</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">✏️</div>
                <div class="placeholder-title">Grading</div>
                <div class="placeholder-text">Review and grade student assignments here.</div>
            </div>
        `,
        afterRender: () => { }
    },

    roster: {
        render: () => `
            <div class="teacher-breadcrumb">Home / Class Roster</div>
            <div class="teacher-section-header">Class Roster</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">👥</div>
                <div class="placeholder-title">Class Roster</div>
                <div class="placeholder-text">View and manage enrolled students in your classes.</div>
            </div>
        `,
        afterRender: () => { }
    },

    profile: {
        render: () => `
            <div class="teacher-breadcrumb">Home / My Profile</div>
            <div class="teacher-section-header">Personal Information</div>
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
            <div class="teacher-breadcrumb">Home / Account Settings</div>
            <div class="teacher-section-header">Settings</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">⚙️</div>
                <div class="placeholder-title">Account Settings</div>
                <div class="placeholder-text">Manage your account preferences and security settings.</div>
            </div>
        `,
        afterRender: () => { }
    },

    help: {
        render: () => `
            <div class="teacher-breadcrumb">Home / Help & Support</div>
            <div class="teacher-section-header">Support Center</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">❓</div>
                <div class="placeholder-title">Help & Support</div>
                <div class="placeholder-text">Get help and contact support for any issues.</div>
            </div>
        `,
        afterRender: () => { }
    },

    schedule: {
        render: () => `
            <div class="teacher-breadcrumb">Home / Schedule</div>
            <div class="teacher-section-header">My Teaching Schedule</div>
            <div class="schedule-container">
                <div class="schedule-day-card teacher-schedule-card">
                    <div class="schedule-day-header teacher-day-header">Monday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">09:00 - 12:00</div>
                        <div class="schedule-course">Introduction to Programming</div>
                        <div class="schedule-meta">32 Students | Lab 101 | CS101-A</div>
                    </div>
                </div>

                <div class="schedule-day-card teacher-schedule-card">
                    <div class="schedule-day-header teacher-day-header">Tuesday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">14:00 - 17:00</div>
                        <div class="schedule-course">Calculus II</div>
                        <div class="schedule-meta">28 Students | Room 205 | MATH201-B</div>
                    </div>
                </div>

                <div class="schedule-day-card teacher-schedule-card">
                    <div class="schedule-day-header teacher-day-header">Wednesday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">10:00 - 13:00</div>
                        <div class="schedule-course">Academic Writing</div>
                        <div class="schedule-meta">25 Students | Room 304 | ENG102-C</div>
                    </div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">15:00 - 17:00</div>
                        <div class="schedule-course">Data Structures</div>
                        <div class="schedule-meta">30 Students | Lab 102 | CS201-A</div>
                    </div>
                </div>

                <div class="schedule-day-card teacher-schedule-card">
                    <div class="schedule-day-header teacher-day-header">Thursday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">09:00 - 12:00</div>
                        <div class="schedule-course">Physics I</div>
                        <div class="schedule-meta">30 Students | Room 401 | PHY101-B</div>
                    </div>
                </div>

                <div class="schedule-day-card teacher-schedule-card">
                    <div class="schedule-day-header teacher-day-header">Friday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">10:00 - 12:00</div>
                        <div class="schedule-course">Introduction to Programming</div>
                        <div class="schedule-meta">32 Students | Lab 101 | CS101-A</div>
                    </div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">13:00 - 16:00</div>
                        <div class="schedule-course">Calculus II</div>
                        <div class="schedule-meta">28 Students | Room 205 | MATH201-B</div>
                    </div>
                </div>

                <div class="schedule-day-card teacher-schedule-card empty">
                    <div class="schedule-day-header teacher-day-header">Saturday</div>
                    <div class="schedule-no-classes">No Classes</div>
                </div>

                <div class="schedule-day-card teacher-schedule-card empty">
                    <div class="schedule-day-header teacher-day-header">Sunday</div>
                    <div class="schedule-no-classes">No Classes</div>
                </div>
            </div>
        `,
        afterRender: () => { }
    }
};