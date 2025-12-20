import http from 'k6/http';
import { check, sleep } from 'k6';
import { Counter, Trend } from 'k6/metrics';
import exec from 'k6/execution';

const BASE_URL = __ENV.BASE_URL || 'http://localhost:5000/api';
const TOTAL_STUDENTS = parseInt(__ENV.STUDENTS || '3000');

// Course instance IDs - update these after seeding
const COURSE_IDS = (__ENV.COURSE_IDS || '12,13,14,15,16,17,18,19,20,21').split(',').map(Number);

const enrollmentSuccess = new Counter('enrollment_success');
const enrollmentFailed = new Counter('enrollment_failed');
const enrollmentCourseFull = new Counter('enrollment_course_full');
const loginDuration = new Trend('login_duration');
const enrollDuration = new Trend('enroll_duration');

export const options = {
    scenarios: {
        enrollment_rush: {
            executor: 'shared-iterations',
            vus: 200,           
            iterations: 3000,
            maxDuration: '15m',
        },
    },
};

export default function () {
    const studentNum = exec.scenario.iterationInTest + 1;
    const email = `loadtest.student${studentNum}@university.edu`;
    const password = 'Student123';

    // Pick random course
    const courseId = COURSE_IDS[Math.floor(Math.random() * COURSE_IDS.length)];

    // Login
    const loginStart = Date.now();
    const loginRes = http.post(
        `${BASE_URL}/authentication/login`,
        JSON.stringify({ email, password }),
        { headers: { 'Content-Type': 'application/json' } }
    );
    loginDuration.add(Date.now() - loginStart);

    // Check if response exists
    if (!loginRes || !loginRes.body) {
        console.log(`Connection failed for ${email}`);
        enrollmentFailed.add(1);
        return;
    }

    let loginData;
    try {
        loginData = loginRes.json();
    } catch (e) {
        console.log(`Invalid JSON for ${email}: ${loginRes.status}`);
        enrollmentFailed.add(1);
        return;
    }

    const loginOk = check(loginRes, {
        'login status is 200': (r) => r.status === 200,
        'login has token': () => loginData?.data?.accessToken !== undefined,
    });

    if (!loginOk || !loginData?.data?.accessToken) {
        enrollmentFailed.add(1);
        return;
    }

    const token = loginData.data.accessToken;

    // Enroll
    const enrollStart = Date.now();
    const enrollRes = http.post(
        `${BASE_URL}/enrollment`,
        JSON.stringify({ courseInstanceId: courseId }),
        {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`,
            },
        }
    );
    enrollDuration.add(Date.now() - enrollStart);

    if (!enrollRes || !enrollRes.body) {
        enrollmentFailed.add(1);
        return;
    }

    if (enrollRes.status === 200 || enrollRes.status === 201) {
        enrollmentSuccess.add(1);
        check(enrollRes, { 'enrollment successful': () => true });
    } else if (enrollRes.status === 400 && enrollRes.body.includes('full')) {
        enrollmentCourseFull.add(1);
        check(enrollRes, { 'course full (expected)': () => true });
    } else if (enrollRes.status === 400 && enrollRes.body.includes('Already enrolled')) {
        // This is also expected - student already enrolled in this course
        enrollmentCourseFull.add(1);  // Count as expected outcome
        check(enrollRes, { 'already enrolled (expected)': () => true });
    } else {
        enrollmentFailed.add(1);
        console.log(`ERROR: ${email} → ${enrollRes.status}: ${enrollRes.body?.substring(0, 150)}`);
    }
}

export function handleSummary(data) {
    const success = data.metrics.enrollment_success?.values?.count || 0;
    const full = data.metrics.enrollment_course_full?.values?.count || 0;
    const failed = data.metrics.enrollment_failed?.values?.count || 0;
    const total = success + full + failed;

    const summary = `
================================================================================
                         ENROLLMENT LOAD TEST RESULTS
================================================================================

TEST CONFIGURATION
------------------
Virtual Users (VUs):     ${data.metrics.vus?.values?.max || 'N/A'}
Total Iterations:        ${total}
Duration:                ${(data.state.testRunDurationMs / 1000).toFixed(1)}s

ENROLLMENT OUTCOMES
-------------------
✓ Successful:            ${success} (${((success / total) * 100).toFixed(1)}%)
✓ Course Full:           ${full} (${((full / total) * 100).toFixed(1)}%)
✗ Failed (Server Busy):  ${failed} (${((failed / total) * 100).toFixed(1)}%)

PERFORMANCE METRICS
-------------------
HTTP Request Duration:
  Average:               ${(data.metrics.http_req_duration?.values?.avg || 0).toFixed(0)}ms
  Median:                ${(data.metrics.http_req_duration?.values?.med || 0).toFixed(0)}ms
  P90:                   ${(data.metrics.http_req_duration?.values['p(90)'] || 0).toFixed(0)}ms
  P95:                   ${(data.metrics.http_req_duration?.values['p(95)'] || 0).toFixed(0)}ms
  Min:                   ${(data.metrics.http_req_duration?.values?.min || 0).toFixed(0)}ms
  Max:                   ${(data.metrics.http_req_duration?.values?.max || 0).toFixed(0)}ms

Login Duration:
  Average:               ${(data.metrics.login_duration?.values?.avg || 0).toFixed(0)}ms
  P95:                   ${(data.metrics.login_duration?.values['p(95)'] || 0).toFixed(0)}ms

Enrollment Duration:
  Average:               ${(data.metrics.enroll_duration?.values?.avg || 0).toFixed(0)}ms
  P95:                   ${(data.metrics.enroll_duration?.values['p(95)'] || 0).toFixed(0)}ms

THROUGHPUT
----------
Requests/sec:            ${(data.metrics.http_reqs?.values?.rate || 0).toFixed(2)}
Iterations/sec:          ${(data.metrics.iterations?.values?.rate || 0).toFixed(2)}

================================================================================
`;

    console.log(summary);

    return {
        'stdout': summary,
        'load-test-results.txt': summary,
    };
}