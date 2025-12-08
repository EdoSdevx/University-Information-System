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
        <div class="teacher-section-header">Grade Management</div>
        
        <div id="toastNotification" class="teacher-grading-toast"></div>
        
        <div class="teacher-grading-container">
            <div class="teacher-grading-selector">
                <label>Select Class:</label>
                <select id="classSelect" class="teacher-grading-class-select">
                    <option value="CS101-A">CS101-A - Introduction to Programming (Dr. Smith)</option>
                    <option value="CS101-B">CS101-B - Introduction to Programming (Dr. Smith)</option>
                    <option value="MATH101-A">MATH101-A - Calculus I (Prof. Johnson)</option>
                    <option value="ENG101-A">ENG101-A - English I (Dr. Williams)</option>
                </select>
            </div>

            <div class="teacher-grading-info">
                <p id="classInfo">CS101-A: Introduction to Programming (32 students)</p>
                <span class="teacher-grading-bell-curve-info">Grades calculated using Bell Curve distribution</span>
            </div>

            <div class="teacher-grading-table-wrapper">
                <table class="teacher-grading-table">
                    <thead>
                        <tr>
                            <th class="teacher-grading-col-student">Student Name</th>
                            <th class="teacher-grading-col-id">Student ID</th>
                            <th class="teacher-grading-col-grade">Exam 1</th>
                            <th class="teacher-grading-col-grade">Exam 2</th>
                            <th class="teacher-grading-col-grade">Final</th>
                            <th class="teacher-grading-col-grade">Project</th>
                            <th class="teacher-grading-col-letter">Letter Grade</th>
                            <th class="teacher-grading-col-action">Action</th>
                        </tr>
                    </thead>
                    <tbody id="gradesTable"></tbody>
                </table>
            </div>

            <button class="teacher-grading-save-btn" id="saveBtn">Save All Grades</button>
        </div>
    `,
        afterRender: () => {
            const classesData = {
                'CS101-A': {
                    name: 'CS101-A - Introduction to Programming (Dr. Smith)',
                    count: 32,
                    students: [
                        { id: 'STU001', name: 'Ahmed Hassan', exam1: 85, exam2: 88, final: 90, project: 92 },
                        { id: 'STU002', name: 'Fatima Khan', exam1: 92, exam2: 94, final: 96, project: 98 },
                        { id: 'STU003', name: 'Ali Yilmaz', exam1: 78, exam2: 80, final: 82, project: null },
                        { id: 'STU004', name: 'Zeynep Demir', exam1: 88, exam2: 90, final: 92, project: 95 },
                        { id: 'STU005', name: 'Mustafa Ozer', exam1: 75, exam2: 77, final: 79, project: 81 }
                    ]
                },
                'CS101-B': {
                    name: 'CS101-B - Introduction to Programming (Dr. Smith)',
                    count: 35,
                    students: [
                        { id: 'STU006', name: 'Layla Ahmed', exam1: 90, exam2: 92, final: 94, project: 96 },
                        { id: 'STU007', name: 'Omar Hassan', exam1: 82, exam2: 84, final: 86, project: 88 },
                        { id: 'STU008', name: 'Noor Ibrahim', exam1: 88, exam2: 90, final: 92, project: null },
                        { id: 'STU009', name: 'Hana Malik', exam1: 85, exam2: 87, final: 89, project: 91 },
                        { id: 'STU010', name: 'Karim Saleh', exam1: 70, exam2: 72, final: 74, project: 76 }
                    ]
                },
                'MATH101-A': {
                    name: 'MATH101-A - Calculus I (Prof. Johnson)',
                    count: 40,
                    students: [
                        { id: 'STU011', name: 'Aisha Mohammed', exam1: 86, exam2: 89, final: 92, project: null },
                        { id: 'STU012', name: 'Ibrahim Rahman', exam1: 84, exam2: 86, final: 88, project: null },
                        { id: 'STU013', name: 'Sara Ahmed', exam1: 91, exam2: 93, final: 95, project: null },
                        { id: 'STU014', name: 'Hassan Ali', exam1: 79, exam2: 81, final: 83, project: null },
                        { id: 'STU015', name: 'Maryam Hassan', exam1: 88, exam2: 90, final: 92, project: null }
                    ]
                },
                'ENG101-A': {
                    name: 'ENG101-A - English I (Dr. Williams)',
                    count: 25,
                    students: [
                        { id: 'STU016', name: 'Leila Farrokhi', exam1: 89, exam2: 91, final: 93, project: 95 },
                        { id: 'STU017', name: 'Reza Pakzad', exam1: 92, exam2: 94, final: 96, project: 98 },
                        { id: 'STU018', name: 'Nastaran Salaem', exam1: 85, exam2: 87, final: 89, project: 91 },
                        { id: 'STU019', name: 'Dariush Karim', exam1: 80, exam2: 82, final: 84, project: 86 },
                        { id: 'STU020', name: 'Parisa Ahmadi', exam1: 88, exam2: 90, final: 92, project: 94 }
                    ]
                }
            };

            // Bell Curve calculation
            function calculateBellCurveGrades(students) {
                // Calculate final scores for each student
                const scores = students.map(student => {
                    let score = student.exam1 * 0.2 + student.exam2 * 0.2 + student.final * 0.4;
                    if (student.project !== null && student.project !== undefined) {
                        score += student.project * 0.2;
                    }
                    return score;
                });

                // Sort scores
                const sortedScores = [...scores].sort((a, b) => b - a);

                // Calculate percentiles and assign grades
                const gradeThresholds = {
                    'A': Math.ceil(sortedScores.length * 0.10),   // Top 10%
                    'B': Math.ceil(sortedScores.length * 0.30),   // Next 20%
                    'C': Math.ceil(sortedScores.length * 0.60),   // Next 30%
                    'D': Math.ceil(sortedScores.length * 0.85),   // Next 25%
                    'F': sortedScores.length                       // Rest
                };

                const gradeMap = {};
                sortedScores.forEach((score, index) => {
                    if (index < gradeThresholds.A) gradeMap[score] = 'A';
                    else if (index < gradeThresholds.B) gradeMap[score] = 'B';
                    else if (index < gradeThresholds.C) gradeMap[score] = 'C';
                    else if (index < gradeThresholds.D) gradeMap[score] = 'D';
                    else gradeMap[score] = 'F';
                });

                return scores.map(score => gradeMap[score]);
            }

            function getLetterGradeClass(letter) {
                if (letter === 'A') return 'teacher-grading-letter-a';
                if (letter === 'B') return 'teacher-grading-letter-b';
                if (letter === 'C') return 'teacher-grading-letter-c';
                if (letter === 'D') return 'teacher-grading-letter-d';
                return 'teacher-grading-letter-f';
            }

            function showToast(message, type = 'success') {
                const toast = document.getElementById('toastNotification');
                toast.textContent = message;
                toast.className = `teacher-grading-toast teacher-grading-toast-${type} teacher-grading-toast-show`;

                setTimeout(() => {
                    toast.classList.remove('teacher-grading-toast-show');
                }, 4000);
            }

            function loadClass(classId) {
                const classData = classesData[classId];
                document.getElementById('classInfo').textContent = `${classData.name} (${classData.count} students)`;

                const bellCurveGrades = calculateBellCurveGrades(classData.students);

                const table = document.getElementById('gradesTable');
                table.innerHTML = classData.students.map((student, idx) => {
                    const letter = bellCurveGrades[idx];
                    return `
                    <tr class="teacher-grading-row">
                        <td class="teacher-grading-col-student">${student.name}</td>
                        <td class="teacher-grading-col-id">${student.id}</td>
                        <td class="teacher-grading-col-grade">
                            <input type="number" class="teacher-grading-input exam1-${idx}" value="${student.exam1}" min="0" max="100" onchange="window.updateGrade(${idx})">
                        </td>
                        <td class="teacher-grading-col-grade">
                            <input type="number" class="teacher-grading-input exam2-${idx}" value="${student.exam2}" min="0" max="100" onchange="window.updateGrade(${idx})">
                        </td>
                        <td class="teacher-grading-col-grade">
                            <input type="number" class="teacher-grading-input final-${idx}" value="${student.final}" min="0" max="100" onchange="window.updateGrade(${idx})">
                        </td>
                        <td class="teacher-grading-col-grade">
                            <input type="number" class="teacher-grading-input project-${idx}" value="${student.project || ''}" min="0" max="100" placeholder="-" onchange="window.updateGrade(${idx})">
                        </td>
                        <td class="teacher-grading-col-letter">
                            <span class="teacher-grading-letter ${getLetterGradeClass(letter)} letter-${idx}">${letter}</span>
                        </td>
                        <td class="teacher-grading-col-action">
                            <button class="teacher-grading-action-btn" onclick="window.clearGrades(${idx})">Clear</button>
                        </td>
                    </tr>
                `;
                }).join('');
            }

            window.updateGrade = function (idx) {
                const classId = document.getElementById('classSelect').value;
                const classData = classesData[classId];

                classData.students[idx].exam1 = parseFloat(document.querySelector(`.exam1-${idx}`).value) || 0;
                classData.students[idx].exam2 = parseFloat(document.querySelector(`.exam2-${idx}`).value) || 0;
                classData.students[idx].final = parseFloat(document.querySelector(`.final-${idx}`).value) || 0;
                classData.students[idx].project = document.querySelector(`.project-${idx}`).value ? parseFloat(document.querySelector(`.project-${idx}`).value) : null;

                const bellCurveGrades = calculateBellCurveGrades(classData.students);
                const letter = bellCurveGrades[idx];

                const letterEl = document.querySelector(`.letter-${idx}`);
                letterEl.textContent = letter;
                letterEl.className = `teacher-grading-letter ${getLetterGradeClass(letter)} letter-${idx}`;
            };

            window.clearGrades = function (idx) {
                document.querySelector(`.exam1-${idx}`).value = '';
                document.querySelector(`.exam2-${idx}`).value = '';
                document.querySelector(`.final-${idx}`).value = '';
                document.querySelector(`.project-${idx}`).value = '';
                document.querySelector(`.letter-${idx}`).textContent = '-';
            };

            document.getElementById('classSelect').addEventListener('change', function () {
                loadClass(this.value);
            });

            document.getElementById('saveBtn').addEventListener('click', function () {
                const classId = document.getElementById('classSelect').value;
                const classData = classesData[classId];
                showToast(`✓ Grades for ${classData.students.length} students saved successfully!`, 'success');
                console.log('Grades saved for class:', classId);
            });

            loadClass('CS101-A');
        }
    },

    attendance: {
        render: () => `
        <div class="teacher-breadcrumb">Home / Attendance</div>
        <div class="teacher-section-header">Mark Attendance</div>
        
        <div id="attendanceToast" class="teacher-attendance-toast"></div>
        
        <div class="teacher-attendance-container">
            <div class="teacher-attendance-selector">
                <div class="teacher-attendance-selector-group">
                    <label>Select Class:</label>
                    <select id="classSelect" class="teacher-attendance-class-select">
                        <option value="CS101-A">CS101-A - Introduction to Programming</option>
                        <option value="CS101-B">CS101-B - Introduction to Programming</option>
                        <option value="MATH101-A">MATH101-A - Calculus I</option>
                        <option value="ENG101-A">ENG101-A - English I</option>
                    </select>
                </div>
                <div class="teacher-attendance-selector-group">
                    <label>Date:</label>
                    <input type="date" id="attendanceDate" class="teacher-attendance-date-input">
                </div>
            </div>

            <div class="teacher-attendance-info">
                <p id="classInfo">CS101-A - Introduction to Programming</p>
            </div>

            <div class="teacher-attendance-list">
                <table class="teacher-attendance-table">
                    <thead>
                        <tr>
                            <th class="teacher-attendance-col-name">Student Name</th>
                            <th class="teacher-attendance-col-id">Student ID</th>
                            <th class="teacher-attendance-col-status">Status</th>
                        </tr>
                    </thead>
                    <tbody id="attendanceTable"></tbody>
                </table>
            </div>

            <button class="teacher-attendance-save-btn" id="saveBtn">Save Attendance</button>
        </div>
    `,
        afterRender: () => {
            const classesData = {
                'CS101-A': {
                    name: 'CS101-A - Introduction to Programming',
                    students: [
                        { id: 'STU001', name: 'Ahmed Hassan' },
                        { id: 'STU002', name: 'Fatima Khan' },
                        { id: 'STU003', name: 'Ali Yilmaz' },
                        { id: 'STU004', name: 'Zeynep Demir' },
                        { id: 'STU005', name: 'Mustafa Ozer' }
                    ]
                },
                'CS101-B': {
                    name: 'CS101-B - Introduction to Programming',
                    students: [
                        { id: 'STU006', name: 'Layla Ahmed' },
                        { id: 'STU007', name: 'Omar Hassan' },
                        { id: 'STU008', name: 'Noor Ibrahim' },
                        { id: 'STU009', name: 'Hana Malik' },
                        { id: 'STU010', name: 'Karim Saleh' }
                    ]
                },
                'MATH101-A': {
                    name: 'MATH101-A - Calculus I',
                    students: [
                        { id: 'STU011', name: 'Aisha Mohammed' },
                        { id: 'STU012', name: 'Ibrahim Rahman' },
                        { id: 'STU013', name: 'Sara Ahmed' },
                        { id: 'STU014', name: 'Hassan Ali' },
                        { id: 'STU015', name: 'Maryam Hassan' }
                    ]
                },
                'ENG101-A': {
                    name: 'ENG101-A - English I',
                    students: [
                        { id: 'STU016', name: 'Leila Farrokhi' },
                        { id: 'STU017', name: 'Reza Pakzad' },
                        { id: 'STU018', name: 'Nastaran Salaem' },
                        { id: 'STU019', name: 'Dariush Karim' },
                        { id: 'STU020', name: 'Parisa Ahmadi' }
                    ]
                }
            };

            const attendanceData = {};

            // Set today's date as default
            const today = new Date().toISOString().split('T')[0];
            document.getElementById('attendanceDate').value = today;

            function loadStudents(classId) {
                const classData = classesData[classId];
                document.getElementById('classInfo').textContent = classData.name;

                const table = document.getElementById('attendanceTable');
                table.innerHTML = classData.students.map((student, idx) => `
                <tr class="teacher-attendance-row">
                    <td class="teacher-attendance-col-name">${student.name}</td>
                    <td class="teacher-attendance-col-id">${student.id}</td>
                    <td class="teacher-attendance-col-status">
                        <div class="teacher-attendance-status-buttons">
                            <button class="teacher-attendance-btn teacher-attendance-btn-present" onclick="window.setAttendance(${idx}, 'present')" data-student="${student.id}">Present</button>
                            <button class="teacher-attendance-btn teacher-attendance-btn-late" onclick="window.setAttendance(${idx}, 'late')" data-student="${student.id}">Late</button>
                            <button class="teacher-attendance-btn teacher-attendance-btn-absent" onclick="window.setAttendance(${idx}, 'absent')" data-student="${student.id}">Absent</button>
                        </div>
                    </td>
                </tr>
            `).join('');
            }

            window.setAttendance = function (idx, status) {
                const btns = document.querySelectorAll(`[data-student]`);
                const allBtns = document.querySelectorAll('.teacher-attendance-btn');
                allBtns.forEach(btn => btn.classList.remove('teacher-attendance-btn-active'));

                const row = document.querySelectorAll('.teacher-attendance-row')[idx];
                const buttons = row.querySelectorAll('.teacher-attendance-btn');
                buttons.forEach(btn => btn.classList.remove('teacher-attendance-btn-active'));

                if (status === 'present') {
                    buttons[0].classList.add('teacher-attendance-btn-active');
                } else if (status === 'late') {
                    buttons[1].classList.add('teacher-attendance-btn-active');
                } else if (status === 'absent') {
                    buttons[2].classList.add('teacher-attendance-btn-active');
                }
            };

            document.getElementById('classSelect').addEventListener('change', function () {
                loadStudents(this.value);
            });

            document.getElementById('saveBtn').addEventListener('click', function () {
                const classId = document.getElementById('classSelect').value;
                const date = document.getElementById('attendanceDate').value;
                const classData = classesData[classId];

                alert(`Attendance saved for ${classData.name} on ${date}`);

                const toast = document.getElementById('attendanceToast');
                toast.textContent = `✓ Attendance marked and saved for ${date}`;
                toast.className = 'teacher-attendance-toast teacher-attendance-toast-success teacher-attendance-toast-show';

                setTimeout(() => {
                    toast.classList.remove('teacher-attendance-toast-show');
                }, 4000);
            });

            loadStudents('CS101-A');
        }
    },

    roster: {
        render: () => `
        <div class="teacher-breadcrumb">Home / Class Roster</div>
        <div class="teacher-section-header">Student Roster</div>
        
        <div class="teacher-roster-container">
            <div class="teacher-roster-selector">
                <label>Select Class:</label>
                <select id="classSelect" class="teacher-roster-class-select">
                    <option value="CS101-A">CS101-A - Introduction to Programming (32 students)</option>
                    <option value="CS101-B">CS101-B - Introduction to Programming (35 students)</option>
                    <option value="MATH101-A">MATH101-A - Calculus I (40 students)</option>
                    <option value="ENG101-A">ENG101-A - English I (25 students)</option>
                </select>
                <input type="text" id="searchInput" class="teacher-roster-search" placeholder="Search by name or ID...">
            </div>

            <div class="teacher-roster-info">
                <p id="classInfo">CS101-A - Introduction to Programming</p>
                <span id="rosterCount">32 students enrolled</span>
            </div>

            <div class="teacher-roster-table-wrapper">
                <table class="teacher-roster-table">
                    <thead>
                        <tr>
                            <th class="teacher-roster-col-id">Student ID</th>
                            <th class="teacher-roster-col-name">Student Name</th>
                            <th class="teacher-roster-col-email">Email</th>
                            <th class="teacher-roster-col-status">Status</th>
                            <th class="teacher-roster-col-enrolled">Enrollment Date</th>
                            <th class="teacher-roster-col-action">Action</th>
                        </tr>
                    </thead>
                    <tbody id="rosterTable"></tbody>
                </table>
            </div>
        </div>
    `,
        afterRender: () => {
            const classesData = {
                'CS101-A': {
                    name: 'CS101-A - Introduction to Programming',
                    count: 32,
                    students: [
                        { id: 'STU001', name: 'Ahmed Hassan', email: 'ahmed.hassan@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 92 },
                        { id: 'STU002', name: 'Fatima Khan', email: 'fatima.khan@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 95 },
                        { id: 'STU003', name: 'Ali Yilmaz', email: 'ali.yilmaz@university.edu', status: 'Active', enrolled: '2024-09-16', attendance: 85 },
                        { id: 'STU004', name: 'Zeynep Demir', email: 'zeynep.demir@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 98 },
                        { id: 'STU005', name: 'Mustafa Ozer', email: 'mustafa.ozer@university.edu', status: 'Inactive', enrolled: '2024-09-20', attendance: 60 }
                    ]
                },
                'CS101-B': {
                    name: 'CS101-B - Introduction to Programming',
                    count: 35,
                    students: [
                        { id: 'STU006', name: 'Layla Ahmed', email: 'layla.ahmed@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 94 },
                        { id: 'STU007', name: 'Omar Hassan', email: 'omar.hassan@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 88 },
                        { id: 'STU008', name: 'Noor Ibrahim', email: 'noor.ibrahim@university.edu', status: 'Active', enrolled: '2024-09-17', attendance: 91 },
                        { id: 'STU009', name: 'Hana Malik', email: 'hana.malik@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 96 },
                        { id: 'STU010', name: 'Karim Saleh', email: 'karim.saleh@university.edu', status: 'Active', enrolled: '2024-09-18', attendance: 75 }
                    ]
                },
                'MATH101-A': {
                    name: 'MATH101-A - Calculus I',
                    count: 40,
                    students: [
                        { id: 'STU011', name: 'Aisha Mohammed', email: 'aisha.mohammed@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 93 },
                        { id: 'STU012', name: 'Ibrahim Rahman', email: 'ibrahim.rahman@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 89 },
                        { id: 'STU013', name: 'Sara Ahmed', email: 'sara.ahmed@university.edu', status: 'Active', enrolled: '2024-09-16', attendance: 97 },
                        { id: 'STU014', name: 'Hassan Ali', email: 'hassan.ali@university.edu', status: 'Inactive', enrolled: '2024-09-19', attendance: 55 },
                        { id: 'STU015', name: 'Maryam Hassan', email: 'maryam.hassan@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 90 }
                    ]
                },
                'ENG101-A': {
                    name: 'ENG101-A - English I',
                    count: 25,
                    students: [
                        { id: 'STU016', name: 'Leila Farrokhi', email: 'leila.farrokhi@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 95 },
                        { id: 'STU017', name: 'Reza Pakzad', email: 'reza.pakzad@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 92 },
                        { id: 'STU018', name: 'Nastaran Salaem', email: 'nastaran.salaem@university.edu', status: 'Active', enrolled: '2024-09-17', attendance: 88 },
                        { id: 'STU019', name: 'Dariush Karim', email: 'dariush.karim@university.edu', status: 'Active', enrolled: '2024-09-15', attendance: 86 },
                        { id: 'STU020', name: 'Parisa Ahmadi', email: 'parisa.ahmadi@university.edu', status: 'Active', enrolled: '2024-09-18', attendance: 94 }
                    ]
                }
            };

            let currentClassId = 'CS101-A';

            function loadRoster(classId) {
                const classData = classesData[classId];
                currentClassId = classId;

                document.getElementById('classInfo').textContent = classData.name;
                document.getElementById('rosterCount').textContent = `${classData.count} students enrolled`;

                renderRoster(classData.students);
            }

            function renderRoster(students) {
                const table = document.getElementById('rosterTable');
                table.innerHTML = students.map(student => `
                <tr class="teacher-roster-row">
                    <td class="teacher-roster-col-id">${student.id}</td>
                    <td class="teacher-roster-col-name">${student.name}</td>
                    <td class="teacher-roster-col-email">${student.email}</td>
                    <td class="teacher-roster-col-status">
                        <span class="teacher-roster-status ${student.status === 'Active' ? 'teacher-roster-status-active' : 'teacher-roster-status-inactive'}">
                            ${student.status}
                        </span>
                    </td>
                    <td class="teacher-roster-col-enrolled">${student.enrolled}</td>
                    <td class="teacher-roster-col-action">
                        <div class="teacher-roster-dropdown">
                            <button class="teacher-roster-dropdown-btn">⋮</button>
                            <div class="teacher-roster-dropdown-menu">
                                <button class="teacher-roster-dropdown-item teacher-roster-dropdown-message" onclick="window.sendMessage('${student.id}', '${student.name}')">
                                    <span>✉</span> Message
                                </button>
                                <button class="teacher-roster-dropdown-item teacher-roster-dropdown-grade" onclick="window.changeGrade('${student.id}', '${student.name}')">
                                    <span>📝</span> Change Grade
                                </button>
                                <button class="teacher-roster-dropdown-item teacher-roster-dropdown-attendance" onclick="window.changeAttendance('${student.id}', '${student.name}', '${student.attendance}')">
                                    <span>📋</span> Change Attendance
                                </button>
                            </div>
                        </div>
                    </td>
                </tr>
            `).join('');
            }

            window.sendMessage = function (studentId, studentName) {
                alert(`Send message to ${studentName} (${studentId})`);
            };

            window.changeGrade = function (studentId, studentName) {
                const modal = document.getElementById('changeGradeModal');
                if (!modal) {
                    // Create modal if it doesn't exist
                    const body = document.body;
                    const modalHTML = `
            <div id="changeGradeModal" class="teacher-change-grade-modal">
                <div class="teacher-change-grade-modal-content">
                    <div class="teacher-change-grade-modal-header">
                        <h3>Change Grade - <span id="modalStudentName"></span></h3>
                        <button class="teacher-change-grade-modal-close" onclick="document.getElementById('changeGradeModal').style.display='none'">×</button>
                    </div>
                    <div class="teacher-change-grade-modal-body">
                        <div class="teacher-change-grade-form">
                            <div class="teacher-change-grade-form-group">
                                <label>Exam 1:</label>
                                <input type="number" id="gradeExam1" min="0" max="100" placeholder="0">
                                <span class="teacher-change-grade-max">/100</span>
                            </div>
                            <div class="teacher-change-grade-form-group">
                                <label>Exam 2:</label>
                                <input type="number" id="gradeExam2" min="0" max="100" placeholder="0">
                                <span class="teacher-change-grade-max">/100</span>
                            </div>
                            <div class="teacher-change-grade-form-group">
                                <label>Final:</label>
                                <input type="number" id="gradeFinal" min="0" max="100" placeholder="0">
                                <span class="teacher-change-grade-max">/100</span>
                            </div>
                            <div class="teacher-change-grade-form-group">
                                <label>Project:</label>
                                <input type="number" id="gradeProject" min="0" max="100" placeholder="Leave empty if no project">
                                <span class="teacher-change-grade-max"></span>
                            </div>
                        </div>

                        <div class="teacher-change-grade-preview">
                            <div class="teacher-change-grade-preview-item">
                                <span>Current Average:</span>
                                <span class="teacher-change-grade-preview-value" id="gradeAverage">-</span>
                            </div>
                            <div class="teacher-change-grade-preview-item">
                                <span>Estimated Grade:</span>
                                <span class="teacher-change-grade-preview-letter" id="gradeLetter">-</span>
                            </div>
                        </div>
                    </div>
                    <div class="teacher-change-grade-modal-footer">
                        <button class="teacher-change-grade-btn-cancel" onclick="document.getElementById('changeGradeModal').style.display='none'">Cancel</button>
                        <button class="teacher-change-grade-btn-save" onclick="window.saveGrade()">Save Grade</button>
                    </div>
                </div>
            </div>
        `;
                    body.insertAdjacentHTML('beforeend', modalHTML);

                    // Add event listeners for real-time calculation
                    document.getElementById('gradeExam1').addEventListener('change', window.updateGradePreview);
                    document.getElementById('gradeExam2').addEventListener('change', window.updateGradePreview);
                    document.getElementById('gradeFinal').addEventListener('change', window.updateGradePreview);
                    document.getElementById('gradeProject').addEventListener('change', window.updateGradePreview);

                    // Close modal on background click
                    document.getElementById('changeGradeModal').addEventListener('click', function (e) {
                        if (e.target === this) this.style.display = 'none';
                    });
                }

                document.getElementById('modalStudentName').textContent = studentName;
                document.getElementById('gradeExam1').value = '';
                document.getElementById('gradeExam2').value = '';
                document.getElementById('gradeFinal').value = '';
                document.getElementById('gradeProject').value = '';
                document.getElementById('gradeAverage').textContent = '-';
                document.getElementById('gradeLetter').textContent = '-';

                document.getElementById('changeGradeModal').style.display = 'flex';
            };

            window.updateGradePreview = function () {
                const exam1 = parseFloat(document.getElementById('gradeExam1').value) || 0;
                const exam2 = parseFloat(document.getElementById('gradeExam2').value) || 0;
                const final = parseFloat(document.getElementById('gradeFinal').value) || 0;
                const project = document.getElementById('gradeProject').value ? parseFloat(document.getElementById('gradeProject').value) : null;

                let average = exam1 * 0.2 + exam2 * 0.2 + final * 0.4;
                if (project !== null) {
                    average += project * 0.2;
                }

                let letterGrade = '-';
                if (average >= 90) letterGrade = 'A';
                else if (average >= 85) letterGrade = 'A-';
                else if (average >= 80) letterGrade = 'B+';
                else if (average >= 75) letterGrade = 'B';
                else if (average >= 70) letterGrade = 'B-';
                else if (average >= 65) letterGrade = 'C+';
                else if (average >= 60) letterGrade = 'C';
                else if (average >= 55) letterGrade = 'C-';
                else if (average >= 50) letterGrade = 'D';
                else if (average > 0) letterGrade = 'F';

                function getLetterClass(letter) {
                    if (letter.startsWith('A')) return 'teacher-change-grade-letter-a';
                    if (letter.startsWith('B')) return 'teacher-change-grade-letter-b';
                    if (letter.startsWith('C')) return 'teacher-change-grade-letter-c';
                    if (letter === 'D') return 'teacher-change-grade-letter-d';
                    if (letter === 'F') return 'teacher-change-grade-letter-f';
                    return '';
                }

                document.getElementById('gradeAverage').textContent = average.toFixed(2);
                const letterEl = document.getElementById('gradeLetter');
                letterEl.textContent = letterGrade;
                letterEl.className = `teacher-change-grade-preview-letter ${getLetterClass(letterGrade)}`;
            };

            window.saveGrade = function () {
                const exam1 = document.getElementById('gradeExam1').value;
                const exam2 = document.getElementById('gradeExam2').value;
                const final = document.getElementById('gradeFinal').value;
                const studentName = document.getElementById('modalStudentName').textContent;

                if (!exam1 || !exam2 || !final) {
                    alert('Please fill in Exam 1, Exam 2, and Final grades');
                    return;
                }

                alert(`Grade saved for ${studentName}`);
                document.getElementById('changeGradeModal').style.display = 'none';
            };

            window.changeAttendance = function (studentId, studentName, currentAttendance) {
                const newAttendance = prompt(`Change attendance for ${studentName}\nCurrent: ${currentAttendance}%\nNew attendance (0-100):`, currentAttendance);
                if (newAttendance !== null) {
                    alert(`Attendance updated to ${newAttendance}% for ${studentName}`);
                }
            };

            window.changeAttendance = function (studentId, studentName, currentAttendance) {
                const newAttendance = prompt(`Change attendance for ${studentName}\nCurrent: ${currentAttendance}%\nNew attendance (0-100):`, currentAttendance);
                if (newAttendance !== null) {
                    alert(`Attendance updated to ${newAttendance}% for ${studentName}`);
                }
            };

            document.getElementById('classSelect').addEventListener('change', function () {
                loadRoster(this.value);
                document.getElementById('searchInput').value = '';
            });

            document.getElementById('searchInput').addEventListener('keyup', function () {
                const searchTerm = this.value.toLowerCase();
                const classData = classesData[currentClassId];
                const filtered = classData.students.filter(student =>
                    student.name.toLowerCase().includes(searchTerm) ||
                    student.id.toLowerCase().includes(searchTerm)
                );
                renderRoster(filtered);
            });

            loadRoster('CS101-A');
        }
    },

    announcements: {
        render: () => `
        <div class="teacher-breadcrumb">Home / Announcements</div>
        <div class="teacher-section-header">Class Announcements</div>
        
        <div class="teacher-announcements-container">
            <div class="teacher-announcements-header">
                <div class="teacher-announcements-info">
                    <p id="classInfo">Select a class to manage announcements</p>
                </div>
                <button class="teacher-announcements-new-btn" onclick="window.openNewAnnouncement()">+ New Announcement</button>
            </div>

            <div class="teacher-announcements-selector">
                <label>Select Class:</label>
                <select id="classSelect" class="teacher-announcements-class-select">
                    <option value="CS101-A">CS101-A - Introduction to Programming</option>
                    <option value="CS101-B">CS101-B - Introduction to Programming</option>
                    <option value="MATH101-A">MATH101-A - Calculus I</option>
                    <option value="ENG101-A">ENG101-A - English I</option>
                </select>
            </div>

            <div class="teacher-announcements-list" id="announcementsList"></div>
        </div>

        <!-- Modal: New Announcement -->
        <div class="teacher-announcement-modal" id="announcementModal">
            <div class="teacher-announcement-modal-content">
                <div class="teacher-announcement-modal-body">
                    <div class="teacher-announcement-form-group">
                        <label>Class:</label>
                        <span id="modalClassName" class="teacher-announcement-class-display"></span>
                    </div>
                    <div class="teacher-announcement-form-group">
                        <label>Title *</label>
                        <input type="text" id="announcementTitle" class="teacher-announcement-input" placeholder="Announcement title">
                    </div>
                    <div class="teacher-announcement-form-group">
                        <label>Message *</label>
                        <textarea id="announcementMessage" class="teacher-announcement-textarea" placeholder="Write your announcement here..." rows="8"></textarea>
                    </div>
                    <div class="teacher-announcement-form-group">
                        <label>
                            <input type="checkbox" id="announcementUrgent" class="teacher-announcement-checkbox">
                            Mark as Important/Urgent
                        </label>
                    </div>
                    <div class="teacher-announcement-form-group">
                        <label>
                            <input type="checkbox" id="announcementEmail" class="teacher-announcement-checkbox" checked>
                            <span class="teacher-announcement-email-label">
                                <span class="teacher-announcement-email-icon">✉</span>
                                Also send as email to all students
                            </span>
                        </label>
                    </div>
                </div>
                <div class="teacher-announcement-modal-footer">
                    <button class="teacher-announcement-btn-cancel" onclick="document.getElementById('announcementModal').style.display='none'">Cancel</button>
                    <button class="teacher-announcement-btn-send" onclick="window.sendAnnouncement()">Send Announcement</button>
                </div>
            </div>
        </div>
    `,
        afterRender: () => {
            const classesData = {
                'CS101-A': {
                    name: 'CS101-A - Introduction to Programming',
                    announcements: [
                        { id: 1, title: 'Midterm Exam Schedule', message: 'The midterm exam will be held on October 15th from 10:00 AM to 12:00 PM in Room 101.', date: '2024-10-05', urgent: true },
                        { id: 2, title: 'Assignment 3 Submitted', message: 'All students must submit Assignment 3 by October 10th. Late submissions will have a 10% penalty.', date: '2024-10-01', urgent: false }
                    ]
                },
                'CS101-B': {
                    name: 'CS101-B - Introduction to Programming',
                    announcements: [
                        { id: 3, title: 'Lab Session Moved', message: 'This week\'s lab session has been rescheduled to Thursday at 2 PM instead of Wednesday.', date: '2024-10-03', urgent: true }
                    ]
                },
                'MATH101-A': {
                    name: 'MATH101-A - Calculus I',
                    announcements: [
                        { id: 4, title: 'Homework Solutions Posted', message: 'Solutions for Chapters 1-3 homework have been posted on the course portal.', date: '2024-10-02', urgent: false }
                    ]
                },
                'ENG101-A': {
                    name: 'ENG101-A - English I',
                    announcements: []
                }
            };

            let currentClassId = 'CS101-A';

            function loadAnnouncements(classId) {
                const classData = classesData[classId];
                currentClassId = classId;

                document.getElementById('classInfo').textContent = classData.name;

                const list = document.getElementById('announcementsList');
                if (classData.announcements.length === 0) {
                    list.innerHTML = `
                    <div class="teacher-announcements-empty">
                        <p>No announcements yet</p>
                        <p>Click "New Announcement" to send one</p>
                    </div>
                `;
                    return;
                }

                list.innerHTML = classData.announcements.map(ann => `
                <div class="teacher-announcement-card ${ann.urgent ? 'teacher-announcement-card-urgent' : ''}">
                    <div class="teacher-announcement-card-header">
                        <div>
                            <h4 class="teacher-announcement-card-title">${ann.title}</h4>
                            ${ann.urgent ? '<span class="teacher-announcement-urgent-badge">URGENT</span>' : ''}
                        </div>
                        <span class="teacher-announcement-card-date">${new Date(ann.date).toLocaleDateString()}</span>
                    </div>
                    <div class="teacher-announcement-card-content">
                        <p>${ann.message}</p>
                    </div>
                    <div class="teacher-announcement-card-actions">
                        <button class="teacher-announcement-edit-btn" onclick="alert('Edit announcement with ID: ${ann.id}')">Edit</button>
                        <button class="teacher-announcement-delete-btn" onclick="window.deleteAnnouncement(${ann.id}, '${classId}')">Delete</button>
                    </div>
                </div>
            `).join('');
            }

            window.openNewAnnouncement = function () {
                document.getElementById('modalClassName').textContent = classesData[currentClassId].name;
                document.getElementById('announcementTitle').value = '';
                document.getElementById('announcementMessage').value = '';
                document.getElementById('announcementUrgent').checked = false;
                document.getElementById('announcementModal').style.display = 'flex';
            };

            window.sendAnnouncement = function () {
                const title = document.getElementById('announcementTitle').value.trim();
                const message = document.getElementById('announcementMessage').value.trim();
                const urgent = document.getElementById('announcementUrgent').checked;
                const sendEmail = document.getElementById('announcementEmail').checked;

                if (!title || !message) {
                    alert('Please fill in both title and message');
                    return;
                }

                const classData = classesData[currentClassId];
                const newAnnouncement = {
                    id: Date.now(),
                    title: title,
                    message: message,
                    date: new Date().toISOString().split('T')[0],
                    urgent: urgent,
                    emailSent: sendEmail
                };

                classData.announcements.unshift(newAnnouncement);

                // Show toast notification
                const toast = document.getElementById('toastNotification');
                if (!toast) {
                    const body = document.body;
                    body.insertAdjacentHTML('beforeend', `<div id="toastNotification" class="teacher-announcement-toast"></div>`);
                }

                const toastEl = document.getElementById('toastNotification');
                const className = classData.name.split(' - ')[1];

                if (sendEmail) {
                    toastEl.textContent = `✓ Announcement sent & email delivered to all students in ${className}`;
                } else {
                    toastEl.textContent = `✓ Announcement posted to ${className}`;
                }

                toastEl.className = 'teacher-announcement-toast teacher-announcement-toast-success teacher-announcement-toast-show';

                setTimeout(() => {
                    toastEl.classList.remove('teacher-announcement-toast-show');
                }, 4000);

                document.getElementById('announcementModal').style.display = 'none';
                loadAnnouncements(currentClassId);
            };

            window.deleteAnnouncement = function (annId, classId) {
                if (confirm('Are you sure you want to delete this announcement?')) {
                    const classData = classesData[classId];
                    classData.announcements = classData.announcements.filter(ann => ann.id !== annId);
                    loadAnnouncements(classId);

                    const toastEl = document.getElementById('toastNotification');
                    toastEl.textContent = '✓ Announcement deleted';
                    toastEl.className = 'teacher-announcement-toast teacher-announcement-toast-success teacher-announcement-toast-show';
                    setTimeout(() => {
                        toastEl.classList.remove('teacher-announcement-toast-show');
                    }, 4000);
                }
            };

            document.getElementById('classSelect').addEventListener('change', function () {
                loadAnnouncements(this.value);
            });

            // Close modal on background click
            document.getElementById('announcementModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            loadAnnouncements('CS101-A');
        }
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