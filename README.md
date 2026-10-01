# CreditWorks Vehicle Management

Technical programming assignment for the CreditWorks Software Engineer position. Create a web application for managing vehicles and vehicle weight categories.

## Technology Stack
Backend Technologies 
- C#
- ASP .NET Core Web API
- Entity Framework Core
- Swagger

Frontend Technologies
- React (Vite)

Database
- Microsoft SQL Server 2026

Testing
- xUnit

## Project Structure 
```
backend/
CWVehicleManagerAPI.sln            Visual Studio solution
CWVehicleManagerAPI/               ASP.NET Core Web API
  Controllers/                     Handles API requests 
  Data/                            Database setup and SeedData
  Mappings/                        Converts database models to API data
  Migrations/                      EF Core migrations (create and update the database)
  Models/Domain/                   Main database models : Vehicle, Manufacturer, VehicleCategory
  Models/DTO/                      Data sent to and received from the API
  Repositories/                    Data access (Reads and saves data in SQL Server)
  Utils/                           Business rules: Validation and category rules
  Program.cs                       Starts and configures the API

CWVehicleManagerAPI.Tests/         Automated backend tests
  Utils/                           Tests validation and category rules
  Mapping/                         Tests model and DTO mapping
  Repositories/                    Tests database queries and sorting

frontend/                          React app 
  src/index.jsx                    Starts the React application
  src/App.jsx                      Main application component
  src/api/                         Sends requests to the backend API
  src/components/                  React components and pages
  src/styles/                      CSS styles
  
package.json                       Frontend packages and scripts
vite.config.js                     Vite and API proxy configuration
```


## Features 

| Requirement | Where |
| --- | --- |
| Add a vehicle (owner, manufacturer, year, weight) | *Add vehicle* page → `POST /api/vehicles` |
| List all vehicles with their category icon | *Vehicles* page → `GET /api/vehicles` |
| Sort by owner, manufacturer, year or weight, ascending or descending | Click a column header (an arrow shows the current sort) |
| Category worked out automatically from the weight | Calculated every time vehicles are read, never stored |
| Create, edit and delete categories, pick an icon | *Categories* page → `POST`, `PUT` and `DELETE /api/categories` |
| No gaps or overlaps between categories | Each category ends where the next one starts; the server adjusts neighbours after every change |
| Vehicles follow category changes immediately | Because the category is never stored on the vehicle |

## Required Software

| Software | Version | Notes |
| --- | --- | --- |
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0 | `dotnet --version` |
| [Node.js](https://nodejs.org/) | 22.12+ | Required by Vite `npm -v` |
| SQL Server | 2026| LocalDB/Express |

## Configuration of Application 

Clone the code base using this repository link (https://github.com/MalishaFdo/creditworks-vehicle-management)

Backend 
- Navigate to backend folder
- Create a new SQL Server database.
- Changing the database connection string (DbConnectionString) with your database details in the *appsettings.Development.json*
- Open NuGet Package Manager console and execute following commands
  - ``Add-Migration init_db``
  - ``Update-Database``
- After that build and run the backend API (API runs on (https://localhost:7052;http://localhost:5009)
- You can check the swagger using this URL (https://localhost:7052/swagger;http://localhost:5009/swagger)

Frontend 
- Navigate to frontend folder
- Create .env file and add API_URL= (https://localhost:7052)
- Open terminal along this folder path and execute below commands to run the application
  - ``npm i``
  - ``npm start``
- Open the browser and go to (http://localhost:5173/)

## Automated Testing 
The tests focus on the main business rules and important application behaviour, including:

- Vehicle category calculation based on weight
- Exact category boundary values
- Category range changes
- Prevention of invalid category ranges
- Vehicle validation
- Category validation
- Existing vehicles changing category when category boundaries change
- Vehicle sorting by owner, manufacturer, year, and weight
- Repository database queries

## API

| Method & Path | What it does |
| --- | --- |
| `GET /api/vehicles` | Get all vehicles |
| `GET /api/vehicles/{id}` | Get one vehicle by its ID |
| `POST /api/vehicles` | Add a new vehicle |
| `GET /api/manufacturers` | Get all manufacturers |
| `GET /api/categories` | Get all vehicle categories |
| `GET /api/categories/{id}` | Get one category by its ID |
| `POST /api/categories` | Add a new category |
| `PUT /api/categories/{id}` | Update an existing category |
| `DELETE /api/categories/{id}` | Delete a category |
| `GET /api/categories/icons` | Get the available category icons |

### Vehicle Sorting

Vehicles can be sorted by owner name, manufacturer, year, or weight.

For example:

`GET /api/vehicles?sortBy=weight&sortDirection=asc`

This gets all vehicles sorted by weight from lowest to highest.

