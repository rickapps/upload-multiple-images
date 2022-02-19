Instructions for UploadFilesMVC Project

First, right click on the Solution in Visual Studio Solution Explorer. Select 'Restore Client-Side Libraries' from the
drop-down menu. This will download a copy of Font-Awesome and add it to the folder wwwroot/lib. Look at the file
libman.json if you are curious how this is accomplished.

Now, before you can successfully run this website, you must create a SQL Server Express LocalDB Database.

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

You will need to log in to the Admin section to create new items and upload photos. The login credentials are stored in
appsettings.json. You can set a status for each item. Draft items are items not ready to be displayed. Active items are
displayed to all users. Archive items are items that have been sold or are no longer available and are not displayed.

Uploaded photos are renamed, copied, and resized. They are stored in wwwroot/pics/. A slideshow is displayed on the
item detail page for uploaded photos. 

Build the project and run. If you get Server 500 errors, make sure you have the latest version of Visual Studio 2022 installed.
You can also try changing the port number in Properties/launchSettings.json. If you have problems uploading photos, you will 
need to grant your web server write permission to folder wwwroot/pics/.