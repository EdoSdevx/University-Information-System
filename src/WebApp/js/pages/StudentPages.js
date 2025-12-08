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
                            <div class="student-course-meta-label">Credits</div>
                            <div class="student-course-meta-value">3</div>
                        </div>
                    </div>
                    <div class="student-course-meta">
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Schedule</div>
                            <div class="student-course-meta-value">MWF 10am</div>
                        </div>
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Grade</div>
                            <div class="student-course-meta-value">A-</div>
                        </div>
                    </div>
                    <div class="student-course-actions">
                        <button class="student-course-btn">View Details</button>
                        <button class="student-course-btn secondary">Drop Course</button>
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
                            <div class="student-course-meta-label">Credits</div>
                            <div class="student-course-meta-value">4</div>
                        </div>
                    </div>
                    <div class="student-course-meta">
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Schedule</div>
                            <div class="student-course-meta-value">TTh 2pm</div>
                        </div>
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Grade</div>
                            <div class="student-course-meta-value">B+</div>
                        </div>
                    </div>
                    <div class="student-course-actions">
                        <button class="student-course-btn">View Details</button>
                        <button class="student-course-btn secondary">Drop Course</button>
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
                            <div class="student-course-meta-label">Credits</div>
                            <div class="student-course-meta-value">3</div>
                        </div>
                    </div>
                    <div class="student-course-meta">
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Schedule</div>
                            <div class="student-course-meta-value">MWF 2pm</div>
                        </div>
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Grade</div>
                            <div class="student-course-meta-value">A</div>
                        </div>
                    </div>
                    <div class="student-course-actions">
                        <button class="student-course-btn">View Details</button>
                        <button class="student-course-btn secondary">Drop Course</button>
                    </div>
                </div>
            </div>

            <div class="student-course-card">
                <div class="student-course-header">
                    <div class="student-course-code">PHY101</div>
                    <div class="student-course-name">Physics I</div>
                </div>
                <div class="student-course-body">
                    <div class="student-course-meta">
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Instructor</div>
                            <div class="student-course-meta-value">Prof. Davis</div>
                        </div>
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Credits</div>
                            <div class="student-course-meta-value">4</div>
                        </div>
                    </div>
                    <div class="student-course-meta">
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Schedule</div>
                            <div class="student-course-meta-value">TTh 10am</div>
                        </div>
                        <div class="student-course-meta-item">
                            <div class="student-course-meta-label">Grade</div>
                            <div class="student-course-meta-value">B</div>
                        </div>
                    </div>
                    <div class="student-course-actions">
                        <button class="student-course-btn">View Details</button>
                        <button class="student-course-btn secondary">Drop Course</button>
                    </div>
                </div>
            </div>
        </div>
    `,
        afterRender: () => { }
    },

    attendance: {
    render: () => `
        <div class="student-breadcrumb">Home / Attendance</div>
        <div class="student-section-header">My Attendance Record</div>
        
        <div class="student-attendance-container">
            <div class="student-attendance-summary">
                <div class="student-attendance-card">
                    <div class="student-attendance-card-label">Overall Attendance</div>
                    <div class="student-attendance-card-percent">92%</div>
                    <div class="student-attendance-card-bar">
                        <div class="student-attendance-card-fill" style="width: 92%"></div>
                    </div>
                </div>
                <div class="student-attendance-card">
                    <div class="student-attendance-card-label">Classes Attended</div>
                    <div class="student-attendance-card-value">23/25</div>
                </div>
                <div class="student-attendance-card">
                    <div class="student-attendance-card-label">Absences</div>
                    <div class="student-attendance-card-value">2</div>
                </div>
                <div class="student-attendance-card">
                    <div class="student-attendance-card-label">Late Arrivals</div>
                    <div class="student-attendance-card-value">1</div>
                </div>
            </div>

            <div class="student-attendance-table-wrapper">
                <h3>Attendance History</h3>
                <table class="student-attendance-table">
                    <thead>
                        <tr>
                            <th class="student-attendance-col-date">Date</th>
                            <th class="student-attendance-col-course">Course</th>
                            <th class="student-attendance-col-status">Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class="student-attendance-row">
                            <td class="student-attendance-col-date">2024-10-09</td>
                            <td class="student-attendance-col-course">CS101 - Introduction to Programming</td>
                            <td class="student-attendance-col-status"><span class="student-attendance-badge student-attendance-badge-present">Present</span></td>
                        </tr>
                        <tr class="student-attendance-row">
                            <td class="student-attendance-col-date">2024-10-08</td>
                            <td class="student-attendance-col-course">MATH101 - Calculus I</td>
                            <td class="student-attendance-col-status"><span class="student-attendance-badge student-attendance-badge-present">Present</span></td>
                        </tr>
                        <tr class="student-attendance-row">
                            <td class="student-attendance-col-date">2024-10-07</td>
                            <td class="student-attendance-col-course">CS101 - Introduction to Programming</td>
                            <td class="student-attendance-col-status"><span class="student-attendance-badge student-attendance-badge-late">Late</span></td>
                        </tr>
                        <tr class="student-attendance-row">
                            <td class="student-attendance-col-date">2024-10-06</td>
                            <td class="student-attendance-col-course">ENG101 - English I</td>
                            <td class="student-attendance-col-status"><span class="student-attendance-badge student-attendance-badge-present">Present</span></td>
                        </tr>
                        <tr class="student-attendance-row">
                            <td class="student-attendance-col-date">2024-10-05</td>
                            <td class="student-attendance-col-course">MATH101 - Calculus I</td>
                            <td class="student-attendance-col-status"><span class="student-attendance-badge student-attendance-badge-absent">Absent</span></td>
                        </tr>
                        <tr class="student-attendance-row">
                            <td class="student-attendance-col-date">2024-10-04</td>
                            <td class="student-attendance-col-course">CS101 - Introduction to Programming</td>
                            <td class="student-attendance-col-status"><span class="student-attendance-badge student-attendance-badge-present">Present</span></td>
                        </tr>
                        <tr class="student-attendance-row">
                            <td class="student-attendance-col-date">2024-10-03</td>
                            <td class="student-attendance-col-course">ENG101 - English I</td>
                            <td class="student-attendance-col-status"><span class="student-attendance-badge student-attendance-badge-present">Present</span></td>
                        </tr>
                        <tr class="student-attendance-row">
                            <td class="student-attendance-col-date">2024-10-02</td>
                            <td class="student-attendance-col-course">MATH101 - Calculus I</td>
                            <td class="student-attendance-col-status"><span class="student-attendance-badge student-attendance-badge-present">Present</span></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    `,
    afterRender: () => {}
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

         <div class="student-assignment-list" id="assignmentsList"></div>
     </div>

     <!-- Modal: Submit Assignment -->
     <div class="student-assignment-submit-modal" id="submitModal">
         <div class="student-assignment-modal-content">
             <div class="student-assignment-modal-header">
                 <h3 id="submitTitle"></h3>
                 <button class="student-assignment-modal-close" onclick="document.getElementById('submitModal').style.display='none'">×</button>
             </div>
             <div class="student-assignment-modal-body">
                 <div class="student-assignment-form-group">
                     <label>Assignment Description:</label>
                     <p id="submitDescription" class="student-assignment-description"></p>
                 </div>
                 <div class="student-assignment-form-group">
                     <label>Due Date:</label>
                     <p id="submitDueDate" class="student-assignment-due-date"></p>
                 </div>
                 <div class="student-assignment-form-group">
                     <label>Upload File *</label>
                     <input type="file" id="submitFile" class="student-assignment-file-input">
                     <p class="student-assignment-file-hint">Max file size: 50MB</p>
                 </div>
                 <div class="student-assignment-form-group">
                     <label>Comments (Optional)</label>
                     <textarea id="submitComments" class="student-assignment-textarea" placeholder="Add any comments..." rows="4"></textarea>
                 </div>
             </div>
             <div class="student-assignment-modal-footer">
                 <button class="student-assignment-btn-cancel" onclick="document.getElementById('submitModal').style.display='none'">Cancel</button>
                 <button class="student-assignment-btn-submit" onclick="window.submitAssignment()">Submit Assignment</button>
             </div>
         </div>
     </div>
 `,
        afterRender: () => {
            const assignments = [
                { id: 1, course: 'CS101', title: 'Variables and Data Types', description: 'Write a program using different data types', dueDate: '2024-10-15', points: 100, status: 'submitted', grade: 95 },
                { id: 2, course: 'CS101', title: 'Functions and Loops', description: 'Implement functions and loop structures', dueDate: '2024-10-22', points: 100, status: 'pending', grade: null },
                { id: 3, course: 'MATH101', title: 'Derivatives Practice', description: 'Solve 20 derivative problems', dueDate: '2024-10-20', points: 50, status: 'graded', grade: 48 },
                { id: 4, course: 'ENG101', title: 'Essay Writing', description: 'Write a 5-page essay', dueDate: '2024-10-25', points: 100, status: 'pending', grade: null }
            ];

            let currentFilter = 'all';

            function renderAssignments() {
                const filtered = currentFilter === 'all' ? assignments : assignments.filter(a => a.status === currentFilter);

                const list = document.getElementById('assignmentsList');
                if (filtered.length === 0) {
                    list.innerHTML = `
                 <div class="student-assignment-empty">
                     <p>No assignments in this category</p>
                 </div>
             `;
                    return;
                }

                list.innerHTML = filtered.map(assign => {
                    const dueDate = new Date(assign.dueDate);
                    const today = new Date();
                    const isOverdue = dueDate < today && assign.status === 'pending';

                    return `
                 <div class="student-assignment-card ${isOverdue ? 'student-assignment-card-overdue' : ''}">
                     <div class="student-assignment-card-header">
                         <div>
                             <span class="student-assignment-course">${assign.course}</span>
                             <h4 class="student-assignment-card-title">${assign.title}</h4>
                         </div>
                         <span class="student-assignment-card-points">${assign.points} pts</span>
                     </div>
                     <div class="student-assignment-card-content">
                         <p>${assign.description}</p>
                         <div class="student-assignment-card-meta">
                             <span>Due: ${new Date(assign.dueDate).toLocaleDateString()}</span>
                             <span class="student-assignment-status-badge student-assignment-status-${assign.status}">${assign.status.charAt(0).toUpperCase() + assign.status.slice(1)}</span>
                             ${assign.grade ? `<span class="student-assignment-grade">${assign.grade}/${assign.points}</span>` : ''}
                         </div>
                     </div>
                     <div class="student-assignment-card-actions">
                         ${assign.status === 'pending' ? `<button class="student-assignment-submit-btn" onclick="window.openSubmitModal(${assign.id}, '${assign.title}', '${assign.description}', '${assign.dueDate}')">Submit</button>` : ''}
                         ${assign.status === 'graded' ? `<button class="student-assignment-view-grade-btn" onclick="alert('Your grade: ${assign.grade}/${assign.points}')">View Grade</button>` : ''}
                     </div>
                 </div>
             `;
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

            window.openSubmitModal = function (assignmentId, title, description, dueDate) {
                document.getElementById('submitTitle').textContent = title;
                document.getElementById('submitDescription').textContent = description;
                document.getElementById('submitDueDate').textContent = new Date(dueDate).toLocaleDateString();
                document.getElementById('submitFile').value = '';
                document.getElementById('submitComments').value = '';
                document.getElementById('submitModal').style.display = 'flex';
            };

            window.submitAssignment = function () {
                const file = document.getElementById('submitFile').value;

                if (!file) {
                    alert('Please select a file');
                    return;
                }

                alert('Assignment submitted successfully!');
                document.getElementById('submitModal').style.display = 'none';
            };

            document.getElementById('submitModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            renderAssignments();
        }
    },

    registration: {
        render: () => `
        <div class="student-breadcrumb">Home / Course Registration</div>
        
        <div class="register-course-curriculum-header">
            <h2>Computer Science Curriculum 2024-2025</h2>
            <p>Select your year and semester, then choose courses to register</p>
        </div>

        <!-- Year Tabs -->
        <div class="register-course-tabs-years">
            <button class="register-course-year-tab active" data-year="1">1st Year</button>
            <button class="register-course-year-tab" data-year="2">2nd Year</button>
            <button class="register-course-year-tab" data-year="3">3rd Year</button>
            <button class="register-course-year-tab" data-year="4">4th Year</button>
        </div>

        <!-- Semester Tabs -->
        <div class="register-course-tabs-semesters">
            <button class="register-course-semester-tab active" data-semester="fall">Fall Semester</button>
            <button class="register-course-semester-tab" data-semester="spring">Spring Semester</button>
        </div>

        <!-- Curriculum Courses -->
        <div class="register-course-selection-box">
            <div class="register-course-selection-header">
                <h3>Curriculum</h3>
                <p id="semesterInfo">1st Year - Fall Semester</p>
            </div>
            <div class="register-course-selection-list" id="coursesList"></div>
        </div>

        <!-- My Selected Courses -->
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

        <!-- Modal: Course Instances -->
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

        <!-- Modal: Instance Details -->
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
        afterRender: () => {
            const curriculum = {
                1: {
                    fall: [
                        { id: 'CS101', name: 'Introduction to Programming', credits: 3 },
                        { id: 'MATH101', name: 'Calculus I', credits: 4 },
                        { id: 'ENG101', name: 'English I', credits: 3 }
                    ],
                    spring: [
                        { id: 'CS102', name: 'Programming II', credits: 3 },
                        { id: 'MATH102', name: 'Calculus II', credits: 4 },
                        { id: 'PHYS101', name: 'Physics I', credits: 4 }
                    ]
                },
                2: {
                    fall: [
                        { id: 'CS201', name: 'Data Structures', credits: 3 },
                        { id: 'CS202', name: 'Database Systems', credits: 4 },
                        { id: 'MATH201', name: 'Discrete Math', credits: 3 }
                    ],
                    spring: [
                        { id: 'CS203', name: 'Web Development', credits: 3 },
                        { id: 'CS204', name: 'Software Engineering', credits: 4 }
                    ]
                },
                3: {
                    fall: [
                        { id: 'CS301', name: 'Algorithms', credits: 3 },
                        { id: 'CS302', name: 'Operating Systems', credits: 4 }
                    ],
                    spring: [
                        { id: 'CS303', name: 'Computer Networks', credits: 3 },
                        { id: 'CS304', name: 'Artificial Intelligence', credits: 4 }
                    ]
                },
                4: {
                    fall: [
                        { id: 'CS401', name: 'Capstone Project I', credits: 3 },
                        { id: 'CS402', name: 'Advanced Topics', credits: 3 }
                    ],
                    spring: [
                        { id: 'CS403', name: 'Capstone Project II', credits: 4 }
                    ]
                }
            };

            const courseInstances = {
                CS101: [
                    { id: 'CS101-A', instructor: 'Dr. Smith', days: ['Monday'], startTime: '09:00', endTime: '11:00', room: '101', seats: '32/40' },
                    { id: 'CS101-B', instructor: 'Dr. Smith', days: ['Tuesday'], startTime: '14:00', endTime: '16:00', room: '102', seats: '35/40' },
                    { id: 'CS101-C', instructor: 'Prof. Jones', days: ['Wednesday'], startTime: '10:00', endTime: '12:00', room: '103', seats: '28/35' }
                ],
                MATH101: [
                    { id: 'MATH101-A', instructor: 'Prof. Johnson', days: ['Monday', 'Wednesday'], startTime: '09:00', endTime: '10:30', room: '201', seats: '40/40' },
                    { id: 'MATH101-B', instructor: 'Prof. Johnson', days: ['Tuesday', 'Thursday'], startTime: '14:00', endTime: '15:30', room: '202', seats: '35/40' }
                ],
                ENG101: [
                    { id: 'ENG101-A', instructor: 'Dr. Williams', days: ['Friday'], startTime: '10:00', endTime: '12:00', room: '301', seats: '25/30' }
                ],
                CS102: [
                    { id: 'CS102-A', instructor: 'Dr. Smith', days: ['Monday', 'Wednesday', 'Friday'], startTime: '10:00', endTime: '11:00', room: '101', seats: '30/40' },
                    { id: 'CS102-B', instructor: 'Prof. Brown', days: ['Tuesday', 'Thursday'], startTime: '15:00', endTime: '16:30', room: '102', seats: '32/40' }
                ],
                MATH102: [
                    { id: 'MATH102-A', instructor: 'Prof. Davis', days: ['Monday', 'Wednesday'], startTime: '10:00', endTime: '11:30', room: '201', seats: '38/40' },
                    { id: 'MATH102-B', instructor: 'Prof. Johnson', days: ['Tuesday', 'Thursday'], startTime: '13:00', endTime: '14:30', room: '202', seats: '36/40' }
                ],
                PHYS101: [
                    { id: 'PHYS101-A', instructor: 'Dr. Anderson', days: ['Monday', 'Wednesday'], startTime: '14:00', endTime: '15:30', room: '401', seats: '25/30' },
                    { id: 'PHYS101-B', instructor: 'Dr. Anderson', days: ['Friday'], startTime: '09:00', endTime: '11:00', room: '402', seats: '28/30' }
                ],
                CS201: [
                    { id: 'CS201-A', instructor: 'Dr. Brown', days: ['Tuesday', 'Thursday'], startTime: '09:00', endTime: '10:30', room: '101', seats: '30/35' }
                ],
                CS202: [
                    { id: 'CS202-A', instructor: 'Prof. Harris', days: ['Monday', 'Wednesday', 'Friday'], startTime: '11:00', endTime: '12:00', room: '201', seats: '25/30' },
                    { id: 'CS202-B', instructor: 'Prof. Harris', days: ['Tuesday', 'Thursday'], startTime: '13:00', endTime: '14:30', room: '202', seats: '28/35' }
                ],
                MATH201: [
                    { id: 'MATH201-A', instructor: 'Prof. Wilson', days: ['Monday', 'Wednesday'], startTime: '10:00', endTime: '11:30', room: '203', seats: '32/40' }
                ],
                CS203: [
                    { id: 'CS203-A', instructor: 'Dr. Anderson', days: ['Wednesday', 'Friday'], startTime: '14:00', endTime: '15:30', room: '301', seats: '28/35' },
                    { id: 'CS203-B', instructor: 'Dr. Taylor', days: ['Monday', 'Wednesday'], startTime: '15:00', endTime: '16:30', room: '302', seats: '25/30' }
                ],
                CS204: [
                    { id: 'CS204-A', instructor: 'Prof. White', days: ['Tuesday', 'Thursday'], startTime: '10:00', endTime: '11:30', room: '101', seats: '22/25' }
                ],
                CS301: [
                    { id: 'CS301-A', instructor: 'Dr. Miller', days: ['Monday', 'Wednesday', 'Friday'], startTime: '09:00', endTime: '10:00', room: '101', seats: '20/25' }
                ],
                CS302: [
                    { id: 'CS302-A', instructor: 'Prof. Davis', days: ['Tuesday', 'Thursday'], startTime: '11:00', endTime: '12:30', room: '102', seats: '18/22' },
                    { id: 'CS302-B', instructor: 'Prof. Davis', days: ['Monday', 'Wednesday'], startTime: '14:00', endTime: '15:30', room: '103', seats: '20/22' }
                ],
                CS303: [
                    { id: 'CS303-A', instructor: 'Dr. Garcia', days: ['Wednesday', 'Friday'], startTime: '10:00', endTime: '11:30', room: '201', seats: '19/25' }
                ],
                CS304: [
                    { id: 'CS304-A', instructor: 'Prof. Rodriguez', days: ['Monday', 'Wednesday'], startTime: '13:00', endTime: '14:30', room: '301', seats: '16/20' },
                    { id: 'CS304-B', instructor: 'Prof. Rodriguez', days: ['Tuesday', 'Thursday'], startTime: '15:00', endTime: '16:30', room: '302', seats: '18/20' }
                ],
                CS401: [
                    { id: 'CS401-A', instructor: 'Dr. Lee', days: ['Friday'], startTime: '09:00', endTime: '12:00', room: '401', seats: '12/15' }
                ],
                CS402: [
                    { id: 'CS402-A', instructor: 'Prof. Martin', days: ['Monday', 'Wednesday'], startTime: '10:00', endTime: '11:30', room: '402', seats: '14/15' }
                ],
                CS403: [
                    { id: 'CS403-A', instructor: 'Dr. Martinez', days: ['Tuesday', 'Thursday'], startTime: '09:00', endTime: '10:30', room: '403', seats: '13/15' }
                ]
            };

            // Collision detection function
            function checkTimeConflict(instance1, instance2) {
                const days1 = new Set(instance1.days);
                const days2 = new Set(instance2.days);

                // Check if they share any days
                let shareDay = false;
                for (let day of days1) {
                    if (days2.has(day)) {
                        shareDay = true;
                        break;
                    }
                }

                if (!shareDay) return false;

                // Convert times to minutes for comparison
                const time1Start = parseInt(instance1.startTime.replace(':', ''));
                const time1End = parseInt(instance1.endTime.replace(':', ''));
                const time2Start = parseInt(instance2.startTime.replace(':', ''));
                const time2End = parseInt(instance2.endTime.replace(':', ''));

                // Check if times overlap
                return time1Start < time2End && time2Start < time1End;
            }

            let currentYear = 1;
            let currentSemester = 'fall';
            const selectedCourses = new Map();

            document.querySelectorAll('.register-course-year-tab').forEach(tab => {
                tab.addEventListener('click', function () {
                    document.querySelectorAll('.register-course-year-tab').forEach(t => t.classList.remove('active'));
                    this.classList.add('active');
                    currentYear = parseInt(this.dataset.year);
                    loadCourses();
                });
            });

            document.querySelectorAll('.register-course-semester-tab').forEach(tab => {
                tab.addEventListener('click', function () {
                    document.querySelectorAll('.register-course-semester-tab').forEach(t => t.classList.remove('active'));
                    this.classList.add('active');
                    currentSemester = this.dataset.semester;
                    loadCourses();
                });
            });

            function loadCourses() {
                const courses = curriculum[currentYear][currentSemester];
                const yearName = currentYear === 1 ? '1st' : currentYear === 2 ? '2nd' : currentYear === 3 ? '3rd' : '4th';
                const semesterName = currentSemester === 'fall' ? 'Fall' : 'Spring';

                document.getElementById('semesterInfo').textContent = `${yearName} Year - ${semesterName} Semester`;

                const coursesList = document.getElementById('coursesList');
                coursesList.innerHTML = courses.map(course => {
                    const isSelected = selectedCourses.has(course.id);
                    return `
                    <div class="register-course-selection-item ${isSelected ? 'register-course-selection-item-selected' : ''}">
                        <div class="register-course-selection-item-info">
                            <div class="register-course-selection-item-code">${course.id}</div>
                            <div class="register-course-selection-item-name">${course.name}</div>
                            <div class="register-course-selection-item-credits">${course.credits} Credits</div>
                        </div>
                        <div class="register-course-selection-item-actions">
                            ${isSelected ? '<span class="register-course-selection-item-tick">✓</span>' : ''}
                            <button class="register-course-selection-item-btn ${isSelected ? 'register-course-selection-item-btn-selected' : ''}" onclick="window.showInstancesModal('${course.id}', '${course.name}', ${course.credits})">${isSelected ? '✓' : '+'}</button>
                        </div>
                    </div>
                `;
                }).join('');
            }

            window.showInstancesModal = function (courseId, courseName, credits) {
                const modal = document.getElementById('instancesModal');
                const title = document.getElementById('instancesModalTitle');
                const list = document.getElementById('instancesList');
                const warning = document.getElementById('collisionWarning');

                title.textContent = `${courseId} - ${courseName}`;
                const instances = courseInstances[courseId] || [];

                warning.innerHTML = '';

                if (instances.length === 0) {
                    list.innerHTML = '<p style="text-align:center;color:#999;">No instances available</p>';
                } else {
                    list.innerHTML = instances.map(inst => {
                        let hasConflict = false;
                        let conflictCourse = '';

                        for (let [selId, selData] of selectedCourses) {
                            const selInstance = courseInstances[selId].find(i => i.id === selData.instance);
                            if (selInstance && checkTimeConflict(inst, selInstance)) {
                                hasConflict = true;
                                conflictCourse = selId;
                                break;
                            }
                        }

                        return `
                        <div class="register-course-instance-card ${hasConflict ? 'register-course-instance-card-conflict' : ''}">
                            ${hasConflict ? `<div class="register-course-instance-conflict-badge">CONFLICT with ${conflictCourse}</div>` : ''}
                            <div class="register-course-instance-info">
                                <div class="register-course-instance-id">${inst.id}</div>
                                <div class="register-course-instance-meta">
                                    <span>Instructor: ${inst.instructor}</span>
                                    <span>Room: ${inst.room}</span>
                                    <span>Seats: ${inst.seats}</span>
                                </div>
                                <div class="register-course-instance-time">
                                    <span class="register-course-instance-day">${inst.days.join(', ')}</span>
                                    <span class="register-course-instance-hour">${inst.startTime}-${inst.endTime}</span>
                                </div>
                            </div>
                            <div class="register-course-instance-actions">
                                <button class="register-course-info-btn" onclick="window.showScheduleDetails('${inst.id}', '${courseName}', '${inst.days.join(', ')}', '${inst.startTime}-${inst.endTime}', '${inst.instructor}', '${inst.room}')">Info</button>
                                <button class="register-course-select-btn ${hasConflict ? 'register-course-select-btn-disabled' : ''}" onclick="window.selectInstance('${courseId}', '${courseName}', ${credits}, '${inst.id}')" ${hasConflict ? 'disabled' : ''}>Select This</button>
                            </div>
                        </div>
                    `;
                    }).join('');
                }
                modal.style.display = 'flex';
            };

            window.selectInstance = function (courseId, courseName, credits, instanceId) {
                selectedCourses.set(courseId, { name: courseName, credits, instance: instanceId });
                document.getElementById('instancesModal').style.display = 'none';
                loadCourses();
                updateSelectedGrid();
            };

            function updateSelectedGrid() {
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
                    grid.innerHTML = Array.from(selectedCourses.entries()).map(([courseId, data]) => `
                    <div class="register-course-selected-card">
                        <div class="register-course-selected-card-header">
                            <div class="register-course-selected-card-code">${courseId}</div>
                            <button class="register-course-selected-card-remove" onclick="window.removeSelected('${courseId}')">×</button>
                        </div>
                        <div class="register-course-selected-card-body">
                            <div class="register-course-selected-card-name">${data.name}</div>
                            <div class="register-course-selected-card-instance">${data.instance}</div>
                            <div class="register-course-selected-card-credits">${data.credits} Credits</div>
                        </div>
                    </div>
                `).join('');
                }
            }

            window.removeSelected = function (courseId) {
                selectedCourses.delete(courseId);
                loadCourses();
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
                            ${dayArray.map(day => `<span class="register-course-schedule-day">${day}</span>`).join('')}
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

            document.getElementById('instancesModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });
            document.getElementById('scheduleModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            document.getElementById('confirmBtn').addEventListener('click', function () {
                const totalCredits = Array.from(selectedCourses.values()).reduce((sum, c) => sum + c.credits, 0);
                alert(`Successfully registered for ${selectedCourses.size} courses (${totalCredits} credits)!`);
                console.log('Selected courses:', Array.from(selectedCourses.entries()));
            });

            loadCourses();
        }
    },  

    grades: {
        render: () => `
        <div class="student-breadcrumb">Home / Grades & Transcripts</div>
        <div class="student-section-header">Your Grades</div>
        
        <div class="student-grades-container">
            <div class="student-grades-header">
                <h3>Current Semester Grades</h3>
                <div class="student-grades-summary">
                    <span>GPA: <strong id="gpa">3.65</strong></span>
                    <span>Credits: <strong>13/15</strong></span>
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
                        <tr class="student-grades-row">
                            <td class="student-grades-col-code">CS101</td>
                            <td class="student-grades-col-name">Introduction to Programming</td>
                            <td class="student-grades-col-grade">85</td>
                            <td class="student-grades-col-grade">88</td>
                            <td class="student-grades-col-grade">90</td>
                            <td class="student-grades-col-grade">92</td>
                            <td class="student-grades-col-letter"><span class="student-grades-letter student-grades-letter-a">A-</span></td>
                            <td class="student-grades-col-credits">3</td>
                        </tr>
                        <tr class="student-grades-row">
                            <td class="student-grades-col-code">MATH101</td>
                            <td class="student-grades-col-name">Calculus I</td>
                            <td class="student-grades-col-grade">82</td>
                            <td class="student-grades-col-grade">85</td>
                            <td class="student-grades-col-grade">88</td>
                            <td class="student-grades-col-grade">-</td>
                            <td class="student-grades-col-letter"><span class="student-grades-letter student-grades-letter-b">B+</span></td>
                            <td class="student-grades-col-credits">4</td>
                        </tr>
                        <tr class="student-grades-row">
                            <td class="student-grades-col-code">ENG101</td>
                            <td class="student-grades-col-name">English I</td>
                            <td class="student-grades-col-grade">90</td>
                            <td class="student-grades-col-grade">92</td>
                            <td class="student-grades-col-grade">94</td>
                            <td class="student-grades-col-grade">95</td>
                            <td class="student-grades-col-letter"><span class="student-grades-letter student-grades-letter-a">A</span></td>
                            <td class="student-grades-col-credits">3</td>
                        </tr>
                        <tr class="student-grades-row">
                            <td class="student-grades-col-code">CS102</td>
                            <td class="student-grades-col-name">Programming II</td>
                            <td class="student-grades-col-grade">78</td>
                            <td class="student-grades-col-grade">80</td>
                            <td class="student-grades-col-grade">82</td>
                            <td class="student-grades-col-grade">85</td>
                            <td class="student-grades-col-letter"><span class="student-grades-letter student-grades-letter-b">B</span></td>
                            <td class="student-grades-col-credits">3</td>
                        </tr>
                        <tr class="student-grades-row">
                            <td class="student-grades-col-code">MATH102</td>
                            <td class="student-grades-col-name">Calculus II</td>
                            <td class="student-grades-col-grade">86</td>
                            <td class="student-grades-col-grade">87</td>
                            <td class="student-grades-col-grade">89</td>
                            <td class="student-grades-col-grade">-</td>
                            <td class="student-grades-col-letter"><span class="student-grades-letter student-grades-letter-a">A-</span></td>
                            <td class="student-grades-col-credits">4</td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="student-grades-info">
                <p><strong>Note:</strong> "-" indicates no project for this course. Final grade is calculated as: (Exam1×20% + Exam2×20% + Final×40% + Project×20%)</p>
            </div>
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
    },

    schedule: {
        render: () => `
            <div class="student-breadcrumb">Home / Schedule</div>
            <div class="student-section-header">My Class Schedule</div>
            <div class="schedule-container">
                <div class="schedule-day-card">
                    <div class="schedule-day-header">Monday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">09:00 - 12:00</div>
                        <div class="schedule-course">Introduction to Programming</div>
                        <div class="schedule-meta">Dr. Smith | Room 101 | Group A1</div>
                    </div>
                </div>

                <div class="schedule-day-card">
                    <div class="schedule-day-header">Tuesday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">14:00 - 17:00</div>
                        <div class="schedule-course">Calculus II</div>
                        <div class="schedule-meta">Prof. Johnson | Room 205 | Group B2</div>
                    </div>
                </div>

                <div class="schedule-day-card">
                    <div class="schedule-day-header">Wednesday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">10:00 - 13:00</div>
                        <div class="schedule-course">Academic Writing</div>
                        <div class="schedule-meta">Dr. Williams | Room 304 | Group C1</div>
                    </div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">15:00 - 17:00</div>
                        <div class="schedule-course">Data Structures</div>
                        <div class="schedule-meta">Dr. Brown | Lab 102 | Group A2</div>
                    </div>
                </div>

                <div class="schedule-day-card">
                    <div class="schedule-day-header">Thursday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">09:00 - 12:00</div>
                        <div class="schedule-course">Physics I</div>
                        <div class="schedule-meta">Prof. Davis | Room 401 | Group B1</div>
                    </div>
                </div>

                <div class="schedule-day-card">
                    <div class="schedule-day-header">Friday</div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">10:00 - 12:00</div>
                        <div class="schedule-course">Introduction to Programming</div>
                        <div class="schedule-meta">Dr. Smith | Lab 101 | Group A1</div>
                    </div>
                    <div class="schedule-class-item">
                        <div class="schedule-time">13:00 - 16:00</div>
                        <div class="schedule-course">Calculus II</div>
                        <div class="schedule-meta">Prof. Johnson | Room 205 | Group B2</div>
                    </div>

                </div>

                <div class="schedule-day-card empty">
                    <div class="schedule-day-header">Saturday</div>
                    <div class="schedule-no-classes">No Classes</div>
                </div>

                <div class="schedule-day-card empty">
                    <div class="schedule-day-header">Sunday</div>
                    <div class="schedule-no-classes">No Classes</div>
                </div>
            </div>
        `,
        afterRender: () => { }
    }
};