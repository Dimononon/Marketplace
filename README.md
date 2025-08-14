Marketplace API

A RESTful Web API for an online marketplace with role-based access control. Built with .NET 8, ASP.NET Core Web API, and Entity Framework Core, the project supports buyers, sellers, and administrators, providing secure authentication and authorization using JWT tokens.

Features

Registration and login with JWT tokens.

Role-based access: Buyer, Seller, Administrator.

Create, read, update, and delete products.

Sellers can edit only their own products.


Create orders and manage order statuses.

Status values stored as enums but returned as readable strings.

Search products by name.

Filter products by price range.

Centralized validation and error handling middleware.

Unified Result<T> pattern for consistent responses.

Separation of concerns with service layer for business logic.

DTO mapping handled only in controllers using AutoMapper.

Technologies Used

.NET 8 / ASP.NET Core Web API

Entity Framework Core / MS SQL Server

AutoMapper

JWT Authentication

LINQ

Middleware for error handling
