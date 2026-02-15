# 🎓 University Management System – Backend (.NET Core 8, Clean Architecture)

The University Management System is a lightweight backend solution designed to manage core academic data within a university environment.
The project is built entirely with C# .NET Core 8 and structured using Clean Architecture, ensuring scalability, maintainability, and a clear separation of concerns.

This backend exposes RESTful APIs that allow the management of students, courses, instructors, departments, and enrollment operations.

# 🚀 Technologies & Architecture

Technology Stack

	•	C# .NET Core 8 Web API
	•	Entity Framework Core
	•	Clean Architecture
	•	SQL Database (configurable: SQL Server, PostgreSQL, etc.)
	•	JWT Authentication (optional depending on project scope)
	•	AutoMapper (DTO mappings)

# Clean Architecture Layers

The project follows a layered architectural pattern: 

   Application/
   
      ├── Interfaces
      ├── Services (Use Cases)
      ├── DTOs
  Domain/
  
      ├── Entities
      ├── Value Objects
  Infrastructure/
  
      ├── EF Core Context
      ├── Repositories
  API/
  
      ├── Controllers
      ├── Endpoints
      ├── Configuration

  Benefits:
  
	•	High testability
	•	Low coupling between modules
	•	Scalable structure suitable for future expansion

# 📘 Core Features

Although the project is intentionally small, it supports essential academic management operations:

# 👨‍🎓 Student Management
	•	Create, update, delete students
	•	Retrieve student details
	•	List all students

# 📚 Course Management
	•	Create and manage courses
	•	Assign instructors to courses
	•	Set course capacity or credit information

# 🧑‍🏫 Instructor Management
	•	Register and update instructor profiles
	•	Link instructors to departments and courses

# 🏢 Department Management
	•	Add and manage academic departments
	•	Associate instructors and courses to departments

# 📝 Enrollment Management
	•	Enroll students in courses
	•	Track student course participation
	•	Prevent duplicate enrollments

