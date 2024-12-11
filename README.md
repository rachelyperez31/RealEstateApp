# RealEstateApp

## Overview
**RealEstateApp** is a comprehensive web application designed to manage real estate properties. Built using **ASP.NET Core MVC (version 7)** with **Bootstrap** for the frontend, the system supports four user roles: **Clients**, **Agents**, **Administrators**, and **Developers**. The app features a robust property listing, secure user authentication with JWT, and various CRUD operations tailored to each role.

---

## Features

### General Functionalities
- **Home**: Displays a list of all properties from newest to oldest with filters:
  - Property type (e.g., apartment, house).
  - Price range.
  - Number of bedrooms and bathrooms.
- **Property Details**:
  - Includes property code, type, sale type, price, size, description, features, and agent details.
  - A slider for property images.

- **Agents**:
  - Lists active agents with their profile pictures and names.
  - Displays properties associated with each agent.

- **Join the App**: Allows users to register as either a Client or an Agent.
  - Clients receive an email for activation.
  - Agents require admin approval for activation.

- **Login**: Secure login with different redirections based on user roles:
  - **Clients**: Redirected to their personalized home page.
  - **Agents**: Redirected to a property management dashboard.
  - **Administrators**: Access to advanced system management tools.

---

### Role-Specific Functionalities

#### **Clients**
- **Home**: Browse properties, and save favorites.
- **Favorites**: View and manage favorite properties.
- **Logout**: Securely log out of the system.

#### **Agents**
- **Property Management**: Add, update, or delete properties assigned to them.
- **Dashboard**: Track properties and manage listings.
- **Profile Management**: Update personal information.

#### **Administrators**
- **Dashboard**: Manage users, agents, developers and global property settings.
- **Agent Management**: Approve, deactivate, or delete agents and their properties.
- **Developer Management**: CRUD operations for developers and activate and deactivate them.
- **Property Type and Sale Type Management**: CRUD operations for property types and sales types.
- **Improvements Management**: Manage property improvements.

#### **Developers**
Developers have specific API-related functionalities and data management responsibilities. These include:

- **Login and Security**:
  - Authenticate and obtain a JWT token for accessing API endpoints.
  - Strict role-based access control:
    - Developers cannot access administrator functionalities.
    - Administrators cannot access developer-specific functionalities.
  - Unauthorized access returns appropriate HTTP status codes:
    - **401 Unauthorized**: For unauthenticated users.
    - **403 Forbidden**: For unauthorized access.

- **API Interaction**:
  - Access endpoints to manage properties, agents, property types, sales types, and improvements.
  - Examples of key endpoints:
    - **Properties**:
      - List all properties or retrieve details by ID or code.
    - **Agents**:
      - List agents or retrieve agent-specific details like assigned properties.
    - **Property Types, Sales Types, and Improvements**:
      - List and retrieve details of these entities.

- **User Management**:
  - Register new developers via an administrator-controlled endpoint.

---

## Technical Requirements
- **Frontend**: ASP.NET Core MVC with **Bootstrap** for responsive design.
- **Authentication**: ASP.NET Identity with JWT for secure login and role management.
- **Data Persistence**: Entity Framework Core with code-first migrations.
- **API Design**:
  - Comprehensive Swagger documentation.
- **Role Validation**: Role-based access control ensuring strict authorization:
  - **401 Unauthorized**: For unauthenticated users.
  - **403 Forbidden**: For unauthorized access attempts.

---

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- SQL Server

### Setup
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/rachelyperezdev/RealEstateApp.git
   cd RealEstateApp

### Setup Database:
1. Update the connection string in `appsettings.json`.
2. Run migrations:
   ```bash
   dotnet ef database update

### Run the application
  ```bash
  dotnet run
