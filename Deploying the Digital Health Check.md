# Deploying the Digital Health Check from Scratch

## Infrastructure

* Create a new resource group in Azure

* Create a Virtual Machine:
    * Windows Server 2019 Datacenter - Gen2
    * Recommended size: D2s_v3 with a Premium SSD
    * Enable inbound ports 80 and 443 (and keep RDP enabled)
    * Create a DNS name for it

* Create a SQL Server and Database:
    * Recommended size: 1GB / 10 DTUs or, if using vCore, disable auto-pause
    * Create a private endpoint connection, integrating with the Private DNS Zone

* Create a new Azure Active Directory App Registration for the website's reporting page.
    * Set the web redirect URI to be the application's web address with /signin-microsoft on the end
        * e.g. <digitalHealthCheckURL>/DigitalHealthCheck/signin-microsoft
    * Create a client secret for accessing the reporting page and note the credentials


## Configuration

### Web server

* Add the Web Server (IIS) feature
* Install the [ASP.NET Core v5 hosting module](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-5.0.16-windows-hosting-bundle-installer)
* Create local user accounts
    * For the website
        * Add a local user for the website
    * Add an equivalent user as a login on the SQL Server instance
        * Add the user to the database and give it the datareader and datawriter roles.
    * For the service
        * Add a local user for the service
* Create an app pool for the website
    * CLR version 4.0
    * Login using the local user just created

### SQL Server

* Add an equivalent user for the website and server as a login on the SQL Server
    * Add the users to the database and give them the *db_datareader* and *db_datawriter* roles.    

## Build the software

### Update Configuration

* In source control, update the *appsettings.json* file found in the *DigitalHealthCheckWeb* folder
    * Replace the *SupportEmail* value with the email address you wish to use to be notified about errors
    * Replace the *Mail* values with the mail server you use.
    * Replace the *Auth.ClientSecret* and *Auth.ClientId* values with the ones created for the Active Directory App Registration created earlier.
    * Replace the *Auth.AuthEndpoint* and *Auth.TokenEndpoint* values with the *OAuth v2 authorization* and *OAuth v2 token endpoints* found by pressing the *Endpoints* button on the App Registration created earlier.
* In source control, update the *appsettings.Production.json* file found in the *DigitalHealthCheckWeb* folder.
    * Replace the *DatabaseConnection* value with the connection string from your new database using the website user credentials you created in SQL server.
* In source control, update the *appsettings.json* file found in the *DigitalHealthCheckService* folder
    * Replace the *SupportEmail* value with the email address you wish to use to be notified about errors
    * Replace the *Mail* values with the mail server you use.
* In source control, update the *appsettings.Production.json* files found in the *DigitalHealthCheckService* folder.
    * Replace the *DatabaseConnection* value with the connection string from your new database using the service user credentials you created in SQL server.
    * Replace the *WebsiteBaseUrl* fields with the root web address of the Digital Health Check website
    * Replace the *ThrivaNotificationEmail.EmailAddress* field with an email address you wish to receive notifications about bloodkit requests (does not contain patient information)
    * Replace the *Serilog.WriteTo.Args.Path* field with the absolute file path of your logs directory.

### Install dependencies

This tool requires the following to be installed: 
* [The dotnet command line tool](https://docs.microsoft.com/en-us/dotnet/core/tools/), 
* [npm](https://nodejs.org/en/), 
* [Entity framework core](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

### Publish

The software can be built with visual studio or with the `dotnet` command line tool. Publish Profiles are included in source control for folder builds in Test, Demo, and Production configurations. Commands for building the production version of each tool are included below:

```
dotnet restore
cd DigitalHealthCheckWeb
npm install
npm run build
cd ../
dotnet publish DigitalHealthCheckWeb\DigitalHealthCheckWeb.csproj -p:PublishProfile=Production -c Release -o publish
dotnet publish DigitalHealthCheckService\DigitalHealthCheckService.csproj -p:PublishProfile=Production -c Release -o publish
dotnet publish LinkGeneratorClient\LinkGeneratorClient.csproj -p:PublishProfile=Production -c Release -o publish
dotnet publish GenerateLinks\GenerateLinks.csproj -p:PublishProfile=Production -c Release -o publish
```

***Note:*** If you use Azure DevOps, there are three pipeline files included in source control. All three of these pipelines will build the software for you.

* **azure-pipelines.yml** publishes test artifacts
* **release-demo-pipeline.yml** publishes demo artifacts
* **release-pipeline.yml** published production artifacts

### Generate migrations

Use the dotnet CLI to generate migrations. The command to do this is:

`dotnet ef migrations script --idempotent -o migrations.sql`

This command should be run from within the *DigitalHealthCheckEF* folder in source control. This will create a *migrations.sql* file in the *DigitalHealthCheckEF* directory that can be run on an empty database to bring it up to the latest schema.

## Deploy the software

### Database

* Run the contents of the *migrations.sql* file on the database.
* Load the contents of the file *TownsendData.csv* from the *DigitalHealthCheckEF* folder in source control into the Townsend table on the database.

### Website

* Create a new web site with the app pool created earlier.
    * Extract the contents of the published website artifacts into this website's folder.
    * Give the user account you created for the website access to read and write to this folder (for logging purposes)

### Service

* Extract the contents of the published service artifacts into the folder you wish to run the service from
    * Run the following command as administrator, in the same folder: `DigitalHealthCheckService.exe install`
    * In *services.msc*, find the new *NHS Digital Health Check Background Service* and set it to log on using the local user you created for the service.
    * Give the user account you created for the service access to read and write to this folder (for logging purposes)