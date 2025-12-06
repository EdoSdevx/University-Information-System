/**
 * StudentPages Module
 * Page templates for student dashboard
 */

export const StudentPages = {
    dashboard: {
        render: () => `
            <div class="student-breadcrumb">Home / Dashboard</div>
            <div class="student-banner">
                <div class="student-banner-title">Course Registration Now Open</div>
                <div class="student-banner-text">Spring 2025 course registration is available. Registration closes December 15, 2024. Please register for your courses promptly.</div>
            </div>
            <div style="margin-bottom: 30px;">
                <div class="student-section-header">Academic Summary</div>
                <div class="student-stats">
                    <div class="student-stat-card">
                        <div class="student-stat-value">4</div>
                        <div class="student-stat-label">Enrolled Courses</div>
                    </div>
                    <div class="student-stat-card">
                        <div class="student-stat-value">3.75</div>
                        <div class="student-stat-label">Current GPA</div>
                    </div>
                    <div class="student-stat-card">
                        <div class="student-stat-value">48</div>
                        <div class="student-stat-label">Credits Earned</div>
                    </div>
                    <div class="student-stat-card">
                        <div class="student-stat-value">3</div>
                        <div class="student-stat-label">Upcoming Exams</div>
                    </div>
                </div>
            </div>
            <div>
                <div class="student-section-header">Current Course Enrollment</div>
                <div class="student-courses">
                    <div class="student-course-card">
                        <div class="student-course-header">
                            <div class="student-course-code">CS101</div>
                            <div class="student-course-name">Introduction to Programming</div>
                        </div>
                        <div class="student-course-body">
                            <div class="student-course-meta">
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Instructor</div>
                                    <div class="student-course-meta-value">Dr. Smith</div>
                                </div>
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Schedule</div>
                                    <div class="student-course-meta-value">MWF 10:00am</div>
                                </div>
                            </div>
                            <div class="student-course-actions">
                                <button class="student-course-btn">View Details</button>
                                <button class="student-course-btn secondary">Syllabus</button>
                            </div>
                        </div>
                    </div>
                    <div class="student-course-card">
                        <div class="student-course-header alt1">
                            <div class="student-course-code">MATH201</div>
                            <div class="student-course-name">Calculus II</div>
                        </div>
                        <div class="student-course-body">
                            <div class="student-course-meta">
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Instructor</div>
                                    <div class="student-course-meta-value">Prof. Johnson</div>
                                </div>
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Schedule</div>
                                    <div class="student-course-meta-value">TTh 2:00pm</div>
                                </div>
                            </div>
                            <div class="student-course-actions">
                                <button class="student-course-btn">View Details</button>
                                <button class="student-course-btn secondary">Syllabus</button>
                            </div>
                        </div>
                    </div>
                    <div class="student-course-card">
                        <div class="student-course-header alt2">
                            <div class="student-course-code">ENG102</div>
                            <div class="student-course-name">Academic Writing</div>
                        </div>
                        <div class="student-course-body">
                            <div class="student-course-meta">
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Instructor</div>
                                    <div class="student-course-meta-value">Dr. Williams</div>
                                </div>
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Schedule</div>
                                    <div class="student-course-meta-value">MWF 2:00pm</div>
                                </div>
                            </div>
                            <div class="student-course-actions">
                                <button class="student-course-btn">View Details</button>
                                <button class="student-course-btn secondary">Syllabus</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `,
        afterRender: () => { }
    },

    courses: {
        render: () => `
            <div class="student-breadcrumb">Home / My Courses</div>
            <div class="student-section-header">Your Enrolled Courses</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">📚</div>
                <div class="placeholder-title">My Courses</div>
                <div class="placeholder-text">View all your enrolled courses for the current semester.</div>
            </div>
        `,
        afterRender: () => { }
    },

    registration: {
        render: () => `
            <div class="student-breadcrumb">Home / Course Registration</div>
            <div class="student-section-header">Register for Courses</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">➕</div>
                <div class="placeholder-title">Course Registration</div>
                <div class="placeholder-text">Search and register for courses for the next semester.</div>
            </div>
        `,
        afterRender: () => { }
    },

    grades: {
        render: () => `
            <div class="student-breadcrumb">Home / Grades & Transcripts</div>
            <div class="student-section-header">Your Academic Records</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">📈</div>
                <div class="placeholder-title">Grades & Transcripts</div>
                <div class="placeholder-text">View your grades and download official transcripts.</div>
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
            <div class="student-breadcrumb">Home / Account Settings</div>
            <div class="student-section-header">Settings</div>
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
            <div class="student-breadcrumb">Home / Help & Support</div>
            <div class="student-section-header">Support Center</div>
            <div class="placeholder-page">
                <div class="placeholder-icon">❓</div>
                <div class="placeholder-title">Help & Support</div>
                <div class="placeholder-text">Get help and contact support for any issues.</div>
            </div>
        `,
        afterRender: () => { }
    }
};