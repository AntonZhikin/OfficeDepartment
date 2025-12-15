# Инструкция по запуску Frontend

## 1. Установка зависимостей

```bash
cd OfficeDepartmentFrontEnd/vite-project
npm install
```

## 2. Запуск Backend

Убедитесь, что backend запущен на `http://localhost:5042`

```bash
cd OfficeDepartment
dotnet run
```

## 3. Запуск Frontend

```bash
cd OfficeDepartmentFrontEnd/vite-project
npm run dev
```

Frontend будет доступен на `http://localhost:5173`

## 4. Вход в систему

- **Username:** `admin`
- **Password:** `admin123`

Или зарегистрируйте нового пользователя через форму регистрации.

## Что уже готово:

✅ Аутентификация (Login/Register)
✅ Dashboard с навигацией
✅ Страница HeadOffices с CRUD операциями
✅ API сервисы для всех сущностей
✅ Защищенные маршруты
✅ Tailwind CSS стилизация

## Что можно добавить:

- Страницы для BranchOffices, Employees, Departments, Tasks (по аналогии с HeadOffices)
- Фильтрация и поиск
- Пагинация
- Валидация форм

## Структура:

```
src/
├── api/              # Все API сервисы готовы
├── components/       # Layout, ProtectedRoute
├── context/          # AuthContext
├── pages/            # Login, Register, Dashboard, HeadOffices
└── types/            # Все TypeScript типы
```



