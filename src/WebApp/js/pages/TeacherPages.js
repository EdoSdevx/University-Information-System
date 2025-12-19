/**
 * TeacherPages Module
 * Page templates for teacher dashboard with teacher-* prefix
 */
import { apiRequest } from '../core/ApiService.js';

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
        <div class="teacher-classes-grid" id="classesGrid">
            <div style="text-align: center; padding: 40px; grid-column: 1/-1;">Loading classes...</div>
        </div>

        <div class="teacher-class-modal" id="classModal" style="display: none;">
            <div class="teacher-class-modal-content">
                <div class="teacher-class-modal-header">
                    <h3 id="modalTitle"></h3>
                    <button class="teacher-class-modal-close" onclick="document.getElementById('classModal').style.display='none'">×</button>
                </div>
                <div class="teacher-class-modal-body">
                    <table class="teacher-class-modal-table">
                        <thead>
                            <tr>
                                <th>Student ID</th>
                                <th>Student Name</th>
                                <th>Email</th>
                                <th>Status</th>
                                <th>Enrolled Date</th>
                            </tr>
                        </thead>
                        <tbody id="classStudentsList"></tbody>
                    </table>
                </div>
            </div>
        </div>
    `,
        afterRender: async () => {
            const formatSchedule = (day1, day2, startTime) => {
                if (!day1 || !startTime) return 'TBA';

                const dayNames = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
                const dayAbbr = dayNames[day1] || 'TBA';

                const formatTime = (time) => {
                    if (!time) return '';
                    const [hours, minutes] = time.split(':');
                    const h = parseInt(hours);
                    const ampm = h >= 12 ? 'pm' : 'am';
                    const displayH = h > 12 ? h - 12 : (h === 0 ? 12 : h);
                    return `${displayH}:${minutes}${ampm}`;
                };

                const timeStr = formatTime(startTime);
                return `${dayAbbr} ${timeStr}`;
            };

            const getStudentCount = async (courseInstanceId) => {
                try {
                    const response = await apiRequest(
                        `/enrollment/course/${courseInstanceId}?pageIndex=1&pageSize=1000`
                    );
                    return response.ok && response.data ? response.data.length : 0;
                } catch (error) {
                    console.error(`Failed to fetch student count:`, error);
                    return 0;
                }
            };

            try {
                const response = await apiRequest('/courseInstance/my-courses?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data || response.data.length === 0) {
                    document.getElementById('classesGrid').innerHTML =
                        '<div style="text-align: center; padding: 40px; grid-column: 1/-1;">No classes assigned.</div>';
                    return;
                }

                const courses = response.data;
                const classCardsHTML = await Promise.all(courses.map(async (course) => {
                    const studentCount = await getStudentCount(course.courseInstanceId);
                    const schedule = formatSchedule(course.day1, course.day2, course.startTime);

                    return `
                    <div class="teacher-class-card">
                        <div class="teacher-class-header">
                            <div class="teacher-class-code">${course.courseCode}</div>
                            <div class="teacher-class-name">${course.courseName}</div>
                        </div>
                        <div class="teacher-class-body">
                            <div class="teacher-class-info">
                                <div><span class="teacher-label">Students:</span> ${studentCount}</div>
                                <div><span class="teacher-label">Schedule:</span> ${schedule}</div>
                                <div><span class="teacher-label">Section:</span> ${course.section || 'A'}</div>
                                <div><span class="teacher-label">Location:</span> ${course.location || 'TBA'}</div>
                            </div>
                            <button class="teacher-btn" onclick="window.viewClass(${course.courseInstanceId}, '${course.courseCode}', '${course.courseName}')">View Class</button>
                        </div>
                    </div>
                `;
                }));

                document.getElementById('classesGrid').innerHTML = classCardsHTML.join('');
            } catch (error) {
                console.error('Failed to load classes:', error);
                document.getElementById('classesGrid').innerHTML =
                    '<div style="text-align: center; padding: 40px; grid-column: 1/-1; color: red;">Failed to load classes</div>';
            }

            window.viewClass = async function (courseInstanceId, courseCode, courseName) {
                document.getElementById('modalTitle').textContent = `${courseCode} - ${courseName}`;
                document.getElementById('classStudentsList').innerHTML = '<tr><td colspan="5" style="text-align: center; padding: 20px;">Loading students...</td></tr>';
                document.getElementById('classModal').style.display = 'flex';

                try {
                    const response = await apiRequest(
                        `/enrollment/course/${courseInstanceId}?pageIndex=1&pageSize=1000`
                    );

                    if (!response.ok || !response.data || response.data.length === 0) {
                        document.getElementById('classStudentsList').innerHTML =
                            '<tr><td colspan="5" style="text-align: center; padding: 20px;">No students enrolled</td></tr>';
                        return;
                    }

                    const students = response.data;
                    const studentsList = students.map(student => `
                    <tr>
                        <td>${student.studentCode || student.studentId}</td>
                        <td>${student.studentName || 'Unknown'}</td>
                        <td>${student.studentEmail || '-'}</td>
                        <td>${student.status || 'Active'}</td>
                        <td>${student.enrolledAt ? new Date(student.enrolledAt).toLocaleDateString() : '-'}</td>
                    </tr>
                `).join('');

                    document.getElementById('classStudentsList').innerHTML = studentsList;
                } catch (error) {
                    console.error('Failed to load students:', error);
                    document.getElementById('classStudentsList').innerHTML =
                        '<tr><td colspan="5" style="text-align: center; padding: 20px; color: red;">Failed to load students</td></tr>';
                }
            };

            document.getElementById('classModal').addEventListener('click', function (e) {
                if (e.target === this) {
                    this.style.display = 'none';
                }
            });
        }
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
                <option value="">Loading classes...</option>
            </select>
        </div>

        <div class="teacher-grading-info">
            <p id="classInfo">Select a class to manage grades</p>
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

    <!-- Confirmation Modal -->
    <div class="teacher-grading-confirm-modal" id="confirmModal">
        <div class="teacher-grading-modal-content">
            <div class="teacher-grading-modal-header">
                <h3>Confirm Grade Changes</h3>
                <button class="teacher-grading-modal-close" onclick="window.closeConfirmModal()">&times;</button>
            </div>
            <div class="teacher-grading-modal-body">
                <div class="teacher-grading-changes-info">
                    Review all changes below before confirming. This action will update grades in the system.
                </div>
                <table class="teacher-grading-changes-table" id="changesTable">
                    <thead>
                        <tr>
                            <th>Student Name</th>
                            <th>Exam 1</th>
                            <th>Exam 2</th>
                            <th>Final</th>
                            <th>Project</th>
                        </tr>
                    </thead>
                    <tbody id="changesBody"></tbody>
                </table>
            </div>
            <div class="teacher-grading-modal-footer">
                <button class="teacher-grading-btn-cancel" onclick="window.closeConfirmModal()">Cancel</button>
                <button class="teacher-grading-btn-confirm" onclick="window.confirmSaveGrades()">Confirm & Save</button>
            </div>
        </div>
    </div>
`,
        afterRender: async () => {
            const classSelect = document.getElementById('classSelect');
            const classInfo = document.getElementById('classInfo');
            const gradesTable = document.getElementById('gradesTable');
            const saveBtn = document.getElementById('saveBtn');
            const confirmModal = document.getElementById('confirmModal');

            let courses = [];
            let currentClassId = null;
            let courseData = {};
            let changedGrades = {};
            let originalGrades = {};

            const calculateBellCurveGrades = (students) => {
                const scores = students.map(student => {
                    let score = (student.exam1 || 0) * 0.2 +
                        (student.exam2 || 0) * 0.2 +
                        (student.final || 0) * 0.4;
                    if (student.project) {
                        score += student.project * 0.2;
                    }
                    return score;
                });

                const sortedScores = [...scores].sort((a, b) => b - a);

                const gradeThresholds = {
                    'A': Math.ceil(sortedScores.length * 0.10),
                    'B': Math.ceil(sortedScores.length * 0.30),
                    'C': Math.ceil(sortedScores.length * 0.60),
                    'D': Math.ceil(sortedScores.length * 0.85),
                    'F': sortedScores.length
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
            };

            const getLetterGradeClass = (letter) => {
                const map = {
                    'A': 'teacher-grading-letter-a',
                    'B': 'teacher-grading-letter-b',
                    'C': 'teacher-grading-letter-c',
                    'D': 'teacher-grading-letter-d',
                    'F': 'teacher-grading-letter-f'
                };
                return map[letter] || '';
            };

            const showToast = (message, type = 'success') => {
                const toast = document.getElementById('toastNotification');
                toast.textContent = message;
                toast.className = `teacher-grading-toast teacher-grading-toast-${type} teacher-grading-toast-show`;

                setTimeout(() => {
                    toast.classList.remove('teacher-grading-toast-show');
                }, 4000);
            };

            const loadGradesForClass = async (courseInstanceId) => {
                gradesTable.innerHTML = '<tr><td colspan="8" style="text-align: center; padding: 20px;">Loading grades...</td></tr>';

                const course = courses.find(c => c.courseInstanceId === courseInstanceId);
                classInfo.textContent = `${course.courseCode}: ${course.courseName} (${course.section})`;

                try {
                    const enrollmentsResponse = await apiRequest(
                        `/enrollment/course/${courseInstanceId}?pageIndex=1&pageSize=1000`
                    );
                    const gradesResponse = await apiRequest(
                        `/grade/course/${courseInstanceId}?pageIndex=1&pageSize=1000`
                    );

                    const enrollments = enrollmentsResponse.ok && enrollmentsResponse.data ? enrollmentsResponse.data : [];
                    const grades = gradesResponse.ok && gradesResponse.data ? gradesResponse.data : [];

                    const studentMap = {};
                    enrollments.forEach(enrollment => {
                        studentMap[enrollment.studentId] = {
                            id: enrollment.studentId,
                            name: enrollment.studentName || 'Unknown',
                            studentId: enrollment.studentCode || enrollment.studentId
                        };
                    });

                    const students = Object.values(studentMap);

                    const studentGrades = students.map(student => {
                        const gradeRecord = grades.find(g => g.studentId === student.id);
                        return {
                            ...student,
                            gradeId: gradeRecord?.id || null,
                            exam1: gradeRecord?.exam1 || null,
                            exam2: gradeRecord?.exam2 || null,
                            final: gradeRecord?.final || null,
                            project: gradeRecord?.project || null
                        };
                    });

                    courseData[courseInstanceId] = {
                        course,
                        students: studentGrades
                    };

                    originalGrades = {};
                    studentGrades.forEach((student, idx) => {
                        originalGrades[idx] = {
                            id: student.id,
                            name: student.name,
                            exam1: student.exam1,
                            exam2: student.exam2,
                            final: student.final,
                            project: student.project
                        };
                    });

                    const bellCurveGrades = calculateBellCurveGrades(studentGrades);

                    gradesTable.innerHTML = studentGrades.map((student, idx) => {
                        const letter = bellCurveGrades[idx];
                        return `
                                <tr class="teacher-grading-row">
                                    <td class="teacher-grading-col-student">${student.name}</td>
                                    <td class="teacher-grading-col-id">${student.studentId}</td>
                                    <td class="teacher-grading-col-grade">
                                        <input type="number" class="teacher-grading-input exam1-${idx}" value="${student.exam1 || ''}" min="0" oninput="if(this.value > 100) this.value = this.value.slice(0, -1)" onchange="window.updateGradeTable(${idx})">
                                    </td>
                                    <td class="teacher-grading-col-grade">
                                        <input type="number" class="teacher-grading-input exam2-${idx}" value="${student.exam2 || ''}" min="0" oninput="if(this.value > 100) this.value = this.value.slice(0, -1)" onchange="window.updateGradeTable(${idx})">
                                    </td>
                                    <td class="teacher-grading-col-grade">
                                        <input type="number" class="teacher-grading-input final-${idx}" value="${student.final || ''}" min="0" oninput="if(this.value > 100) this.value = this.value.slice(0, -1)" onchange="window.updateGradeTable(${idx})">
                                    </td>
                                    <td class="teacher-grading-col-grade">
                                        <input type="number" class="teacher-grading-input project-${idx}" value="${student.project || ''}" min="0" placeholder="-" oninput="if(this.value > 100) this.value = this.value.slice(0, -1)" onchange="window.updateGradeTable(${idx})">
                                    </td>
                                    <td class="teacher-grading-col-letter">
                                        <span class="teacher-grading-letter ${getLetterGradeClass(letter)} letter-${idx}">${letter}</span>
                                    </td>
                                    <td class="teacher-grading-col-action">
                                        <button class="teacher-grading-action-btn" onclick="window.clearGradesTable(${idx})">Clear</button>
                                    </td>
                                </tr>
                            `;
                    }).join('');

                    changedGrades = {};
                } catch (error) {
                    console.error('Failed to load grades:', error);
                    gradesTable.innerHTML = '<tr><td colspan="8" style="text-align: center; padding: 20px; color: red;">Failed to load grades</td></tr>';
                }
            };

            window.updateGradeTable = function (idx) {
                const courseId = currentClassId;
                const course = courseData[courseId];
                const student = course.students[idx];

                const exam1 = parseFloat(document.querySelector(`.exam1-${idx}`).value) || null;
                const exam2 = parseFloat(document.querySelector(`.exam2-${idx}`).value) || null;
                const final = parseFloat(document.querySelector(`.final-${idx}`).value) || null;
                const project = document.querySelector(`.project-${idx}`).value ? parseFloat(document.querySelector(`.project-${idx}`).value) : null;

                student.exam1 = exam1;
                student.exam2 = exam2;
                student.final = final;
                student.project = project;

                changedGrades[student.id] = {
                    idx,
                    gradeId: student.gradeId,
                    courseInstanceId: courseId,
                    studentId: student.id,
                    studentName: student.name,
                    exam1,
                    exam2,
                    final,
                    project
                };

                const bellCurveGrades = calculateBellCurveGrades(course.students);
                const letter = bellCurveGrades[idx];

                const letterEl = document.querySelector(`.letter-${idx}`);
                letterEl.textContent = letter;
                letterEl.className = `teacher-grading-letter ${getLetterGradeClass(letter)} letter-${idx}`;
            };

            window.clearGradesTable = function (idx) {
                document.querySelector(`.exam1-${idx}`).value = '';
                document.querySelector(`.exam2-${idx}`).value = '';
                document.querySelector(`.final-${idx}`).value = '';
                document.querySelector(`.project-${idx}`).value = '';
                document.querySelector(`.letter-${idx}`).textContent = '-';
                window.updateGradeTable(idx);
            };

            window.openConfirmModal = function () {
                const studentChanges = {};

                for (const [studentId, gradeData] of Object.entries(changedGrades)) {
                    const orig = originalGrades[gradeData.idx];
                    const components = ['exam1', 'exam2', 'final', 'project'];
                    let hasChanges = false;

                    const changeData = {
                        studentName: gradeData.studentName,
                        exam1: { orig: orig.exam1 ?? '-', new: gradeData.exam1 ?? '-', changed: false },
                        exam2: { orig: orig.exam2 ?? '-', new: gradeData.exam2 ?? '-', changed: false },
                        final: { orig: orig.final ?? '-', new: gradeData.final ?? '-', changed: false },
                        project: { orig: orig.project ?? '-', new: gradeData.project ?? '-', changed: false }
                    };

                    components.forEach(comp => {
                        if (gradeData[comp] !== orig[comp]) {
                            changeData[comp].changed = true;
                            hasChanges = true;
                        }
                    });

                    if (hasChanges) {
                        studentChanges[gradeData.idx] = changeData;
                    }
                }

                if (Object.keys(studentChanges).length === 0) {
                    showToast('No changes detected', 'warning');
                    return;
                }

                const changesTable = document.getElementById('changesTable');
                changesTable.innerHTML = `
        <thead>
            <tr>
                <th>Student Name</th>
                <th>Exam 1</th>
                <th>Exam 2</th>
                <th>Final</th>
                <th>Project</th>
            </tr>
        </thead>
        <tbody id="changesBody"></tbody>
    `;

                const changesBody = document.getElementById('changesBody');
                changesBody.innerHTML = Object.values(studentChanges).map(change => `
        <tr>
            <td style="font-weight: 600; color: #2c2c2c;">${change.studentName}</td>
            <td style="text-align: center;">
                <span style="color: #666;">${change.exam1.orig}</span>
                ${change.exam1.changed ? `<span style="color: #999;"> → </span><span style="color: #27ae60; font-weight: 600;">${change.exam1.new}</span>` : ''}
            </td>
            <td style="text-align: center;">
                <span style="color: #666;">${change.exam2.orig}</span>
                ${change.exam2.changed ? `<span style="color: #999;"> → </span><span style="color: #27ae60; font-weight: 600;">${change.exam2.new}</span>` : ''}
            </td>
            <td style="text-align: center;">
                <span style="color: #666;">${change.final.orig}</span>
                ${change.final.changed ? `<span style="color: #999;"> → </span><span style="color: #27ae60; font-weight: 600;">${change.final.new}</span>` : ''}
            </td>
            <td style="text-align: center;">
                <span style="color: #666;">${change.project.orig}</span>
                ${change.project.changed ? `<span style="color: #999;"> → </span><span style="color: #27ae60; font-weight: 600;">${change.project.new}</span>` : ''}
            </td>
        </tr>
    `).join('');

                confirmModal.classList.add('show');
            };

            window.closeConfirmModal = function () {
                confirmModal.classList.remove('show');
            };

            window.confirmSaveGrades = async function () {
                const btn = document.querySelector('.teacher-grading-btn-confirm');
                btn.disabled = true;
                btn.textContent = 'Saving...';

                let successCount = 0;
                let failCount = 0;

                for (const [studentId, gradeData] of Object.entries(changedGrades)) {
                    try {
                        let response;
                        if (gradeData.gradeId) {
                            response = await apiRequest(`/grade/${gradeData.gradeId}`, 'PUT', {
                                exam1: gradeData.exam1,
                                exam2: gradeData.exam2,
                                final: gradeData.final,
                                project: gradeData.project
                            });
                        } else {
                            response = await apiRequest('/grade', 'POST', {
                                studentId: gradeData.studentId,
                                courseInstanceId: gradeData.courseInstanceId,
                                exam1: gradeData.exam1,
                                exam2: gradeData.exam2,
                                final: gradeData.final,
                                project: gradeData.project
                            });
                        }

                        if (response.ok) {
                            successCount++;
                        } else {
                            failCount++;
                        }
                    } catch (error) {
                        failCount++;
                        console.error(`Error saving grade:`, error);
                    }
                }

                showToast(`Saved ${successCount} grade${successCount !== 1 ? 's' : ''}${failCount > 0 ? ` (${failCount} failed)` : ''}`);

                btn.disabled = false;
                btn.textContent = 'Confirm & Save';
                closeConfirmModal();
                changedGrades = {};

                await loadGradesForClass(currentClassId);
            };

            try {
                const response = await apiRequest('/courseInstance/my-courses?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data || response.data.length === 0) {
                    classSelect.innerHTML = '<option value="">No classes assigned</option>';
                    classInfo.textContent = 'No classes assigned';
                    return;
                }

                courses = response.data;
                currentClassId = courses[0].courseInstanceId;

                classSelect.innerHTML = courses.map(course => `
            <option value="${course.courseInstanceId}">
                ${course.courseCode} - ${course.courseName} (${course.section})
            </option>
        `).join('');

                classSelect.value = currentClassId;
                await loadGradesForClass(currentClassId);
            } catch (error) {
                console.error('Failed to load courses:', error);
                classSelect.innerHTML = '<option value="">Error loading classes</option>';
            }

            classSelect.addEventListener('change', function () {
                currentClassId = parseInt(this.value);
                loadGradesForClass(currentClassId);
            });

            saveBtn.addEventListener('click', function () {
                window.openConfirmModal();
            });

            confirmModal.addEventListener('click', function (e) {
                if (e.target === this) {
                    window.closeConfirmModal();
                }
            });
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
                <option value="">Loading classes...</option>
            </select>
        </div>
    </div>

    <div class="teacher-attendance-info">
        <p id="classInfo">Select a class to mark attendance</p>
    </div>

    <div class="teacher-attendance-weeks">
        <div class="teacher-attendance-weeks-header">
            <h4>Select Week</h4>
        </div>
        <div class="teacher-attendance-weeks-grid" id="weeksGrid"></div>
    </div>

    <div class="teacher-attendance-days" id="daysContainer" style="display: none; margin-bottom: 20px;">
        <div class="teacher-attendance-weeks-header">
            <h4>Select Day</h4>
        </div>
        <div id="daysGrid"></div>
    </div>

    <div class="teacher-attendance-list">
        <div class="teacher-attendance-list-header">
            <label class="teacher-attendance-select-all">
                <input type="checkbox" id="selectAllCheckbox" onchange="window.toggleSelectAll()">
                <span>Select All Students</span>
            </label>
        </div>
        <table class="teacher-attendance-table">
            <thead>
                <tr>
                    <th class="teacher-attendance-col-select"></th>
                    <th class="teacher-attendance-col-name">Student Name</th>
                    <th class="teacher-attendance-col-id">Student ID</th>
                    <th class="teacher-attendance-col-status">Mark Status</th>
                </tr>
            </thead>
            <tbody id="attendanceTable"></tbody>
        </table>
    </div>

    <div class="teacher-attendance-actions">
        <button class="teacher-attendance-btn-mark teacher-attendance-btn-present" onclick="window.markSelected('Present')">Mark Selected as Present</button>
        <button class="teacher-attendance-btn-mark teacher-attendance-btn-absent" onclick="window.markSelected('Absent')">Mark Selected as Absent</button>
    </div>

    <button class="teacher-attendance-save-btn" id="saveBtn">Save Attendance</button>
</div>
`,
        afterRender: async () => {
            const classSelect = document.getElementById('classSelect');
            const classInfo = document.getElementById('classInfo');
            const attendanceTable = document.getElementById('attendanceTable');
            const saveBtn = document.getElementById('saveBtn');
            const weeksGrid = document.getElementById('weeksGrid');
            const daysContainer = document.getElementById('daysContainer');
            const daysGrid = document.getElementById('daysGrid').parentElement;

            let courses = [];
            let loadedAttendance = {};
            let currentCourseId = null;
            let currentCourseData = null;
            let selectedWeek = 1;
            let selectedDay = null;
            const selectedStudents = new Set();
            const attendanceData = {};

            const showToast = (message, type = 'success') => {
                const toast = document.getElementById('attendanceToast');
                toast.textContent = message;
                toast.className = `teacher-attendance-toast teacher-attendance-toast-${type} teacher-attendance-toast-show`;
                setTimeout(() => {
                    toast.classList.remove('teacher-attendance-toast-show');
                }, 4000);
            };

            const daysOfWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

            const loadStudents = async (courseInstanceId) => {
                try {
                    const response = await apiRequest(
                        `/enrollment/course/${courseInstanceId}?pageIndex=1&pageSize=1000`
                    );

                    if (!response.ok) {
                        attendanceTable.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 20px; color: red;">Failed to load students</td></tr>';
                        return;
                    }

                    currentCourseId = courseInstanceId;
                    const enrollments = response.data;

                    const course = courses.find(c => c.courseInstanceId === courseInstanceId);
                    classInfo.textContent = `${course.courseCode}: ${course.courseName} (${course.section})`;

                    if (enrollments.length === 0) {
                        attendanceTable.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 20px; color: #888;">No students enrolled in this class</td></tr>';
                        currentCourseData = null;
                        daysContainer.style.display = 'none';
                        return;
                    }

                    currentCourseData = {
                        courseInstanceId,
                        day1: course.day1,
                        day2: course.day2,
                        students: enrollments.map(e => ({
                            enrollmentId: e.enrollmentId,
                            studentId: e.studentId,
                            studentName: e.studentName
                        }))
                    };

                    loadDays();
                    selectedDay = course.day1;
                    loadStudentTable();

                    await loadWeekAttendance(selectedWeek);
                } catch (error) {
                    console.error('Failed to load students:', error);
                    attendanceTable.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 20px; color: red;">Failed to load students</td></tr>';
                }
            };

            const loadDays = () => {
                if (!currentCourseData) return;

                let html = `
                <button class="teacher-attendance-week-btn teacher-attendance-week-btn-active" data-day="${currentCourseData.day1}" onclick="window.selectDay(${currentCourseData.day1})">
                    ${daysOfWeek[currentCourseData.day1]}
                </button>
            `;

                if (currentCourseData.day2 !== null && currentCourseData.day2 !== undefined) {
                    html += `
                    <button class="teacher-attendance-week-btn" data-day="${currentCourseData.day2}" onclick="window.selectDay(${currentCourseData.day2})">
                        ${daysOfWeek[currentCourseData.day2]}
                    </button>
                `;
                }

                document.getElementById('daysGrid').innerHTML = html;
                daysContainer.style.display = 'block';
            };

            const loadStudentTable = () => {
                attendanceTable.innerHTML = currentCourseData.students.map(student => `
                <tr class="teacher-attendance-row">
                    <td class="teacher-attendance-col-select">
                        <input type="checkbox" class="teacher-attendance-checkbox" value="${student.enrollmentId}" onchange="window.updateSelectedStudents()">
                    </td>
                    <td class="teacher-attendance-col-name">${student.studentName}</td>
                    <td class="teacher-attendance-col-id">${student.studentId}</td>
                    <td class="teacher-attendance-col-status">
                        <span class="teacher-attendance-status-display" id="status-${student.enrollmentId}">-</span>
                    </td>
                </tr>
            `).join('');

                selectedStudents.clear();
                document.getElementById('selectAllCheckbox').checked = false;
            };

            const loadWeekAttendance = async (week) => {
                if (!currentCourseId || !currentCourseData || !selectedDay) return;

                try {
                    const response = await apiRequest(
                        `/attendance/course/${currentCourseId}?week=${week}&pageIndex=1&pageSize=1000`
                    );

                    if (response.ok && response.data && Array.isArray(response.data)) {
                        const attendanceMap = {};
                        response.data.forEach(record => {
                            if (record.day === selectedDay) {
                                attendanceMap[record.enrollmentId] = record.status;
                                loadedAttendance[`${record.enrollmentId}-${week}-${selectedDay}`] = {
                                    id: record.id,
                                    status: record.status
                                };
                            }
                        });

                        currentCourseData.students.forEach(student => {
                            const statusDisplay = document.getElementById(`status-${student.enrollmentId}`);
                            if (statusDisplay) {
                                if (attendanceMap[student.enrollmentId]) {
                                    const status = attendanceMap[student.enrollmentId];
                                    const displayStatus = status.charAt(0).toUpperCase() + status.slice(1);
                                    statusDisplay.textContent = displayStatus;
                                    statusDisplay.className = `teacher-attendance-status-display teacher-attendance-status-${status.toLowerCase()}`;

                                    if (!attendanceData[student.enrollmentId]) {
                                        attendanceData[student.enrollmentId] = {};
                                    }
                                    attendanceData[student.enrollmentId][`${week}-${selectedDay}`] = status;
                                } else {
                                    statusDisplay.textContent = '-';
                                    statusDisplay.className = 'teacher-attendance-status-display';
                                }
                            }
                        });
                    }
                } catch (error) {
                    console.error('Failed to load week attendance:', error);
                }
            };

            const loadWeeks = () => {
                let html = '';
                for (let i = 1; i <= 16; i++) {
                    html += `
                    <button class="teacher-attendance-week-btn ${i === 1 ? 'teacher-attendance-week-btn-active' : ''}" onclick="window.selectWeek(${i})">
                        Week ${i}
                    </button>
                `;
                }
                weeksGrid.innerHTML = html;
            };

            window.selectWeek = function (week) {
                selectedWeek = week;
                document.querySelectorAll('#weeksGrid .teacher-attendance-week-btn').forEach((btn) => {
                    btn.classList.remove('teacher-attendance-week-btn-active');
                });
                document.querySelectorAll('#weeksGrid .teacher-attendance-week-btn')[week - 1].classList.add('teacher-attendance-week-btn-active');
                loadWeekAttendance(week);
            };

            window.selectDay = function (day) {
                selectedDay = day;
                document.querySelectorAll('#daysGrid .teacher-attendance-week-btn').forEach((btn) => {
                    btn.classList.remove('teacher-attendance-week-btn-active');
                });
                document.querySelectorAll('#daysGrid .teacher-attendance-week-btn').forEach((btn) => {
                    if (parseInt(btn.getAttribute('data-day')) === day) {
                        btn.classList.add('teacher-attendance-week-btn-active');
                    }
                });
                loadWeekAttendance(selectedWeek);
            };

            window.toggleSelectAll = function () {
                const checkboxes = document.querySelectorAll('.teacher-attendance-checkbox');
                const isChecked = document.getElementById('selectAllCheckbox').checked;

                if (isChecked) {
                    checkboxes.forEach(cb => {
                        cb.checked = true;
                        selectedStudents.add(cb.value);
                    });
                } else {
                    checkboxes.forEach(cb => {
                        cb.checked = false;
                        selectedStudents.delete(cb.value);
                    });
                }
            };

            window.updateSelectedStudents = function () {
                selectedStudents.clear();
                document.querySelectorAll('.teacher-attendance-checkbox:checked').forEach(cb => {
                    selectedStudents.add(cb.value);
                });

                const allCheckboxes = document.querySelectorAll('.teacher-attendance-checkbox');
                const checkedCount = document.querySelectorAll('.teacher-attendance-checkbox:checked').length;
                document.getElementById('selectAllCheckbox').checked = checkedCount === allCheckboxes.length;
            };

            window.markSelected = function (status) {
                if (selectedStudents.size === 0) {
                    showToast('Please select at least one student', 'warning');
                    return;
                }

                const statusText = status.charAt(0).toUpperCase() + status.slice(1);
                selectedStudents.forEach(enrollmentId => {
                    document.getElementById(`status-${enrollmentId}`).textContent = statusText;
                    document.getElementById(`status-${enrollmentId}`).className = `teacher-attendance-status-display teacher-attendance-status-${status.toLowerCase()}`;

                    if (!attendanceData[enrollmentId]) {
                        attendanceData[enrollmentId] = {};
                    }
                    attendanceData[enrollmentId][`${selectedWeek}-${selectedDay}`] = status;
                });

                showToast(`✓ Marked ${selectedStudents.size} students as ${statusText} for Week ${selectedWeek}`);

                document.querySelectorAll('.teacher-attendance-checkbox').forEach(cb => {
                    cb.checked = false;
                });
                document.getElementById('selectAllCheckbox').checked = false;
                selectedStudents.clear();
            };

            classSelect.addEventListener('change', function () {
                if (this.value) {
                    loadStudents(parseInt(this.value));
                }
            });

            saveBtn.addEventListener('click', async function () {
                if (Object.keys(attendanceData).length === 0) {
                    showToast('No attendance records to save', 'warning');
                    return;
                }

                saveBtn.disabled = true;
                saveBtn.textContent = 'Saving...';

                let successCount = 0;
                let failCount = 0;

                try {
                    const weekDayKey = `${selectedWeek}-${selectedDay}`;

                    for (const [enrollmentId, weekDays] of Object.entries(attendanceData)) {
                        const status = weekDays[weekDayKey];

                        if (!status) continue;

                        try {
                            const intEnrollmentId = parseInt(enrollmentId);
                            const key = `${intEnrollmentId}-${selectedWeek}-${selectedDay}`;

                            const loadedRecord = loadedAttendance[key];
                            if (loadedRecord && loadedRecord.status === status) {
                                continue; 
                            }

                            if (loadedRecord) {
                                const attendanceId = loadedRecord.id;
                                const response = await apiRequest(
                                    `/attendance/${attendanceId}`,
                                    'PUT',
                                    { status: status }
                                );

                                if (response.ok) {
                                    successCount++;
                                } else {
                                    failCount++;
                                }
                            } else {

                                const response = await apiRequest(
                                    '/attendance/mark',
                                    'POST',
                                    {
                                        enrollmentId: intEnrollmentId,
                                        week: selectedWeek,
                                        day: selectedDay,
                                        status: status
                                    }
                                );

                                if (response.ok) {
                                    successCount++;
                                } else {
                                    failCount++;
                                }
                            }
                        } catch (error) {
                            failCount++;
                            console.error('Error saving attendance:', error);
                        }
                    }

                    showToast(`✓ Saved ${successCount} attendance record${successCount !== 1 ? 's' : ''}${failCount > 0 ? ` (${failCount} failed)` : ''}`);

                    document.querySelectorAll('.teacher-attendance-checkbox').forEach(cb => {
                        cb.checked = false;
                    });
                    document.getElementById('selectAllCheckbox').checked = false;

                    for (const enrollmentId of Object.keys(attendanceData)) {
                        delete attendanceData[enrollmentId][weekDayKey];
                        if (Object.keys(attendanceData[enrollmentId]).length === 0) {
                            delete attendanceData[enrollmentId];
                        }
                    }

                    await loadWeekAttendance(selectedWeek);
                } catch (error) {
                    console.error('Save error:', error);
                    showToast('Error saving attendance', 'error');
                }

                saveBtn.disabled = false;
                saveBtn.textContent = 'Save Attendance';
            });

            try {
                const response = await apiRequest('/courseInstance/my-courses?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data || response.data.length === 0) {
                    classSelect.innerHTML = '<option value="">No classes assigned</option>';
                    classInfo.textContent = 'No classes assigned';
                    return;
                }

                courses = response.data;

                classSelect.innerHTML = courses.map(course => `
                <option value="${course.courseInstanceId}">
                    ${course.courseCode} - ${course.courseName} (${course.section})
                </option>
            `).join('');

                if (courses.length > 0) {
                    classSelect.value = courses[0].courseInstanceId;
                    await loadStudents(courses[0].courseInstanceId);
                }
            } catch (error) {
                console.error('Failed to load courses:', error);
                classSelect.innerHTML = '<option value="">Error loading classes</option>';
            }

            loadWeeks();
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
                <option value="">Loading classes...</option>
            </select>
            <input type="text" id="searchInput" class="teacher-roster-search" placeholder="Search by name or ID...">
        </div>

        <div class="teacher-roster-info">
            <p id="classInfo">Select a class to view roster</p>
            <span id="rosterCount">0 students enrolled</span>
        </div>

        <div class="teacher-roster-table-wrapper">
            <table class="teacher-roster-table">
                <thead>
                    <tr>
                        <th class="teacher-roster-col-id">Student ID</th>
                        <th class="teacher-roster-col-name">Student Name</th>
                        <th class="teacher-roster-col-email">Email</th>
                        <th class="teacher-roster-col-enrolled">Enrollment Date</th>
                        <th class="teacher-roster-col-action">Action</th>
                    </tr>
                </thead>
                <tbody id="rosterTable"></tbody>
            </table>
        </div>
    </div>
    `,
        afterRender: async () => {
            const classSelect = document.getElementById('classSelect');
            const classInfo = document.getElementById('classInfo');
            const rosterCount = document.getElementById('rosterCount');
            const rosterTable = document.getElementById('rosterTable');
            const searchInput = document.getElementById('searchInput');

            let courses = [];
            let currentCourseId = null;
            let allStudents = [];

            const renderRoster = (students) => {
                rosterTable.innerHTML = students.map(student => `
                <tr class="teacher-roster-row">
                    <td class="teacher-roster-col-id">${student.studentId}</td>
                    <td class="teacher-roster-col-name">${student.studentName}</td>
                    <td class="teacher-roster-col-email">${student.studentEmail || 'N/A'}</td>
                    <td class="teacher-roster-col-enrolled">${new Date(student.enrolledAt).toLocaleDateString()}</td>
                    <td class="teacher-roster-col-action">
                        <div class="teacher-roster-dropdown">
                            <button class="teacher-roster-dropdown-btn">⋮</button>
                            <div class="teacher-roster-dropdown-menu">
                                <button class="teacher-roster-dropdown-item teacher-roster-dropdown-message" onclick="window.sendMessage('${student.enrollmentId}', '${student.studentName}')">
                                    <span>✉</span> Message
                                </button>
                            </div>
                        </div>
                    </td>
                </tr>
            `).join('');
            };

            const loadRoster = async (courseInstanceId) => {
                try {
                    const response = await apiRequest(
                        `/enrollment/course/${courseInstanceId}?pageIndex=1&pageSize=1000`
                    );

                    if (!response.ok) {
                        rosterTable.innerHTML = '<tr><td colspan="5" style="text-align: center; padding: 20px; color: red;">Failed to load roster</td></tr>';
                        return;
                    }

                    currentCourseId = courseInstanceId;
                    allStudents = response.data;

                    const course = courses.find(c => c.courseInstanceId === courseInstanceId);
                    classInfo.textContent = `${course.courseCode}: ${course.courseName} (${course.section})`;
                    rosterCount.textContent = `${allStudents.length} students enrolled`;

                    if (allStudents.length === 0) {
                        rosterTable.innerHTML = '<tr><td colspan="5" style="text-align: center; padding: 20px; color: #888;">No students enrolled in this class</td></tr>';
                        return;
                    }

                    renderRoster(allStudents);
                } catch (error) {
                    console.error('Failed to load roster:', error);
                    rosterTable.innerHTML = '<tr><td colspan="5" style="text-align: center; padding: 20px; color: red;">Failed to load roster</td></tr>';
                }
            };

            classSelect.addEventListener('change', function () {
                if (this.value) {
                    loadRoster(parseInt(this.value));
                    searchInput.value = '';
                }
            });

            searchInput.addEventListener('keyup', function () {
                const searchTerm = this.value.toLowerCase();
                const filtered = allStudents.filter(student =>
                    student.studentName.toLowerCase().includes(searchTerm) ||
                    student.studentId.toString().includes(searchTerm)
                );
                renderRoster(filtered);
            });

            window.sendMessage = function (enrollmentId, studentName) {
                alert(`Send message to ${studentName}`);
            };

            try {
                const response = await apiRequest('/courseInstance/my-courses?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data || response.data.length === 0) {
                    classSelect.innerHTML = '<option value="">No classes assigned</option>';
                    classInfo.textContent = 'No classes assigned';
                    return;
                }

                courses = response.data;
                classSelect.innerHTML = courses.map(course => `
                <option value="${course.courseInstanceId}">
                    ${course.courseCode} - ${course.courseName} (${course.section})
                </option>
            `).join('');

                if (courses.length > 0) {
                    classSelect.value = courses[0].courseInstanceId;
                    await loadRoster(courses[0].courseInstanceId);
                }
            } catch (error) {
                console.error('Failed to load courses:', error);
                classSelect.innerHTML = '<option value="">Error loading classes</option>';
            }
        }
    },

    assignments: {
        render: () => `
    <div class="teacher-breadcrumb">Home / Assignments</div>
    <div class="teacher-section-header">Manage Assignments</div>
    
    <div id="assignmentToast" class="teacher-assignment-toast"></div>
    
    <div class="teacher-assignment-container">
        <div class="teacher-assignment-header">
            <div class="teacher-assignment-info">
                <p id="classInfo">Select a class to manage assignments</p>
            </div>
            <button class="teacher-assignment-new-btn" onclick="window.openNewAssignment()">+ New Assignment</button>
        </div>

        <div class="teacher-assignment-selector">
            <label>Select Class:</label>
            <select id="classSelect" class="teacher-assignment-class-select">
                <option value="">Loading courses...</option>
            </select>
        </div>

        <div class="teacher-assignment-list" id="assignmentsList">
            <div style="text-align: center; padding: 40px;">Loading assignments...</div>
        </div>
    </div>

    <div class="teacher-assignment-modal" id="assignmentModal" style="display: none;">
        <div class="teacher-assignment-modal-content">
            <div class="teacher-assignment-modal-header">
                <h3 id="assignmentModalTitle">Create New Assignment</h3>
                <button class="teacher-assignment-modal-close" onclick="document.getElementById('assignmentModal').style.display='none'">×</button>
            </div>
            <div class="teacher-assignment-modal-body">
                <div class="teacher-assignment-form-group">
                    <label>Class:</label>
                    <span id="modalClassName" class="teacher-assignment-class-display"></span>
                </div>
                <div class="teacher-assignment-form-group">
                    <label>Title *</label>
                    <input type="text" id="assignmentTitle" class="teacher-assignment-input" placeholder="Assignment title">
                </div>
                <div class="teacher-assignment-form-group">
                    <label>Description *</label>
                    <textarea id="assignmentDescription" class="teacher-assignment-textarea" placeholder="Assignment description..." rows="6"></textarea>
                </div>
                <div class="teacher-assignment-form-group">
                    <label>Due Date * (MM/DD/YYYY)</label>
                    <input type="text" id="assignmentDueDate" class="teacher-assignment-input" placeholder="MM/DD/YYYY" maxlength="10">
                </div>
                <div class="teacher-assignment-form-group">
                    <label>Total Points *</label>
                    <input type="number" id="assignmentPoints" class="teacher-assignment-input" placeholder="100" min="1">
                </div>
            </div>
            <div class="teacher-assignment-modal-footer">
                <button class="teacher-assignment-btn-cancel" onclick="document.getElementById('assignmentModal').style.display='none'">Cancel</button>
                <button class="teacher-assignment-btn-create" onclick="window.createAssignment()">Create Assignment</button>
            </div>
        </div>
    </div>

    <div class="teacher-assignment-submissions-modal" id="submissionsModal" style="display: none;">
        <div class="teacher-assignment-modal-content">
            <div class="teacher-assignment-modal-header">
                <h3 id="submissionsTitle"></h3>
                <button class="teacher-assignment-modal-close" onclick="document.getElementById('submissionsModal').style.display='none'">×</button>
            </div>
            <div class="teacher-assignment-modal-body">
                <table class="teacher-assignment-submissions-table">
                    <thead>
                        <tr>
                            <th>Student Name</th>
                            <th>Status</th>
                            <th>Submitted</th>
                            <th>Grade</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody id="submissionsTable"></tbody>
                </table>
            </div>
        </div>
    </div>

    <div class="teacher-assignment-modal" id="gradeModal" style="display: none;">
        <div class="teacher-assignment-modal-content">
            <div class="teacher-assignment-modal-header">
                <h3>Grade Submission</h3>
                <button class="teacher-assignment-modal-close" onclick="document.getElementById('gradeModal').style.display='none'">×</button>
            </div>
            <div class="teacher-assignment-modal-body">
                <div class="teacher-assignment-form-group">
                    <label>Student:</label>
                    <p id="gradeStudentName" style="margin: 0; font-size: 13px; color: #2c2c2c;"></p>
                </div>
                <div class="teacher-assignment-form-group">
                    <label>Grade (out of <span id="gradeMaxPoints"></span>) *</label>
                    <input type="number" id="gradePoints" class="teacher-assignment-input" placeholder="0" min="0">
                </div>
                <div class="teacher-assignment-form-group">
                    <label>Feedback</label>
                    <textarea id="gradeFeedback" class="teacher-assignment-textarea" placeholder="Optional feedback..." rows="4"></textarea>
                </div>
            </div>
            <div class="teacher-assignment-modal-footer">
                <button class="teacher-assignment-btn-cancel" onclick="document.getElementById('gradeModal').style.display='none'">Cancel</button>
                <button class="teacher-assignment-btn-create" onclick="window.submitGrade()">Submit Grade</button>
            </div>
        </div>
    </div>
`,
        afterRender: async () => {
            let courses = [];
            let currentCourseInstanceId = null;
            let currentCourseName = '';
            let currentAssignmentId = null;
            let currentSubmissionId = null;
            let currentEditingAssignmentId = null;
            let isEditMode = false;

            function formatDateInput(input) {
                let value = input.value.replace(/\D/g, '');

                if (value.length >= 2) {
                    value = value.substring(0, 2) + '/' + value.substring(2);
                }
                if (value.length >= 5) {
                    value = value.substring(0, 5) + '/' + value.substring(5, 9);
                }

                input.value = value;
            }

            function parseDate(dateString) {
                const parts = dateString.split('/');
                if (parts.length !== 3) return null;

                const month = parseInt(parts[0]);
                const day = parseInt(parts[1]);
                const year = parseInt(parts[2]);

                if (month < 1 || month > 12 || day < 1 || day > 31 || year < 2000) {
                    return null;
                }

                return `${year}-${String(month).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
            }

            async function loadCourses() {
                const response = await apiRequest('/courseInstance/my-courses?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data) {
                    document.getElementById('classSelect').innerHTML = '<option value="">No courses available</option>';
                    return;
                }

                const responseData = response.data.data || response.data;
                courses = responseData;

                if (courses.length === 0) {
                    document.getElementById('classSelect').innerHTML = '<option value="">No courses assigned</option>';
                    document.getElementById('assignmentsList').innerHTML = `
                    <div class="teacher-assignment-empty">
                        <p>No courses assigned to you</p>
                    </div>
                `;
                    return;
                }

                document.getElementById('classSelect').innerHTML = courses.map(course =>
                    `<option value="${course.courseInstanceId}">${course.courseCode} - ${course.courseName} (Section ${course.section})</option>`
                ).join('');

                currentCourseInstanceId = courses[0].courseInstanceId;
                currentCourseName = `${courses[0].courseCode} - ${courses[0].courseName}`;

                await loadAssignments(currentCourseInstanceId, currentCourseName);
            }

            async function loadAssignments(courseInstanceId, courseName) {
                currentCourseInstanceId = courseInstanceId;
                currentCourseName = courseName;

                document.getElementById('classInfo').textContent = courseName;
                document.getElementById('assignmentsList').innerHTML = '<div style="text-align: center; padding: 40px;">Loading assignments...</div>';

                const response = await apiRequest(`/assignment/course/${courseInstanceId}`);

                if (!response.ok || !response.data) {
                    document.getElementById('assignmentsList').innerHTML = `
                    <div class="teacher-assignment-empty">
                        <p>Failed to load assignments</p>
                        <p>Please try again</p>
                    </div>
                `;
                    return;
                }

                const assignments = response.data;

                if (assignments.length === 0) {
                    document.getElementById('assignmentsList').innerHTML = `
                    <div class="teacher-assignment-empty">
                        <p>No assignments yet</p>
                        <p>Click "New Assignment" to create one</p>
                    </div>
                `;
                    return;
                }

                document.getElementById('assignmentsList').innerHTML = assignments.map(assign => {
                    const dueDate = new Date(assign.dueDate);
                    const today = new Date();
                    const isOverdue = dueDate < today;

                    return `
                            <div class="teacher-assignment-card ${isOverdue ? 'teacher-assignment-card-overdue' : ''}">
                                <div class="teacher-assignment-card-header">
                                    <h4 class="teacher-assignment-card-title">${assign.title}</h4>
                                    <span class="teacher-assignment-card-points">${assign.totalPoints} pts</span>
                                </div>
                                <div class="teacher-assignment-card-content">
                                    <p>${assign.description}</p>
                                    <div class="teacher-assignment-card-meta">
                                        <span class="teacher-assignment-due-date">Due: ${new Date(assign.dueDate).toLocaleDateString()}</span>
                                        <span class="teacher-assignment-submission-count">Submissions: ${assign.submissionsCount} | Graded: ${assign.gradedCount}</span>
                                    </div>
                                </div>
                                <div class="teacher-assignment-card-actions">
                                    <button class="teacher-assignment-view-btn" onclick="window.viewSubmissions(${assign.id}, '${assign.title.replace(/'/g, "\\'")}', ${assign.totalPoints})">View Submissions</button>
                
                                    <button class="teacher-assignment-edit-btn" onclick="window.openEditAssignment(${assign.id}, '${assign.title.replace(/'/g, "\\'")}', '${assign.description.replace(/'/g, "\\'")}', '${assign.dueDate}', ${assign.totalPoints})">Edit</button>
                                    <button class="teacher-assignment-delete-btn" onclick="window.deleteAssignment(${assign.id})">Delete</button>
                                </div>
                            </div>
                        `;
                }).join('');
            }

            window.openNewAssignment = function () {
                if (!currentCourseInstanceId) {
                    alert('Please select a course first');
                    return;
                }

                isEditMode = false;
                currentEditingAssignmentId = null;

                document.getElementById('assignmentModalTitle').textContent = 'Create New Assignment';
                document.getElementById('modalClassName').textContent = currentCourseName;
                document.getElementById('assignmentTitle').value = '';
                document.getElementById('assignmentDescription').value = '';
                document.getElementById('assignmentDueDate').value = '';
                document.getElementById('assignmentPoints').value = '100';
                document.getElementById('assignmentModal').style.display = 'flex';
            };

            window.openEditAssignment = function (assignmentId, title, description, dueDate, totalPoints) {
                currentEditingAssignmentId = assignmentId;
                isEditMode = true;

                document.getElementById('assignmentModalTitle').textContent = 'Edit Assignment';
                document.getElementById('modalClassName').textContent = currentCourseName;
                document.getElementById('assignmentTitle').value = title;
                document.getElementById('assignmentDescription').value = description;

                const date = new Date(dueDate);
                const formattedDate = `${String(date.getMonth() + 1).padStart(2, '0')}/${String(date.getDate()).padStart(2, '0')}/${date.getFullYear()}`;
                document.getElementById('assignmentDueDate').value = formattedDate;

                document.getElementById('assignmentPoints').value = totalPoints;
                document.getElementById('assignmentModal').style.display = 'flex';
            };

            window.createAssignment = async function () {
                const title = document.getElementById('assignmentTitle').value.trim();
                const description = document.getElementById('assignmentDescription').value.trim();
                const dueDateInput = document.getElementById('assignmentDueDate').value;
                const points = parseInt(document.getElementById('assignmentPoints').value);

                if (!title || !description || !dueDateInput || !points) {
                    alert('Please fill in all fields');
                    return;
                }

                const dueDate = parseDate(dueDateInput);
                if (!dueDate) {
                    alert('Please enter a valid date in MM/DD/YYYY format');
                    return;
                }

                let response;
                if (isEditMode) {
                    response = await apiRequest(`/assignment/${currentEditingAssignmentId}`, 'PUT', {
                        title: title,
                        description: description,
                        dueDate: dueDate,
                        totalPoints: points
                    });
                } else {
                    response = await apiRequest('/assignment', 'POST', {
                        courseInstanceId: currentCourseInstanceId,
                        title: title,
                        description: description,
                        dueDate: dueDate,
                        totalPoints: points
                    });
                }

                if (!response.ok) {
                    alert(`Failed to ${isEditMode ? 'update' : 'create'} assignment: ` + (response.message || 'Unknown error'));
                    return;
                }

                const toast = document.getElementById('assignmentToast');
                toast.textContent = `✓ Assignment "${title}" ${isEditMode ? 'updated' : 'created'} successfully`;
                toast.className = 'teacher-assignment-toast teacher-assignment-toast-success teacher-assignment-toast-show';

                setTimeout(() => {
                    toast.classList.remove('teacher-assignment-toast-show');
                }, 4000);

                document.getElementById('assignmentModal').style.display = 'none';
                await loadAssignments(currentCourseInstanceId, currentCourseName);
            };

            window.viewSubmissions = async function (assignmentId, assignmentTitle, totalPoints) {
                currentAssignmentId = assignmentId;
                document.getElementById('submissionsTitle').textContent = assignmentTitle;
                document.getElementById('gradeMaxPoints').textContent = totalPoints;

                const response = await apiRequest(`/assignment/${assignmentId}/submissions`);
                if (!response.ok || !response.data) {
                    alert('Failed to load submissions');
                    return;
                }

                const submissions = response.data;
                const FILE_BASE_URL = 'http://localhost:5000/';

                const tableHeader = document.querySelector('.teacher-assignment-submissions-table thead tr');
                if (!tableHeader.innerHTML.includes('File')) {
                    tableHeader.innerHTML = `
                                            <th>Student Name</th>
                                            <th>Status</th>
                                            <th>Submitted</th>
                                            <th>File</th>
                                            <th>Grade</th>
                                            <th>Action</th>
                                        `;
                }

                const tableBody = document.getElementById('submissionsTable');
                if (submissions.length === 0) {
                    tableBody.innerHTML = `
            <tr>
                <td colspan="6" style="text-align: center; padding: 20px;">No submissions yet</td>
            </tr>
        `;
                } else {
                    tableBody.innerHTML = submissions.map(sub => {
                        let fileLink = '<span style="color: #9ca3af;">No file</span>';
                        if (sub.fileUrl) {
                            const cleanPath = sub.fileUrl.startsWith('/')
                                ? sub.fileUrl.substring(1)
                                : sub.fileUrl;
                            const fullUrl = `${FILE_BASE_URL}${cleanPath}`;

                            const fileName = sub.fileUrl.split(/[/\\]/).pop();

                            const extension = fileName.split('.').pop().toLowerCase();
                            let icon = '📄';
                            if (extension === 'pdf') icon = '📕';
                            else if (['doc', 'docx'].includes(extension)) icon = '📘';
                            else if (['jpg', 'jpeg', 'png', 'gif'].includes(extension)) icon = '🖼️';
                            else if (['zip', 'rar'].includes(extension)) icon = '📦';

                            fileLink = `<a href="${fullUrl}" target="_blank" style="color: #2563eb; text-decoration: none; display: inline-flex; align-items: center; gap: 4px;" title="${fileName}">
                    <span>${icon}</span>
                    <span style="text-decoration: underline;">View File</span>
                </a>`;
                        }

                        return `
            <tr class="teacher-assignment-submission-row">
                <td>${sub.studentName}</td>
                <td><span class="teacher-assignment-status-badge teacher-assignment-status-${sub.status.toLowerCase()}">${sub.status}</span></td>
                <td>${sub.submittedAt ? new Date(sub.submittedAt).toLocaleDateString() : '-'}</td>
                <td>${fileLink}</td>
                <td>${sub.grade !== null && sub.grade !== undefined ? sub.grade + ' pts' : '-'}</td>
                <td>
                    ${sub.submittedAt ? `
                        <button class="teacher-assignment-view-btn" onclick="window.openGradeModal(${sub.id}, '${sub.studentName.replace(/'/g, "\\'")}', ${sub.grade || 0})">
                            ${sub.grade !== null && sub.grade !== undefined ? 'Edit Grade' : 'Grade'}
                        </button>
                    ` : '-'}
                </td>
            </tr>
        `}).join('');
                }

                document.getElementById('submissionsModal').style.display = 'flex';
            };

            window.openGradeModal = function (submissionId, studentName, currentGrade) {
                currentSubmissionId = submissionId;
                document.getElementById('gradeStudentName').textContent = studentName;
                document.getElementById('gradePoints').value = currentGrade || '';
                document.getElementById('gradeFeedback').value = '';
                document.getElementById('gradeModal').style.display = 'flex';
            };

            window.submitGrade = async function () {
                const grade = parseInt(document.getElementById('gradePoints').value);
                const feedback = document.getElementById('gradeFeedback').value.trim();
                const maxPoints = parseInt(document.getElementById('gradeMaxPoints').textContent);

                if (isNaN(grade) || grade < 0) {
                    alert('Please enter a valid grade');
                    return;
                }

                if (grade > maxPoints) {
                    alert(`Grade cannot exceed ${maxPoints} points`);
                    return;
                }

                const response = await apiRequest(`/assignment/submissions/${currentSubmissionId}/grade`, 'POST', {
                    grade: grade,
                    feedback: feedback || null
                });

                if (!response.ok) {
                    alert('Failed to submit grade: ' + (response.message || 'Unknown error'));
                    return;
                }

                const toast = document.getElementById('assignmentToast');
                toast.textContent = '✓ Grade submitted successfully';
                toast.className = 'teacher-assignment-toast teacher-assignment-toast-success teacher-assignment-toast-show';

                setTimeout(() => {
                    toast.classList.remove('teacher-assignment-toast-show');
                }, 4000);

                document.getElementById('gradeModal').style.display = 'none';

                await window.viewSubmissions(currentAssignmentId, document.getElementById('submissionsTitle').textContent, parseInt(document.getElementById('gradeMaxPoints').textContent));
            };

            window.deleteAssignment = async function (assignmentId) {
                if (!confirm('Are you sure you want to delete this assignment? This action cannot be undone.')) {
                    return;
                }

                const response = await apiRequest(`/assignment/${assignmentId}`, 'DELETE');

                if (!response.ok) {
                    alert('Failed to delete assignment: ' + (response.message || 'Unknown error'));
                    return;
                }

                const toast = document.getElementById('assignmentToast');
                toast.textContent = '✓ Assignment deleted successfully';
                toast.className = 'teacher-assignment-toast teacher-assignment-toast-success teacher-assignment-toast-show';

                setTimeout(() => {
                    toast.classList.remove('teacher-assignment-toast-show');
                }, 4000);

                await loadAssignments(currentCourseInstanceId, currentCourseName);
            };

            const dateInput = document.getElementById('assignmentDueDate');
            dateInput.addEventListener('input', function () {
                formatDateInput(this);
            });

            document.getElementById('classSelect').addEventListener('change', async function () {
                const courseInstanceId = parseInt(this.value);
                const selectedCourse = courses.find(c => c.courseInstanceId === courseInstanceId);

                if (selectedCourse) {
                    const courseName = `${selectedCourse.courseCode} - ${selectedCourse.courseName}`;
                    await loadAssignments(courseInstanceId, courseName);
                }
            });

            document.getElementById('assignmentModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            document.getElementById('submissionsModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            document.getElementById('gradeModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            await loadCourses();
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
                <option value="">Loading courses...</option>
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
        afterRender: async () => {
            const classSelect = document.getElementById('classSelect');
            const classInfo = document.getElementById('classInfo');
            const announcementsList = document.getElementById('announcementsList');

            let courses = [];
            let allAnnouncements = [];
            let currentCourseInstanceId = null;
            let editingAnnouncementId = null;

            // Load courses
            async function loadCourses() {
                const response = await apiRequest('/courseInstance/my-courses?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data || response.data.length === 0) {
                    classSelect.innerHTML = '<option value="">No courses available</option>';
                    classInfo.textContent = 'No courses assigned';
                    announcementsList.innerHTML = '<div class="teacher-announcements-empty"><p>No courses available</p></div>';
                    return;
                }

                courses = response.data;
                currentCourseInstanceId = courses[0].courseInstanceId;

                classSelect.innerHTML = courses.map(course => `
                <option value="${course.courseInstanceId}">
                    ${course.courseCode} - ${course.courseName} (Section ${course.section || 'A'})
                </option>
            `).join('');

                classSelect.value = currentCourseInstanceId;
                loadAnnouncements();
            }

            // Load announcements
            async function loadAnnouncements() {
                announcementsList.innerHTML = '<div style="text-align: center; padding: 20px;">Loading announcements...</div>';

                const response = await apiRequest('/announcement/my-announcements-teacher?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data) {
                    allAnnouncements = [];
                } else {
                    allAnnouncements = response.data.sort((a, b) =>
                        new Date(b.publishedAt) - new Date(a.publishedAt)
                    );
                }

                displayAnnouncements();
            }

            function displayAnnouncements() {
                const filtered = allAnnouncements.filter(a => a.targetCourseInstanceId === currentCourseInstanceId);
                const currentCourse = courses.find(c => c.courseInstanceId === currentCourseInstanceId);

                classInfo.textContent = currentCourse ? `${currentCourse.courseCode} - ${currentCourse.courseName}` : 'Select a class';

                if (filtered.length === 0) {
                    announcementsList.innerHTML = `
                    <div class="teacher-announcements-empty">
                        <p>No announcements yet</p>
                        <p>Click "New Announcement" to send one</p>
                    </div>
                `;
                    return;
                }

                announcementsList.innerHTML = filtered.map(ann => {
                    const isUrgent = ann.title && ann.title.toLowerCase().includes('urgent') ||
                        ann.content && ann.content.toLowerCase().includes('urgent');

                    return `
                    <div class="teacher-announcement-card ${isUrgent ? 'teacher-announcement-card-urgent' : ''}">
                        <div class="teacher-announcement-card-header">
                            <div>
                                <h4 class="teacher-announcement-card-title">${ann.title}</h4>
                                ${isUrgent ? '<span class="teacher-announcement-urgent-badge">URGENT</span>' : ''}
                            </div>
                            <span class="teacher-announcement-card-date">${new Date(ann.publishedAt).toLocaleDateString()}</span>
                        </div>
                        <div class="teacher-announcement-card-content">
                            <p>${ann.content.substring(0, 150)}${ann.content.length > 150 ? '...' : ''}</p>
                        </div>
                        <div class="teacher-announcement-card-actions">
                            <button class="teacher-announcement-edit-btn" onclick="window.editAnnouncement(${ann.id})">Edit</button>
                            <button class="teacher-announcement-delete-btn" onclick="window.deleteAnnouncement(${ann.id})">Delete</button>
                        </div>
                    </div>
                `;
                }).join('');
            }

            window.openNewAnnouncement = function () {
                editingAnnouncementId = null;
                const currentCourse = courses.find(c => c.courseInstanceId === currentCourseInstanceId);
                document.getElementById('modalClassName').textContent = currentCourse ?
                    `${currentCourse.courseCode} - ${currentCourse.courseName}` : 'Select a course';
                document.getElementById('announcementTitle').value = '';
                document.getElementById('announcementMessage').value = '';
                document.getElementById('announcementUrgent').checked = false;
                document.querySelector('.teacher-announcement-btn-send').textContent = 'Send Announcement';
                document.getElementById('announcementModal').style.display = 'flex';
            };

            window.editAnnouncement = async function (announcementId) {
                const ann = allAnnouncements.find(a => a.id === announcementId);
                if (!ann) {
                    alert('Could not load announcement');
                    return;
                }

                editingAnnouncementId = announcementId;
                const currentCourse = courses.find(c => c.courseInstanceId === currentCourseInstanceId);
                document.getElementById('modalClassName').textContent = currentCourse ?
                    `${currentCourse.courseCode} - ${currentCourse.courseName}` : 'Select a course';
                document.getElementById('announcementTitle').value = ann.title;
                document.getElementById('announcementMessage').value = ann.content;
                document.getElementById('announcementUrgent').checked = ann.title && ann.title.toLowerCase().includes('urgent');
                document.querySelector('.teacher-announcement-btn-send').textContent = 'Update Announcement';
                document.getElementById('announcementModal').style.display = 'flex';
            };

            window.sendAnnouncement = async function () {
                const title = document.getElementById('announcementTitle').value.trim();
                const content = document.getElementById('announcementMessage').value.trim();
                const isUrgent = document.getElementById('announcementUrgent').checked;
                const sendEmail = document.getElementById('announcementEmail').checked;

                if (!title || !content) {
                    alert('Please fill in both title and message');
                    return;
                }

                const request = {
                    title: title,
                    content: content,
                    targetCourseInstanceId: currentCourseInstanceId
                };

                let response;
                if (editingAnnouncementId) {
                    response = await apiRequest(`/announcement/${editingAnnouncementId}`, 'PUT', request);
                } else {
                    response = await apiRequest(`/announcement`, 'POST', request);
                    }
                

                if (response.ok) {
                    document.getElementById('announcementModal').style.display = 'none';
                    await loadAnnouncements();

                    const toast = document.getElementById('toastNotification') || document.createElement('div');
                    if (!document.getElementById('toastNotification')) {
                        toast.id = 'toastNotification';
                        document.body.appendChild(toast);
                    }

                    const currentCourse = courses.find(c => c.courseInstanceId === currentCourseInstanceId);
                    const courseName = currentCourse ? currentCourse.courseCode : 'Course';
                    const message = editingAnnouncementId ? 'updated' : 'sent';

                    toast.textContent = `✓ Announcement ${message} & ${sendEmail ? 'email delivered' : 'posted'} to ${courseName}`;
                    toast.className = 'teacher-announcement-toast teacher-announcement-toast-success teacher-announcement-toast-show';

                    setTimeout(() => {
                        toast.classList.remove('teacher-announcement-toast-show');
                    }, 4000);
                } else {
                    alert('Error: ' + (response.data?.message || 'Failed to save announcement'));
                }
            };

            window.deleteAnnouncement = async function (announcementId) {
                if (!confirm('Are you sure you want to delete this announcement?')) {
                    return;
                }

                const response = await apiRequest(`/announcement/${announcementId}`, 'DELETE');

                if (response.ok) {
                    await loadAnnouncements();

                    const toast = document.getElementById('toastNotification') || document.createElement('div');
                    if (!document.getElementById('toastNotification')) {
                        toast.id = 'toastNotification';
                        document.body.appendChild(toast);
                    }

                    toast.textContent = '✓ Announcement deleted';
                    toast.className = 'teacher-announcement-toast teacher-announcement-toast-success teacher-announcement-toast-show';

                    setTimeout(() => {
                        toast.classList.remove('teacher-announcement-toast-show');
                    }, 4000);
                } else {
                    alert('Error: ' + (response.data?.message || 'Failed to delete announcement'));
                }
            };

            classSelect.addEventListener('change', function () {
                currentCourseInstanceId = parseInt(this.value);
                displayAnnouncements();
            });

            document.getElementById('announcementModal').addEventListener('click', function (e) {
                if (e.target === this) this.style.display = 'none';
            });

            await loadCourses();
        }
    },

    profile: {
        render: () => `
    <div class="teacher-breadcrumb">Home / My Profile</div>
    <div class="teacher-section-header">Personal Profile</div>
    
    <div class="teacher-profile-container">
        <div class="teacher-profile-header">
            <div class="teacher-profile-pic-wrapper">
                <img id="profilePic" src="https://via.placeholder.com/120?text=Avatar" class="teacher-profile-pic">
            </div>
            <div class="teacher-profile-info">
                <h2 id="profileName">Loading...</h2>
                <p id="profileEmail" class="teacher-profile-email">Email: -</p>
                <p id="profileDepartment" class="teacher-profile-department">Department: -</p>
            </div>
        </div>
        
        <div class="teacher-profile-tabs">
            <button class="teacher-profile-tab-btn" data-tab="personal">Personal Info</button>
            <button class="teacher-profile-tab-btn" data-tab="contact">Contact</button>
            <button class="teacher-profile-tab-btn" data-tab="security">Security</button>
        </div>
        
        <div id="personalTab" class="teacher-profile-tab-content">
            <div class="teacher-profile-form">
                <div class="teacher-profile-form-group">
                    <label>First Name</label>
                    <input type="text" id="firstName" class="teacher-profile-input">
                </div>
                <div class="teacher-profile-form-group">
                    <label>Last Name</label>
                    <input type="text" id="lastName" class="teacher-profile-input">
                </div>
                <div class="teacher-profile-form-group">
                    <label>Email</label>
                    <input type="email" id="profileEmailInput" class="teacher-profile-input" readonly>
                </div>
                <div class="teacher-profile-form-group">
                    <label>Phone Number</label>
                    <input type="tel" id="phoneNumber" class="teacher-profile-input" placeholder="+90 (555) 000-0000">
                </div>
                <div class="teacher-profile-form-group">
                    <label>Date of Birth</label>
                    <input type="date" id="dateOfBirth" class="teacher-profile-input">
                </div>
                <div class="teacher-profile-form-group">
                    <label>Address</label>
                    <input type="text" id="address" class="teacher-profile-input" placeholder="Street address">
                </div>
                <div class="teacher-profile-form-group">
                    <label>City</label>
                    <input type="text" id="city" class="teacher-profile-input">
                </div>
                <div class="teacher-profile-form-group">
                    <label>Department</label>
                    <input type="text" id="department" class="teacher-profile-input" readonly>
                </div>
                <div class="teacher-profile-form-group">
                    <label>Office Location</label>
                    <input type="text" id="officeLocation" class="teacher-profile-input" placeholder="Building and room number">
                </div>
                <button class="teacher-profile-btn-save" id="savePersonalBtn">Save Changes</button>
            </div>
        </div>
        
        <div id="contactTab" class="teacher-profile-tab-content">
            <div class="teacher-profile-form">
                <h4>Emergency Contact</h4>
                <div class="teacher-profile-form-group">
                    <label>Emergency Contact Name</label>
                    <input type="text" id="emergencyName" class="teacher-profile-input" placeholder="Full name">
                </div>
                <div class="teacher-profile-form-group">
                    <label>Emergency Contact Phone</label>
                    <input type="tel" id="emergencyPhone" class="teacher-profile-input" placeholder="+90 (555) 000-0000">
                </div>
                <div class="teacher-profile-form-group">
                    <label>Relationship</label>
                    <select id="emergencyRelationship" class="teacher-profile-input">
                        <option value="">Select relationship</option>
                        <option value="Spouse">Spouse</option>
                        <option value="Parent">Parent</option>
                        <option value="Sibling">Sibling</option>
                        <option value="Child">Child</option>
                        <option value="Friend">Friend</option>
                        <option value="Other">Other</option>
                    </select>
                </div>
                <button class="teacher-profile-btn-save" id="saveContactBtn">Save Changes</button>
            </div>
        </div>
        
        <div id="securityTab" class="teacher-profile-tab-content">
            <div class="teacher-profile-form">
                <h4>Change Password</h4>
                <div class="teacher-profile-form-group">
                    <label>Current Password</label>
                    <input type="password" id="currentPassword" class="teacher-profile-input" placeholder="Enter current password">
                </div>
                <div class="teacher-profile-form-group">
                    <label>New Password</label>
                    <input type="password" id="newPassword" class="teacher-profile-input" placeholder="Enter new password (min. 8 characters)">
                </div>
                <div class="teacher-profile-form-group">
                    <label>Confirm New Password</label>
                    <input type="password" id="confirmPassword" class="teacher-profile-input" placeholder="Confirm new password">
                </div>
                <button class="teacher-profile-btn-save" id="changePasswordBtn">Change Password</button>
            </div>
        </div>
    </div>
`,
        afterRender: async () => {
            function showProfileTab(tabName) {
                document.querySelectorAll('.teacher-profile-tab-content').forEach(el => {
                    el.style.display = 'none';
                });

                document.querySelectorAll('.teacher-profile-tab-btn').forEach(btn => {
                    btn.classList.remove('teacher-profile-tab-active');
                });

                const selectedTab = document.getElementById(tabName + 'Tab');
                if (selectedTab) {
                    selectedTab.style.display = 'block';
                }

                const activeBtn = document.querySelector(`[data-tab="${tabName}"]`);
                if (activeBtn) {
                    activeBtn.classList.add('teacher-profile-tab-active');
                }
            }

            showProfileTab('personal');

            document.querySelectorAll('.teacher-profile-tab-btn').forEach(btn => {
                btn.addEventListener('click', function () {
                    const tab = this.getAttribute('data-tab');
                    showProfileTab(tab);
                });
            });

            const response = await apiRequest('/user/profile/teacher');

            if (!response.ok || !response.data) {
                console.error('Failed to load profile');
                document.getElementById('profileName').textContent = 'Error loading profile';
                return;
            }

            const profile = response.data;

            document.getElementById('profileName').textContent = `${profile.firstName} ${profile.lastName}`;
            document.getElementById('profileEmail').textContent = `Email: ${profile.email}`;
            document.getElementById('profileDepartment').textContent = `Department: ${profile.departmentName || 'N/A'}`;

            document.getElementById('firstName').value = profile.firstName || '';
            document.getElementById('lastName').value = profile.lastName || '';
            document.getElementById('profileEmailInput').value = profile.email || '';
            document.getElementById('phoneNumber').value = profile.phoneNumber || '';
            document.getElementById('dateOfBirth').value = profile.dateOfBirth ? profile.dateOfBirth.split('T')[0] : '';
            document.getElementById('address').value = profile.address || '';
            document.getElementById('city').value = profile.city || '';
            document.getElementById('department').value = profile.departmentName || '';
            document.getElementById('officeLocation').value = profile.officeLocation || '';

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
                    city: document.getElementById('city').value,
                    officeLocation: document.getElementById('officeLocation').value
                };

                const saveResponse = await apiRequest('/user/profile/teacher', 'PUT', data);
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
    <div class="teacher-breadcrumb">Home / Help & Support</div>
    <div class="teacher-section-header">Support & Resources</div>
    
    <div class="teacher-help-container">
        <div id="helpContacts">
            <div style="text-align: center; padding: 40px;">Loading contacts...</div>
        </div>
    </div>
`,
        afterRender: async () => {
            const response = await apiRequest('/user/help-contacts/teacher');

            if (!response.ok || !response.data) {
                document.getElementById('helpContacts').innerHTML =
                    '<div style="text-align: center; padding: 40px; color: red;">Failed to load contact information</div>';
                return;
            }

            const data = response.data;

            const html = `
        <div class="help-section">
            <!-- Department Information -->
            <div class="help-category">
                <h3 class="help-category-title">Your Department</h3>
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
                ${data.departmentHeadName ? `
                    <div class="help-contact-item">
                        <label>Department Head:</label>
                        <span>${data.departmentHeadName}</span>
                    </div>
                ` : ''}
                ${data.departmentHeadEmail ? `
                    <div class="help-contact-item">
                        <label>Head's Email:</label>
                        <a href="mailto:${data.departmentHeadEmail}">${data.departmentHeadEmail}</a>
                    </div>
                ` : ''}
            </div>

            <!-- Administrative Services -->
            <div class="help-category">
                <h3 class="help-category-title">Administrative Services</h3>
                ${data.registrarEmail ? `
                    <div class="help-contact-item">
                        <label>Registrar Office:</label>
                        <a href="mailto:${data.registrarEmail}">${data.registrarEmail}</a>
                    </div>
                ` : ''}
                ${data.academicAffairsEmail ? `
                    <div class="help-contact-item">
                        <label>Academic Affairs:</label>
                        <a href="mailto:${data.academicAffairsEmail}">${data.academicAffairsEmail}</a>
                    </div>
                ` : ''}
                ${data.hrEmail ? `
                    <div class="help-contact-item">
                        <label>Human Resources:</label>
                        <a href="mailto:${data.hrEmail}">${data.hrEmail}</a>
                    </div>
                ` : ''}
                ${data.itSupportEmail ? `
                    <div class="help-contact-item">
                        <label>IT Support:</label>
                        <a href="mailto:${data.itSupportEmail}">${data.itSupportEmail}</a>
                    </div>
                ` : ''}
            </div>

            <!-- Student Support Resources -->
            <div class="help-category">
                <h3 class="help-category-title">Student Support Resources</h3>
                <p class="help-category-description">For referring students who need assistance</p>
                ${data.studentAffairsEmail ? `
                    <div class="help-contact-item">
                        <label>Student Affairs:</label>
                        <a href="mailto:${data.studentAffairsEmail}">${data.studentAffairsEmail}</a>
                    </div>
                ` : ''}
                ${data.counselingEmail ? `
                    <div class="help-contact-item">
                        <label>Counseling Services:</label>
                        <a href="mailto:${data.counselingEmail}">${data.counselingEmail}</a>
                    </div>
                ` : ''}
                ${data.disabilityServicesEmail ? `
                    <div class="help-contact-item">
                        <label>Disability Services:</label>
                        <a href="mailto:${data.disabilityServicesEmail}">${data.disabilityServicesEmail}</a>
                    </div>
                ` : ''}
            </div>    
        </div>
    `;

            document.getElementById('helpContacts').innerHTML = html;
        }
    },

    schedule: {
        render: () => `
        <div class="teacher-breadcrumb">Home / Schedule</div>
        <div class="teacher-section-header">My Teaching Schedule</div>
        <div class="schedule-container" id="scheduleContainer">
            <div style="text-align: center; padding: 40px; color: #999;">Loading schedule...</div>
        </div>
    `,
        afterRender: async () => {
            const scheduleContainer = document.getElementById('scheduleContainer');
            const daysOfWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

            try {
                const response = await apiRequest('/courseInstance/my-courses?pageIndex=1&pageSize=100');

                if (!response.ok || !response.data || response.data.length === 0) {
                    scheduleContainer.innerHTML = '<div style="text-align: center; padding: 40px; color: #999;">No courses assigned</div>';
                    return;
                }

                const courses = response.data;

                const scheduleByDay = {};
                daysOfWeek.forEach(day => {
                    scheduleByDay[day] = [];
                });

                courses.forEach(course => {
                    const courseData = {
                        code: course.courseCode,
                        name: course.courseName,
                        section: course.section,
                        location: course.location || 'TBA',
                        enrollment: course.currentEnrollmentCount || 0,
                        capacity: course.capacity || 0,
                        startTime: course.startTime,
                        endTime: course.endTime
                    };

                    if (course.day1) {
                        const dayName = daysOfWeek[course.day1];
                        scheduleByDay[dayName].push({
                            ...courseData,
                            start: course.startTime,
                            end: course.endTime
                        });
                    }

                    if (course.day2) {
                        const dayName = daysOfWeek[course.day2];
                        scheduleByDay[dayName].push({
                            ...courseData,
                            start: course.startTime,
                            end: course.endTime
                        });
                    }
                });

                Object.keys(scheduleByDay).forEach(day => {
                    scheduleByDay[day].sort((a, b) => {
                        if (!a.start || !b.start) return 0;
                        return a.start.localeCompare(b.start);
                    });
                });

                const formatTime = (timeString) => {
                    if (!timeString) return 'TBA';
                    // timeString is HH:mm format
                    return timeString.substring(0, 5);
                };

                let html = '';
                daysOfWeek.forEach(day => {
                    const dayCourses = scheduleByDay[day];
                    html += `
                    <div class="schedule-day-card teacher-schedule-card ${dayCourses.length === 0 ? 'empty' : ''}">
                        <div class="schedule-day-header teacher-day-header">${day}</div>
                        ${dayCourses.length > 0 ? dayCourses.map(course => `
                            <div class="schedule-class-item">
                                <div class="schedule-time">${formatTime(course.start)} - ${formatTime(course.end)}</div>
                                <div class="schedule-course">${course.name}</div>
                                <div class="schedule-meta">${course.enrollment} Students | ${course.location} | ${course.code}-${course.section}</div>
                            </div>
                        `).join('') : '<div class="schedule-no-classes">No Classes</div>'}
                    </div>
                `;
                });

                scheduleContainer.innerHTML = html;
            } catch (error) {
                console.error('Failed to load schedule:', error);
                scheduleContainer.innerHTML = '<div style="text-align: center; padding: 40px; color: red;">Failed to load schedule</div>';
            }
        }
    }
};