/**
 * Main Module
 * Application entry point - imports all modules and initializes app
 */

// Import core modules
import { AppState } from './core/AppState.js';
import { Router } from './core/Router.js';

// Import component modules
import { HeaderComponent } from './components/HeaderComponent.js';
import { SidebarComponent } from './components/SidebarComponent.js';

// Import page modules
import { StudentPages } from './pages/StudentPages.js';
import { TeacherPages } from './pages/TeacherPages.js';
import { AdminPages } from './pages/AdminPages.js';

/**
 * App Module
 * Initializes the application and handles login/logout flows
 */
const App = {
    /**
     * Initialize app - attach event listeners
     */
    init() {
        console.log('Initializing application...');

        const loginForm = document.getElementById('loginForm');
        if (loginForm) {
            loginForm.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleLogin();
            });
        }
    },

    /**
     * Handle login form submission
     */
    handleLogin() {
        const email = document.getElementById('loginEmail').value;
        const password = document.getElementById('loginPassword').value;
        const role = document.querySelector('input[name="role"]:checked').value;

        // Create mock JWT token
        const payload = btoa(JSON.stringify({ email, role, iat: Date.now() }));
        const token = `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.${payload}.mock_signature`;

        // Update app state
        AppState.setUser(email, role);
        AppState.token = token;

        console.log('? JWT Token (demo):', token);

        // Show dashboard
        this.showDashboard();
    },

    /**
     * Show dashboard and render header, sidebar, and initial page
     */
    showDashboard() {
        document.getElementById('loginPage').style.display = 'none';
        document.getElementById('dashboardPage').classList.add('visible');

        // Set router pages based on user role
        this.setRouterPages(AppState.role);

        // Render header
        document.getElementById('headerContainer').innerHTML = HeaderComponent.render();
        HeaderComponent.attach(() => this.logout());

        // Render sidebar
        document.getElementById('sidebarContainer').innerHTML = SidebarComponent.render(AppState.role);
        SidebarComponent.attach();

        // Navigate to dashboard page
        Router.goToPage('dashboard');

        console.log('? Dashboard shown');
    },

    /**
     * Set router pages based on user role
     */
    setRouterPages(role) {
        switch (role) {
            case 'student':
                Router.Pages = StudentPages;
                break;
            case 'teacher':
                Router.Pages = TeacherPages;
                break;
            case 'admin':
                Router.Pages = AdminPages;
                break;
            default:
                Router.Pages = StudentPages;
        }
        console.log('? Router pages set for role:', role);
    },

    /**
     * Handle logout - reset state and show login page
     */
    logout() {
        AppState.reset();

        document.getElementById('loginPage').style.display = 'flex';
        document.getElementById('dashboardPage').classList.remove('visible');
        document.getElementById('loginForm').reset();
        document.getElementById('roleStudent').checked = true;

        console.log('? Logged out');
    }
};

/**
 * Initialize app when DOM is ready
 */
window.addEventListener('DOMContentLoaded', () => {
    App.init();
});