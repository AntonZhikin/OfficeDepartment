# Office Department Frontend

Frontend приложение для управления офисами компании, построенное на React + Vite + TypeScript.

## Технологии

- **React 19** - UI библиотека
- **TypeScript** - Типизация
- **Vite** - Сборщик и dev server
- **React Router** - Маршрутизация
- **Axios** - HTTP клиент
- **Tailwind CSS** - Стилизация

## Установка

```bash
npm install
```

## Запуск

```bash
npm run dev
```

Приложение будет доступно по адресу: `http://localhost:5173`

## Сборка для продакшена

```bash
npm run build
```

## Структура проекта

```
src/
├── api/              # API сервисы (axios)
├── components/       # React компоненты
├── context/          # React Context (Auth)
├── pages/            # Страницы приложения
└── types/            # TypeScript типы
```

## Backend

Убедитесь, что backend запущен на `http://localhost:5042`

## Аутентификация

- Дефолтный админ: `admin` / `admin123`
- Или зарегистрируйте нового пользователя
