import { HeaderComponent } from './components/HeaderComponent.js';
import { SidebarComponent } from './components/SidebarComponent.js';

import { StudentPages } from './pages/StudentPages.js';
import { TeacherPages } from './pages/TeacherPages.js';
import { AdminPages } from './pages/AdminPages.js';

import { Router } from './core/Router.js';
import { AppState } from './core/AppState.js';
import { apiRequest } from './core/ApiService.js';

const App = {
    init() {
        console.log('Initializing application...');
        this.checkExistingSession();
        this.attachLoginForm();
    },

    checkExistingSession() {
        const token = localStorage.getItem('jwtToken');
        const email = localStorage.getItem('email');
        const role = localStorage.getItem('role');

        if (token && email && role) {
            console.log('Existing session found, loading dashboard...');
            AppState.setUser(email, role);
            this.showDashboard(email, role);
        } else {
            console.log('No session, showing login page');
            this.showLoginPage();
        }
    },

    attachLoginForm() {
        const loginForm = document.getElementById('loginForm');
        if (loginForm) {
            loginForm.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleLogin();
            });
        }
    },

    async handleLogin() {
        const emailInput = document.getElementById('loginEmail');
        const passwordInput = document.getElementById('loginPassword');
        const roleInput = document.querySelector('input[name="role"]:checked');
        const loginBtn = document.querySelector('button[type="submit"]');
        const errorContainer = document.getElementById('loginError');

        if (!emailInput || !passwordInput || !roleInput) {
            console.error('Login form elements not found');
            return;
        }

        const email = emailInput.value.trim();
        const password = passwordInput.value;
        const role = roleInput.value;

        if (!email || !password) {
            this.showLoginError('Please enter email and password', errorContainer);
            return;
        }

        loginBtn.disabled = true;
        loginBtn.textContent = 'Signing in...';

        if (errorContainer) {
            errorContainer.style.display = 'none';
            errorContainer.textContent = '';
        }

        try {
            const response = await apiRequest('/authentication/login', 'POST', {
                email,
                password
            });

            if (!response.ok) {
                const message = response.message || 'Login failed. Please try again.';
                this.showLoginError(message, errorContainer);
                loginBtn.disabled = false;
                loginBtn.textContent = 'Sign In';
                return;
            }

            const actualRole = response.data.role.toLowerCase();

            // Warn if selected role doesn't match actual role
            if (role.toLowerCase() !== actualRole) {
                console.warn(
                    `Role mismatch: You selected "${role}" but your actual role is "${actualRole}"`
                );
                this.showLoginError(
                    `Note: You are registered as a ${actualRole}, not a ${role}`,
                    errorContainer
                );
                loginBtn.disabled = false;
                loginBtn.textContent = 'Sign In';
                return;
            }

            AppState.setUser(email, actualRole);

            localStorage.setItem('jwtToken', response.data.accessToken);
            localStorage.setItem("refreshToken", response.data.refreshToken);
            localStorage.setItem('email', email);
            localStorage.setItem('role', actualRole);


            console.log('Login successful for:', email);
            this.showDashboard(email, actualRole);

        } catch (error) {
            console.error('Login error:', error);
            this.showLoginError('An unexpected error occurred. Please try again.', errorContainer);
            loginBtn.disabled = false;
            loginBtn.textContent = 'Sign In';
        }
    },

    showLoginError(message, errorContainer) {
        if (errorContainer) {
            errorContainer.textContent = message;
            errorContainer.style.display = 'block';
        }
        console.warn('Login failed:', message);
    },

    showLoginPage() {
        const loginPage = document.getElementById('loginPage');
        const dashboardPage = document.getElementById('dashboardPage');

        if (loginPage) loginPage.style.display = 'flex';
        if (dashboardPage) dashboardPage.classList.remove('visible');
    },

    showDashboard(email, role) {
        const loginPage = document.getElementById('loginPage');
        const dashboardPage = document.getElementById('dashboardPage');

        if (loginPage) loginPage.style.display = 'none';
        if (dashboardPage) dashboardPage.classList.add('visible');

        this.setRouterPages(role);

        const headerContainer = document.getElementById('headerContainer');
        if (headerContainer) {
            headerContainer.innerHTML = HeaderComponent.render(email, role);
            HeaderComponent.attach(() => this.logout());
        }

        const sidebarContainer = document.getElementById('sidebarContainer');
        if (sidebarContainer) {
            sidebarContainer.innerHTML = SidebarComponent.render(role);
            SidebarComponent.attach();
        }

        Router.goToPage('dashboard');
        console.log('Dashboard shown for user:', email);
    },

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
    },

    logout() {
        localStorage.removeItem('jwtToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('email');
        localStorage.removeItem('role');

        const loginForm = document.getElementById('loginForm');
        if (loginForm) {
            loginForm.reset();
        }

        const roleStudent = document.getElementById('roleStudent');
        if (roleStudent) {
            roleStudent.checked = true;
        }

        this.showLoginPage();
        console.log('Logged out successfully');
    }
};

window.addEventListener('DOMContentLoaded', () => {
    App.init();
});