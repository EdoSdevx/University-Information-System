import http from 'k6/http';
import { check, sleep } from 'k6';
import { Counter, Trend } from 'k6/metrics';
import exec from 'k6/execution';

// -----------------------------------------------------------------------------
// 1. CONFIGURATION
// -----------------------------------------------------------------------------
const BASE_URL = __ENV.BASE_URL || 'http://localhost:5000/api';

//60 course test
const defaultCourses = '12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61';
const COURSE_IDS = (__ENV.COURSE_IDS || defaultCourses).split(',').map(Number);

// -----------------------------------------------------------------------------
// 2. METRICS
// -----------------------------------------------------------------------------
const enrollmentSuccess = new Counter('enrollment_success');
const enrollmentCourseFull = new Counter('enrollment_course_full');
const enrollmentFailed = new Counter('enrollment_failed');
const activeUsers = new Counter('active_users_browsing');

const loginDuration = new Trend('login_duration');
const enrollDuration = new Trend('enroll_duration');

export const options = {
    scenarios: {
        registration_day: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                { duration: '30s', target: 3000 },
                { duration: '2m', target: 3000 },
                { duration: '10s', target: 0 },
            ],
            gracefulRampDown: '30s',
        },
    },
    noConnectionReuse: false,
    userAgent: 'K6-SmartUser',
};

// -----------------------------------------------------------------------------
// 3. VU STATE
// -----------------------------------------------------------------------------
let vuToken = null;
let isEnrolled = false;

export default function () {
    // 0. EXIT IF HAPPY
    if (isEnrolled) {
        sleep(10);
        return;
    }

    // 1. LOGIN (Once)
    if (!vuToken) {
        const studentId = exec.vu.idInTest;

        const loginStart = Date.now();
        const res = http.post(
            `${BASE_URL}/authentication/login-load-test`,
            JSON.stringify(studentId),
            { headers: { 'Content-Type': 'application/json' } }
        );
        loginDuration.add(Date.now() - loginStart);

        if (res.status === 200) {
            vuToken = res.json().data.accessToken;
        } else {
            sleep(2);
            return;
        }
    }

    // 2. TRY TO ENROLL
    activeUsers.add(1);

    const courseId = COURSE_IDS[Math.floor(Math.random() * COURSE_IDS.length)];

    const enrollStart = Date.now();
    const res = http.post(
        `${BASE_URL}/enrollment`,
        JSON.stringify({ courseInstanceId: courseId }),
        {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${vuToken}`,
            },
            timeout: '10s'
        }
    );
    enrollDuration.add(Date.now() - enrollStart);

  
    if (res.status === 200 || res.status === 201) {
        enrollmentSuccess.add(1);
        isEnrolled = true;
    }
    else if (res.status === 400 || res.status === 409) {
        enrollmentCourseFull.add(1);
        sleep(Math.random() * 5 + 5);
    }
    else {
        enrollmentFailed.add(1);
        sleep(10);
    }
}


// 4. DETAILED SUMMARY

export function handleSummary(data) {
    const success = data.metrics.enrollment_success?.values?.count || 0;
    const full = data.metrics.enrollment_course_full?.values?.count || 0;
    const failed = data.metrics.enrollment_failed?.values?.count || 0;
    const total = success + full + failed;


    const getMetric = (name, stat) => {
        const metric = data.metrics[name];
        return metric ? metric.values[stat].toFixed(0) : '0';
    };

    const getRate = (name) => {
        const metric = data.metrics[name];
        return metric ? metric.values.rate.toFixed(2) : '0.00';
    };

    const summary = `
================================================================================
                     ENROLLMENT LOAD TEST RESULTS (FINAL)
================================================================================

TEST CONFIGURATION
------------------
Virtual Users (VUs):     ${data.metrics.vus?.values?.max || 'N/A'}
Total Requests:          ${total}
Duration:                ${(data.state.testRunDurationMs / 1000).toFixed(1)}s

ENROLLMENT OUTCOMES
-------------------
✓ Successful:            ${success} (${total > 0 ? ((success / total) * 100).toFixed(1) : 0}%)
✓ Course Full:           ${full} (${total > 0 ? ((full / total) * 100).toFixed(1) : 0}%)
✗ Failed (Error/Timeout): ${failed} (${total > 0 ? ((failed / total) * 100).toFixed(1) : 0}%)

PERFORMANCE METRICS
-------------------
HTTP Request Duration (Total):
  Average:               ${getMetric('http_req_duration', 'avg')}ms
  Median:                ${getMetric('http_req_duration', 'med')}ms
  P90:                   ${getMetric('http_req_duration', 'p(90)')}ms
  P95:                   ${getMetric('http_req_duration', 'p(95)')}ms
  Min:                   ${getMetric('http_req_duration', 'min')}ms
  Max:                   ${getMetric('http_req_duration', 'max')}ms

Login Duration:
  Average:               ${getMetric('login_duration', 'avg')}ms
  P95:                   ${getMetric('login_duration', 'p(95)')}ms

Enrollment Duration:
  Average:               ${getMetric('enroll_duration', 'avg')}ms
  P95:                   ${getMetric('enroll_duration', 'p(95)')}ms

THROUGHPUT
----------
Requests/sec:            ${getRate('http_reqs')}
Enrollments/sec:         ${(total / (data.state.testRunDurationMs / 1000)).toFixed(2)}

================================================================================
`;

    return {
        'stdout': summary,
        'load-test-login-bypass-txt': summary,
    };
}