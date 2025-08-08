# AAUP LabMaster System

**AAUP LabMaster** is a web-based application developed as a graduation project at An-Najah National University. It manages the reservation and supervision of laboratory equipment, helping students book available tools and allowing supervisors and admins to monitor and control lab assets efficiently.

---

## Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or later  
- SQL Server  
- Visual Studio 2022 (or any C#-compatible IDE)  
- [Node.js](https://nodejs.org/) (optional, for frontend scripts)

---

##  How to Run the Project

1. **Clone the repository:**

```bash
git clone https://github.com/your-username/AAUP_LabMaster.git
cd AAUP_LabMaster

Configure the database connection:

In appsettings.json, set your connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=LabMasterDB;Trusted_Connection=True;"
}


Apply database migrations:
dotnet ef database update

Run the project:
dotnet run


 Project Structure
Controllers/ → Request handlers (Account, Admin, User, Supervisor, etc.)

EntityManager/ → Business logic and data managers (e.g., BookingManager, LabManager)

EntityDTO/ → Data Transfer Objects

Models/ → EF Core entity classes

Views/ → Frontend Razor Views

wwwroot/ → Static frontend assets (CSS, JS, images)

Migrations/ → Database migrations



 Input / Output Overview
Users:

Input: Register, login, request equipment booking

Output: Booking status, equipment list

Supervisors:

Input: Add equipment, approve/reject bookings

Output: Usage reports, device availability

Admins:

Input: Manage users and devices

Output: Dashboard statistics and controls


 Notes
The system follows the MVC architecture: Model - View - Controller

Entity Framework Core is used for database operations

Supports future extensibility (email notifications, reporting, statistics, etc.)


Developer
Name: Thekra Jaradat

Field: Computer Science Graduate


