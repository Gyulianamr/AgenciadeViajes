// Cambio de tema
const toggleSwitch = document.getElementById('theme-toggle');
const themeText = document.getElementById('theme-text');

if (localStorage.getItem('theme') === 'dark') {
    document.body.classList.add('dark-mode');
    toggleSwitch.checked = true;
    themeText.textContent = 'Modo Oscuro';
} else {
    document.body.classList.add('light-mode');
    themeText.textContent = 'Modo Claro';
}

toggleSwitch.addEventListener('change', function () {
    if (this.checked) {
        document.body.classList.replace('light-mode', 'dark-mode');
        themeText.textContent = 'Modo Oscuro';
        localStorage.setItem('theme', 'dark');
    } else {
        document.body.classList.replace('dark-mode', 'light-mode');
        themeText.textContent = 'Modo Claro';
        localStorage.setItem('theme', 'light');
    }
});
// Menú plegable
const menuToggle = document.getElementById('menu-toggle');
const sidebar = document.getElementById('sidebar');
const mainContent = document.querySelector('.main-content');

let isMenuOpen = true;

menuToggle.addEventListener('click', function () {
    if (isMenuOpen) {
        sidebar.classList.add('collapsed');
        mainContent.classList.add('shifted');
    } else {
        sidebar.classList.remove('collapsed');
        mainContent.classList.remove('shifted');
    }
    isMenuOpen = !isMenuOpen;
});