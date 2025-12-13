/**
 * StudentPages Module
 * Page templates for student dashboard
 */

import { apiRequest } from '../core/ApiService.js';

const StudentDashboardService = {
  

    calculateGPA(grades) {
        if (!grades || grades.length === 0) return 0;

        const gradePoints = {
            'A': 4.0,
            'B': 3.0,
            'C': 2.0,
            'D': 1.0,
            'F': 0.0
        };

        let totalPoints = 0;
        let totalCourses = 0;

        grades.forEach(grade => {
            const points = gradePoints[grade.letterGrade] || 0;
            totalPoints += points;
            totalCourses++;
        });

        return totalCourses > 0 ? (totalPoints / totalCourses).toFixed(2) : 0;
    },

    formatSchedule(day1, day2, startTime, endTime) {
        if (!day1 || !startTime) return 'TBA';

        const getDayAbbr = (day) => {
            const abbr = { 0: 'Sun', 1: 'Mon', 2: 'Tue', 3: 'Wed', 4: 'Thu', 5: 'Fri', 6: 'Sat' };
            return abbr[day] || '';
        };

        const formatTime = (time) => {
            if (!time) return '';
            const [hours, minutes] = time.split(':');
            const h = parseInt(hours);
            const m = minutes;
            const ampm = h >= 12 ? 'pm' : 'am';
            const displayH = h > 12 ? h - 12 : (h === 0 ? 12 : h);
            return `${displayH}:${m}${ampm}`;
        };

        const day1Str = getDayAbbr(day1);
        const day2Str = day2 ? getDayAbbr(day2) : '';
        const timeStr = formatTime(startTime);

        if (day2Str) {
            return `${day1Str}/${day2Str} ${timeStr}`;
        }
        return `${day1Str} ${timeStr}`;
    },

    getColorClass(index) {
        const colors = ['', 'alt1', 'alt2'];
        return colors[index % colors.length];
    }
};

