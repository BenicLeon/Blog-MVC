# Blog Application

This project is a simple blog application built using .NET Core ASP MVC with Entity Framework Core. The application allows users to register, log in, create and manage blog posts, add comments, and view users.

## Features

- **User Authentication:**
  - Users can log in via `Account/Login`.
  - Users can register via `Account/Register`.
  - After logging in, users are redirected to the Blog page where they can manage blogs.

- **Blog Management:**
  - Users can create new blog posts via `Blog/Create`.
  - Users can edit existing blog posts via `Blog/Edit/{id}`.
  - Users can delete blog posts via `Blog/Delete/{id}`.
  - Users can add comments to blog posts via `Blog/CreateComment/{blogId}`.
  - Users can search for blogs by title.

- **User Management:**
  - All users can view the list of users via `User/Index`.
  - Users can add, edit, and delete other users, just like blog posts. (`User/Create`, `User/Edit/{id}`, `User/Delete/{id}`).
  - There is no role-based authorization implemented, meaning all logged-in users have the same access rights to manage blogs and users.

- **Session Management:**
  - Users can log out using `Account/Logout`, which will log them out and clear the session data.

## Setup Instructions

To set up and run the application locally, follow these steps:

### 1. **Create the Database**

The application requires a SQL Server database. First, create a database named `blogs`.

1. Run the `CreateTable` script to generate the necessary tables in the `blogs` database.
2. Use the `SeedTable` script to populate the database with sample data.

### 2. **Configure Connection String**
- Connection string can be found right click on connection->properties
- Once the database is set up, update the connection string in the `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "ConnectionString": "your_connection_string_here"
  }
}

