Before you can successfully run this website, you must create a SQL Server Express LocalDB Database.

1). From the Visual Studio 2022 menu, select:
Tools->NuGet Package Manager->Package Manager Console
2). Enter the following two commands:
Add-Migration Initial
Update-Database

Add-Migrations will add a Migrations Folder to the project that contains instructions on creating the database tables.
Update-Database will create the database file specified in appsettings.json. If you do not alter this file, the command
will generate the file C:\Users\<username>\RickAppsUploadFilesMVC.mdf. It will run the commands in the migration file to
generate the tables and relations.

If you want to use a database other than LocalDB, create a new connection string in appsettings.json and edit Startup.cs. 