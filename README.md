# Digital Health Check

## Building

The solution can be built in Visual Studio 2019+ or using the `dotnet` commandline tool. `npm install` should be run in the **DigitalHealthCheckWeb** folder before building for the first time. After building the solution, also use `npm run build` to compile the js and css front-end resources using webpack, if they have changed.

### Prerequisites

This application should be built on the [long term support (LTS) build of Node 16](https://nodejs.org/en/).

This application requires the dotnet entity framework tools to be installed: `dotnet tool install --global dotnet-ef`

#### AES hex key

Currently the Aes hex key used in encrypting and decrypting the sensitive information that is bundled as part of the invite URL is hard-coded. As such, no key is provided in the source and you will need to generate a 256 bit hex key yourself. Replace the values of `AesHexKey` in GenerateLinks/Program.cs, LinkGeneratorClient/App.config, and DigitalHealthCheckWeb/Startup.cs to use the same generated value.

### Configuration to replace

The following configuration files will need updates to match your own infrastructure:

#### DigitalHealthCheckWeb/appsettings.json

**ConnectionStrings.DatabaseConnection** should point to your own Digital Health Check database. The default value provided will use an in-memory SQL server instance on your own machine, if one exists. 

> Note: This string is also used by DigitalHealthCheckEF for all Entity Framework operations. So update this string before trying to apply migrations.

**Mail** should use your email credentials. The implementation of this will vary depending on the mail server you use.

**Auth** is used to connect to an Azure Active Directory endpoint to verify users before they access the Reporting dashboard. Configure the *ClientSecret*, *ClientId*, *AuthEndpoint* and *TokenEndpoint* according to the [Microsoft Account external login setup guide](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-6.0)

**GoogleAnalyticsId** is the Google Analytics tag to use on the site

**EveryoneHealthReferralEmail** is the email address to send patient referrals to who have consented to referral to a follow up service.

**SupportEmail** is the email address to send error messages to as they happen.

#### UrlAvailabilityTests/appsettings.json

**ConnectionStrings.DatabaseConnection** should point to a database you intend to use for URL checking, such as a test database. The target database should be kept up to date using Entity Framework Migrations to ensure that the link checker's results are accurate.

#### LinkGeneratorClient/app.config

**NotifyAPI** is should point to your Gov.UK Notify API key to enable link dispatch using Gov.UK Notify. See [GOV.UK Notify's documention](https://www.notifications.service.gov.uk/documentation) for more information.

**NotifyTemplateId** should be the unique ID of the message template you want to send using Gov.UK Notify. See [GOV.UK Notify's documention](https://www.notifications.service.gov.uk/documentation) for more information.

**BitlyToken** should point to your Bitly API token for shortening the generated links. See [Bitly's API documentation](https://dev.bitly.com/) for more information.

**BitlyGroup** should point to the organisational group in Bitly that you will use for shortening. See [Bitly's API documentation](https://dev.bitly.com) for more information.

**UseProxy** Set to true if you're connecting to the Gov.UK Notify API through a proxy.

**MinimumPatientAge** Is used for validation to ensure patients that are too young for a health check are not invited.

**LinkFormat** Set this to a format string with two inputs that specifies the URL format to generate. *{0}* sets the patient's unique id, and *{1}* is the patient's encrypted hash. Typically you will set it to something like `https://<digital-health-check-url>/?id={0}&amp;hash={1}`

#### OneOffEmailDispatch/appsettings.json

**Mail** should use your email credentials. The implementation of this will vary depending on the mail server you use.

**ConnectionStrings.DatabaseConnection** should point to your own Digital Health Check database. The default value provided will use an in-memory SQL server instance on your own machine, if one exists. 

**BaseURL** should point to the URL of the Digital Health Check website's root page.

**EveryoneHealthReferralEmail** is the email address to send patient referrals to who have consented to referral to a follow up service.

#### DigitalHealthCheckService/appsettings.json

**TimerInterval** In milliseconds, determines how often the service runs.

**SupportEmail** is the email address to send error messages to as they happen.

**Mail** should use your email credentials. The implementation of this will vary depending on the mail server you use.

**PatientEmails** configures the task to send reminder emails to patients who have requested them.

* **StartTime** The earliest time in the day to run this task.
* **StopTime** The latest time in the day to run this task.
* **Enabled** Whether this task runs or not.
* **WebsiteBaseURL** should point to the URL of the Digital Health Check website's root page.
* **FirstReminderGap** How long after a check is completed should we wait before sending the first reminder.
* **SecondReminderGap** How long after a check is completed should we wait before sending the second reminder.
* **SecondSurveyGap** How long after a check is completed should we wait before sending an email about the patient's second survey.

**ThrivaNotificationEmail** configures the task to send notification emails about blood kit requests.

* **StartTime** The earliest time in the day to run this task.
* **StopTime** The latest time in the day to run this task.
* **Enabled** Whether this task runs or not.
* **WebsiteBaseURL** should point to the URL of the Digital Health Check website's root page.
* **Frequency** how often to run this task.
* **EmailAddress** the email address to dispatch notifications to.

**ConnectionStrings.DatabaseConnection** should point to a database you intend to use for URL checking, such as a test database. The target database should be kept up to date using Entity Framework Migrations to ensure that the link checker's results are accurate.

## Testing

### Unit Tests

Unit tests can be run from inside visual studio or through the `dotnet` commandline tool.

### GDS Component Tests

The tests in this project verify that its implementation of the GDS components matches their specifications. The fixtures are specified by the GDS team and are part of the npm package that is downloaded. As such, the npm packages must be installed before running these tests.

### End to End Tests

End to End tests are set up using Playwright operating inside of Jest. They can be run using `npm run test`. The End to End tests currently target a demo deployment of the site. You may need to change the URLs used to point to your own implementation.

### URL Availability Tests

The URLs used for Digital Health Check interventions are defined in **Database.cs**. This project tests against all of the interventions in a given database and verifies that a page loads when that URL is requested and that a non-error status code is returned.

Note that some websites return an error status code when automated systems access them. In these cases these tests will fail, but they will catch the majority of dead links.

These tests can be run in Visual Studio, via `dotnet`, or configured to run automatically in a build service of your choice.

## Structure

### BlazorComponentTests

Automated tests to ensure the Gov.UK design components conform to Gov.UK design specifications.

### DigitalHealthCheckCommon
This project is a class library containing code that is common to multiple projects in the solution, such as the encryption that is used in both the website and the link generator, as well as the risk score calculator.

### DigitalHealthCheckEF

This project houses the Entity Framework Core database definition and migrations. The database structure is quite simple. A brief description of each entity follows:

#### AnonymisedBarrier

This entity matches the return type of the AnonymisedBarriersReport table-valued function.

#### AnonymisedIntervention

This entity matches the return type of the AnonymisedInterventionsReport table-valued function.

#### AnonymisedPatient

This entity matches the return type of the AnonymisedDataset table-valed function.

#### BiometricAssessmentSuccessReport

This entity matches the return type of the BiometricAssessmentSuccessReport table-valued function.

#### DetailedUptakeReport

This entity matches the return type of the DetailedUptakeReport table-valued function.

#### InterventionEligibilityReport

This entity matches the return type of the InterventionEligibilityReport table-valued function.

#### Setting

This record holds site and service configuration settings that are updated during runtime.

#### HealthCheck

This record holds the measurements submitted by a patient as they navigate the Digital Health Check, as well as the results the application generates for them. 

The record is built up slowly over the course of the application, and it is expected that a patient can leave the site and come back at any time to continue where they left off. As such, almost all fields are nullable.

#### IdentityVariant

If a patient has undergone gender affirmation treatment, we offer them the choice to generate results with either male or female sex parameter, and to see both sets of results before proceeding with followups based on one set of results. The IdentityVariant contains the second set of results a patient chooses to generate. If they choose to proceed with their second set of results, that set becomes the primary health check, and the primary health check is stored in the IdentityVariant.

#### FollowUp

After a patient receives their results, they are shown a number of follow ups based on their preferred health priorities, followed by additional, optional priorities as determined based on their results. A patient will always see a minimum of two follow ups, and can choose to see more. For example, a patient with elevated blood pressure might be recommended a blood pressure follow up after their prioritised follow ups.

Each follow up page has a similar structure, and the results are stored in FollowUp entities that are linked to the main HealthCheck entity. Some FollowUps include a `SelectedItem`, which refers to a set of options that has been presented to them for addressing their health priority. One example, for those who do not know their blood pressure, is a list of options of where they might get their blood pressure checked. The chosen option is stored in `SelectedItem`. Not all FollowUps use this field.

#### Barrier

For each follow up, a patient will be shown a set of potential barriers they may experience in addressing that aspect of their health, such as a fear of doctors, or not having the right support. Each barrier that is shown is defined here.

#### CustomBarrier

IF a patient chooses to specify their own barrier during a follow up, it will be stored here.

#### Intervention

For each barrier, one or more interventions are designed to address the barrier for the patient. Each intervention is a URL to a resource. Some interventions have no barrier ids, they are designed to appear regardless of chosen barriers.

#### BloodKitRequest

If a patient does not know their blood pressure, cholesterol, or blood sugar, they will be offered a follow up to get those measured. One option is a home fingerprick blood kit. Any who request those have their requests stored here.

#### Townsend

This table holds Townsend deprivation index and IMD data for every postcode in the UK. Note that sometimes the Townsend or IMD score can be null.

#### ValidationError

When a user has an issue on the page due to validation, it is recorded here to help improve the site.




### DigitalHealthCheckService

This project is a background windows service, built using the [TopShelf](http://topshelf-project.com/) framework. 

It runs periodic reminder email dispatch and blood kit request notifications, but can be extended to run any background tasks needed.

### DigitalHealthCheckWeb
This project is the website itself, built using ASP.NET Core Razor Pages and ASP.NET Core Razor Components.

Razor Components are typically a part of the Blazor system, however - since one requirement was that the application function as well as it can without javascript - Blazor was not a suitable infrastructure. Nonetheless, Razor Components provide a huge benefit to development, allowing us to create and reuse subsections of the webpages and keeping the code for each page small and succinct.

A page accessable at /Item would have the following files associated with it:

* Components/Pages/ItemPage.razor - The Razor Component containing the full page layout.
* Pages/Item.cshtml - The Razor Page that responds to the /Item route. This markup creates a static `ItemPage` Razor Component and passes in data from the `ItemPageModel`
* Pages/Item.cshtml.cs - The `ItemPageModel` that handles GET and POST requests.

Most PageModels also contain two internal class definitions: `UnsanitisedModel` and `SanitisedModel`. The former encapsulates all the data sent to the PageModel in POST requests, and the latter is the same after `ValidateAndSanitize` has parsed each field for validity and converted the data from strings to the appropriate types.

As a user navigates through the Health Check process, a record is built up of their answers. This is used to generate a set of risk scores using the QRISK 3, QDiabetes 2018, GPPAQ, AUDIT, and GAD2 algorithms. After seeing their results, users are given the opportunity to navigate a series of recommended follow ups based on their results and priorities, and finally are sent a digest of their results and advice.

The reporting dashboard runs SQL Table-Valued Function reports, and exports them as CSV files (or TSV in the case of the Anonymised Dataset). It is secured using the [Azure Active Directory App Registrations](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins) system.

### GenerateLinks

This project contains a console application designed for generating unique invite URLs to the Digital Health Check website. For more information on its usage, see the GenerateLinks/Readme.md file.

Currently the Aes hex key used in encrypting the sensitive information that is bundled as part of the invite URL is hard-coded in the application in Program.cs. As such, no key is provided in the source and you will need to generate a 256 bit hex key yourself. Ensure this value matches the one in DigitalHealthCheckWeb/Startup.cs

### LinkGeneratorClient

This project contains a WPF application that acts as a GUI-based version of GenerateLinks. Additionally, it connects to a Bitly account to shorten the generated links. The input is a CSV file of patient information, and the output is the same file with an additional column for the shortened unique invite link.

### LinkGeneratorCommon

This project contains common code between the GenerateLinks and LinkGeneratorClient projects.

### OneOffEmailDispatch

A simple command line utility tool to send templated emails to patients.

### QRiskEstimator

This project contains a WPF application based off of LinkGeneratorClient that will estimate a QRISK 3 score for a batch of patients. The input is a CSV file of patient information, and the output is the same file with an additional column for estimated QRISK score.

### ServiceComponents

Razor components used by the DigitalHealthCheckService and OneOffEmailDispatch projects for rendering emails. This allows all the static Razor Component templating tools to be used to build emails.

## Deployment

Three Azure DevOps pipelines are included in the root of the application for producing Demo, Test, and Production releases. These use corresponding publish profiles for each major project.

### Website

The website requires no special configuration for deployment on any environment already configured to deploy ASP.NET Core 3 applications. While it has only been tested under a Windows hosting environment, it is possible it will work fine in other environments, but that is untested.

### Database

The database is defined using Entity Framework Core code-first migrations and can be deployed using any of the techniques EF Core supports.

### Service

The service can be deployed using the command line and the [Top Shelf command line arguments](http://docs.topshelf-project.com/en/latest/overview/commandline.html).

## Risk Score License

The CVD Risk Score algorithm this tool uses is the QRISK 3 CVD risk algorithm and is available via the GNU LGPLv3 license terms and can be found in the source at DigitalHealthCheckCommon/Risks/QRisk3.cs 

The diabetes risk score algorithm this tool uses is the QDiabetes 2018 risk algorithm and is available via the GNU Affero GPLv3 license terms and can be found in the source at DigitalHealthCheckCommon/Risks/QDiabetes.cs

We provide an interface for you to replace this implementation with your own version of the risk score calculator, `IRiskScoreCalculator`, and a stub for testing `StubRiskScoreCalculator` should you need to replace these implementations.

The initial version of this file, to be found at http://svn.clinrisk.co.uk/opensource/qrisk2, faithfully implements QRISK3-2017. ClinRisk Ltd. have released this code under the GNU Lesser General Public License to enable others to implement the algorithm faithfully. However, the nature of the GNU Lesser General Public License is such that we cannot prevent, for example, someone accidentally altering the coefficients, getting the inputs wrong, or just poor programming. ClinRisk Ltd. stress, therefore, that it is the responsibility of the end user to check that the source that they receive produces the same results as the original code found at https://qrisk.org.
Inaccurate implementations of risk scores can lead to wrong patients being given the wrong treatment.

The initial version of this file, to be found at http://qdiabetes.org, faithfully implements QDiabetes-2018. ClinRisk Ltd. have released this code under the GNU Affero General Public License to enable others to implement the algorithm faithfully. However, the nature of the GNU Affero General Public License is such that we cannot prevent, for example, someone accidentally altering the coefficients, getting the inputs wrong, or just poor programming. ClinRisk Ltd. stress, therefore, that it is the responsibility of the end user to check that the source that they receive produces the same results as the original code found at http://qdiabetes.org. Inaccurate implementations of risk scores can lead to wrong patients being given the wrong treatment.