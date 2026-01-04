# All The Beans

A lightweight application for managing and displaying coffee beans with a "Bean of the Day" feature. The project includes both a .NET Core Web API backend and a React frontend. 

## Features

### Backend (API)
- **CRUD Operations**: Create, read, update, and delete coffee beans
- **Bean of the Day**: Automatically selects a random bean each day (ensuring it's different from the previous day)
- **Search**: Search beans by name, country, or colour
- **Database Seeding**: Automatically imports initial data from JSON file on first run

### Frontend (Web)
- **Bean List**: Display all available coffee beans in a responsive grid
- **Bean Details**: View detailed information about a selected bean
- **Bean of the Day**: Featured bean displayed on the home page
- **Search**: Filter beans by name, country, or colour
- **Order Form**: Place orders for coffee beans with validation

## Tech Stack

### Backend
- .NET 10.0
- Entity Framework Core
- SQLite
- ASP.NET Core Web API
- xUnit & Moq (testing)

### Frontend
- React 19
- TypeScript
- Vite
- Bootstrap 5
- React Router
- Axios

## Technology Choices

### Backend

**.NET 10.0** - Using the latest stable version for increased security and access to the latest features.

**Entity Framework Core** - Used it for the code-first approach and migrations. Makes it easier to manage the database schema and keep it in sync with the code.

**SQLite** - File-based database, no separate server needed. Good fit for a lightweight app like this and keeps setup simple.

**ASP.NET Core Web API** - Has built-in support for REST APIs, dependency injection, and validation making the implementation of API simpler.

**xUnit & Moq** - Standard testing tools for .NET. xUnit is pretty common, and Moq helps with mocking in tests.

### Frontend

**React** - Chose React because I have some experience with it and it's widely used. Makes it easier to build interactive UIs with components.

**TypeScript** - Adds type safety which helps catch errors early.

**Vite** - Fast build tool and dev server.

**Bootstrap** - Quick way to get a responsive layout without writing lots of custom CSS.

**React Router** - Standard way to handle routing in React apps. Makes navigation between pages straightforward.

**Axios** - Simple HTTP client for making API calls.

## Prerequisites

- .NET 10.0 SDK
- Node.js (v18 or higher recommended)
- npm or yarn

## Setup

### Backend (API)

```bash
dotnet restore
cd AllTheBeans.Api
dotnet run
```

The database will be created automatically and seeded from `AllTheBeans.json` on first run. The API will run on `http://localhost:5114`.

### Frontend (Web)

```bash
cd all-the-beans-web
npm install
npm run dev
```

The frontend will run on `http://localhost:5173` and connect to the API automatically.

### Running Both

1. Start the API first (in one terminal)
2. Then start the frontend (in another terminal)
3. Open `http://localhost:5173` in your browser

## API Endpoints

Base URL: `http://localhost:5114` (HTTP) or `https://localhost:7285` (HTTPS)

### Beans

#### Get All Beans
```
GET /api/beans
```
Returns a list of all coffee beans.

#### Get Bean by ID
```
GET /api/beans/{id}
```
Returns a specific bean by its ID.

**Response:** 200 OK with bean data, or 404 Not Found

#### Create Bean
```
POST /api/beans
Content-Type: application/json

{
  "name": "Bean Name",
  "description": "Bean description",
  "country": "Country",
  "colour": "Brown",
  "cost": 15.99,
  "image": "image.jpg"
}
```

**Response:** 201 Created with the new bean data

#### Update Bean
```
PUT /api/beans/{id}
Content-Type: application/json

{
  "name": "Updated Name",
  "cost": 20.00
}
```
All fields are optional - only provided fields will be updated.

**Response:** 200 OK with updated bean, or 404 Not Found

#### Delete Bean
```
DELETE /api/beans/{id}
```

**Response:** 204 No Content on success, or 404 Not Found

#### Search Beans
```
GET /api/beans/search?name=KLUGGER&country=Colombia&colour=green
```
Search parameters are optional. You can search by:
- `name`: Partial match on bean name
- `country`: Exact match on country
- `colour`: Exact match on colour

### Bean of the Day

#### Get Bean of the Day
```
GET /api/beanoftheday
```
Returns the current "Bean of the Day". If no bean has been selected for today, one will be automatically selected ensuring it's different from yesterday's bean.

**Response:** 200 OK with bean data (with `isBeanOfTheDay: true`), or 404 Not Found if no beans exist

## Testing

### Running Unit Tests
```bash
dotnet test
```

### Testing the API
Use the `.http` file in the project or test with curl/Postman. Base URL: `http://localhost:5114`

## Project Structure

```
AllTheBeans.Api/              # Backend API
├── Controllers/              # API controllers
│   ├── BeansController.cs
│   └── BeanOfTheDayController.cs
├── Services/                 # Business logic
│   ├── BeanService.cs
│   ├── BeanOfTheDayService.cs
│   └── DataImporter.cs
├── Models/                   # Domain models and DTOs
│   ├── Bean.cs
│   ├── BeanOfTheDay.cs
│   └── Dtos/
├── Data/                     # Database context
│   └── AllTheBeansDbContext.cs
├── Migrations/               # EF Core migrations
└── Program.cs                 # Application entry point

all-the-beans-web/            # Frontend React app
├── src/
│   ├── pages/               # Page components
│   │   ├── Home.tsx
│   │   ├── BeanList.tsx
│   │   ├── BeanDetail.tsx
│   │   └── OrderForm.tsx
│   ├── services/            # API service
│   │   └── api.ts
│   ├── types/               # TypeScript types
│   │   └── Bean.ts
│   ├── App.tsx              # Main app component
│   └── main.tsx             # Entry point
└── package.json
```

## Notes

### Backend
- Database: SQLite file at `AllTheBeans.Api/allthebeans.db`
- Migrations run automatically on startup
- HTTP is available for easier testing. HTTPS will be enforced if this API is deployed to production.
- CORS is configured to allow requests from `http://localhost:5173`

### Frontend
- Orders are stored in browser localStorage (no backend persistence)
- Search is done client-side for instant results
- The app expects the API to be running on `http://localhost:5114`

