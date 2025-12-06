/**
 * AppState Module
 * Manages global application state (user, role, token, current page)
 */

export const AppState = {
    user: null,
    role: 'student',
    token: null,
    currentPage: 'dashboard',

    setUser(email, role) {
        this.user = email;
        this.role = role;
        this.currentPage = 'dashboard';
        console.log('? AppState updated:', { user: email, role });
    },

    getInitials() {
        if (!this.user) return 'U';
        return this.user.split('@')[0]
            .split('.')
            .map(n => n[0].toUpperCase())
            .join('');
    },

    reset() {
        this.user = null;
        this.token = null;
        this.currentPage = 'dashboard';
        console.log('? AppState reset');
    }
};