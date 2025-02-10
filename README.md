# Task Prioritization API

A comprehensive API designed to manage tasks with features like adding, removing, updating, filtering, and sorting. Built using ASP.NET Core MVC, with a service layer for business logic and MSSQL Cloud Container Database for data storage.

## Features
- **Task Management**: Add, update, delete tasks.
- **Sorting & Filtering**: Sort tasks by priority, due date, or other criteria. Filter tasks based on status or category.
- **Business Logic**: Service layer for managing business rules and task prioritization.
- **Cloud Database**: MSSQL Cloud Container for scalable and reliable storage.

## Technologies Used
- ASP.NET Core MVC (Model-View-Controller)
- MSSQL Cloud Database
- Service Layer for Business Logic
- Entity Framework Core (ORM)

## Getting Started

### Prerequisites
- .NET Core SDK (>= 6.0)
- SQL Server (MSSQL) or access to a Cloud Docker MSSQL Container

### Future Improvements
- Add user authentication and authorization
- Implement task categorization and labels
- Introduce a priority calculation algorithm with more sophisticated rules

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/MaximVanchev/TaskPrioritizationAPI.git
