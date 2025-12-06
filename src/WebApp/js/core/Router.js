/**
 * Router Module
 * Handles page navigation and content injection
 */

import { AppState } from './AppState.js';

export const Router = {
    Pages: {}, // Will be set by App based on role

    goToPage(pageName) {
        if (!this.Pages[pageName]) {
            console.warn('Page not found:', pageName);
            return;
        }

        const page = this.Pages[pageName];
        const container = document.getElementById('appContainer');

        // Render page content
        container.innerHTML = page.render();

        // Run page-specific initialization
        if (page.afterRender) {
            page.afterRender();
        }

        // Update AppState
        AppState.currentPage = pageName;

        // Update active sidebar item
        this.updateActiveMenuItem(pageName);

        console.log('? Navigated to:', pageName);
    },

    updateActiveMenuItem(pageName) {
        document.querySelectorAll('.dashboard-sidebar-item').forEach(item => {
            item.classList.remove('active');
            if (item.dataset.page === pageName) {
                item.classList.add('active');
            }
        });
    }
};