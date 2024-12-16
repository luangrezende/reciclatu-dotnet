# ProjetoTU - Recycling Web Application

**Version:** 3.0  

## Overview

ProjetoTU is a web application developed as a final project at Universidade São Judas.  
Its primary goal is to assist businesses and partners in the proper collection of recyclable materials.

## Features

- **User Registration and Authentication:** Secure sign-up and login functionalities for businesses and partners.
- **Collection Scheduling:** Users can schedule pickups for recyclable materials.
- **Real-time Notifications:** Stay informed about upcoming collections and status updates.
- **Administrative Dashboard:** Manage users, schedules, and view analytics.

## Technologies Used

- **Frontend:**
  - HTML5, CSS3, JavaScript
  - [Bootstrap 4](https://getbootstrap.com/)
  - [jQuery](https://jquery.com/)
  - [Font Awesome](https://fontawesome.com/)

- **Backend:**
  - [ASP.NET MVC with Razor](https://dotnet.microsoft.com/apps/aspnet/mvc)
  - C#

- **Database:**
  - [SQL Server 2017](https://www.microsoft.com/en-us/sql-server/sql-server-2017)

- **Development Tools:**
  - [Microsoft Visual Studio 2019](https://visualstudio.microsoft.com/vs/older-downloads/)

- **Deployment:**
  - [Microsoft Azure - App Service](https://azure.microsoft.com/en-us/services/app-service/)

## Getting Started

To set up the project locally, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/luangrezende/reciclatu-dotnet-final.git
   ```

2. **Open the project in Visual Studio:**
   - Ensure you have the correct version of Visual Studio installed with ASP.NET development workload.

3. **Set up the database:**
   - Create a SQL Server database.
   - Update the connection string in the `web.config` file to match your database credentials.

4. **Run the project:**
   - Build and run the application using Visual Studio.
   - Access the app at `http://localhost:xxxx` (replace `xxxx` with the port number shown in Visual Studio).

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
