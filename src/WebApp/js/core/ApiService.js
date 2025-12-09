const API_BASE_URL = 'http://localhost:5000/api';


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

            const accessToken = localStorage.getItem('jwtToken');
            const refreshToken = localStorage.getItem('refreshToken');

            if (refreshToken) {
                const refreshResult = await attemptTokenRefresh(accessToken, refreshToken);
                if (refreshResult.success) {
                    return apiRequest(endpoint, method, body);
                }
            }

            localStorage.removeItem('jwtToken');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem('email');
            return { ok: false, status: 401, data: null };
        }

        let payload;

        const contentType = response.headers.get("content-type");

        if (contentType && contentType.includes("application/json")) {
            payload = await response.json(); // Usually returns to ResultService.cs form
        } else {
            payload = await response.text();
        }

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
                data: payload.data
            };
        }

        return { // raw string etc.
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
    try {
        const response = await fetch(`${API_BASE_URL}/authentication/refresh-token`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ accessToken, refreshToken })
        });

        if (!response.ok) {
            return { success: false };
        }

        const data = await response.json();

        let tokens;

        if (data.success && data.data) {
            tokens = data.data;
        } else if (data.accessToken) {
            tokens = data;
        } else {
            return { success: false };
        }

        if (tokens.accessToken) {
            localStorage.setItem('jwtToken', tokens.accessToken);
        }
        if (tokens.refreshToken) {
            localStorage.setItem('refreshToken', tokens.refreshToken);
        }

        return {
            success: true,
            accessToken: tokens.accessToken,
            refreshToken: tokens.refreshToken
        };
    } catch (error) {
        console.error('Token refresh failed:', error);
        return { success: false };
    }
}