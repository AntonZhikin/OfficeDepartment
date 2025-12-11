# Office Department Management System

Веб-приложение для управления офисами компании с полным функционалом CRUD операций, аутентификацией и аудитом действий.

## Архитектура проекта

Проект организован по принципам Clean Architecture и SOLID:

```
OfficeDepartment/
├── Domain/                    # Доменный слой
│   └── Entities/             # Сущности базы данных
├── Infrastructure/           # Инфраструктурный слой
│   ├── Data/                 # DbContext и конфигурации БД
│   └── Services/             # Сервисы (JWT, Audit, PasswordHasher)
├── Handlers/                 # Бизнес-логика (Handler Pattern)
├── Requests/                 # DTOs для запросов
└── Controllers/             # API контроллеры
```

## Сущности

1. **HeadOffice** - Главный офис компании
2. **BranchOffice** - Дочерние офисы
3. **OfficeTask** - Задачи для офисов
4. **Employee** - Сотрудники
5. **Department** - Отделы
6. **User** - Пользователи системы
7. **AuditLog** - Лог действий пользователей

## Взаимосвязи

- **HeadOffice** → **BranchOffice** (One-to-Many)
- **HeadOffice** → **Department** (One-to-Many)
- **BranchOffice** → **OfficeTask** (One-to-Many)
- **BranchOffice** → **Employee** (One-to-Many)
- **Department** → **Employee** (One-to-Many)
- **Employee** → **OfficeTask** (One-to-Many)

## Технологии

- **.NET 9.0** - Backend framework
- **Entity Framework Core** - ORM
- **PostgreSQL** - База данных
- **JWT Bearer** - Аутентификация
- **Swagger/OpenAPI** - Документация API

## Требования

- .NET 9.0 SDK
- PostgreSQL 16+ (или Docker)
- Visual Studio 2022 / Rider / VS Code

## Установка и запуск

### 1. Клонирование репозитория

```bash
git clone <repository-url>
cd OfficeDepartment
```

### 2. Настройка базы данных

#### Вариант A: Использование Docker Compose

```bash
docker-compose up -d
```

#### Вариант B: Локальная установка PostgreSQL

Создайте базу данных:
```sql
CREATE DATABASE OfficeDepartment;
```

### 3. Настройка строки подключения

Отредактируйте `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=OfficeDepartment;Username=postgres;Password=postgres"
  }
}
```

### 4. Восстановление пакетов и запуск

```bash
dotnet restore
dotnet build
dotnet run
```

Приложение будет доступно по адресу: `https://localhost:5001` или `http://localhost:5000`

## API Документация

После запуска приложения откройте Swagger UI:
- Development: `https://localhost:5001/swagger`

## Аутентификация

### Регистрация пользователя

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "user1",
  "email": "user1@example.com",
  "password": "password123"
}
```

### Вход в систему

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "user1",
  "password": "password123"
}
```

Ответ содержит JWT токен, который нужно использовать в заголовке `Authorization: Bearer <token>`

### Дефолтный администратор

При первом запуске создается администратор:
- **Username:** `admin`
- **Password:** `admin123`
- **Role:** `Admin`

## Роли пользователей

- **Admin** - Полный доступ ко всем операциям (CRUD)
- **User** - Чтение данных, создание и обновление задач

## API Endpoints

### HeadOffice (Главный офис)

- `GET /api/HeadOffice` - Получить список (с фильтрацией и пагинацией)
- `GET /api/HeadOffice/{id}` - Получить по ID
- `POST /api/HeadOffice` - Создать (только Admin)
- `PUT /api/HeadOffice/{id}` - Обновить (только Admin)
- `DELETE /api/HeadOffice/{id}` - Удалить (только Admin)

### BranchOffice (Дочерний офис)

- `GET /api/BranchOffice` - Получить список
- `GET /api/BranchOffice/{id}` - Получить по ID
- `POST /api/BranchOffice` - Создать (только Admin)
- `PUT /api/BranchOffice/{id}` - Обновить (только Admin)
- `DELETE /api/BranchOffice/{id}` - Удалить (только Admin)

### OfficeTask (Задачи)

- `GET /api/OfficeTask` - Получить список
- `GET /api/OfficeTask/{id}` - Получить по ID
- `POST /api/OfficeTask` - Создать
- `PUT /api/OfficeTask/{id}` - Обновить
- `DELETE /api/OfficeTask/{id}` - Удалить (только Admin)

### Employee (Сотрудники)

- `GET /api/Employee` - Получить список
- `GET /api/Employee/{id}` - Получить по ID
- `POST /api/Employee` - Создать (только Admin)
- `PUT /api/Employee/{id}` - Обновить (только Admin)
- `DELETE /api/Employee/{id}` - Удалить (только Admin)

### Department (Отделы)

- `GET /api/Department` - Получить список
- `GET /api/Department/{id}` - Получить по ID
- `POST /api/Department` - Создать (только Admin)
- `PUT /api/Department/{id}` - Обновить (только Admin)
- `DELETE /api/Department/{id}` - Удалить (только Admin)

### AuditLog (Логи действий)

- `GET /api/AuditLog` - Получить список (только Admin)
- `GET /api/AuditLog/{id}` - Получить по ID (только Admin)

## Фильтрация и пагинация

Все GET endpoints поддерживают фильтрацию и пагинацию через query параметры:

```
GET /api/HeadOffice?SearchTerm=Москва&City=Москва&Page=1&PageSize=10
```

## Принципы проектирования

### SOLID

- **S (Single Responsibility)** - Каждый класс имеет одну ответственность
- **O (Open/Closed)** - Классы открыты для расширения через интерфейсы
- **L (Liskov Substitution)** - Интерфейсы могут быть заменены реализациями
- **I (Interface Segregation)** - Интерфейсы разделены по функциональности
- **D (Dependency Inversion)** - Зависимости через интерфейсы, не конкретные классы

### DRY (Don't Repeat Yourself)

- Общий `BaseController` для общих методов
- Переиспользуемые Handlers и Services

### KISS (Keep It Simple, Stupid)

- Простая и понятная структура проекта
- Минимум абстракций, максимум ясности

## Логирование действий (Audit Log)

Все операции Create, Update, Delete автоматически логируются в таблицу `AuditLogs` с информацией:
- Действие (Action)
- Тип сущности (EntityType)
- ID сущности (EntityId)
- Пользователь (UserId)
- Старые и новые значения
- IP адрес
- Временная метка

## Миграции базы данных

### Создание первой миграции

```bash
cd OfficeDepartment
dotnet ef migrations add InitialCreate --project . --startup-project .
```

### Применение миграций

```bash
dotnet ef database update --project . --startup-project .
```

### Создание новой миграции после изменения моделей

```bash
dotnet ef migrations add MigrationName --project . --startup-project .
dotnet ef database update --project . --startup-project .
```

**Важно:** При первом запуске приложение автоматически применит миграции или создаст БД, если миграций еще нет.

## Тестирование

Для тестирования API можно использовать:
- Swagger UI (встроенный)
- Postman
- curl
- http файл (OfficeDepartment.http)

## Разработка

### Структура Handler

Каждый Handler содержит бизнес-логику для работы с сущностью:
- Валидация данных
- Работа с базой данных
- Логирование действий через AuditService

### Добавление новой сущности

1. Создать Entity в `Domain/Entities/`
2. Добавить в `ApplicationDbContext`
3. Создать Request DTOs в `Requests/`
4. Создать Handler в `Handlers/`
5. Создать Controller в `Controllers/`
6. Зарегистрировать в `Program.cs`

## Лицензия

Учебный проект