export const StudentPages = {
    dashboard: {
        render: () => `
        <div class="student-breadcrumb">Home / Dashboard</div>
        <div id="enrollmentBanner"></div>
        <div style="margin-bottom: 30px;">
            <div class="student-section-header">Academic Summary</div>
            <div id="academicSummary" class="student-stats">
                <div class="student-stat-card">
                    <div class="student-stat-value loading">-</div>
                    <div class="student-stat-label">Enrolled Courses</div>
                </div>
                <div class="student-stat-card">
                    <div class="student-stat-value loading">-</div>
                    <div class="student-stat-label">Current GPA</div>
                </div>
                <div class="student-stat-card">
                    <div class="student-stat-value loading">-</div>
                    <div class="student-stat-label">Credits Earned</div>
                </div>
                <div class="student-stat-card">
                    <div class="student-stat-value loading">-</div>
                    <div class="student-stat-label">Upcoming Exams</div>
                </div>
            </div>
        </div>
        <div>
            <div class="student-section-header">Current Course Enrollment</div>
            <div id="enrolledCourses" class="student-courses">
                <div style="text-align: center; padding: 20px;">Loading courses...</div>
            </div>
        </div>
    `,
        afterRender: async () => {
            try {
                // Fetch academic year data
                const academicYearResponse = await apiRequest('/academicYear/active');
                const enrollmentsResponse = await apiRequest('/enrollment/my-enrollments?pageIndex=1&pageSize=100');
                const gradesResponse = await apiRequest('/grade/my-grades?pageIndex=1&pageSize=100');

                // Check enrollment period
                const now = new Date();
                let isEnrollmentOpen = false;
                let enrollmentEndDate = null;

                if (academicYearResponse.ok && academicYearResponse.data) {
                    const academicYear = academicYearResponse.data;
                    const startDate = new Date(academicYear.enrollmentStartDate);
                    const endDate = new Date(academicYear.enrollmentEndDate);
                    isEnrollmentOpen = now >= startDate && now <= endDate;
                    enrollmentEndDate = endDate;
                }

                // Get enrollment count from response
                const enrollmentCount = enrollmentsResponse.ok && enrollmentsResponse.data
                    ? (enrollmentsResponse.data.totalCount || enrollmentsResponse.data.length || 0)
                    : 0;

                const enrollments = enrollmentsResponse.ok && enrollmentsResponse.data
                    ? (enrollmentsResponse.data.data || enrollmentsResponse.data || [])
                    : [];

                const grades = gradesResponse.ok && gradesResponse.data
                    ? (gradesResponse.data.data || gradesResponse.data || [])
                    : [];

                // Display appropriate banner
                const bannerContainer = document.getElementById('enrollmentBanner');
                if (isEnrollmentOpen) {
                    const formattedDate = enrollmentEndDate.toLocaleDateString('en-US', {
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric'
                    });
                    bannerContainer.innerHTML = `
                    <div class="student-banner">
                        <div class="student-banner-title">Course Registration Now Open</div>
                        <div class="student-banner-text">
                            ${enrollmentCount > 0
                            ? `You are currently registered for ${enrollmentCount} course${enrollmentCount !== 1 ? 's' : ''}. `
                            : ''}Registration closes on ${formattedDate}. ${enrollmentCount === 0 ? 'Please register for your courses promptly.' : 'You can still add or drop courses.'}
                        </div>
                    </div>
                `;
                } else {
                    bannerContainer.innerHTML = `
                    <div class="student-banner">
                        <div class="student-banner-title">Registration Period Closed</div>
                        <div class="student-banner-text">
                            You are registered for ${enrollmentCount} course${enrollmentCount !== 1 ? 's' : ''} this semester. Course registration is currently closed.
                        </div>
                    </div>
                `;
                }

                // Calculate statistics
                const gpa = StudentDashboardService.calculateGPA(grades);
                const creditsEarned = enrollments.reduce((sum, e) => sum + (e.creditHours || 3), 0);

                // Update summary cards
                const summaryHTML = `
                <div class="student-stat-card">
                    <div class="student-stat-value">${enrollmentCount}</div>
                    <div class="student-stat-label">Enrolled Courses</div>
                </div>
                <div class="student-stat-card">
                    <div class="student-stat-value">${gpa}</div>
                    <div class="student-stat-label">Current GPA</div>
                </div>
                <div class="student-stat-card">
                    <div class="student-stat-value">${creditsEarned}</div>
                    <div class="student-stat-label">Credits Earned</div>
                </div>
                <div class="student-stat-card">
                    <div class="student-stat-value">${enrollments.length > 0 ? Math.ceil(enrollments.length * 0.75) : 0}</div>
                    <div class="student-stat-label">Upcoming Exams</div>
                </div>
            `;
                document.getElementById('academicSummary').innerHTML = summaryHTML;

                // Display enrolled courses
                if (enrollments.length === 0) {
                    document.getElementById('enrolledCourses').innerHTML =
                        '<div style="text-align: center; padding: 20px;">No courses enrolled yet. <a href="#registration">Browse available courses</a></div>';
                    return;
                }

                const coursesHTML = enrollments.map((enrollment, index) => {
                    const colorClass = StudentDashboardService.getColorClass(index);

                    return `
                    <div class="student-course-card">
                        <div class="student-course-header ${colorClass}">
                            <div class="student-course-code">${enrollment.courseCode || 'N/A'}</div>
                            <div class="student-course-name">${enrollment.courseName || 'Course'}</div>
                        </div>
                        <div class="student-course-body">
                            <div class="student-course-meta">
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Instructor</div>
                                    <div class="student-course-meta-value">${enrollment.teacherName || 'TBA'}</div>
                                </div>
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Section</div>
                                    <div class="student-course-meta-value">${enrollment.section || 'N/A'}</div>
                                </div>
                                <div class="student-course-meta-item">
                                    <div class="student-course-meta-label">Status</div>
                                    <div class="student-course-meta-value">${enrollment.status || 'Active'}</div>
                                </div>
                            </div>
                            <div class="student-course-actions">
                                <button class="student-course-btn" data-course-id="${enrollment.id}">View Details</button>
                                <button class="student-course-btn secondary" data-course-instance-id="${enrollment.courseInstanceId}">Drop Course</button>
                            </div>
                        </div>
                    </div>
                `;
                }).join('');

                document.getElementById('enrolledCourses').innerHTML = coursesHTML;

                // Add drop course functionality
                document.querySelectorAll('[data-course-instance-id]').forEach(btn => {
                    btn.addEventListener('click', async () => {
                        if (confirm('Are you sure you want to drop this course?')) {
                            const response = await apiRequest('/enrollment/drop', 'POST', {
                                courseInstanceId: parseInt(btn.dataset.courseInstanceId)
                            });

                            if (response.ok) {
                                alert('Course dropped successfully');
                                window.location.reload();
                            } else {
                                alert('Failed to drop course: ' + (response.message || 'Unknown error'));
                            }
                        }
                    });
                });

            } catch (error) {
                console.error('Dashboard load error:', error);
                document.getElementById('enrolledCourses').innerHTML =
                    '<div style="text-align: center; padding: 20px; color: red;">Failed to load dashboard data</div>';
            }
        }
    },

    courses: {
        render: () => `
        <div class="student-breadcrumb">Home / My Courses</div>
        <div class="student-section-header">Your Enrolled Courses</div>
        <div id="coursesList" class="student-courses">
            <div style="text-align: center; padding: 20px;">Loading courses...</div>
        </div>

        <!-- Course Details Modal -->
        <div class="register-course-schedule-modal" id="courseDetailModal">
            <div class="register-course-modal-content">
                <div class="register-course-modal-header">
                    <h3 id="courseDetailTitle">Course Details</h3>
                    <button class="register-course-modal-close" onclick="document.getElementById('courseDetailModal').style.display='none'">×</button>
                </div>
                <div class="register-course-modal-body">
                    <div id="courseDetailContent"></div>
                </div>
            </div>
        </div>
    `,
        afterRender: async () => {
            const response = await apiRequest('/enrollment/my-enrollments?pageIndex=1&pageSize=100');

            if (!response.ok || !response.data) {
                document.getElementById('coursesList').innerHTML =
                    '<div style="text-align: center; padding: 20px;">Failed to load courses.</div>';
                return;
            }

            const courses = response.data.data || response.data || [];

            if (courses.length === 0) {
                document.getElementById('coursesList').innerHTML =
                    '<div style="text-align: center; padding: 20px;">No courses enrolled yet.</div>';
                return;
            }

            const coursesHTML = courses.map((enrollment, index) => {
                const colorClass = StudentDashboardService.getColorClass(index);

                return `
                <div class="student-course-card">
                    <div class="student-course-header ${colorClass}">
                        <div class="student-course-code">${enrollment.courseCode}</div>
                        <div class="student-course-name">${enrollment.courseName}</div>
                    </div>
                    <div class="student-course-body">
                        <div class="student-course-meta">
                            <div class="student-course-meta-item">
                                <div class="student-course-meta-label">Instructor</div>
                                <div class="student-course-meta-value">${enrollment.teacherName}</div>
                            </div>
                            <div class="student-course-meta-item">
                                <div class="student-course-meta-label">Section</div>
                                <div class="student-course-meta-value">${enrollment.section}</div>
                            </div>
                            <div class="student-course-meta-item">
                                <div class="student-course-meta-label">Status</div>
                                <div class="student-course-meta-value">${enrollment.status}</div>
                            </div>
                            <div class="student-course-meta-item">
                                <div class="student-course-meta-label">Enrolled</div>
                                <div class="student-course-meta-value">${new Date(enrollment.enrolledAt).toLocaleDateString()}</div>
                            </div>
                        </div>
                        <div class="student-course-actions">
                            <button class="student-course-btn" onclick="window.showCourseDetails(${enrollment.courseInstanceId}, '${enrollment.courseCode}', '${enrollment.courseName}')">View Details</button>
                            <button class="student-course-btn secondary" data-course-instance-id="${enrollment.courseInstanceId}">Drop Course</button>
                        </div>
                    </div>
                </div>
            `;
            }).join('');

            document.getElementById('coursesList').innerHTML = coursesHTML;

            // Show course details function
            window.showCourseDetails = async function (courseInstanceId, courseCode, courseName) {
                const modal = document.getElementById('courseDetailModal');
                const title = document.getElementById('courseDetailTitle');
                const content = document.getElementById('courseDetailContent');

                title.textContent = `${courseCode} - ${courseName}`;
                content.innerHTML = '<div style="text-align: center; padding: 20px;">Loading details...</div>';
                modal.style.display = 'flex';

                try {
                    const detailResponse = await apiRequest(`/courseInstance/${courseInstanceId}/enrollment-detail`);

                    if (!detailResponse.ok || !detailResponse.data) {
                        content.innerHTML = '<div style="text-align: center; padding: 20px; color: red;">Failed to load course details</div>';
                        return;
                    }

                    const details = detailResponse.data;

                    const getDayName = (dayNum) => {
                        const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
                        return days[dayNum] || 'TBA';
                    };

                    const formatTime = (time) => {
                        if (!time) return 'TBA';
                        const [hours, minutes] = time.split(':');
                        const h = parseInt(hours);
                        const ampm = h >= 12 ? 'PM' : 'AM';
                        const displayH = h > 12 ? h - 12 : (h === 0 ? 12 : h);
                        return `${displayH}:${minutes} ${ampm}`;
                    };

                    const scheduleDays = [];
                    if (details.day1 !== null && details.day1 !== undefined) {
                        scheduleDays.push(getDayName(details.day1));
                    }
                    if (details.day2 !== null && details.day2 !== undefined) {
                        scheduleDays.push(getDayName(details.day2));
                    }

                    const detailHTML = `
                    <div class="register-course-schedule-details">
                        <div class="register-course-schedule-item">
                            <label>Course Code:</label>
                            <span>${details.courseCode || courseCode}</span>
                        </div>
                        <div class="register-course-schedule-item">
                            <label>Course Name:</label>
                            <span>${details.courseName || courseName}</span>
                        </div>
                        <div class="register-course-schedule-item">
                            <label>Section:</label>
                            <span>${details.section || 'N/A'}</span>
                        </div>
                        <div class="register-course-schedule-item">
                            <label>Instructor:</label>
                            <span>${details.teacherName || 'TBA'}</span>
                        </div>
                        <div class="register-course-schedule-item">
                            <label>Credits:</label>
                            <span>${details.credits} Credits</span>
                        </div>
                        <div class="register-course-schedule-item">
                            <label>Room:</label>
                            <span>${details.location || 'TBA'}</span>
                        </div>
                        <div class="register-course-schedule-item">
                            <label>Schedule:</label>
                            <div class="register-course-schedule-days">
                                ${scheduleDays.length > 0
                            ? scheduleDays.map(day => `<span style="display: inline-block; background: #f0f0f0; padding: 4px 8px; border-radius: 3px; font-size: 11px; margin-right: 6px;">${day}</span>`).join('')
                            : '<span>TBA</span>'
                        }
                            </div>
                        </div>
                        <div class="register-course-schedule-item">
                            <label>Time:</label>
                            <span>${formatTime(details.startTime)} - ${formatTime(details.endTime)}</span>
                        </div>
                    </div>
                `;

                    content.innerHTML = detailHTML;

                } catch (error) {
                    console.error('Error loading course details:', error);
                    content.innerHTML = '<div style="text-align: center; padding: 20px; color: red;">Failed to load course details</div>';
                }
            };

            document.querySelectorAll('[data-course-instance-id]').forEach(btn => {
                btn.addEventListener('click', async () => {
                    if (confirm('Are you sure you want to drop this course?')) {
                        const dropResponse = await apiRequest('/enrollment/drop', 'POST', {
                            courseInstanceId: parseInt(btn.dataset.courseInstanceId)
                        });

                        if (dropResponse.ok) {
                            alert('Course dropped successfully');
                            window.location.reload();
                        } else {
                            alert('Failed to drop course: ' + (dropResponse.message || 'Unknown error'));
                        }
                    }
                });
            });

            document.getElementById('courseDetailModal').addEventListener('click', function (e) {
                if (e.target === this) {
                    this.style.display = 'none';
                }
            });
        }
    },

    schedule: {
        render: () => `
            <div class="student-breadcrumb">Home / Schedule</div>
            <div class="student-section-header">My Class Schedule</div>
            <div id="scheduleContainer" class="schedule-container">
                <div style="text-align: center; padding: 20px;">Loading schedule...</div>
            </div>
        `,
        afterRender: async () => {
            const response = await apiRequest('/courseInstance/my-schedule');

            if (!response.ok || !response.data || response.data.length === 0) {
                document.getElementById('scheduleContainer').innerHTML =
                    '<div style="text-align: center; padding: 20px;">No scheduled classes.</div>';
                return;
            }

            const courseInstances = response.data;

            const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
            const scheduleByDay = {};

            dayNames.forEach(day => {
                scheduleByDay[day] = [];
            });

            courseInstances.forEach(course => {
                if (course.day1) {
                    const dayName = dayNames[course.day1];
                    scheduleByDay[dayName].push({
                        courseName: course.courseName,
                        courseCode: course.courseCode,
                        instructor: course.teacherName,
                        location: course.location,
                        startTime: course.startTime,
                        endTime: course.endTime,
                        section: course.section
                    });
                }

                if (course.day2) {
                    const dayName = dayNames[course.day2];
                    scheduleByDay[dayName].push({
                        courseName: course.courseName,
                        courseCode: course.courseCode,
                        instructor: course.teacherName,
                        location: course.location,
                        startTime: course.startTime,
                        endTime: course.endTime,
                        section: course.section
                    });
                }
            });

            const scheduleHTML = dayNames.map(day => {
                const classes = scheduleByDay[day];

                if (classes.length === 0) {
                    return `
                        <div class="schedule-day-card empty">
                            <div class="schedule-day-header">${day}</div>
                            <div class="schedule-no-classes">No Classes</div>
                        </div>
                    `;
                }

                const classesHTML = classes.map(cls => `
                    <div class="schedule-class-item">
                        <div class="schedule-time">${cls.startTime} - ${cls.endTime}</div>
                        <div class="schedule-course">${cls.courseCode} - ${cls.courseName}</div>
                        <div class="schedule-meta">${cls.instructor} | ${cls.location || 'TBA'} | ${cls.section}</div>
                    </div>
                `).join('');

                return `
                    <div class="schedule-day-card">
                        <div class="schedule-day-header">${day}</div>
                        ${classesHTML}
                    </div>
                `;
            }).join('');

            document.getElementById('scheduleContainer').innerHTML = scheduleHTML;
        }
    },

    announcements: {
        render: () => `
        <div class="student-announcements-breadcrumb">Home / Announcements</div>
        <div class="student-announcements-section-header">Course Announcements</div>
        
        <div class="student-announcements-container">
            <div class="student-announcements-top-bar">
                <div class="student-announcements-selector">
                    <label>Select Course:</label>
                    <select id="courseSelect" class="student-announcements-course-select">
                        <option value="">Loading courses...</option>
                    </select>
                </div>
                <div class="student-announcements-info">
                    <span id="announcementCount">0 announcements</span>
                </div>
            </div>

            <div class="student-announcements-list" id="announcementsList"></div>
        </div>

        <div class="student-announcements-modal" id="detailModal" style="display: none;">
            <div class="student-announcements-modal-content">
                <div class="student-announcements-modal-header">
                    <div class="student-announcements-modal-header-content">
                        <h3 id="detailTitle"></h3>
                        <p id="detailCourseInfo"></p>
                    </div>
                    <button class="student-announcements-modal-close" onclick="document.getElementById('detailModal').style.display='none'">&times;</button>
                </div>
                <div class="student-announcements-modal-body">
                    <div id="detailUrgent" class="student-announcements-detail-urgent" style="display: none;">
                        <span class="student-announcements-urgent-badge-large">URGENT</span>
                    </div>
                    <p id="detailMessage" class="student-announcements-detail-message"></p>
                    <div class="student-announcements-detail-meta">
                        <div class="student-announcements-detail-meta-item">
                            <span class="student-announcements-detail-label">Posted:</span>
                            <span id="detailDate"></span>
                        </div>
                        <div class="student-announcements-detail-meta-item">
                            <span class="student-announcements-detail-label">By:</span>
                            <span id="detailTeacher"></span>
                        </div>
                        <div class="student-announcements-detail-meta-item">
                            <span class="student-announcements-detail-label">Course:</span>
                            <span id="detailCourse"></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `,
        afterRender: async () => {
            const courseSelect = document.getElementById('courseSelect');
            const announcementsList = document.getElementById('announcementsList');
            const announcementCount = document.getElementById('announcementCount');
            const detailModal = document.getElementById('detailModal');

            let courses = [];
            let allAnnouncements = [];
            let currentCourseId = null;

            const enrollResponse = await apiRequest('/enrollment/my-enrollments?pageIndex=1&pageSize=100');

            if (enrollResponse.ok && enrollResponse.data) {
                const responseData = enrollResponse.data.data || enrollResponse.data;

                if (responseData.length > 0) {
                    courses = responseData;

                    courseSelect.innerHTML = '<option value="">All Courses</option>' +
                        courses.map(course => `
                <option value="${course.courseInstanceId}">
                    ${course.courseCode} - ${course.courseName}
                </option>
            `).join('');

                    courseSelect.value = "";

                    loadAnnouncements();
                } else {
                    courseSelect.innerHTML = '<option value="">No courses enrolled</option>';
                    announcementsList.innerHTML = '<div class="student-announcements-empty">No courses available</div>';
                }
            } else {
                courseSelect.innerHTML = '<option value="">No courses enrolled</option>';
                announcementsList.innerHTML = '<div class="student-announcements-empty">No courses available</div>';
            }

            async function loadAnnouncements() {
                announcementsList.innerHTML = '<div style="text-align: center; padding: 40px 20px;">Loading announcements...</div>';

                const response = await apiRequest('/announcement/my-announcements?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data) {
                    announcementsList.innerHTML = '<div class="student-announcements-empty">Could not load announcements</div>';
                    announcementCount.textContent = '0 announcements';
                    return;
                }

                const responseData = response.data.data || response.data;

                allAnnouncements = responseData.sort((a, b) =>
                    new Date(b.publishedAt) - new Date(a.publishedAt)
                );

                displayAnnouncements();
            }

            function displayAnnouncements() {
                let filtered = allAnnouncements;

                if (currentCourseId) {
                    filtered = allAnnouncements.filter(a => a.targetCourseInstanceId === currentCourseId);
                }

                announcementCount.textContent = `${filtered.length} announcement${filtered.length !== 1 ? 's' : ''}`;

                if (filtered.length === 0) {
                    announcementsList.innerHTML = `
            <div class="student-announcements-empty">
                <p>No announcements ${currentCourseId ? 'for this course' : 'available'}</p>
            </div>
        `;
                    return;
                }

                announcementsList.innerHTML = filtered.map(ann => {
                    const course = courses.find(c => c.courseInstanceId === ann.targetCourseInstanceId) || {};

                    const isUrgent = ann.content && (ann.content.toLowerCase().includes('urgent') || ann.content.toLowerCase().includes('important'));

                    return `
            <div class="student-announcements-card ${isUrgent ? 'student-announcements-card-urgent' : ''}">
                <div class="student-announcements-card-header">
                    <div class="student-announcements-card-info">
                        <div class="student-announcements-card-meta">
                            <span class="student-announcements-card-course">${course.courseCode || 'N/A'}</span>
                            <span class="student-announcements-card-teacher">${course.teacherName || 'N/A'}</span>
                            ${isUrgent ? '<span class="student-announcements-urgent-badge">URGENT</span>' : ''}
                        </div>
                        <h4 class="student-announcements-card-title">${ann.title}</h4>
                    </div>
                    <span class="student-announcements-card-date">${new Date(ann.publishedAt).toLocaleDateString()}</span>
                </div>
                <div class="student-announcements-card-preview">
                    <p>${ann.content.substring(0, 120)}${ann.content.length > 120 ? '...' : ''}</p>
                </div>
                <div class="student-announcements-card-actions">
                    <button class="student-announcements-view-btn" onclick="window.viewAnnouncementDetail(${ann.id})">
                        View Details
                    </button>
                </div>
            </div>
        `;
                }).join('');
            }

            window.viewAnnouncementDetail = async (announcementId) => {
                const response = await apiRequest(`/announcement/${announcementId}/detail`);

                if (!response.ok || !response.data) {
                    alert('Could not load announcement details');
                    return;
                }

                const ann = response.data;
                const date = new Date(ann.publishedAt).toLocaleDateString('en-US', {
                    year: 'numeric',
                    month: 'long',
                    day: 'numeric'
                });
                const isUrgent = ann.content && (ann.content.toLowerCase().includes('urgent') || ann.content.toLowerCase().includes('important'));

                document.getElementById('detailTitle').textContent = ann.title;
                document.getElementById('detailMessage').textContent = ann.content;
                document.getElementById('detailDate').textContent = date;
                document.getElementById('detailTeacher').textContent = ann.createdByTeacherName;
                document.getElementById('detailCourse').textContent = `${ann.courseCode} - ${ann.courseName}`;
                document.getElementById('detailCourseInfo').textContent = `${ann.courseCode} - ${ann.courseName}`;

                const urgentDiv = document.getElementById('detailUrgent');
                if (isUrgent) {
                    urgentDiv.style.display = 'block';
                } else {
                    urgentDiv.style.display = 'none';
                }

                detailModal.style.display = 'flex';
            };

            courseSelect.addEventListener('change', () => {
                currentCourseId = courseSelect.value ? parseInt(courseSelect.value) : null;
                displayAnnouncements();
            });

            detailModal.addEventListener('click', (e) => {
                if (e.target === detailModal) {
                    detailModal.style.display = 'none';
                }
            });
        }
    },

    attendance: {
        render: () => `
        <div class="student-breadcrumb">Home / Attendance</div>
        <div class="student-section-header">My Attendance Record</div>
        <div id="attendanceContainer">
            <div style="text-align: center; padding: 20px;">Loading attendance...</div>
        </div>

        <div id="attendanceModal" class="student-attendance-modal" style="display: none;">
            <div class="student-attendance-modal-content">
                <div class="student-attendance-modal-header">
                    <h3 id="modalTitle"></h3>
                    <button class="student-attendance-modal-close">&times;</button>
                </div>
                <div class="student-attendance-modal-body">
                    <div id="weeklyContainer"></div>
                </div>
            </div>
        </div>
    `,
        afterRender: async () => {
            const response = await apiRequest('/attendance/my-attendances?pageIndex=1&pageSize=100');

            if (!response.ok || !response.data || response.data.length === 0) {
                document.getElementById('attendanceContainer').innerHTML =
                    '<div style="text-align: center; padding: 20px;">No attendance records yet.</div>';
                return;
            }

            const records = response.data;
            const grouped = {};

            records.forEach(record => {
                const key = `${record.courseCode}|${record.courseName}`;
                if (!grouped[key]) {
                    grouped[key] = {
                        courseCode: record.courseCode,
                        courseName: record.courseName,
                        records: []
                    };
                }
                grouped[key].records.push(record);
            });

            const courses = Object.values(grouped).map(course => {
                const total = course.records.length;
                const present = course.records.filter(r => r.status === 'Present').length;
                const absent = course.records.filter(r => r.status === 'Absent').length;
                const percent = total > 0 ? ((present / total) * 100).toFixed(1) : 0;

                return {
                    ...course,
                    total,
                    present,
                    absent,
                    percent
                };
            });

            const statusClass = (status) => {
                const map = {
                    'Present': 'present',
                    'Absent': 'absent',
                };
                return map[status] || '';
            };

            const html = `
            <div class="student-attendance-list">
                ${courses.map((course, idx) => `
                    <div class="student-attendance-row">
                        <div class="student-attendance-info">
                            <div class="student-attendance-course-title">
                                <strong>${course.courseCode}</strong> - ${course.courseName}
                            </div>
                            <div class="student-attendance-stats">
                                <span class="student-attendance-stat">Present: ${course.present}</span>
                                <span class="student-attendance-stat">Absent: ${course.absent}</span>
                            </div>
                        </div>
                        <div class="student-attendance-percent">
                            ${course.percent}%
                        </div>
                        <button class="student-attendance-btn" data-course-index="${idx}">
                            View Weekly
                        </button>
                    </div>
                `).join('')}
            </div>
        `;

            document.getElementById('attendanceContainer').innerHTML = html;

            const closeBtn = document.querySelector('.student-attendance-modal-close');
            closeBtn.addEventListener('click', () => {
                document.getElementById('attendanceModal').style.display = 'none';
            });

            document.getElementById('attendanceModal').addEventListener('click', (e) => {
                if (e.target.id === 'attendanceModal') {
                    document.getElementById('attendanceModal').style.display = 'none';
                }
            });

            document.querySelectorAll('.student-attendance-btn').forEach(btn => {
                btn.addEventListener('click', () => {
                    const idx = parseInt(btn.dataset.courseIndex);
                    showWeekly(courses[idx]);
                });
            });

            function showWeekly(course) {
                document.getElementById('modalTitle').textContent =
                    `${course.courseCode} - ${course.courseName}`;

                const weeks = {};
                course.records.forEach(record => {
                    const weekNum = record.week;
                    if (!weeks[weekNum]) {
                        weeks[weekNum] = [];
                    }
                    weeks[weekNum].push(record);
                });

                const sortedWeeks = Object.keys(weeks).map(Number).sort((a, b) => a - b);

                const html = `
            ${sortedWeeks.map(weekNum => {
                    const records = weeks[weekNum];

                    return `
                    <div class="student-attendance-week">
                        <div class="student-attendance-week-title">
                            Week ${weekNum}
                        </div>
                        <div class="student-attendance-week-grid">
                            ${records.map(record => {
                        const date = new Date(record.createdAt);
                        const dayName = date.toLocaleDateString('en-US', { weekday: 'short' });
                        const dayDate = date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });

                        return `
                                    <div class="student-attendance-day-box student-attendance-day-${statusClass(record.status)}">
                                        <div class="student-attendance-day-name">${dayName}</div>
                                        <div class="student-attendance-day-date">${dayDate}</div>
                                        <div class="student-attendance-day-status">${record.status}</div>                                          
                                    </div>
                                `;
                    }).join('')}
                        </div>
                    </div>
                `;
                }).join('')}
        `;

                document.getElementById('weeklyContainer').innerHTML = html;
                document.getElementById('attendanceModal').style.display = 'flex';
            }
        }
    },

    assignments: {
        render: () => `
                    <div class="student-breadcrumb">Home / Assignments</div>
                    <div class="student-section-header">My Assignments</div>

                    <div class="student-assignment-container">
        
                        <div class="student-assignment-tabs">
                            <button class="student-assignment-tab active" onclick="window.filterAssignments('all')">All</button>
                            <button class="student-assignment-tab" onclick="window.filterAssignments('pending')">Pending</button>
                            <button class="student-assignment-tab" onclick="window.filterAssignments('submitted')">Submitted</button>
                            <button class="student-assignment-tab" onclick="window.filterAssignments('graded')">Graded</button>
                        </div>

                        <div class="student-assignment-list" id="assignmentsList">
                            <div style="text-align: center; padding: 40px;">Loading assignments...</div>
                        </div>
                    </div>

                    <div class="student-assignment-submit-modal" id="submitModal">
                        <div class="student-assignment-modal-content">
                            <div class="student-assignment-modal-header">
                                <h3 id="submitTitle"></h3>
                                <button class="student-assignment-modal-close" onclick="document.getElementById('submitModal').style.display='none'">×</button>
                            </div>
            
                            <div class="student-assignment-modal-body">
                                <div class="student-assignment-form-group">
                                    <label>Course:</label>
                                    <p id="submitCourse" class="student-assignment-course-name"></p>
                                </div>

                                <div class="student-assignment-form-group">
                                    <label>Assignment Description:</label>
                                    <p id="submitDescription" class="student-assignment-description"></p>
                                </div>

                                <div class="student-assignment-form-group">
                                    <label>Due Date:</label>
                                    <p id="submitDueDate" class="student-assignment-due-date"></p>
                                </div>

                                <div class="student-assignment-form-group">
                                    <label>Total Points:</label>
                                    <p id="submitPoints" class="student-assignment-points"></p>
                                </div>

                                <div class="student-assignment-form-group">
                                    <label>Upload File</label>
                                    <input type="file" id="submitFile" class="student-assignment-file-input" 
                                           accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt,.zip,.rar,.7z,.tar,.gz,.jpg,.jpeg,.png,.gif,.bmp,.svg">
                                    <p class="student-assignment-file-hint">
                                        Supported: PDF, Word, Excel, PowerPoint, Images, Text, Archives (ZIP, RAR, 7Z, TAR, GZ)<br>
                                        <strong>Note:</strong> For multiple files, please compress them into a ZIP or RAR archive first. Max file size: 50MB
                                    </p>
                                    <div id="filePreview" style="margin-top: 10px;"></div>
                                </div>

                                <div class="student-assignment-form-group">
                                    <label>Submission Text</label>
                                    <textarea id="submitText" class="student-assignment-textarea" placeholder="Enter your submission text or additional comments..." rows="8"></textarea>
                                </div>
                            </div>

                            <div class="student-assignment-modal-footer">
                                <button class="student-assignment-btn-cancel" onclick="document.getElementById('submitModal').style.display='none'">Cancel</button>
                                <button class="student-assignment-btn-submit" onclick="window.submitAssignment()">Submit Assignment</button>
                            </div>
                        </div>
                    </div>

                    <div class="student-assignment-modal" id="gradeModal">
                        <div class="student-assignment-modal-content">
                            <div class="student-assignment-modal-header">
                                <h3>Assignment Grade</h3>
                                <button class="student-assignment-modal-close" onclick="document.getElementById('gradeModal').style.display='none'">×</button>
                            </div>
            
                            <div class="student-assignment-modal-body">
                                <div class="student-assignment-form-group">
                                    <label>Assignment:</label>
                                    <p id="gradeTitle" style="margin: 0; font-weight: 600;"></p>
                                </div>

                                <div class="student-assignment-form-group">
                                    <label>Grade:</label>
                                    <p id="gradeScore" style="margin: 0; font-size: 24px; font-weight: 700; color: #2ecc71;"></p>
                                </div>

                                <div class="student-assignment-form-group">
                                    <label>Feedback:</label>
                                    <p id="gradeFeedback" style="margin: 0; color: #555;"></p>
                                </div>

                                <div class="student-assignment-form-group">
                                    <label>Submitted:</label>
                                    <p id="gradeSubmittedAt" style="margin: 0; color: #666;"></p>
                                </div>
                            </div>

                            <div class="student-assignment-modal-footer">
                                <button class="student-assignment-btn-cancel" onclick="document.getElementById('gradeModal').style.display='none'">Close</button>
                            </div>
                        </div>
                    </div>
                `,
        afterRender: async () => {
            let allAssignments = [];
            let currentFilter = 'all';
            let currentSubmittingAssignment = null;
            let selectedFile = null;
            const fileInput = document.getElementById('submitFile');
            if (fileInput) {
                fileInput.addEventListener('change', function (e) {
                    if (e.target.files.length > 0) {
                        handleFileSelection(e.target.files[0]);
                    }
                });
            }

            function handleFileSelection(file) {
                const maxSize = 50 * 1024 * 1024;
                if (file.size > maxSize) {
                    alert('File size cannot exceed 50MB. Please choose a smaller file or compress it.');
                    fileInput.value = '';
                    return;
                }
                const allowedExtensions = ['pdf', 'doc', 'docx', 'xls', 'xlsx', 'ppt', 'pptx', 'txt', 'zip', 'rar', '7z', 'tar', 'gz', 'jpg', 'jpeg', 'png', 'gif', 'bmp', 'svg'];
                const extension = file.name.split('.').pop().toLowerCase();
                if (!allowedExtensions.includes(extension)) {
                    alert(`File type .${extension} is not supported.`);
                    fileInput.value = '';
                    return;
                }
                selectedFile = file;
                displayFilePreview();
            }

            function displayFilePreview() {
                const previewDiv = document.getElementById('filePreview');
                if (!selectedFile) {
                    previewDiv.innerHTML = '';
                    return;
                }
                const fileSizeMB = (selectedFile.size / (1024 * 1024)).toFixed(2);
                const fileIcon = getFileIcon(selectedFile.name);
                previewDiv.innerHTML = `<div style="background: #e8f5e9; padding: 12px; border-radius: 6px; border: 1px solid #81c784; display: flex; justify-content: space-between; align-items: center;"><div style="display: flex; align-items: center; gap: 10px;"><span style="font-size: 24px;">${fileIcon}</span><div><div style="font-weight: 600; color: #2e7d32; font-size: 13px;">${selectedFile.name}</div><div style="color: #558b2f; font-size: 11px;">${fileSizeMB} MB</div></div></div><button onclick="window.removeFile()" style="background: #c62828; color: white; border: none; padding: 6px 12px; border-radius: 4px; cursor: pointer; font-size: 11px; font-weight: 600;">Remove</button></div>`;
            }

            function getFileIcon(filename) {
                const ext = filename.split('.').pop().toLowerCase();
                const icons = {
                    'pdf': '📄',
                    'doc': '📝',
                    'docx': '📝',
                    'xls': '📊',
                    'xlsx': '📊',
                    'ppt': '📽️',
                    'pptx': '📽️',
                    'txt': '📃',
                    'zip': '🗜️',
                    'rar': '🗜️',
                    '7z': '🗜️',
                    'tar': '🗜️',
                    'gz': '🗜️',
                    'jpg': '🖼️',
                    'jpeg': '🖼️',
                    'png': '🖼️',
                    'gif': '🖼️',
                    'bmp': '🖼️',
                    'svg': '🖼️'
                };
                return icons[ext] || '📎';
            }
            window.removeFile = function () {
                selectedFile = null;
                fileInput.value = '';
                displayFilePreview();
            };
            async function loadAssignments() {
                document.getElementById('assignmentsList').innerHTML = '<div style="text-align: center; padding: 40px;">Loading assignments...</div>';
                const response = await apiRequest('/assignment/my-assignments');
                if (!response.ok || !response.data) {
                    document.getElementById('assignmentsList').innerHTML = `<div class="student-assignment-empty"><p>Failed to load assignments</p><p>Please try again</p></div>`;
                    return;
                }
                allAssignments = response.data.map(a => ({
                    ...a,
                    status: a.grade !== null && a.grade !== undefined ? 'graded' : a.isSubmitted ? 'submitted' : 'pending'
                }));
                renderAssignments();
            }

            function renderAssignments() {
                const filtered = currentFilter === 'all' ? allAssignments : allAssignments.filter(a => a.status === currentFilter);
                const list = document.getElementById('assignmentsList');
                if (filtered.length === 0) {
                    list.innerHTML = `<div class="student-assignment-empty"><p>No assignments in this category</p></div>`;
                    return;
                }
                list.innerHTML = filtered.map(assign => {
                    const dueDate = new Date(assign.dueDate);
                    const today = new Date();
                    const isOverdue = dueDate < today && assign.status === 'pending';
                    return `<div class="student-assignment-card ${isOverdue ? 'student-assignment-card-overdue' : ''}"><div class="student-assignment-card-header"><div><span class="student-assignment-course">${assign.courseCode}</span><h4 class="student-assignment-card-title">${assign.title}</h4></div><span class="student-assignment-card-points">${assign.totalPoints} pts</span></div><div class="student-assignment-card-content"><p>${assign.description}</p><div class="student-assignment-card-meta"><span>Due: ${new Date(assign.dueDate).toLocaleDateString()}</span><span class="student-assignment-status-badge student-assignment-status-${assign.status}">${assign.status.charAt(0).toUpperCase() + assign.status.slice(1)}</span>${assign.grade !== null && assign.grade !== undefined ? `<span class="student-assignment-grade">${assign.grade}/${assign.totalPoints}</span>` : ''}</div><div class="student-assignment-teacher"><span>Instructor: ${assign.teacherName}</span></div></div><div class="student-assignment-card-actions">${assign.status === 'pending' ? `<button class="student-assignment-submit-btn" onclick="window.openSubmitModal(${assign.id}, '${assign.title.replace(/'/g, "\\'")}', '${assign.description.replace(/'/g, "\\'")}', '${assign.dueDate}', '${assign.courseCode} - ${assign.courseName}', ${assign.totalPoints})">Submit</button>` : ''}${assign.status === 'graded' ? `<button class="student-assignment-view-grade-btn" onclick="window.viewGrade('${assign.title.replace(/'/g, "\\'")}', ${assign.grade}, ${assign.totalPoints}, '${assign.feedback ? assign.feedback.replace(/'/g, "\\'") : 'No feedback provided'}', '${assign.submittedAt}')">View Grade</button>` : ''}${assign.status === 'submitted' ? `<button class="student-assignment-view-grade-btn" disabled style="opacity: 0.6; cursor: not-allowed;">Awaiting Grade</button>` : ''}</div></div>`;
                }).join('');
            }
            window.filterAssignments = function (filter) {
                currentFilter = filter;
                document.querySelectorAll('.student-assignment-tab').forEach(tab => {
                    tab.classList.remove('active');
                });
                event.target.classList.add('active');
                renderAssignments();
            };
            window.openSubmitModal = function (assignmentId, title, description, dueDate, courseName, totalPoints) {
                currentSubmittingAssignment = assignmentId;
                selectedFile = null;
                document.getElementById('submitTitle').textContent = title;
                document.getElementById('submitCourse').textContent = courseName;
                document.getElementById('submitDescription').textContent = description;
                document.getElementById('submitDueDate').textContent = new Date(dueDate).toLocaleDateString();
                document.getElementById('submitPoints').textContent = totalPoints + ' points';
                document.getElementById('submitText').value = '';
                fileInput.value = '';
                displayFilePreview();
                document.getElementById('submitModal').style.display = 'flex';
            };
            window.submitAssignment = async function () {
                const submissionText = document.getElementById('submitText').value.trim();
                if (!submissionText && !selectedFile) {
                    alert('Please provide either submission text or upload a file');
                    return;
                }
                const formData = new FormData();
                formData.append('assignmentId', currentSubmittingAssignment);
                formData.append('submissionText', submissionText);
                if (selectedFile) {
                    formData.append('file', selectedFile);
                }
                const submitBtn = document.querySelector('.student-assignment-btn-submit');
                const originalText = submitBtn.textContent;
                submitBtn.textContent = 'Submitting...';
                submitBtn.disabled = true;

                try {
                    // 4. Send to Backend
                    // IMPORTANT: Ensure your apiRequest helper does NOT set 'Content-Type': 'application/json' 
                    // when passing FormData. The browser must set the Content-Type boundary automatically.
                    const response = await apiRequest('/assignment/submit', 'POST', formData);

                    if (response.ok) {
                        alert('Assignment submitted successfully!');
                        document.getElementById('submitModal').style.display = 'none';

                        // Clear inputs
                        document.getElementById('submitText').value = '';
                        window.removeFile();

                        // Refresh list
                        await loadAssignments();
                    } else {
                        alert(response.message || 'Failed to submit assignment. Please try again.');
                    }
                } catch (error) {
                    console.error('Submission error:', error);
                    alert('An error occurred while submitting. Please check your connection.');
                } finally {
                    // 5. Reset Button
                    submitBtn.textContent = originalText;
                    submitBtn.disabled = false;
                }

                document.getElementById('submitModal').style.display = 'none';
                await loadAssignments();
            };
            window.viewGrade = function (title, grade, totalPoints, feedback, submittedAt) {
                document.getElementById('gradeTitle').textContent = title;
                document.getElementById('gradeScore').textContent = `${grade} / ${totalPoints}`;
                document.getElementById('gradeFeedback').textContent = feedback;
                document.getElementById('gradeSubmittedAt').textContent = submittedAt ? new Date(submittedAt).toLocaleString() : 'N/A';
                document.getElementById('gradeModal').style.display = 'flex';
            };
            document.getElementById('submitModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });
            document.getElementById('gradeModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });
            await loadAssignments();
        }
    },

    registration: {
        render: () => `
    <div class="student-breadcrumb">Home / Course Registration</div>
    
    <div class="register-course-curriculum-header">
        <h2>Course Registration 2024-2025</h2>
        <p>Select your year and semester, then choose courses to register</p>
    </div>

    <div class="register-course-tabs-years">
        <button class="register-course-year-tab active" data-year="1">1st Year</button>
        <button class="register-course-year-tab" data-year="2">2nd Year</button>
        <button class="register-course-year-tab" data-year="3">3rd Year</button>
        <button class="register-course-year-tab" data-year="4">4th Year</button>
    </div>

    <div class="register-course-tabs-semesters">
        <button class="register-course-semester-tab active" data-semester="fall">Fall Semester</button>
        <button class="register-course-semester-tab" data-semester="spring">Spring Semester</button>
    </div>

    <div class="register-course-main-wrapper">
        <div class="register-course-selection-box">
            <div class="register-course-selection-header">
                <h3>Curriculum</h3>
                <p id="semesterInfo">1st Year - Fall Semester</p>
            </div>
            <div class="register-course-selection-list" id="coursesList"></div>
        </div>

        <div class="register-course-selected-box">
            <div class="register-course-selected-box-header">
                <h3>My Selected Courses (<span id="selectedCount">0</span>)</h3>
                <div class="register-course-credits-indicator">
                    <div class="register-course-credits-bar">
                        <div class="register-course-credits-fill" id="creditsFill" style="width: 0%"></div>
                    </div>
                    <span id="creditsText">0/25 Credits</span>
                </div>
            </div>
            <div class="register-course-selected-grid" id="selectedGrid">
                <div class="register-course-selected-empty-state">
                    <p>No courses selected yet. Click + to add courses from the curriculum above.</p>
                </div>
            </div>
            <button class="register-course-confirm-btn" id="confirmBtn" disabled>Confirm Registration</button>
        </div>
    </div>

    <div class="register-course-instances-modal" id="instancesModal">
        <div class="register-course-modal-content">
            <div class="register-course-modal-header">
                <h3 id="instancesModalTitle"></h3>
                <button class="register-course-modal-close" onclick="document.getElementById('instancesModal').style.display='none'">×</button>
            </div>
            <div class="register-course-modal-body">
                <div id="collisionWarning"></div>
                <div class="register-course-instances-list" id="instancesList"></div>
            </div>
        </div>
    </div>

    <div class="register-course-schedule-modal" id="scheduleModal">
        <div class="register-course-modal-content">
            <div class="register-course-modal-header">
                <h3>Course Instance Details</h3>
                <button class="register-course-modal-close" onclick="document.getElementById('scheduleModal').style.display='none'">×</button>
            </div>
            <div class="register-course-modal-body">
                <div id="scheduleContent"></div>
            </div>
        </div>
    </div>
`,
        afterRender: async () => {
            const selectedCourses = new Map();
            let currentYear = 1;
            let currentSemester = 'fall';
            let allCourses = [];
            let courseInstances = {};

            const getDayName = (dayNum) => {
                const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
                return days[dayNum] || 'TBA';
            };

            const getDayAbbr = (dayNum) => {
                const abbr = { 0: 'Sun', 1: 'Mon', 2: 'Tue', 3: 'Wed', 4: 'Thu', 5: 'Fri', 6: 'Sat' };
                return abbr[dayNum] || 'TBA';
            };

            const formatTime = (time) => {
                if (!time) return 'TBA';
                const [hours, minutes] = time.split(':');
                const h = parseInt(hours);
                const ampm = h >= 12 ? 'pm' : 'am';
                const displayH = h > 12 ? h - 12 : (h === 0 ? 12 : h);
                return `${displayH}:${minutes}${ampm}`;
            };

            const formatSchedule = (day1, day2, startTime) => {
                if (day1 === null || day1 === undefined || !startTime) return 'TBA';
                const day1Str = getDayAbbr(day1);
                const day2Str = day2 !== null && day2 !== undefined ? getDayAbbr(day2) : '';
                const timeStr = formatTime(startTime);

                if (day2Str && day2Str !== 'TBA') {
                    return `${day1Str}/${day2Str} ${timeStr}`;
                }
                return `${day1Str} ${timeStr}`;
            };

            const loadAcademicYear = async () => {
                const response = await apiRequest('/academicYear/active');
                if (!response.ok || !response.data) return false;
                return true;
            };

            const loadAllCourses = async () => {
                const response = await apiRequest(`/courseInstance/available/1?pageIndex=1&pageSize=100`);
                if (!response.ok || !response.data) return;

                const courses = response.data.data || response.data || [];

                courseInstances = {};
                courses.forEach(course => {
                    if (!courseInstances[course.courseCode]) {
                        courseInstances[course.courseCode] = [];
                    }

                    const days = [];
                    if (course.day1 !== null && course.day1 !== undefined) {
                        days.push(getDayName(course.day1));
                    }
                    if (course.day2 !== null && course.day2 !== undefined) {
                        days.push(getDayName(course.day2));
                    }

                    courseInstances[course.courseCode].push({
                        id: course.courseInstanceId,
                        instructor: course.teacherName,
                        day1: course.day1,
                        day2: course.day2,
                        days: days.length > 0 ? days : ['TBA'],
                        startTime: course.startTime || '00:00',
                        endTime: course.endTime || '00:00',
                        room: course.location || 'TBA',
                        seats: `${course.currentEnrollmentCount}/${course.capacity}`,
                        credits: course.credits || 3
                    });
                });

                allCourses = [...new Map(courses.map(item => [item.courseCode, item])).values()];
                filterAndDisplayCourses();
            };

            const checkTimeConflict = (instance1, instance2) => {
                const days1 = new Set(instance1.days || []);
                const days2 = new Set(instance2.days || []);

                let shareDay = false;
                for (let day of days1) {
                    if (days2.has(day)) {
                        shareDay = true;
                        break;
                    }
                }

                if (!shareDay) return false;

                const time1Start = parseInt(instance1.startTime.replace(':', '') || 0);
                const time1End = parseInt(instance1.endTime.replace(':', '') || 0);
                const time2Start = parseInt(instance2.startTime.replace(':', '') || 0);
                const time2End = parseInt(instance2.endTime.replace(':', '') || 0);

                return time1Start < time2End && time2Start < time1End;
            };

            const parseCourseCode = (courseCode) => {
                const numericMatch = courseCode.match(/\d+/);
                if (!numericMatch) return { year: 1, semester: 'fall' };

                const numericPart = numericMatch[0];
                const firstDigit = parseInt(numericPart.charAt(0));
                const lastDigit = parseInt(numericPart.charAt(numericPart.length - 1));

                const year = (firstDigit >= 1 && firstDigit <= 4) ? firstDigit : 1;
                const semester = lastDigit === 2 ? 'spring' : 'fall';

                return { year, semester };
            };


            const filterAndDisplayCourses = () => {
                const coursesList = document.getElementById('coursesList');
                const yearName = currentYear === 1 ? '1st' : currentYear === 2 ? '2nd' : currentYear === 3 ? '3rd' : '4th';
                const semesterName = currentSemester === 'fall' ? 'Fall' : 'Spring';

                document.getElementById('semesterInfo').textContent = `${yearName} Year - ${semesterName} Semester`;

                if (allCourses.length === 0) {
                    coursesList.innerHTML = '<div style="text-align: center; padding: 20px; color: #999;">No courses available</div>';
                    return;
                }

                const filtered = allCourses.filter(course => {
                    const { year, semester } = parseCourseCode(course.courseCode);
                    return year === currentYear && semester === currentSemester;
                });

                if (filtered.length === 0) {
                    coursesList.innerHTML = '<div style="text-align: center; padding: 20px; color: #999;">No courses available for this selection</div>';
                    return;
                }

                coursesList.innerHTML = filtered.map(course => {
                    const isSelected = selectedCourses.has(course.courseCode);
                    return `
            <div class="register-course-selection-item ${isSelected ? 'register-course-selection-item-selected' : ''}">
                <div class="register-course-selection-item-info">
                    <div class="register-course-selection-item-code">${course.courseCode}</div>
                    <div class="register-course-selection-item-name">${course.courseName}</div>
                    <div class="register-course-selection-item-credits">${course.credits || 3} Credits</div>
                </div>
                <div class="register-course-selection-item-actions">
                    ${isSelected ? '<span class="register-course-selection-item-tick">✓</span>' : ''}
                    <button class="register-course-selection-item-btn ${isSelected ? 'register-course-selection-item-btn-selected' : ''}" onclick="window.showInstancesModal('${course.courseCode}', '${course.courseName}', ${course.credits || 3})">${isSelected ? '✓' : '+'}</button>
                </div>
            </div>
        `;
                }).join('');
            };

            document.querySelectorAll('.register-course-year-tab').forEach(tab => {
                tab.addEventListener('click', function () {
                    document.querySelectorAll('.register-course-year-tab').forEach(t => t.classList.remove('active'));
                    this.classList.add('active');
                    currentYear = parseInt(this.dataset.year);
                    filterAndDisplayCourses();
                });
            });

            document.querySelectorAll('.register-course-semester-tab').forEach(tab => {
                tab.addEventListener('click', function () {
                    document.querySelectorAll('.register-course-semester-tab').forEach(t => t.classList.remove('active'));
                    this.classList.add('active');
                    currentSemester = this.dataset.semester;
                    filterAndDisplayCourses();
                });
            });

            window.showInstancesModal = function (courseCode, courseName, credits) {
                const modal = document.getElementById('instancesModal');
                const title = document.getElementById('instancesModalTitle');
                const list = document.getElementById('instancesList');
                const warning = document.getElementById('collisionWarning');

                title.textContent = `${courseCode} - ${courseName}`;
                const instances = courseInstances[courseCode] || [];

                warning.innerHTML = '';

                if (instances.length === 0) {
                    list.innerHTML = '<p style="text-align:center;color:#999;">No instances available</p>';
                } else {
                    list.innerHTML = instances.map(inst => {
                        let hasConflict = false;
                        let conflictCourse = '';

                        for (let [selCode, selData] of selectedCourses) {
                            const selInstances = courseInstances[selCode] || [];
                            const selInstance = selInstances.find(i => i.id === selData.instance);
                            if (selInstance && checkTimeConflict(inst, selInstance)) {
                                hasConflict = true;
                                conflictCourse = selCode;
                                break;
                            }
                        }

                        const schedule = formatSchedule(inst.day1, inst.day2, inst.startTime);

                        return `
                        <div class="register-course-instance-card ${hasConflict ? 'register-course-instance-card-conflict' : ''}">
                            ${hasConflict ? `<div class="register-course-instance-conflict-badge">CONFLICT with ${conflictCourse}</div>` : ''}
                            <div class="register-course-instance-info">
                                <div class="register-course-instance-id">Section ${inst.id}</div>
                                <div class="register-course-instance-meta">
                                    <span>Instructor: ${inst.instructor}</span>
                                    <span>Room: ${inst.room}</span>
                                    <span>Seats: ${inst.seats}</span>
                                </div>
                                <div class="register-course-instance-time">
                                    <span class="register-course-instance-day">${inst.days.join(', ')}</span>
                                    <span class="register-course-instance-hour">${formatTime(inst.startTime)} - ${formatTime(inst.endTime)}</span>
                                </div>
                            </div>
                            <div class="register-course-instance-actions">
                                <button class="register-course-info-btn" onclick="window.showScheduleDetails('${inst.id}', '${courseName}', '${inst.days.join(', ')}', '${formatTime(inst.startTime)} - ${formatTime(inst.endTime)}', '${inst.instructor}', '${inst.room}')">Info</button>
                                <button class="register-course-select-btn ${hasConflict ? 'register-course-select-btn-disabled' : ''}" onclick="window.selectInstance('${courseCode}', '${courseName}', ${credits}, '${inst.id}')" ${hasConflict ? 'disabled' : ''}>Select</button>
                            </div>
                        </div>
                    `;
                    }).join('');
                }
                modal.style.display = 'flex';
            };

            window.selectInstance = function (courseCode, courseName, credits, instanceId) {
                selectedCourses.set(courseCode, { name: courseName, credits, instance: instanceId });
                document.getElementById('instancesModal').style.display = 'none';
                filterAndDisplayCourses();
                updateSelectedGrid();
            };

            window.showScheduleDetails = function (instanceId, courseName, days, times, instructor, room) {
                const modal = document.getElementById('scheduleModal');
                const content = document.getElementById('scheduleContent');
                const dayArray = days.split(', ');

                const scheduleHTML = `
                <div class="register-course-schedule-details">
                    <div class="register-course-schedule-item">
                        <label>Instance:</label>
                        <span>${instanceId}</span>
                    </div>
                    <div class="register-course-schedule-item">
                        <label>Course:</label>
                        <span>${courseName}</span>
                    </div>
                    <div class="register-course-schedule-item">
                        <label>Instructor:</label>
                        <span>${instructor}</span>
                    </div>
                    <div class="register-course-schedule-item">
                        <label>Room:</label>
                        <span>${room}</span>
                    </div>
                    <div class="register-course-schedule-item">
                        <label>Schedule:</label>
                        <div class="register-course-schedule-days">
                            ${dayArray.map(day => `<span style="display: inline-block; background: #f0f0f0; padding: 4px 8px; border-radius: 3px; font-size: 11px; margin-right: 6px;">${day}</span>`).join('')}
                        </div>
                    </div>
                    <div class="register-course-schedule-item">
                        <label>Time:</label>
                        <span>${times}</span>
                    </div>
                </div>
            `;

                content.innerHTML = scheduleHTML;
                modal.style.display = 'flex';
            };

            const updateSelectedGrid = () => {
                const grid = document.getElementById('selectedGrid');
                const count = document.getElementById('selectedCount');
                const creditsFill = document.getElementById('creditsFill');
                const creditsText = document.getElementById('creditsText');
                const confirmBtn = document.getElementById('confirmBtn');

                const totalCredits = Array.from(selectedCourses.values()).reduce((sum, c) => sum + c.credits, 0);
                const creditsPercent = (totalCredits / 25) * 100;

                count.textContent = selectedCourses.size;
                creditsText.textContent = `${totalCredits}/25 Credits`;
                creditsFill.style.width = creditsPercent + '%';
                creditsFill.style.background = totalCredits > 25 ? '#ff6b6b' : '#27ae60';

                confirmBtn.disabled = selectedCourses.size === 0 || totalCredits > 25;

                if (selectedCourses.size === 0) {
                    grid.innerHTML = '<div class="register-course-selected-empty-state"><p>No courses selected yet. Click + to add courses from the curriculum above.</p></div>';
                } else {
                    grid.innerHTML = Array.from(selectedCourses.entries()).map(([courseCode, data]) => `
                    <div class="register-course-selected-card">
                        <div class="register-course-selected-card-header">
                            <div class="register-course-selected-card-code">${courseCode}</div>
                            <button class="register-course-selected-card-remove" onclick="window.removeSelected('${courseCode}')">×</button>
                        </div>
                        <div class="register-course-selected-card-body">
                            <div class="register-course-selected-card-name">${data.name}</div>
                            <div class="register-course-selected-card-instance">${data.instance}</div>
                            <div class="register-course-selected-card-credits">${data.credits} Credits</div>
                        </div>
                    </div>
                `).join('');
                }
            };

            window.removeSelected = function (courseCode) {
                selectedCourses.delete(courseCode);
                filterAndDisplayCourses();
                updateSelectedGrid();
            };

            document.getElementById('instancesModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            document.getElementById('scheduleModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            document.getElementById('confirmBtn').addEventListener('click', async function () {
                const confirmBtn = this;
                confirmBtn.disabled = true;
                confirmBtn.textContent = 'Registering...';

                let successCount = 0;
                let failCount = 0;

                for (let [courseCode, data] of selectedCourses) {
                    const response = await apiRequest('/enrollment', 'POST', { courseInstanceId: data.instance });

                    if (response.ok) {
                        successCount++;
                    } else {
                        failCount++;
                    }
                }

                const totalCredits = Array.from(selectedCourses.values()).reduce((sum, c) => sum + c.credits, 0);
                alert(`Successfully registered for ${successCount} courses (${totalCredits} credits)!`);
                console.log('Selected courses:', Array.from(selectedCourses.entries()));

                confirmBtn.disabled = false;
                confirmBtn.textContent = 'Confirm Registration';
            });

            if (await loadAcademicYear()) {
                await loadAllCourses();
            }
        }
    },

    grades: {
        render: () => `
        <div class="student-breadcrumb">Home / Grades & Transcripts</div>
        <div class="student-section-header">Your Grades</div>
        <div id="gradesContainer">
            <div style="text-align: center; padding: 20px;">Loading grades...</div>
        </div>
    `,
        afterRender: async () => {
            try {
                const [enrollmentsResponse, gradesResponse] = await Promise.all([
                    apiRequest('/enrollment/my-enrollments?pageIndex=1&pageSize=100'),
                    apiRequest('/grade/my-grades?pageIndex=1&pageSize=100')
                ]);

                if (!enrollmentsResponse.ok || !enrollmentsResponse.data) {
                    document.getElementById('gradesContainer').innerHTML =
                        '<div style="text-align: center; padding: 20px;">Failed to load courses.</div>';
                    return;
                }

                const enrollments = enrollmentsResponse.data.data || enrollmentsResponse.data || [];
                const grades = gradesResponse.ok && gradesResponse.data
                    ? (gradesResponse.data.data || gradesResponse.data || [])
                    : [];

                if (enrollments.length === 0) {
                    document.getElementById('gradesContainer').innerHTML =
                        '<div style="text-align: center; padding: 20px;">No courses enrolled yet.</div>';
                    return;
                }

                const courseGrades = enrollments.map(enrollment => {
                    const gradeData = grades.find(g =>
                        g.courseCode === enrollment.courseCode ||
                        g.courseInstanceId === enrollment.courseInstanceId
                    );

                    return {
                        courseCode: enrollment.courseCode,
                        courseName: enrollment.courseName,
                        creditHours: enrollment.creditHours || 3,
                        exam1: gradeData?.exam1 || null,
                        exam2: gradeData?.exam2 || null,
                        final: gradeData?.final || null,
                        project: gradeData?.project || null,
                        letterGrade: gradeData?.letterGrade || null,
                        hasGrade: !!gradeData
                    };
                });

                const coursesWithGrades = courseGrades.filter(c => c.letterGrade);
                const gpa = StudentDashboardService.calculateGPA(coursesWithGrades);
                const totalCredits = courseGrades.reduce((sum, g) => sum + g.creditHours, 0);
                const earnedCredits = coursesWithGrades.reduce((sum, g) => sum + g.creditHours, 0);

                const gradesHTML = `
                <div class="student-grades-container">
                    <div class="student-grades-header">
                        <div>
                            <h3>Current Semester Grades</h3>
                            <p class="student-grades-subtitle">${courseGrades.length} course(s)</p>
                        </div>
                        <div class="student-grades-summary">
                            <span>GPA: <strong>${gpa}</strong></span>
                            <span>Credits: <strong>${earnedCredits}/${totalCredits}</strong></span>
                        </div>
                    </div>

                    <div class="student-grades-table-wrapper">
                        <table class="student-grades-table">
                            <thead>
                                <tr>
                                    <th class="student-grades-col-code">Course Code</th>
                                    <th class="student-grades-col-name">Course Name</th>
                                    <th class="student-grades-col-grade">Exam 1</th>
                                    <th class="student-grades-col-grade">Exam 2</th>
                                    <th class="student-grades-col-grade">Final</th>
                                    <th class="student-grades-col-grade">Project</th>
                                    <th class="student-grades-col-letter">Letter Grade</th>
                                    <th class="student-grades-col-credits">Credits</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${courseGrades.map(grade => `
                                <tr class="student-grades-row ${!grade.hasGrade ? 'student-grades-row-pending' : ''}">
                                    <td class="student-grades-col-code"><strong>${grade.courseCode}</strong></td>
                                    <td class="student-grades-col-name">${grade.courseName}</td>
                                    <td class="student-grades-col-grade">${grade.exam1 ?? '-'}</td>
                                    <td class="student-grades-col-grade">${grade.exam2 ?? '-'}</td>
                                    <td class="student-grades-col-grade">${grade.final ?? '-'}</td>
                                    <td class="student-grades-col-grade">${grade.project ?? '-'}</td>
                                    <td class="student-grades-col-letter">
                                        ${grade.letterGrade
                        ? `<span class="student-grades-letter student-grades-letter-${grade.letterGrade.toLowerCase()}">${grade.letterGrade}</span>`
                        : '-'
                    }
                                    </td>
                                    <td class="student-grades-col-credits">${grade.creditHours}</td>
                                </tr>
                            `).join('')}
                            </tbody>
                        </table>
                    </div>

                    <div class="student-grades-info">
                        <p><strong>Note:</strong> Grades are updated as instructors submit them. Courses showing "Pending" have not been graded yet. Contact your instructor if you believe there's an error.</p>
                    </div>
                </div>
            `;

                document.getElementById('gradesContainer').innerHTML = gradesHTML;
            } catch (error) {
                console.error('Error loading grades:', error);
                document.getElementById('gradesContainer').innerHTML =
                    '<div style="text-align: center; padding: 20px; color: red;">Failed to load grades.</div>';
            }
        }
    },

    profile: {
        render: () => `
    <div class="student-breadcrumb">Home / My Profile</div>
    <div class="student-section-header">Personal Profile</div>
    
    <div class="student-profile-container">
        <div class="student-profile-header">
            <div class="student-profile-pic-wrapper">
                <img id="profilePic" src="https://via.placeholder.com/120?text=Avatar" class="student-profile-pic">
            </div>
            <div class="student-profile-info">
                <h2 id="profileName">Loading...</h2>
                <p id="profileEmail" class="student-profile-email">Email: -</p>
                <p id="profileMajor" class="student-profile-major">Major: -</p>
            </div>
        </div>
        
        <div class="student-profile-tabs">
            <button class="student-profile-tab-btn" data-tab="personal">Personal Info</button>
            <button class="student-profile-tab-btn" data-tab="contact">Contact</button>
            <button class="student-profile-tab-btn" data-tab="security">Security</button>
        </div>
        
        <div id="personalTab" class="student-profile-tab-content">
            <div class="student-profile-form">
                <div class="student-profile-form-group">
                    <label>First Name</label>
                    <input type="text" id="firstName" class="student-profile-input">
                </div>
                <div class="student-profile-form-group">
                    <label>Last Name</label>
                    <input type="text" id="lastName" class="student-profile-input">
                </div>
                <div class="student-profile-form-group">
                    <label>Email</label>
                    <input type="email" id="profileEmailInput" class="student-profile-input" readonly>
                </div>
                <div class="student-profile-form-group">
                    <label>Phone Number</label>
                    <input type="tel" id="phoneNumber" class="student-profile-input" placeholder="+90 (555) 000-0000">
                </div>
                <div class="student-profile-form-group">
                    <label>Date of Birth</label>
                    <input type="date" id="dateOfBirth" class="student-profile-input">
                </div>
                <div class="student-profile-form-group">
                    <label>Address</label>
                    <input type="text" id="address" class="student-profile-input" placeholder="Street address">
                </div>
                <div class="student-profile-form-group">
                    <label>City</label>
                    <input type="text" id="city" class="student-profile-input">
                </div>
                <div class="student-profile-form-group">
                    <label>Major</label>
                    <input type="text" id="major" class="student-profile-input" readonly>
                </div>
                <div class="student-profile-form-group">
                    <label>Academic Year</label>
                    <input type="text" id="academicYear" class="student-profile-input" readonly>
                </div>
                <button class="student-profile-btn-save" id="savePersonalBtn">Save Changes</button>
            </div>
        </div>
        
        <div id="contactTab" class="student-profile-tab-content">
            <div class="student-profile-form">
                <h4>Emergency Contact</h4>
                <div class="student-profile-form-group">
                    <label>Emergency Contact Name</label>
                    <input type="text" id="emergencyName" class="student-profile-input" placeholder="Full name">
                </div>
                <div class="student-profile-form-group">
                    <label>Emergency Contact Phone</label>
                    <input type="tel" id="emergencyPhone" class="student-profile-input" placeholder="+90 (555) 000-0000">
                </div>
                <div class="student-profile-form-group">
                    <label>Relationship</label>
                    <select id="emergencyRelationship" class="student-profile-input">
                        <option value="">Select relationship</option>
                        <option value="Parent">Parent</option>
                        <option value="Guardian">Guardian</option>
                        <option value="Sibling">Sibling</option>
                        <option value="Spouse">Spouse</option>
                        <option value="Friend">Friend</option>
                        <option value="Other">Other</option>
                    </select>
                </div>
                <button class="student-profile-btn-save" id="saveContactBtn">Save Changes</button>
            </div>
        </div>
        
        <div id="securityTab" class="student-profile-tab-content">
            <div class="student-profile-form">
                <h4>Change Password</h4>
                <div class="student-profile-form-group">
                    <label>Current Password</label>
                    <input type="password" id="currentPassword" class="student-profile-input" placeholder="Enter current password">
                </div>
                <div class="student-profile-form-group">
                    <label>New Password</label>
                    <input type="password" id="newPassword" class="student-profile-input" placeholder="Enter new password (min. 8 characters)">
                </div>
                <div class="student-profile-form-group">
                    <label>Confirm New Password</label>
                    <input type="password" id="confirmPassword" class="student-profile-input" placeholder="Confirm new password">
                </div>
                <button class="student-profile-btn-save" id="changePasswordBtn">Change Password</button>
            </div>
        </div>
    </div>
`,
        afterRender: async () => {
            function showProfileTab(tabName) {
                document.querySelectorAll('.student-profile-tab-content').forEach(el => {
                    el.style.display = 'none';
                });

                document.querySelectorAll('.student-profile-tab-btn').forEach(btn => {
                    btn.classList.remove('student-profile-tab-active');
                });

                const selectedTab = document.getElementById(tabName + 'Tab');
                if (selectedTab) {
                    selectedTab.style.display = 'block';
                }

                const activeBtn = document.querySelector(`[data-tab="${tabName}"]`);
                if (activeBtn) {
                    activeBtn.classList.add('student-profile-tab-active');
                }
            }

            showProfileTab('personal');

            document.querySelectorAll('.student-profile-tab-btn').forEach(btn => {
                btn.addEventListener('click', function () {
                    const tab = this.getAttribute('data-tab');
                    showProfileTab(tab);
                });
            });

            const response = await apiRequest('/user/profile');

            if (!response.ok || !response.data) {
                console.error('Failed to load profile');
                document.getElementById('profileName').textContent = 'Error loading profile';
                return;
            }

            const profile = response.data;

 
            document.getElementById('profileName').textContent = `${profile.firstName} ${profile.lastName}`;
            document.getElementById('profileEmail').textContent = `Email: ${profile.email}`;
            document.getElementById('profileMajor').textContent = `Major: ${profile.major || 'N/A'}`;

            document.getElementById('firstName').value = profile.firstName || '';
            document.getElementById('lastName').value = profile.lastName || '';
            document.getElementById('profileEmailInput').value = profile.email || '';
            document.getElementById('phoneNumber').value = profile.phoneNumber || '';
            document.getElementById('dateOfBirth').value = profile.dateOfBirth ? profile.dateOfBirth.split('T')[0] : '';
            document.getElementById('address').value = profile.address || '';
            document.getElementById('city').value = profile.city || '';
            document.getElementById('major').value = profile.major || '';
            document.getElementById('academicYear').value = profile.academicYear || '';

            document.getElementById('emergencyName').value = profile.emergencyContactName || '';
            document.getElementById('emergencyPhone').value = profile.emergencyContactPhone || '';
            document.getElementById('emergencyRelationship').value = profile.emergencyContactRelationship || '';

            document.getElementById('savePersonalBtn').addEventListener('click', async function () {
                const data = {
                    firstName: document.getElementById('firstName').value,
                    lastName: document.getElementById('lastName').value,
                    phoneNumber: document.getElementById('phoneNumber').value,
                    dateOfBirth: document.getElementById('dateOfBirth').value || null,
                    address: document.getElementById('address').value,
                    city: document.getElementById('city').value
                };

                const saveResponse = await apiRequest('/user/profile', 'PUT', data);
                if (saveResponse.ok) {

                    document.getElementById('profileName').textContent = `${data.firstName} ${data.lastName}`;
                    alert('Personal information updated successfully!');
                } else {
                    alert('Failed to update personal information: ' + (saveResponse.message || 'Unknown error'));
                }
            });

            document.getElementById('saveContactBtn').addEventListener('click', async function () {
                const data = {
                    emergencyContactName: document.getElementById('emergencyName').value,
                    emergencyContactPhone: document.getElementById('emergencyPhone').value,
                    emergencyContactRelationship: document.getElementById('emergencyRelationship').value
                };

                const saveResponse = await apiRequest('/user/profile', 'PUT', data);
                if (saveResponse.ok) {
                    alert('Contact information updated successfully!');
                } else {
                    alert('Failed to update contact information: ' + (saveResponse.message || 'Unknown error'));
                }
            });

            document.getElementById('changePasswordBtn').addEventListener('click', async function () {
                const current = document.getElementById('currentPassword').value;
                const newPass = document.getElementById('newPassword').value;
                const confirm = document.getElementById('confirmPassword').value;

                if (!current || !newPass || !confirm) {
                    alert('Please fill all fields');
                    return;
                }

                if (newPass.length < 8) {
                    alert('New password must be at least 8 characters long');
                    return;
                }

                if (newPass !== confirm) {
                    alert('New passwords do not match');
                    return;
                }

                const pwdResponse = await apiRequest('/user/change-password', 'POST', {
                    currentPassword: current,
                    newPassword: newPass
                });

                if (pwdResponse.ok) {
                    alert('Password changed successfully!');
                    document.getElementById('currentPassword').value = '';
                    document.getElementById('newPassword').value = '';
                    document.getElementById('confirmPassword').value = '';
                } else {
                    alert('Failed to change password: ' + (pwdResponse.message || 'Unknown error'));
                }
            });
        }
    },

    help: {
        render: () => `
        <div class="student-breadcrumb">Home / Help & Support</div>
        <div class="student-section-header">Contact Information</div>
        
        <div class="student-help-container">
            <div id="helpContacts">
                <div style="text-align: center; padding: 40px;">Loading contacts...</div>
            </div>
        </div>
    `,
        afterRender: async () => {
            const response = await apiRequest('/user/help-contacts');

            if (!response.ok || !response.data) {
                document.getElementById('helpContacts').innerHTML =
                    '<div style="text-align: center; padding: 40px; color: red;">Failed to load contact information</div>';
                return;
            }

            const data = response.data;

            const html = `
            <div class="help-section">
                <!-- Department Contact -->
                <div class="help-category">
                    <h3 class="help-category-title">Department Contact</h3>
                    <div class="help-contact-item">
                        <label>Department:</label>
                        <span>${data.departmentName || 'N/A'}</span>
                    </div>
                    ${data.departmentEmail ? `
                        <div class="help-contact-item">
                            <label>Department Email:</label>
                            <a href="mailto:${data.departmentEmail}">${data.departmentEmail}</a>
                        </div>
                    ` : ''}
                    ${data.departmentSecretaryEmail ? `
                        <div class="help-contact-item">
                            <label>Secretary Office:</label>
                            <a href="mailto:${data.departmentSecretaryEmail}">${data.departmentSecretaryEmail}</a>
                        </div>
                    ` : ''}
                </div>

                <!-- University Services -->
                <div class="help-category">
                    <h3 class="help-category-title">University Services</h3>
                    <div class="help-contact-item">
                        <label>Student Affairs:</label>
                        <a href="mailto:${data.studentAffairsEmail}">${data.studentAffairsEmail}</a>
                    </div>
                    ${data.registrarEmail ? `
                        <div class="help-contact-item">
                            <label>Registrar Office:</label>
                            <a href="mailto:${data.registrarEmail}">${data.registrarEmail}</a>
                        </div>
                    ` : ''}
                    ${data.itSupportEmail ? `
                        <div class="help-contact-item">
                            <label>IT Support:</label>
                            <a href="mailto:${data.itSupportEmail}">${data.itSupportEmail}</a>
                        </div>
                    ` : ''}
                </div>

                <!-- Course Instructors -->
                ${data.courseInstructors && data.courseInstructors.length > 0 ? `
                    <div class="help-category">
                        <h3 class="help-category-title">Course Instructors</h3>
                        <div class="help-instructors-grid">
                            ${data.courseInstructors.map(instructor => `
                                <div class="help-instructor-card">
                                    <div class="help-instructor-course-code">${instructor.courseCode}</div>
                                    <div class="help-instructor-course-name">${instructor.courseName}</div>
                                    <div class="help-instructor-name">${instructor.instructorName}</div>
                                    ${instructor.instructorEmail ?
                    `<a href="mailto:${instructor.instructorEmail}" class="help-instructor-email">${instructor.instructorEmail}</a>`
                    : '<span class="help-no-email">No email available</span>'
                }
                                </div>
                            `).join('')}
                        </div>
                    </div>
                ` : `
                    <div class="help-category">
                        <h3 class="help-category-title">Course Instructors</h3>
                        <div class="help-empty">
                            <p>You are not currently enrolled in any courses.</p>
                        </div>
                    </div>
                `}
            </div>
        `;

            document.getElementById('helpContacts').innerHTML = html;
        }
    },

};