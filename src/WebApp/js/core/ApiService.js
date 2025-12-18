const API_BASE_URL = 'http://localhost:5000/api';

let isRefreshing = false;
let refreshPromise = null;

export async function apiRequest(endpoint, method = 'GET', body = null) {
    const token = localStorage.getItem('jwtToken');

    const headers = {};

    if (body && !(body instanceof FormData)) {
        headers['Content-Type'] = 'application/json';
    }

    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    const config = {
        method,
        headers,
    };

    if (body) {
        config.body = (body instanceof FormData) ? body : JSON.stringify(body);
    }

    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, config);

        if (response.status === 401) {
            const refreshToken = localStorage.getItem('refreshToken');
            const accessToken = localStorage.getItem('jwtToken');

            if (refreshToken && accessToken) {
                const refreshResult = await attemptTokenRefresh(accessToken, refreshToken);

                if (refreshResult.success) {
                    //  the original request with new token
                    return apiRequest(endpoint, method, body);
                }
            }

            clearAuthAndRedirect();
            return { ok: false, status: 401, data: null, message: 'Session expired' };
        }

        let payload;
        const contentType = response.headers.get("content-type");

        if (contentType && contentType.includes("application/json")) {
            payload = await response.json();
        } else {
            payload = await response.text();
        }

        // Handle ResultService format
        if (payload && typeof payload === 'object' && 'success' in payload && 'statusCode' in payload) {
            if (!payload.success) {
                return {
                    ok: false,
                    status: payload.statusCode,
                    message: payload.message,
                    data: null
                };
            }
            return {
                ok: true,
                status: payload.statusCode,
                message: payload.message,
                data: payload.data,
                pageIndex: payload.pageIndex,
                pageSize: payload.pageSize,
                totalCount: payload.totalCount,
                totalPages: payload.totalPages,
                hasPreviousPage: payload.hasPreviousPage,
                hasNextPage: payload.hasNextPage
            };
        }

        // Raw response
        return {
            ok: response.ok,
            status: response.status,
            data: payload
        };

    } catch (error) {
        console.error("API Error:", error);
        return { ok: false, error: error.message };
    }
}

async function attemptTokenRefresh(accessToken, refreshToken) {

    if (isRefreshing) {
        return refreshPromise;
    }

    isRefreshing = true;

    refreshPromise = (async () => {
        try {
            console.log('Attempting token refresh...');

            const response = await fetch(`${API_BASE_URL}/authentication/refresh-token`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ accessToken, refreshToken })
            });

            if (!response.ok) {
                console.log('Token refresh failed - server returned:', response.status);
                return { success: false };
            }

            const data = await response.json();

            let tokens;
            if (data.success && data.data) {
                tokens = data.data;
            } else if (data.accessToken) {
                tokens = data;
            } else {
                console.log('Token refresh failed - invalid response format');
                return { success: false };
            }

            if (tokens.accessToken) {
                localStorage.setItem('jwtToken', tokens.accessToken);
            }
            if (tokens.refreshToken) {
                localStorage.setItem('refreshToken', tokens.refreshToken);
            }

            console.log('Token refresh successful');
            return {
                success: true,
                accessToken: tokens.accessToken,
                refreshToken: tokens.refreshToken
            };
        } catch (error) {
            console.error('Token refresh failed:', error);
            return { success: false };
        } finally {
            isRefreshing = false;
            refreshPromise = null;
        }
    })();

    return refreshPromise;
}

function clearAuthAndRedirect() {
    localStorage.removeItem('jwtToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('email');
    localStorage.removeItem('userRole');

    if (!window.location.pathname.includes('login')) {
        window.location.href = '/login.html';
    }
}