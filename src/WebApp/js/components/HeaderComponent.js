/**
 * HeaderComponent Module
 * Renders the dashboard header with user information
 */

import { AppState } from '../core/AppState.js';

export const HeaderComponent = {
    render() {
        const initials = AppState.getInitials();
        const userFirstName = AppState.user.split('@')[0];
        const roleLabel = AppState.role.charAt(0).toUpperCase() + AppState.role.slice(1);

        return `
            <div class="dashboard-header">
                <div class="dashboard-header-title">STUDENT PORTAL</div>
                <div class="dashboard-user-section">
                    <div class="dashboard-user-info">
                        <div class="dashboard-user-name">${userFirstName}</div>
                        <div class="dashboard-user-role">${roleLabel}</div>
                    </div>
                    <div class="dashboard-avatar">${initials}</div>
                    <button class="dashboard-logout-btn" id="logoutBtn">Sign Out</button>
                </div>
            </div>
        `;
    },

    attach(logoutCallback) {
        const logoutBtn = document.getElementById('logoutBtn');
        if (logoutBtn) {
            logoutBtn.addEventListener('click', logoutCallback);
        }
    }
};