const modeSwitch = document.getElementById('modeSwitch');
const themeText = document.getElementById('theme-text');

// Verificar si hay un modo guardado en localStorage
const savedMode = localStorage.getItem('mode');

// Aplicar el modo guardado si existe
if (savedMode === 'dark') {
    document.body.classList.add('dark-mode');
    modeSwitch.checked = true;
    themeText.textContent = "Modo Oscuro";
} else {
    document.body.classList.add('light-mode');
    modeSwitch.checked = false;
    themeText.textContent = "Modo Claro";
}

// Escuchar el cambio en el interruptor
modeSwitch.addEventListener('change', () => {
    if (modeSwitch.checked) {
        document.body.classList.add('dark-mode');
        document.body.classList.remove('light-mode');
        localStorage.setItem('mode', 'dark');
        themeText.textContent = "Modo Oscuro";
    } else {
        document.body.classList.add('light-mode');
        document.body.classList.remove('dark-mode');
        localStorage.setItem('mode', 'light');
        themeText.textContent = "Modo Claro";
    }
});
