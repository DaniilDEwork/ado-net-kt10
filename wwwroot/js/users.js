let editingId = null;
const form = document.getElementById('user-form');
const nameInput = document.getElementById('name');
const emailInput = document.getElementById('email');

function resetUser() {
    editingId = null;
    form.reset();
    document.getElementById('form-title').textContent = 'Добавление пользователя';
    document.getElementById('save-user').textContent = 'Добавить пользователя';
}

async function loadUsers(showResponse = false) {
    const users = await apiRequest('GET', '/api/users', undefined, showResponse);
    const tbody = document.getElementById('users-body');
    tbody.replaceChildren();
    for (const user of users) {
        const row = document.createElement('tr');
        cell(row, user.id);
        cell(row, user.userName);
        cell(row, user.email);
        const actions = cell(row, '');
        actionButton(actions, 'Просмотр', async () => {
            await apiRequest('GET', `/api/users/${user.id}`);
        });
        actionButton(actions, 'Изменить', async () => {
            const current = await apiRequest('GET', `/api/users/${user.id}`);
            editingId = current.id;
            nameInput.value = current.userName;
            emailInput.value = current.email;
            document.getElementById('form-title').textContent = `Редактирование пользователя с ID ${current.id}`;
            document.getElementById('save-user').textContent = 'Сохранить изменения';
        });
        actionButton(actions, 'Удалить', async () => {
            await apiRequest('DELETE', `/api/users/${user.id}`);
            if (editingId === user.id) resetUser();
            await loadUsers();
        });
        tbody.append(row);
    }
    if (!users.length) emptyRow(tbody, 4, 'Пользователей пока нет');
}

form.addEventListener('submit', event => {
    event.preventDefault();
    perform(async () => {
        await apiRequest(editingId === null ? 'POST' : 'PUT', editingId === null ? '/api/users' : `/api/users/${editingId}`, {
            userName: nameInput.value,
            email: emailInput.value
        });
        await loadUsers();
    });
});
document.getElementById('lookup-form').addEventListener('submit', event => {
    event.preventDefault();
    perform(async () => {
        const id = document.getElementById('lookup-id').value;
        if (!id) throw new Error('Введите ID пользователя');
        await apiRequest('GET', `/api/users/${encodeURIComponent(id)}`);
    });
});
document.getElementById('reset-user').addEventListener('click', resetUser);
document.getElementById('refresh-users').addEventListener('click', () => perform(() => loadUsers(true)));
perform(() => loadUsers(true));
