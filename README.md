# SAOnlineMart

# Database and Migration Guide

## How to create the database and configure it

### 1. Create the database

1. In Microsoft Server Managment Studio create a new database.

### 2. Configure The Database Connection in Visual Studio

1. Open **Visual Studio**.
2. Find the **Server Explorer** panel.
3. Add a new **Data Connection**.
4. Change the data source to **Microsoft SQL Server**.
5. Enter your server name.
6. Select the database that was created
7. Click **Advanced**
8. Set **Trust Server Certificate** To `True`
9. Click **OK**
10. Click **OK** to complete the process

### 3. Update Connection String in `appsettings.json`

1. Open the `appsettings.json` file.
2. Set the `DefaultConnection` to the new database connection string

### 4. Run the database migrations
1.  Locate the `Package Manager Console`
2.  Input the following `dotnet ef database update`

### 5. In the event the migration fails (Optional)
1. Delete all the files in the `Migrations` folder
2. Input the following `dotnet ef migrations add InitialMigration` and press enter
3. Then input the following `dotnet ef database update` and press enter

### 6. Run the application
