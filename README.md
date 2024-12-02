# Subscription API

This API manages SMS-based subscription services with various discount rules and promotions. It provides functionality for creating and managing subscriptions, handling discounts, and ensuring idempotency to prevent duplicate requests.


## Table of Contents

- [Subscription API](#subscription-api)
  - [Table of Contents](#table-of-contents)
  - [Architecture \& Patterns](#architecture--patterns)
    - [1. Clean Architecture](#1-clean-architecture)
    - [2. CQRS with MediatR](#2-cqrs-with-mediatr)
    - [3. Repository Pattern](#3-repository-pattern)
    - [4. Unit of Work](#4-unit-of-work)
    - [5. Factory Pattern](#5-factory-pattern)
    - [6. Middleware Pipeline](#6-middleware-pipeline)
  - [🔑 Key Features](#-key-features)
  - [⚙️ Prerequisites](#️-prerequisites)
  - [🚀 Getting Started](#-getting-started)
    - [1. Install .NET 8 SDK](#1-install-net-8-sdk)
    - [2. Restore Dependencies](#2-restore-dependencies)
    - [3. Build the Project](#3-build-the-project)
    - [4. Run the Project](#4-run-the-project)
    - [5. Access Swagger UI](#5-access-swagger-ui)
  - [🛠️ How to Install .NET 8 SDK](#️-how-to-install-net-8-sdk)
  - [.NET SDK Version Specification](#net-sdk-version-specification)

---

## Architecture & Patterns

### 1. Clean Architecture
The project follows Clean Architecture principles with clear separation of concerns:
- **Domain**: Core business logic and entities
- **Application**: Use cases and business rules
- **Infrastructure**: External concerns (database, caching, etc.)
- **API**: HTTP interface and controllers

### 2. CQRS with MediatR
Commands and Queries are separated:
- **Commands**: `CreateSubscriptionCommand`, `UnsubscribeCommand`
- **Queries**: `GetSubscriptionSummaryQuery`
- Each handler focuses on a single responsibility

### 3. Repository Pattern
- Generic `Repository<T>` base class
- Specific repositories for `Service` and `Subscription`
- Abstracts data access logic
- Enables easy unit testing through interface-based design

### 4. Unit of Work
- Manages transactions and data consistency
- Coordinates work of multiple repositories
- Ensures atomic operations

### 5. Factory Pattern
- `SubscriptionFactory` creates subscription instances
- Encapsulates creation logic
- Makes subscription creation consistent across the application

### 6. Middleware Pipeline
- `ExceptionMiddleware`: Global error handling
- `IdempotencyMiddleware`: Ensures idempotent operations

---

## 🔑 Key Features

1. **Idempotency** 🛡️  
   - Prevents duplicate operations  
   - Uses distributed cache  
   - Configurable expiration time  

2. **Input Validation** ✅  
   - FluentValidation for request validation  
   - Custom exception types  
   - Consistent error responses  

3. **Error Handling** ⚠️  
   - Global exception middleware  
   - Custom domain exceptions  
   - Detailed error messages  

4. **Swagger Documentation** 📜  
   - API documentation  
   - Request/response examples  
   - Authentication header documentation  

---

## ⚙️ Prerequisites

Before running this project, you'll need to have the following:

- **.NET 8 SDK**: The project requires the .NET 8 SDK. Ensure you have it installed on your machine.
- **Visual Studio (Optional)**: If you're using Visual Studio, ensure you have the latest version that supports .NET 8.

You can check the version of .NET SDK installed by running:
```bash
dotnet --version
```

---

## 🚀 Getting Started

To get started with the project, follow these steps:

### 1. Install .NET 8 SDK  
Ensure that .NET 8 SDK is installed on your machine. You can check your installed version with:
```bash
dotnet --version
```

If you don’t have .NET 8 installed, you can download it from the official website:  
[Download .NET 8 SDK](https://dotnet.microsoft.com/download/dotnet)

### 2. Restore Dependencies  
Run the following command to restore the necessary NuGet packages for the project:
```bash
dotnet restore
```

### 3. Build the Project  
Compile the project and generate the necessary output files:
```bash
dotnet build
```

### 4. Run the Project  
After building the project, you can run it:
```bash
dotnet run
```
This will start the API at `https://localhost:5001` or `http://localhost:5000`.

### 5. Access Swagger UI  
Open the Swagger UI in your browser to interact with the API:  
[Swagger UI](https://localhost:5001/swagger)

---

## 🛠️ How to Install .NET 8 SDK

If you don’t have the .NET 8 SDK installed, you can download it from the official .NET website:  
[Download .NET 8 SDK](https://dotnet.microsoft.com/download/dotnet)

On most systems, you can also install it via a package manager:
- **Windows**: Via [MSI Installer](https://dotnet.microsoft.com/download/dotnet) or via Windows Package Manager (`winget`).
- **Linux**: Follow the instructions for your specific Linux distribution (e.g., `apt`, `dnf`, etc.).
- **macOS**: Use `brew install dotnet-sdk` if you have Homebrew installed.

---

## .NET SDK Version Specification

Ensure your `.csproj` file is targeting .NET 8. Below is an example of how to specify the target framework for .NET 8:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <DocumentationFile>bin\Debug\net8.0\SubscriptionApi.xml</DocumentationFile>
  </PropertyGroup>

</Project>
```