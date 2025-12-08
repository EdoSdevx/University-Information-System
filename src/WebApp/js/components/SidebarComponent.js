/**
 * SidebarComponent Module
 * Renders role-specific sidebar navigation
 */

import { Router } from '../core/Router.js';

export const SidebarComponent = {
    // Menu configuration per role
    menuConfig: {
        student: {
            navigation: [
                { page: 'dashboard', label: 'Dashboard' },
                { page: 'courses', label: 'My Courses' },
                { page: 'attendance', label: 'Attendances'},
                { page: 'schedule', label: 'My Schedule' },
                { page: 'registration', label: 'Course Registration' },
                { page: 'grades', label: 'Grades & Transcripts' }
            ],
            account: [
                { page: 'profile', label: 'My Profile' },
                { page: 'settings', label: 'Account Settings' },
                { page: 'help', label: 'Help & Support' }
            ]
        },
        teacher: {
            navigation: [
                { page: 'dashboard', label: 'Dashboard' },
                { page: 'classes', label: 'My Classes' },
                { page: 'schedule', label: 'My Schedule' },
                { page: 'grading', label: 'Grading' },
                { page: 'attendance', label: 'Attendance'},
                { page: 'roster', label: 'Class Roster' },
                { page: 'announcements', label: 'Announcements'}
            ],
            account: [
                { page: 'profile', label: 'My Profile' },
                { page: 'settings', label: 'Account Settings' },
                { page: 'help', label: 'Help & Support' }
            ]
        },
        admin: {
            navigation: [
                { page: 'dashboard', label: 'Dashboard' },
                { page: 'users', label: 'User Management' },
                { page: 'courses', label: 'Course Management' },
                { page: 'reports', label: 'Reports' }
            ],
            account: [
                { page: 'profile', label: 'My Profile' },
                { page: 'settings', label: 'System Settings' },
                { page: 'logs', label: 'Activity Logs' }
            ]
        }
    },

    render(role) {
        const config = this.menuConfig[role];
        if (!config) return '';

        const navItems = config.navigation
            .map(item => `
                <div class="dashboard-sidebar-item ${item.page === 'dashboard' ? 'active' : ''}" data-page="${item.page}">
                    ${item.label}
                </div>
            `)
            .join('');

        const accountItems = config.account
            .map(item => `
                <div class="dashboard-sidebar-item" data-page="${item.page}">
                    ${item.label}
                </div>
            `)
            .join('');

        return `
            <div class="dashboard-sidebar">
                <div class="dashboard-sidebar-section">
                    <div class="dashboard-sidebar-title">Navigation</div>
                    ${navItems}
                </div>
                <div class="dashboard-sidebar-section">
                    <div class="dashboard-sidebar-title">Account</div>
                    ${accountItems}
                </div>
            </div>
        `;
    },

    attach() {
        document.querySelectorAll('.dashboard-sidebar-item').forEach(item => {
            item.addEventListener('click', () => {
                Router.goToPage(item.dataset.page);
            });
        });
    }
};