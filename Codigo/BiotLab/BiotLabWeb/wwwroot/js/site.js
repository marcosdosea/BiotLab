(() => {
    const createIcons = () => {
        if (window.lucide?.createIcons) {
            window.lucide.createIcons();
        }
    };

    const sidebar = document.getElementById('sidebar');
    const backdrop = document.getElementById('sidebarBackdrop');
    const openSidebar = () => {
        sidebar?.classList.remove('-translate-x-full');
        backdrop?.classList.remove('hidden');
        document.body.classList.add('overflow-hidden');
    };
    const closeSidebar = () => {
        sidebar?.classList.add('-translate-x-full');
        backdrop?.classList.add('hidden');
        document.body.classList.remove('overflow-hidden');
    };

    document.querySelectorAll('[data-sidebar-open]').forEach((button) => button.addEventListener('click', openSidebar));
    document.querySelectorAll('[data-sidebar-close]').forEach((button) => button.addEventListener('click', closeSidebar));

    const userMenuButton = document.getElementById('userMenuButton');
    const userMenuDropdown = document.getElementById('userMenuDropdown');
    const userMenuWrapper = document.getElementById('userMenuWrapper');

    userMenuButton?.addEventListener('click', (event) => {
        event.stopPropagation();
        userMenuDropdown?.classList.toggle('hidden');
    });

    document.addEventListener('click', (event) => {
        if (userMenuWrapper && !userMenuWrapper.contains(event.target)) {
            userMenuDropdown?.classList.add('hidden');
        }
    });

    const currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll('.nav-link[href]').forEach((link) => {
        const href = link.getAttribute('href')?.toLowerCase();
        if (!href || href === '/') return;
        if (currentPath === href || currentPath.startsWith(href + '/')) {
            link.classList.add('active');
        }
    });

    document.querySelectorAll('[data-table-search]').forEach((input) => {
        const table = input.closest('.rounded-3xl, .overflow-hidden')?.querySelector('[data-table]');
        if (!table) return;
        const rows = Array.from(table.querySelectorAll('tbody tr'));
        input.addEventListener('input', () => {
            const term = input.value.trim().toLowerCase();
            rows.forEach((row) => {
                row.style.display = row.innerText.toLowerCase().includes(term) ? '' : 'none';
            });
        });
    });

    document.querySelectorAll('#Estado, input[name="Estado"]').forEach((input) => {
        input.addEventListener('input', () => {
            input.value = (input.value || '').replace(/[^a-zA-Z]/g, '').slice(0, 2).toUpperCase();
        });
        input.value = (input.value || '').replace(/[^a-zA-Z]/g, '').slice(0, 2).toUpperCase();
    });


    createIcons();
})();
