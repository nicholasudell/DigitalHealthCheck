using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DigitalHealthCheckEF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FollowUp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeReminded = table.Column<bool>(type: "bit", nullable: true),
                    ConfidentToChange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetADate = table.Column<bool>(type: "bit", nullable: true),
                    DoYouHavePlans = table.Column<bool>(type: "bit", nullable: true),
                    ImportantToChange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedOption = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AUDIT = table.Column<int>(type: "int", nullable: true),
                    MSASQ = table.Column<int>(type: "int", nullable: true),
                    GAD2 = table.Column<int>(type: "int", nullable: true),
                    GPPAQ = table.Column<int>(type: "int", nullable: true),
                    HeartAge = table.Column<int>(type: "int", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: true),
                    PolycysticOvaries = table.Column<bool>(type: "bit", nullable: true),
                    QDiabetes = table.Column<double>(type: "float", nullable: true),
                    QRisk = table.Column<double>(type: "float", nullable: true),
                    GestationalDiabetes = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityVariants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Townsend",
                columns: table => new
                {
                    Postcode = table.Column<string>(type: "varchar(8)", nullable: false),
                    Lsoa = table.Column<string>(type: "varchar(12)", nullable: true),
                    Townsend = table.Column<decimal>(type: "decimal(18,13)", nullable: true),
                    IMDQuintile = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Townsend", x => x.Postcode);
                });

            migrationBuilder.CreateTable(
                name: "Interventions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarrierId = table.Column<int>(type: "int", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interventions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interventions_Barriers_BarrierId",
                        column: x => x.BarrierId,
                        principalTable: "Barriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Anxious = table.Column<int>(type: "int", nullable: true),
                    AtrialFibrillation = table.Column<bool>(type: "bit", nullable: true),
                    HealthCheckCompleted = table.Column<bool>(type: "bit", nullable: false),
                    HeightAndWeightUpdated = table.Column<bool>(type: "bit", nullable: false),
                    HeightAndWeightUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BloodSugarUpdated = table.Column<bool>(type: "bit", nullable: false),
                    BloodSugarUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BloodPressureUpdated = table.Column<bool>(type: "bit", nullable: false),
                    BloodPressureUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CholesterolUpdated = table.Column<bool>(type: "bit", nullable: false),
                    CholesterolUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HealthCheckCompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AtypicalAntipsychoticMedication = table.Column<int>(type: "int", nullable: true),
                    AUDIT = table.Column<int>(type: "int", nullable: true),
                    BloodPressureTreatment = table.Column<bool>(type: "bit", nullable: true),
                    BloodSugar = table.Column<float>(type: "real", nullable: true),
                    VariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GenderAffirmation = table.Column<bool>(type: "bit", nullable: true),
                    ChronicKidneyDisease = table.Column<bool>(type: "bit", nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactsConcernedByDrinking = table.Column<int>(type: "int", nullable: true),
                    Control = table.Column<int>(type: "int", nullable: true),
                    Cycling = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiastolicBloodPressure = table.Column<int>(type: "int", nullable: true),
                    Disinterested = table.Column<bool>(type: "bit", nullable: true),
                    DrinkingFrequency = table.Column<int>(type: "int", nullable: true),
                    DrinksAlcohol = table.Column<bool>(type: "bit", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ethnicity = table.Column<int>(type: "int", nullable: true),
                    FailedResponsibilityDueToAlcohol = table.Column<int>(type: "int", nullable: true),
                    FamilyHistoryCVD = table.Column<bool>(type: "bit", nullable: true),
                    FamilyHistoryDiabetes = table.Column<bool>(type: "bit", nullable: true),
                    FeelingDown = table.Column<bool>(type: "bit", nullable: true),
                    FirstHealthPriority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstHealthPriorityAfterResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowUpAddressCholesterol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CholesterolFollowUpId = table.Column<int>(type: "int", nullable: true),
                    BloodPressureFollowUpId = table.Column<int>(type: "int", nullable: true),
                    BloodSugarFollowUpId = table.Column<int>(type: "int", nullable: true),
                    BookGPAppointmentFollowUpId = table.Column<int>(type: "int", nullable: true),
                    DrinkLessFollowUpId = table.Column<int>(type: "int", nullable: true),
                    HealthyWeightFollowUpId = table.Column<int>(type: "int", nullable: true),
                    ImproveBloodPressureFollowUpId = table.Column<int>(type: "int", nullable: true),
                    ImproveCholesterolFollowUpId = table.Column<int>(type: "int", nullable: true),
                    MentalWellbeingFollowUpId = table.Column<int>(type: "int", nullable: true),
                    ImproveBloodSugarFollowUpId = table.Column<int>(type: "int", nullable: true),
                    QuitSmokingFollowUpId = table.Column<int>(type: "int", nullable: true),
                    MoveMoreFollowUpId = table.Column<int>(type: "int", nullable: true),
                    HeightAndWeightFollowUpId = table.Column<int>(type: "int", nullable: true),
                    GAD2 = table.Column<int>(type: "int", nullable: true),
                    Gardening = table.Column<int>(type: "int", nullable: true),
                    CustomIdentity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GestationalDiabetes = table.Column<bool>(type: "bit", nullable: true),
                    GPEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPPAQ = table.Column<int>(type: "int", nullable: true),
                    GPSurgery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuiltAfterDrinking = table.Column<int>(type: "int", nullable: true),
                    HdlCholesterol = table.Column<float>(type: "real", nullable: true),
                    HeartAge = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: true),
                    Housework = table.Column<int>(type: "int", nullable: true),
                    InjuryCausedByDrinking = table.Column<int>(type: "int", nullable: true),
                    KnowYourBloodPressure = table.Column<int>(type: "int", nullable: true),
                    KnowYourCholesterol = table.Column<int>(type: "int", nullable: true),
                    KnowYourHbA1c = table.Column<int>(type: "int", nullable: true),
                    MemoryLossAfterDrinking = table.Column<int>(type: "int", nullable: true),
                    Migraines = table.Column<bool>(type: "bit", nullable: true),
                    MSASQ = table.Column<int>(type: "int", nullable: true),
                    NeededToDrinkAlcoholMorningAfter = table.Column<int>(type: "int", nullable: true),
                    NHSNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicalActivity = table.Column<int>(type: "int", nullable: true),
                    PolycysticOvaries = table.Column<bool>(type: "bit", nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrefersImperialHeightAndWeight = table.Column<bool>(type: "bit", nullable: true),
                    QDiabetes = table.Column<double>(type: "float", nullable: true),
                    QRisk = table.Column<double>(type: "float", nullable: true),
                    RheumatoidArthritis = table.Column<bool>(type: "bit", nullable: true),
                    SecondHealthPriorityAfterResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SevereMentalIllness = table.Column<int>(type: "int", nullable: true),
                    SexAtBirth = table.Column<int>(type: "int", nullable: true),
                    SexForResults = table.Column<int>(type: "int", nullable: true),
                    SmokingStatus = table.Column<int>(type: "int", nullable: true),
                    SmsNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Steroids = table.Column<bool>(type: "bit", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemicLupusErythematosus = table.Column<int>(type: "int", nullable: true),
                    SystolicBloodPressure = table.Column<int>(type: "int", nullable: true),
                    TotalCholesterol = table.Column<float>(type: "real", nullable: true),
                    TypicalDayAlcoholUnits = table.Column<int>(type: "int", nullable: true),
                    UnableToStopDrinking = table.Column<int>(type: "int", nullable: true),
                    SkipMentalHealthQuestions = table.Column<bool>(type: "bit", nullable: true),
                    UnderCare = table.Column<bool>(type: "bit", nullable: true),
                    ValidationDateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidationOverwritten = table.Column<bool>(type: "bit", nullable: false),
                    ValidationPostcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Walking = table.Column<int>(type: "int", nullable: true),
                    WalkingPace = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    WorkActivity = table.Column<int>(type: "int", nullable: true),
                    CalculatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EveryoneHealthConsent = table.Column<bool>(type: "bit", nullable: true),
                    PrefersToContactEveryoneHealth = table.Column<bool>(type: "bit", nullable: true),
                    ReminderStatus = table.Column<int>(type: "int", nullable: false),
                    LastContactDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClickedStartNow = table.Column<bool>(type: "bit", nullable: false),
                    CheckStartedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InitialSexForResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WentToFindBloodPressure = table.Column<bool>(type: "bit", nullable: false),
                    WentToFindBloodSugar = table.Column<bool>(type: "bit", nullable: false),
                    WentToFindCholesterol = table.Column<bool>(type: "bit", nullable: false),
                    SubmittedCheckYourAnswers = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedHealthCheckIncompleteEmail = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedHealthCheckCompleteEmail = table.Column<bool>(type: "bit", nullable: false),
                    EveryoneHealthReferralSent = table.Column<bool>(type: "bit", nullable: false),
                    GPEmailSent = table.Column<bool>(type: "bit", nullable: false),
                    CompletePageSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    ContactInformationUpdatedOnComplete = table.Column<bool>(type: "bit", nullable: false),
                    HeightAndWeightSkipped = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_BloodPressureFollowUpId",
                        column: x => x.BloodPressureFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_BloodSugarFollowUpId",
                        column: x => x.BloodSugarFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_BookGPAppointmentFollowUpId",
                        column: x => x.BookGPAppointmentFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_CholesterolFollowUpId",
                        column: x => x.CholesterolFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_DrinkLessFollowUpId",
                        column: x => x.DrinkLessFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_HealthyWeightFollowUpId",
                        column: x => x.HealthyWeightFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_HeightAndWeightFollowUpId",
                        column: x => x.HeightAndWeightFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_ImproveBloodPressureFollowUpId",
                        column: x => x.ImproveBloodPressureFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_ImproveBloodSugarFollowUpId",
                        column: x => x.ImproveBloodSugarFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_ImproveCholesterolFollowUpId",
                        column: x => x.ImproveCholesterolFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_MentalWellbeingFollowUpId",
                        column: x => x.MentalWellbeingFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_MoveMoreFollowUpId",
                        column: x => x.MoveMoreFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_FollowUp_QuitSmokingFollowUpId",
                        column: x => x.QuitSmokingFollowUpId,
                        principalTable: "FollowUp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HealthChecks_IdentityVariants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "IdentityVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BarrierHealthCheck",
                columns: table => new
                {
                    ChosenBarriersId = table.Column<int>(type: "int", nullable: false),
                    HealthChecksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarrierHealthCheck", x => new { x.ChosenBarriersId, x.HealthChecksId });
                    table.ForeignKey(
                        name: "FK_BarrierHealthCheck_Barriers_ChosenBarriersId",
                        column: x => x.ChosenBarriersId,
                        principalTable: "Barriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarrierHealthCheck_HealthChecks_HealthChecksId",
                        column: x => x.HealthChecksId,
                        principalTable: "HealthChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BloodKitRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateRequested = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodKitRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodKitRequests_HealthChecks_CheckId",
                        column: x => x.CheckId,
                        principalTable: "HealthChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomBarriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthCheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomBarriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomBarriers_HealthChecks_HealthCheckId",
                        column: x => x.HealthCheckId,
                        principalTable: "HealthChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HealthCheckIntervention",
                columns: table => new
                {
                    ChosenInterventionsId = table.Column<int>(type: "int", nullable: false),
                    HealthChecksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheckIntervention", x => new { x.ChosenInterventionsId, x.HealthChecksId });
                    table.ForeignKey(
                        name: "FK_HealthCheckIntervention_HealthChecks_HealthChecksId",
                        column: x => x.HealthChecksId,
                        principalTable: "HealthChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthCheckIntervention_Interventions_ChosenInterventionsId",
                        column: x => x.ChosenInterventionsId,
                        principalTable: "Interventions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValidationErrors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthCheckId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Page = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorControl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationErrors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationErrors_HealthChecks_HealthCheckId",
                        column: x => x.HealthCheckId,
                        principalTable: "HealthChecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Barriers",
                columns: new[] { "Id", "Category", "Order", "Text" },
                values: new object[,]
                {
                    { 1, "smoking", 1, "All of my friends smoke" },
                    { 107, "improvebloodpressure", 8, "I do not know how to track what I am eating" },
                    { 106, "improvebloodpressure", 7, "I do not have time to be physically active" },
                    { 105, "improvebloodpressure", 6, "I do not know how to cut down my salt intake" },
                    { 104, "improvebloodpressure", 5, "I know that a lack of sleep can impact blood pressure but I have difficulty sleeping" },
                    { 103, "improvebloodpressure", 4, "I know that I should cut down on smoking to help lower my blood pressure, but I do not have the will power or motivation to quit" },
                    { 102, "improvebloodpressure", 3, "I do not have the will power to make healthy lifestyle changes that will help lower my blood pressure" },
                    { 101, "improvebloodpressure", 2, "I do not see how moving more can help me to be healthier" },
                    { 100, "improvebloodpressure", 1, "I do not know what lifestyle changes I can make to lower my blood pressure" },
                    { 99, "improvecholesterol", 8, "I know that I should cut down on smoking to help lower my cholesterol, but I do not have the will power or motivation to quit" },
                    { 97, "improvecholesterol", 7, "I know that drinking less alcohol can help lower my cholesterol, but I do not have opportunities to socialise that do not include alcohol" },
                    { 96, "improvecholesterol", 6, "I do not know what lifestyle changes I can make to lower my cholesterol " },
                    { 95, "improvecholesterol", 5, "I do not believe that my cholesterol levels are bad for my health " },
                    { 108, "improvebloodpressure", 9, "I need to lose weight to help lower my blood pressure, but I do not know how to" },
                    { 94, "improvecholesterol", 4, "I feel stressed which impacts my unhealthy lifestyle habits" },
                    { 92, "improvecholesterol", 2, "I do not have time to be physically active" },
                    { 91, "improvecholesterol", 1, "I do not know what foods I should eat or avoid that will help lower my cholesterol" },
                    { 90, "gp", 8, "I do not want to overstretch the NHS" },
                    { 89, "gp", 7, "I am afraid (of the results)" },
                    { 88, "gp", 6, "I do not like doctors" },
                    { 87, "gp", 5, "I have mobility limitations" },
                    { 85, "gp", 3, "I do not have time" },
                    { 84, "gp", 2, "I do not think it will do me any good" },
                    { 83, "gp", 1, "I do not see the point" },
                    { 81, "mental", 21, "I need help coping with bereavement" },
                    { 79, "mental", 20, "I am struggling with money issues" },
                    { 78, "mental", 19, "I am struggling to afford food" },
                    { 93, "improvecholesterol", 3, "I do not know how to get started to moving more" },
                    { 77, "mental", 18, "I am struggling with my bills" },
                    { 109, "improvebloodpressure", 10, "I do not have anyone to eat more healthily and become more active with" },
                    { 111, "improvebloodsugar", 2, "I do not see why I should be concerned about my blood sugar levels" },
                    { 140, "bloodpressure", 7, "I am afraid of my blood pressure results" },
                    { 138, "bloodpressure", 6, "I have mobility limitations" },
                    { 137, "bloodpressure", 5, "It is hard to get an appointment at the GP clinic to get my blood pressure measured" },
                    { 136, "bloodpressure", 4, "I do not know where I can get my blood pressure measured" },
                    { 135, "bloodpressure", 3, "I do not have time" },
                    { 134, "bloodpressure", 2, "I do not think it will do me any good" },
                    { 133, "bloodpressure", 1, "I do not see the point" },
                    { 132, "bloodsugar", 6, "I am afraid of needles" },
                    { 131, "bloodsugar", 5, "I am afraid of the results from my blood test" },
                    { 129, "bloodsugar", 4, "I have mobility limitations" },
                    { 128, "bloodsugar", 3, "It is hard to get an appointment at the GP clinic to get my blood sugar measured" }
                });

            migrationBuilder.InsertData(
                table: "Barriers",
                columns: new[] { "Id", "Category", "Order", "Text" },
                values: new object[,]
                {
                    { 127, "bloodsugar", 2, "I do not have time" },
                    { 110, "improvebloodsugar", 1, "I do not think that I can eat healthily" },
                    { 126, "bloodsugar", 1, "I do not see the point" },
                    { 124, "cholesterol", 5, "I am afraid of the results from my blood test" },
                    { 122, "cholesterol", 4, "I have mobility limitations" },
                    { 121, "cholesterol", 3, "It is hard to get an appointment at the GP clinic to get my cholesterol measured" },
                    { 120, "cholesterol", 2, "I do not have time" },
                    { 119, "cholesterol", 1, "I do not see the point" },
                    { 118, "improvebloodsugar", 9, "I do not have anyone to eat more healthily and become more active with" },
                    { 117, "improvebloodsugar", 8, "I cannot tell how much sugar is in the foods that I buy" },
                    { 116, "improvebloodsugar", 7, "I do not have access to a gym or swimming facilities to help me move more" },
                    { 115, "improvebloodsugar", 6, "I do not know how to move more" },
                    { 114, "improvebloodsugar", 5, "I do not believe that I am capable of moving more" },
                    { 113, "improvebloodsugar", 4, "I do not know how to track what I am eating" },
                    { 112, "improvebloodsugar", 3, "I comfort eat when I am feeling low, anxious or stressed" },
                    { 125, "cholesterol", 6, "I am afraid of needles" },
                    { 76, "mental", 17, "As a carer who looks after the needs of someone else, I am struggling to look after my own wellbeing" },
                    { 86, "gp", 4, "It is hard to get an appointment" },
                    { 73, "mental", 15, "I am struggling with employment problems" },
                    { 29, "move", 6, "I do not know how to run" },
                    { 28, "move", 5, "I do not know what healthy levels of exercise are" },
                    { 27, "move", 4, "I can't afford to pay for exercise activities " },
                    { 26, "move", 3, "I have a health condition which makes it difficult to build physical activity into my life" },
                    { 25, "move", 2, "I have no one to exercise with" },
                    { 74, "mental", 16, "I am struggling with childcare problems" },
                    { 23, "weight", 13, "I do not believe that I am good at sport" },
                    { 22, "weight", 12, "I do not know how to track and build up my activity levels  " },
                    { 21, "weight", 11, "I do not know how to track what I am eating" },
                    { 20, "weight", 10, "I do not know how to get started on moving more" },
                    { 19, "weight", 9, "I am worried people at what people might think of me if I try to become more active or attempt to eat more healthily" },
                    { 18, "weight", 8, "I do not see how moving more can help me to be healthier" },
                    { 30, "move", 7, "I do not know what support programmes are available to suit my level of activity" },
                    { 17, "weight", 7, "I do not have access to a gym or swimming facilities to help me move more" },
                    { 15, "weight", 5, "I do not have access to healthy food" },
                    { 14, "weight", 4, "I cannot afford to eat well" },
                    { 13, "weight", 3, "I do not have anyone to become more healthy with" },
                    { 12, "weight", 2, "I do not know how to lose weight" },
                    { 11, "weight", 1, "I do not know what a healthy diet involves" },
                    { 10, "smoking", 8, "I have strong nicotine cravings" },
                    { 9, "smoking", 7, "I do not see the benefit of cutting down or quitting smoking" },
                    { 8, "smoking", 6, "I do not have the will power/motivation to quit smoking" },
                    { 7, "smoking", 5, "I have tried to quit smoking before and failed" }
                });

            migrationBuilder.InsertData(
                table: "Barriers",
                columns: new[] { "Id", "Category", "Order", "Text" },
                values: new object[,]
                {
                    { 6, "smoking", 4, "I enjoy smoking/I use smoking to manage stress" },
                    { 3, "smoking", 3, "I do not know how to quit" },
                    { 2, "smoking", 2, "I do not have access to smoking cessation aids (e.g. nicotine patches, vaping)" },
                    { 16, "weight", 6, "I feel stressed which impacts my unhealthy eating habits " },
                    { 32, "move", 9, "I do not know how to get started in moving more" },
                    { 24, "move", 1, "I do not have the motivation or will power to move more" },
                    { 34, "move", 11, "I do not see how moving more can help me to be healthier" },
                    { 71, "mental", 14, "I am struggling with housing issues" },
                    { 70, "mental", 13, "I do not know where to access mental health support services that can help me to manage my anxiety and depression and which address topics such as discrimination, bereavement and coping during the pandemic" },
                    { 68, "mental", 12, "I am struggling with my gender identity or sexual orientation, including discrimination" },
                    { 67, "mental", 11, "I do not have the skills to alter my ways of negative thinking" },
                    { 65, "mental", 10, "I cannot find ways to express myself creatively" },
                    { 33, "move", 10, "I am worried people will laugh at me if I try to become more active" },
                    { 59, "mental", 8, "I do not have anyone to connect with that can relate to my unique experiences" },
                    { 57, "mental", 7, "I do not have any tools to help me relax" },
                    { 55, "mental", 6, "I have difficulty sleeping which affects my mental wellbeing" },
                    { 53, "mental", 5, "I do not know what physical activities I can do to help improve my mood" },
                    { 50, "mental", 4, "I need support that fits around my needs and busy lifestyle" },
                    { 49, "mental", 3, "I do not believe that physical exercise can improve my mental wellbeing" },
                    { 48, "mental", 2, "I do not know where to access information and support on mental wellbeing services in Southwark" },
                    { 62, "mental", 9, "I feel lonely and socially isolated" },
                    { 46, "alcohol", 9, "I cannot stop drinking even if I want to " },
                    { 47, "mental", 1, "I need to speak with someone to help improve my mental wellbeing" },
                    { 35, "move", 12, "I do not have access to a gym or swimming facilities to help me move more" },
                    { 36, "move", 13, "I do not believe that I am good at sport" },
                    { 38, "alcohol", 1, "I do not think the amount I am drinking is bad for my health" },
                    { 39, "alcohol", 2, "I am not sure of the health impact of excessive drinking" },
                    { 40, "alcohol", 3, "I do not know how to cut down my alcohol intake" },
                    { 37, "move", 14, "I do not have time to be physically active" },
                    { 41, "alcohol", 4, "I do not know how to meet more people and socialise in a way that doesn't involve alcohol" },
                    { 42, "alcohol", 5, "I do not have friends or family that I can talk to that understand my drinking difficulties" },
                    { 43, "alcohol", 6, "I use drinking to manage stress" },
                    { 44, "alcohol", 7, "There is often alcohol in my home" },
                    { 45, "alcohol", 8, "My mental wellbeing impacts my drinking habits" }
                });

            migrationBuilder.InsertData(
                table: "Interventions",
                columns: new[] { "Id", "BarrierId", "Category", "LinkDescription", "LinkTitle", "Text", "Url" },
                values: new object[,]
                {
                    { 56, null, "alcohol", "Southwark Healthy Lifestyle Hub for services to help manage weight, diet, physical activity, mental wellbeing, alcohol intake and/or smoking habits", "Southwark Healthy Lifestyle Hub ", "Direct me to a free telephone consultation for mental wellbeing services through Southwark's Healthy Lifestyle Hub", "https://southwark.everyonehealth.co.uk/" },
                    { 124, null, "improvebloodpressure", "Southwark Healthy Lifestyle Hub for services to help manage weight, diet, physical activity, mental wellbeing, alcohol intake and/or smoking habits", "Southwark Healthy Lifestyle Hub ", "Direct me to a free telephone consultation for physical activity, weight management and/or smoking cessation services through Southwark's Healthy Lifestyle Hub", "https://southwark.everyonehealth.co.uk/" },
                    { 123, null, "improvebloodpressure", "Southwark's digital health and wellbeing coach", "Southwark Health Coach App", "Direct me to Southwark's digital health and wellbeing coach", "https://southwark.health-coach.app/" },
                    { 112, null, "improvecholesterol", "Southwark Healthy Lifestyle Hub for services to help manage weight, diet, physical activity, mental wellbeing, alcohol intake and/or smoking habits", "Southwark Healthy Lifestyle Hub ", "Direct me to a free telephone consultation for physical activity, weight management and/or smoking cessation services through Southwark's Healthy Lifestyle Hub", "https://southwark.everyonehealth.co.uk/" },
                    { 79, null, "mental", "Southwark's digital health and wellbeing coach", "Southwark Health Coach App", "Direct me to Southwark's digital health and wellbeing coach", "https://southwark.health-coach.app/" },
                    { 60, null, "mental", "Southwark Healthy Lifestyle Hub for services to help manage weight, diet, physical activity, mental wellbeing, alcohol intake and/or smoking habits", "Southwark Healthy Lifestyle Hub", "Direct me to a free telephone consultation for mental wellbeing services through Southwark's Healthy Lifestyle Hub", "https://southwark.everyonehealth.co.uk/" },
                    { 125, null, "improvebloodsugar", "NHS Diabetes Prevention Programme", "", "Direct me to the NHS Diabetes Prevention Programme self-referral registration page", "https://preventing-diabetes.co.uk/self-referral/" },
                    { 55, null, "alcohol", "Southwark's digital health and wellbeing coach", "Southwark Health Coach App", "Direct me to Southwark's digital health and wellbeing coach", "https://southwark.health-coach.app/" }
                });

            migrationBuilder.InsertData(
                table: "Interventions",
                columns: new[] { "Id", "BarrierId", "Category", "LinkDescription", "LinkTitle", "Text", "Url" },
                values: new object[,]
                {
                    { 24, null, "weight", "Southwark healthy weight programme", "Southwark healthy weight programme", "Send me a link to holistic healthy weight services for people with a BMI 30+", "https://www.guysandstthomas.nhs.uk/our-services/nutrition-and-dietetics/south-east-London-healthy-weight-programme-new.aspx" },
                    { 44, null, "move", "Southwark's digital health and wellbeing coach", "Southwark Health Coach App", "Direct me to Southwark's digital health and wellbeing coach", "https://southwark.health-coach.app/" },
                    { 28, null, "weight", "Southwark Healthy Lifestyle Hub ", "Southwark Healthy Lifestyle Hub ", "Direct me to a free telephone consultation for weight management services through Southwark's Healthy Lifestyle Hub", "https://southwark.everyonehealth.co.uk/" },
                    { 27, null, "weight", "Southwark's digital health and wellbeing coach", "Southwark Health Coach App", "Direct me to Southwark's digital health and wellbeing coach", "https://southwark.health-coach.app/" },
                    { 25, null, "weight", "Balance and Fast: South East London healthy weight programme", "South East London healthy weight programme (balance and fast)", "Send me a link to holistic healthy weight services for people with a BMI 40+", "https://www.guysandstthomas.nhs.uk/our-services/nutrition-and-dietetics/south-east-London-healthy-weight-programme-new.aspx" },
                    { 12, null, "smoking", "Southwark's digital health and wellbeing coach", "Southwark's digital health and wellbeing coach", "Direct me to Southwark's digital health and wellbeing coach", "https://southwark.health-coach.app/" },
                    { 11, null, "smoking", "Southwark Healthy Lifestyle Hub ", "Southwark Stop Smoking Services", "Direct me to a free telephone consultation for smoking cessation services through Southwark's Healthy Lifestyle Hub", "https://southwark.everyonehealth.co.uk/" },
                    { 135, null, "improvebloodsugar", "Southwark Health Coach App", "Southwark Health Coach App", "Direct me to Southwark's digital health and wellbeing coach", "https://southwark.health-coach.app/" },
                    { 45, null, "move", "Southwark Healthy Lifestyle Hub for services to help manage weight, diet, physical activity, mental wellbeing, alcohol intake and/or smoking habits", "Southwark Healthy Lifestyle Hub ", "Direct me to a free telephone consultation for physical activity services through Southwark's Healthy Lifestyle Hub", "https://southwark.everyonehealth.co.uk/" },
                    { 136, null, "improvebloodsugar", "Southwark Healthy Lifestyle Hub for services to help manage weight, diet, physical activity, mental wellbeing, alcohol intake and/or smoking habits", "Southwark Healthy Lifestyle Hub ", "Direct me to a free telephone consultation for physical activity, weight management and/or smoking cessation services through Southwark's Healthy Lifestyle Hub", "https://southwark.everyonehealth.co.uk/" }
                });

            migrationBuilder.InsertData(
                table: "Interventions",
                columns: new[] { "Id", "BarrierId", "Category", "LinkDescription", "LinkTitle", "Text", "Url" },
                values: new object[,]
                {
                    { 1, 1, "smoking", "Directory of physical activities and clubs in Southwark ", "Southwark Directory of Physical Activities and Clubs", "Direct me to Southwark's directory of activities and clubs for things to do with my friends that do not involve smoking", "https://www.southwark.gov.uk/leisure-and-sport/local-sport-and-physical-activities" },
                    { 116, 103, "improvebloodpressure", "NHS Quit Smoking App", "NHS Smoke Free App", "Direct me to an app that can help to keep me motivated to quit smoking", "https://www.nhs.uk/better-health/quit-smoking/" },
                    { 115, 102, "improvebloodpressure", "Real Stories - blood pressure", "", "Send me real stories of people that have suffered with high blood pressure to motivate me to improve my health", "http://www.bloodpressureuk.org/resources/real-stories/" },
                    { 114, 101, "improvebloodpressure", "NHS Benefits of exercise", "NHS Exercise Health Benefits", "Send me information about the benefits of exercise ", "https://www.nhs.uk/live-well/exercise/exercise-health-benefits/" },
                    { 113, 100, "improvebloodpressure", "How to reduce blood pressure", "", "Send me top tips on lifestyle changes that I can make to lower my blood pressure", "https://www.bhf.org.uk/informationsupport/heart-matters-magazine/research/blood-pressure/blood-pressure-tips" },
                    { 111, 99, "improvecholesterol", "NHS Quit Smoking App", "NHS Smoke Free App", "Direct me to an app that can help to keep me motivated to quit smoking", "https://www.nhs.uk/better-health/quit-smoking/" },
                    { 110, 97, "improvecholesterol", "Volunteering opportunities in Southwark", "Community Southwark", "Direct me to volunteering opportunities in Southwark to help me meet new people and find ways to socialise without alcohol ", "https://www.communitysouthwark.org/Pages/Category/alt-volunteer-listing-page" },
                    { 109, 97, "improvecholesterol", "MeetUp Alcohol-free social groups in London", "MeetUp London Alcohol Free", "Find me social activities that do not involve drinking ", "https://www.meetup.com/topics/nondrinker/gb/17/london/" },
                    { 108, 96, "improvecholesterol", "How do I lower my cholesterol? Top 5 questions", "", "Direct me to commonly asked questions on ways to improve cholesterol levels through healthy lifestyle changes", "https://www.bhf.org.uk/informationsupport/risk-factors/high-cholesterol/five-top-questions-about-lowering-cholesterol" },
                    { 107, 95, "improvecholesterol", "What is high cholesterol?", "", "Direct me to information on health risks of high cholesterol", "https://www.heartuk.org.uk/cholesterol/what-is-high-cholesterol" },
                    { 106, 94, "improvecholesterol", "Headspace meditation and sleep app ", "Headspace", "Direct me to the Headspace app to help me de-stress", "https://www.headspace.com/" },
                    { 105, 93, "improvecholesterol", "Joe Wicks low impact workout video for beginners", "ULTIMATE BEGINNERS Low Impact Workout | The Body Coach TV (Joe Wicks)", "Direct me to a low impact workout video for beginners on YouTube to show me how to get started to moving more", "https://www.YouTube.com/watch?v=7HqGCwt4F1I" },
                    { 104, 92, "improvecholesterol", "Get active your way - NHS", "NHS Get Active Your Way", "Send me ideas on how I can fit more movement into my day", "https://www.nhs.uk/live-well/exercise/get-active-your-way/" },
                    { 103, 91, "improvecholesterol", "Healthy living for your heart", "", "Send me information on easy ways to eating healthily to help lower my cholesterol", "https://www.heartuk.org.uk/healthy-living/introduction" },
                    { 102, 90, "gp", "Tips to get the most out of your GP appointment", "", "Send me advice on how to make the most out of my appointment ", "https://www.healthwatch.co.uk/advice-and-information/2021-07-26/top-tips-get-most-out-your-gp-appointment" },
                    { 101, 89, "gp", "NHS What to ask your doctor", "", "Send me advice on how I can prepare for an appointment with a GP and tips on how to lower my anxiety over fear for the appointment", "https://www.nhs.uk/nhs-services/gps/what-to-ask-your-doctor/" },
                    { 100, 88, "gp", "NHS Health Advice", "", "Show me all the different ways I can access health advice ", "https://www.nhs.uk/" },
                    { 99, 87, "gp", "GP Appointments and booking", "", "Send me information on how to have a telephone or online consultation with a GP", "https://www.nhs.uk/nhs-services/gps/gp-appointments-and-bookings/" },
                    { 85, 73, "mental", "Southwark Council: organisations in Southwark that can help access employment information or legal advice about work-related issues", "Southwark employment support", "Direct me to organisations in Southwark that can help access employment information or legal advice about work-related issues", "https://www.southwark.gov.uk/benefits-and-support/advice-services?chapter=4" },
                    { 86, 74, "mental", "Children and Family centres in Southwark that provide advice and support for parents and carers (services are for expectant mothers and parents up until their children go into reception class at primary school)", "Southwark Family Centres", "Direct me to the Children and Family centres in Southwark that provide advice and support for expectant mothers and parents/carers up until their children go into reception class at primary school", "https://www.southwark.gov.uk/childcare-and-parenting/advice-and-support-for-families/children-s-centres" },
                    { 87, 74, "mental", "Southwark Family Information Service (FIS) for information on local services available to children, young people and families in Southwark", "FIS", "Direct me to the Southwark Family Information Service (FIS) for information on local services available to children, young people and families in Southwark", "https://www.southwark.gov.uk/childcare-and-parenting/advice-and-support-for-families/family-information-service" },
                    { 88, 76, "mental", "Southwark Carers support services for carer's wellbeing", "Southwark Carers", "Direct me to Southwark Carers for support services to help me take care of my own wellbeing", "https://www.southwarkcarers.org.uk/" },
                    { 89, 77, "mental", "Southwark Council's emergency and hardship scheme applications for support with household bills", "Southwark hardship fund (bills)", "Direct me to Southwark Council's emergency and hardship scheme applications for support with my household bills", "https://www.southwark.gov.uk/benefits-and-support/hardship-fund" },
                    { 90, 78, "mental", "Southwark Foodbank for emergency food and additional support alongside the underlying issues behind the crisis", "Southwark Foodbank", "Direct me to the Southwark Foodbank for emergency food and additional support alongside the underlying issues behind my food crisis", "https://southwark.foodbank.org.uk/get-help/" },
                    { 117, 104, "improvebloodpressure", "Good Thinking - Apps for getting better sleep", "Good Thinking Sleep Aid Apps", "Direct me to a range of apps and advice that can help me to get better sleep", "https://www.good-thinking.uk/sleep/" },
                    { 91, 79, "mental", "Southwark's Citizen Advice centre for debt and money advice", "Citizen Advice Southwark", "Direct me to Southwark's Citizen Advice centre for debt and money advice", "https://www.citizensadvicesouthwark.org.uk/projects-and-services/outreach-projects/" },
                    { 93, 81, "mental", "Talking Therapies Southwark NHS Trust - specialist online groups and workshops covering a range of topics related to psychological wellbeing, including bereavement", "Talking therapies Southwark", "Direct me to specialist online groups and workshops for bereavement through Southwark's NHS Talking Therapies", "https://talkingtherapiessouthwark.nhs.uk/about-us/treatment-options/" },
                    { 94, 81, "mental", "Cruse Charity: bereavement helplines and assistance finding services", "Cruse Charity", "Direct me to a national organisation that provide bereavement helplines and assistance finding services", "https://www.cruse.org.uk/get-support/" },
                    { 95, 83, "gp", "Real stories - heart disease", "BHF My Story", "Show me real stories of people with heart disease to motivate me to go to the GP clinic", "https://www.bhf.org.uk/informationsupport/heart-matters-magazine/my-story" },
                    { 96, 84, "gp", "NHS Healthy Body", "NHS Healthy Body", "Show me simple things that I can do to look after my heart health", "https://www.nhs.uk/live-well/healthy-body/" },
                    { 97, 85, "gp", "GP Appointments and booking", "", "Send me information on how to book a weekend or evening consultation with a GP", "https://www.nhs.uk/nhs-services/gps/gp-appointments-and-bookings/" },
                    { 98, 86, "gp", "GP Appointments and booking", "", "Send me information on how I can book a GP appointment online", "https://www.nhs.uk/nhs-services/gps/gp-appointments-and-bookings/" },
                    { 92, 79, "mental", "Southwark Council: services that provide information, advice and legal help in relation to money", "Southwark advice", "Direct me to a range of services that provide information, advice and legal help in relation to money", "https://www.southwark.gov.uk/benefits-and-support/advice-services?chapter=3" },
                    { 84, 71, "mental", "Southwark Council: information, advice and legal help on a variety of housing issues", "Southwark Website of services for housing support", "Direct me to online links that can provide me with information, advice and legal help on a variety of housing issues", "https://www.southwark.gov.uk/benefits-and-support/advice-services?chapter=5" },
                    { 118, 105, "improvebloodpressure", "7 salt-slashing techniques", "", "Send me tips on how to cut down my salt intake to help lower my blood pressure", "https://www.bhf.org.uk/informationsupport/heart-matters-magazine/nutrition/sugar-salt-and-fat/hold-the-salt/seven-salt-slashing-tactics" },
                    { 120, 107, "improvebloodpressure", "My Fitness Pal App to track my diet ", "My Fitness Pal App", "Direct me to an app to help track my diet", "https://www.myfitnesspal.com/" },
                    { 156, 138, "bloodpressure", "AgeUK Transport services for the elderly and disabled ", "", "Direct me to AgeUK for potential transport assistance to get to my health appointment", "https://www.ageuk.org.uk/services/in-your-area/transport/" },
                    { 155, 137, "bloodpressure", "Book a health check test at my leisure centre", "", "Direct me to the online booking webpage where I can make an appointment to have my blood pressure measured at one of Southwark's leisure centres", "https://outlook.office365.com/owa/calendar/DigitalHealthCheckSouthwark@everyonehealth.co.uk/bookings/" },
                    { 154, 136, "bloodpressure", "Where to get your blood pressure checked - guidance", "", "Show me the different options available to get my blood pressure measured", "http://www.bloodpressureuk.org/your-blood-pressure/getting-diagnosed/where-can-you-get-a-blood-pressure-check/" },
                    { 153, 135, "bloodpressure", "How to measure your blood pressure at home", "", "Give me tips on how to measure my blood pressure at home ", "http://www.bloodpressureuk.org/your-blood-pressure/how-to-lower-your-blood-pressure/monitoring-your-blood-pressure-at-home/how-to-measure-your-blood-pressure-at-home/" },
                    { 152, 134, "bloodpressure", "Why should I know my blood pressure? ", "", "Show me the benefits of getting my blood pressure checked", "https://www.bhf.org.uk/informationsupport/heart-matters-magazine/medical/tests/blood-pressure" },
                    { 151, 133, "bloodpressure", "Real stories - high blood pressure", "", "Send me stories of real people with high blood pressure issues", "http://www.bloodpressureuk.org/resources/real-stories/" }
                });

            migrationBuilder.InsertData(
                table: "Interventions",
                columns: new[] { "Id", "BarrierId", "Category", "LinkDescription", "LinkTitle", "Text", "Url" },
                values: new object[,]
                {
                    { 150, 132, "bloodsugar", "NHS Overcoming your fear of needles", "", "Send me tips on overcoming a fear of needles", "https://www.guysandstthomas.nhs.uk/resources/patient-information/all-patients/overcoming-your-fear-of-needles.pdf" },
                    { 149, 131, "bloodsugar", "21 Top Tips on Practicing Mindfulness for Beginners", "", "Send me tips on practicing mindfulness for beginners to help lower my anxiety on getting my results", "https://www.therapyforyou.co.uk/post/practicing-mindfulness-for-beginners" },
                    { 148, 129, "bloodsugar", "NHS Guidance for driving and using public transport with a mobility issue", "", "Direct me to tips for driving and using public transport for those with mobility issues", "https://www.nhs.uk/conditions/social-care-and-support-guide/care-services-equipment-and-care-homes/driving-and-using-public-transport/" },
                    { 147, 129, "bloodsugar", "AgeUK Transport services for the elderly and disabled ", "", "Direct me to AgeUK for potential transport assistance to get to my health appointment", "https://www.ageuk.org.uk/services/in-your-area/transport/" },
                    { 146, 128, "bloodsugar", "Book a health check test at my leisure centre", "", "Direct me to the online booking webpage where I can make an appointment to have my blood sugar level measured at one of Southwark's leisure centres", "https://outlook.office365.com/owa/calendar/DigitalHealthCheckSouthwark@everyonehealth.co.uk/bookings/" },
                    { 145, 127, "bloodsugar", "Book a health check test at my leisure centre", "", "Help me to book an appointment that fits around my lifestyle", "https://outlook.office365.com/owa/calendar/DigitalHealthCheckSouthwark@everyonehealth.co.uk/bookings/" },
                    { 144, 126, "bloodsugar", "Complications of diabetes", "", "Send me information about the health consequences of diabetes", "https://www.diabetes.org.uk/guide-to-diabetes/complications" },
                    { 143, 125, "cholesterol", "NHS Overcoming your fear of needles", "", "Send me tips on overcoming a fear of needles", "https://www.guysandstthomas.nhs.uk/resources/patient-information/all-patients/overcoming-your-fear-of-needles.pdf" },
                    { 142, 124, "cholesterol", "21 Top Tips on Practicing Mindfulness for Beginners", "", "Send me tips on practicing mindfulness for beginners to help lower my anxiety on getting my results", "https://www.therapyforyou.co.uk/post/practicing-mindfulness-for-beginners" },
                    { 141, 122, "cholesterol", "NHS Guidance for driving and using public transport with a mobility issue", "", "Direct me to tips for driving and using public transport for those with mobility issues", "https://www.nhs.uk/conditions/social-care-and-support-guide/care-services-equipment-and-care-homes/driving-and-using-public-transport/" },
                    { 140, 122, "cholesterol", "AgeUK Transport services for the elderly and disabled ", "", "Direct me to AgeUK for potential transport assistance to get to my health appointment", "https://www.ageuk.org.uk/services/in-your-area/transport/" },
                    { 121, 108, "improvebloodpressure", "NHS Weight loss plan", "NHS weight loss plan", "Direct me to the online NHS Weight Loss plan so that I can plan my meals and make healthier food choices to help lower my blood pressure", "https://www.nhs.uk/better-health/lose-weight/" },
                    { 122, 109, "improvebloodpressure", "My local Slimming World group", "Slimming world", "Direct me to my local Slimming World group so that I can find others to eat more healthily and be more active with", "https://www.slimmingworld.co.uk/nearest-group-search" },
                    { 126, 110, "improvebloodsugar", "NHS Weight loss plan app", "", "Direct me to the NHS Weight Loss Plan app so that I can plan my meals and make healthier food choices", "https://www.nhs.uk/better-health/lose-weight/" },
                    { 127, 111, "improvebloodsugar", "Complications of diabetes", "", "Send me information about the health consequences of diabetes", "https://www.diabetes.org.uk/guide-to-diabetes/complications" },
                    { 128, 112, "improvebloodsugar", "Talking therapies Southwark", "", "Direct me to Southwark Talking Therapies, my local free self-referral NHS talking therapies service, for support with my mental wellbeing", "https://talkingtherapiessouthwark.nhs.uk/" },
                    { 129, 113, "improvebloodsugar", "My Fitness Pal App to track my diet ", "", "Direct me to an app to help track my diet", "https://www.myfitnesspal.com/" },
                    { 119, 106, "improvebloodpressure", "Get active your way - NHS", "NHS Get Active Your Way", "Send me information on how I can fit more movement into my day", "https://www.nhs.uk/live-well/exercise/get-active-your-way/" },
                    { 130, 114, "improvebloodsugar", "EXi app - 12-week exercise plan", "", "Direct me to an app that can help me set an exercise plan suitable for me", "https://exi.life/" },
                    { 132, 116, "improvebloodsugar", "Access to free swim and gym facilities in Southwark", "Free Swim and Gym", "Direct me to free swim and gym facilities in Southwark", "https://www.southwark.gov.uk/leisure-and-sport/free-swim-and-gym" },
                    { 133, 117, "improvebloodsugar", "Change4Life food scanner app", "", "Direct me to an app that can tell me amount of sugar in food at when I am food shopping", "https://play.google.com/store/apps/details?id=com.phe.c4lfoodsmart&hl=en_GB&gl=US" },
                    { 134, 118, "improvebloodsugar", "My local Slimming World group", "", "Direct me to my local Slimming World group so that I can find others to eat more healthily and be more active with", "https://www.slimmingworld.co.uk/nearest-group-search" },
                    { 137, 119, "cholesterol", "What is high cholesterol? ", "", "Direct me to information on health risks of high cholesterol", "https://www.heartuk.org.uk/cholesterol/what-is-high-cholesterol" },
                    { 138, 120, "cholesterol", "Book a health check test at my leisure centre", "", "Help me to book an appointment that fits around my lifestyle", "https://outlook.office365.com/owa/calendar/DigitalHealthCheckSouthwark@everyonehealth.co.uk/bookings/" },
                    { 139, 121, "cholesterol", "Book a health check test at my leisure centre", "", "Direct me to the online booking webpage where I can make an appointment to have my cholesterol levels measured at one of Southwark's leisure centres", "https://outlook.office365.com/owa/calendar/DigitalHealthCheckSouthwark@everyonehealth.co.uk/bookings/" },
                    { 131, 115, "improvebloodsugar", "", "Couch to 5K App", "Direct me to the Couch to 5K app and online programme to help me gradually increase my stamina and to move more", "https://www.nhs.uk/live-well/exercise/get-running-with-couch-to-5k/" },
                    { 83, 71, "mental", "Southwark's Citizen Advice centre for housing advice", "Southwark Citizen Advice", "Direct me to Southwark's Citizen Advice centre for housing advice", "https://www.citizensadvicesouthwark.org.uk/projects-and-services/outreach-projects/" },
                    { 82, 70, "mental", "Talking Therapies Southwark NHS Trust - specialist online groups and workshops covering a range of topics related to psychological wellbeing, such as coping well during the pandemic, bereavement, mindfulness, LGBTQ wellbeing, student wellbeing, and ethnic minority empowerment.", "", "Direct me to specialist online groups and workshops covering a range of topics related to psychological wellbeing, such as coping well during the pandemic, bereavement, mindfulness, LGBTQ wellbeing, student wellbeing, and ethnic minority empowerment.", "https://talkingtherapiessouthwark.nhs.uk/about-us/treatment-options/" },
                    { 81, 68, "mental", "Community Southwark: LGBTQ+ organisations in Southwark that can offer help to people experiencing issues relating to sexuality, gender and identity", "Community Southwark Support for LGBTQ+ community", "Direct me to a range of LGBTQ+ organisations in Southwark that can offer help to people experiencing issues relating to sexuality, gender and identity", "https://www.communitysouthwark.org/news/support-for-lgbtq-people" },
                    { 38, 32, "move", "Joe Wicks low impact workout video for beginners", "ULTIMATE BEGINNERS Low Impact Workout | The Body Coach TV (Joe Wicks)", "Direct me to a low impact workout video for beginners on YouTube to show me how to get started to moving more", "https://www.YouTube.com/watch?v=7HqGCwt4F1I" },
                    { 36, 30, "move", "Kickstart, Active Boost and Cardiactive - Southwark exercise programmes ", "Exercise on Referral", "Show me specialised group activity programmes that I can be referred onto by my GP if I am eligible ", "https://southwark.everyonehealth.co.uk/services/gp-exercise-referral/" },
                    { 35, 29, "move", "Get running with Couch to 5K", "Couch to 5K App", "Direct me to the Couch to 5K app and online programme", "https://www.nhs.uk/live-well/exercise/get-running-with-couch-to-5k/" },
                    { 34, 28, "move", "NHS Exercise levels", "NHS LiveWell Exercise", "Direct me to information on healthy exercise levels", "https://www.nhs.uk/live-well/exercise/" },
                    { 33, 27, "move", "Join the movement, Sport England", "Sport England", "Direct me to free outdoor exercise activities", "https://www.sportengland.org/jointhemovement?section=get_active_away_from_home" },
                    { 32, 26, "move", "We are undefeatable - getting started", "We are undefeatable", "Direct me to the We Are Undefeatable website to help me find activities that are right for me with my health condition ", "https://weareundefeatable.co.uk/getting-started" },
                    { 37, 25, "move", "Everyone Health Walks Southwark", "Everyone Health Walks Southwark/Walking for Health", "Direct me to my nearest walking group (for all abilities) ", "https://www.walkingforhealth.org.uk/walkfinder/london/fusion-health-walks-southwark" },
                    { 31, 25, "move", "RunTogether running group", "RunTogether running group", "Direct me to my nearest RunTogether group so that I can go running with other people", "https://runtogether.co.uk/group-running/find-a-runtogether-group-run/" },
                    { 30, 24, "move", "Return to play - motivational sports video", "Sports ", "Direct me to YouTube videos to inspire me to move more", "https://www.YouTube.com/watch?v=SNVwDam-gSY" },
                    { 29, 23, "weight", "Our Stories, from This Girl Can", "This Girl Can Our Stories (Jen)", "Direct me to inspirational stories about people that do not think they are sporty people (like me)", "https://www.thisgirlcan.co.uk/stories/jen/" },
                    { 26, 22, "weight", "NHS Active10 ", "NHS Active10", "Direct me to an app to track my activity and help me set goals", "https://www.nhs.uk/oneyou/active10/home/" },
                    { 23, 21, "weight", "My Fitness Pal App to track my diet ", "My Fitness Pal App", "Direct me to an app that can help me to track my diet", "https://www.myfitnesspal.com/" },
                    { 22, 20, "weight", "Joe Wicks low impact workout video for beginners", "ULTIMATE BEGINNERS Low Impact Workout | The Body Coach TV (Joe Wicks)", "Direct me to a low impact workout video for beginners on YouTube to show me how to get started to moving more", "https://www.YouTube.com/watch?v=7HqGCwt4F1I" }
                });

            migrationBuilder.InsertData(
                table: "Interventions",
                columns: new[] { "Id", "BarrierId", "Category", "LinkDescription", "LinkTitle", "Text", "Url" },
                values: new object[,]
                {
                    { 21, 19, "weight", "Vouchers for Slimming World and Weight Watchers", "Vouchers for Slimming World and Weight Watchers", "Show me how to access vouchers for weight management support groups, to connect with others that might experience similar challenges to me", "https://southwark.everyonehealth.co.uk/services/weight-management/" },
                    { 20, 18, "weight", "NHS Health benefits of exercise", "NHS Exercise Health Benefits", "Send me information about the benefits of exercise ", "https://www.nhs.uk/live-well/exercise/exercise-health-benefits/" },
                    { 19, 17, "weight", "Access to free swim and gym facilities in Southwark", "Free Swim and Gym", "Direct me to free swim and gym facilities in Southwark", "https://www.southwark.gov.uk/leisure-and-sport/free-swim-and-gym" },
                    { 18, 16, "weight", "Headspace meditation and sleep app ", "Headspace", "Direct me to the Headspace app to help me de-stress", "https://www.headspace.com/" },
                    { 2, 2, "smoking", "Stop smoking support services in Southwark", "Southwark website - stop smoking telephone support page", "Direct me to my local support services that can provide me with smoking cessation aids (such as nicotine replacement, Champix or e-cigarettes)", "https://www.southwark.gov.uk/health-and-wellbeing/health-advice-and-support/stop-smoking?chapter=3" },
                    { 3, 3, "smoking", "10 self-help tips to stop smoking on the NHS website", "NHS Quitting strategies", "Show me strategies online to quit smoking", "https://www.nhs.uk/live-well/quit-smoking/10-self-help-tips-to-stop-smoking/" },
                    { 4, 3, "smoking", "Face-to-face stop smoking support services in Southwark ", "Southwark website - face to face", "Direct me to a pharmacy that can help me quit smoking", "https://www.southwark.gov.uk/health-and-wellbeing/health-advice-and-support/stop-smoking?chapter=4" },
                    { 5, 3, "smoking", "Stop smoking telephone support services in Southwark", "Southwark website - stop smoking telephone support page", "Direct me to a telephone service that can help me quit smoking", "https://www.southwark.gov.uk/health-and-wellbeing/health-advice-and-support/stop-smoking?chapter=3" },
                    { 6, 6, "smoking", "Headspace meditation and sleep app ", "Headspace", "Direct me to the Headspace app for an alternative healthy, relaxing activity to do", "https://www.headspace.com/" },
                    { 7, 7, "smoking", "Southwark Stop Smoking Service for plans to quit smoking", "Southwark Stop Smoking Service", "Direct me to a local service that can help me make a plan to quit smoking", "https://southwark.everyonehealth.co.uk/stop-smoking-service/" },
                    { 39, 33, "move", "Directory of physical activities and clubs in Southwark ", "Directory of physical activities and clubs in Southwark ", "Direct me to local sports and physical activities for people like me ", "https://www.southwark.gov.uk/leisure-and-sport/local-sport-and-physical-activities" },
                    { 8, 8, "smoking", "NHS Smoke Free App", "NHS Smoke Free App", "Direct me to an app that can help to keep me motivated to quit smoking", "https://www.nhs.uk/better-health/quit-smoking/" },
                    { 10, 10, "smoking", "Southwark Stop Smoking support nicotine cravings", "Southwark website - stop smoking telephone support page", "Direct me to my local support services that provide me with tools to help me manage my cravings", "https://www.southwark.gov.uk/health-and-wellbeing/health-advice-and-support/stop-smoking?chapter=3" },
                    { 13, 11, "weight", "NHS Healthy eating guidelines", "NHS healthy guudlines", "Send me information on what is included in a healthy diet", "https://www.nhs.uk/live-well/eat-well/the-eatwell-guide/" },
                    { 14, 12, "weight", "NHS Weight loss plan", "NHS weight loss plan", "Send me to the NHS Weight Loss Plan", "https://www.nhs.uk/better-health/lose-weight/" },
                    { 15, 13, "weight", "My local Slimming World group", "Slimming world", "Direct me to my local Slimming World group", "https://www.slimmingworld.co.uk/nearest-group-search" },
                    { 16, 14, "weight", "NHS Recipes and tips to eat well for less", "NHS Eat Well for Less", "Send me tips and recipes on how to eat healthily on a budget", "https://www.nhs.uk/live-well/eat-well/20-tips-to-eat-well-for-less/?tabname=recipes-and-tips" },
                    { 17, 15, "weight", "Food access services in Southwark", "Southwark Food Access Services", "Direct me to Southwark's food access services", "https://www.southwark.gov.uk/health-and-wellbeing/health-advice-and-support/healthy-eating?chapter=6" },
                    { 9, 9, "smoking", "NHS Benefits of stopping smoking", "NHS Benefits of stopping smoking", "Send me information about the benefits of stopping smoking", "https://www.nhs.uk/live-well/quit-smoking/" },
                    { 40, 34, "move", "NHS Benefits of exercise", "NHS Exercise Health Benefits", "Send me information about the benefits of exercise ", "https://www.nhs.uk/live-well/exercise/exercise-health-benefits/" },
                    { 41, 35, "move", "Access to free swim and gym facilities in Southwark", "Free Swim and Gym", "Direct me to free swim and gym facilities in Southwark", "https://www.southwark.gov.uk/leisure-and-sport/free-swim-and-gym" },
                    { 42, 36, "move", "Our Stories, from This Girl Can", "This Girl Can Our Stories (Jen)", "Direct me to inspirational stories about people that think like me", "https://www.thisgirlcan.co.uk/stories/jen/" },
                    { 66, 55, "mental", "Good Thinking Digital NHS service - Apps for getting better sleep", "Good Thinking Sleep Aid Apps", "Direct me to apps for getting better sleep", "https://www.good-thinking.uk/sleep/" },
                    { 67, 55, "mental", "Therapy Resources, Sleep - Talking Therapies Southwark NHS Trust", "Talking therapies Southwark", "Direct me to free NHS therapy resources to help me sleep", "https://talkingtherapiessouthwark.nhs.uk/self-help-resources/therapy-resources/" },
                    { 68, 57, "mental", "Headspace meditation and sleep app", "Headspace", "Direct me to the Headspace app to help me relax", "https://www.headspace.com/" },
                    { 69, 57, "mental", "Good Thinking Digital NHS service - Deal with stress", "Good thinking", "Direct me to the Good Thinking Digital Mental Wellbeing website for resources on how to deal with stress", "https://www.good-thinking.uk/stress/" },
                    { 70, 59, "mental", "Talking Therapies Southwark NHS Trust", "Talking therapies Southwark", "Direct me to Talking Therapies Southwark, my local free NHS talking therapies service, for emotional and mental health support from those that have an understanding of what I am going through", "https://talkingtherapiessouthwark.nhs.uk/" },
                    { 71, 59, "mental", "Qwell: online emotional wellbeing and mental health support services for adults", "Qwell App", "Direct me to online safe and anonymous peer support communities so that I can connect with others that have a better understanding of what I might be going through", "https://www.qwell.io/" },
                    { 65, 53, "mental", "Directory of physical activities and clubs in Southwark", "Directory of physical activities and clubs in Southwark", "Direct me to local sports and physical activities for me to explore and try", "https://www.southwark.gov.uk/leisure-and-sport/local-sport-and-physical-activities" },
                    { 72, 59, "mental", "Southwark wellbeing Hub Peer Support", "Together Southwark Peer Support", "Direct me to peer support services in Southwark so that I can talk to someone who has been through something similar to me", "https://www.together-uk.org/southwark-wellbeing-hub/get-support/peer-support-self-management-groups/" },
                    { 74, 62, "mental", "Time & Talents Southwark community organisation for opportunities to volunteer and socialise", "", "Direct me to Time & Talents in Southwark for opportunities to volunteer and socialise", "https://www.timeandtalents.org.uk/" },
                    { 75, 62, "mental", "Link Age Southwark for befriending, group activities and opportunities to socialise for older adults (aged 60 years plus or adults living with dementia)", "", "Direct me to Link Age in Southwark for befriending, group activities and opportunities to socialise for older adults (aged 60 years plus or adults living with dementia)", "https://www.linkagesouthwark.org/" },
                    { 76, 65, "mental", "Free workshops and events for mental wellbeing such as creative writing, arts appreciation and mindfulness", "Together Southwark Wellbeing Workshops", "Direct me to free workshops and events for mental wellbeing such as creative writing, arts appreciation and mindfulness", "https://www.ticketsource.co.uk/southwark-wellbeing-hub" },
                    { 77, 65, "mental", "The Dragon Cafe: an informal, creative and safe space for people to meet and take part in a range of activities", "The Dragon Cafe Charity ", "Direct me to an informal, creative and safe space to take part in a range of creative activities and meet people", "https://www.dragoncafe.co.uk/" },
                    { 78, 67, "mental", "DIY CBT- Talking therapies Southwark NHS Trust", "NHS DIY Talking Therapies", "Direct me to NHS therapy self-help videos to help me set goals and learn strategies that I can use to improve my mental wellbeing", "https://talkingtherapiessouthwark.nhs.uk/self-help-resources/therapy-resources/diy-cbt-cognitive-behavioural-therapy-videos/" },
                    { 80, 68, "mental", "Stonewall help and advice", "Stonewall Charity", "Direct me to help, support and local services for LGBTQ+ people", "https://www.stonewall.org.uk/help-and-advice" },
                    { 73, 62, "mental", "Social clubs in London", "Southwark London Cares", "Connect me to Southwark London Cares for opportunities to volunteer and socialise", "https://southlondoncares.org.uk/social-clubs" },
                    { 157, 138, "bloodpressure", "NHS Guidance for driving and using public transport with a mobility issue", "", "Direct me to tips for driving and using public transport for those with mobility issues", "https://www.nhs.uk/conditions/social-care-and-support-guide/care-services-equipment-and-care-homes/driving-and-using-public-transport/" },
                    { 64, 53, "mental", "Everyone Health Walks Southwark", "Everyone Health Walks Southwark/Walking for Health", "Direct me to my nearest walking group (for all abilities)", "https://www.walkingforhealth.org.uk/walkfinder/london/fusion-health-walks-southwark" },
                    { 62, 50, "mental", "Qwell: online emotional wellbeing and mental health support services for adults", "Qwell", "Direct me to a service that provides one-to-one online chat sessions with experienced counsellors", "https://www.qwell.io/" },
                    { 43, 37, "move", "Get active your way - NHS", "NHS Get Active Your Way", "Send me information on how I can fit more movement into my day", "https://www.nhs.uk/live-well/exercise/get-active-your-way/" },
                    { 46, 38, "alcohol", "Drinkaware drink free days", "DrinkAware Drink Free Days", "Send me an online tool to help me discover my drinking risk level and set a plan to reduce my drinking", "https://www.drinkaware.co.uk/tools/drink-free-days" }
                });

            migrationBuilder.InsertData(
                table: "Interventions",
                columns: new[] { "Id", "BarrierId", "Category", "LinkDescription", "LinkTitle", "Text", "Url" },
                values: new object[,]
                {
                    { 47, 39, "alcohol", "NHS Alcohol misuse risks", "NHS Risks of drinking alcohol", "Direct me to information on the health risks of drinking alcohol", "https://www.nhs.uk/conditions/alcohol-misuse/risks/" },
                    { 48, 40, "alcohol", "NHS Tips on cutting down - Alcohol support", "NHS Tips on cutting down alcohol", "Send me tips on how to cut down on my alcohol consumption", "https://www.nhs.uk/live-well/alcohol-support/tips-on-cutting-down-alcohol/" },
                    { 49, 41, "alcohol", "Volunteering opportunities in Southwark", "Community Southwark", "Direct me to volunteering opportunities in Southwark to help me meet new people and find ways to socialise without alcohol ", "https://www.communitysouthwark.org/Pages/Category/alt-volunteer-listing-page" },
                    { 50, 42, "alcohol", "Alcoholics Anonymous Support Group", "AA Groups", "Direct me to my nearest alcoholics anonymous (AA)  group for support from those that understand what I might be going through", "https://www.alcoholics-anonymous.org.uk/" },
                    { 63, 50, "mental", "Talking Therapies Southwark NHS Trust", "Talking therapies Southwark", "Direct me to Southwark Talking Therapies, my local free self-referral NHS talking therapies service, to find online support for my mental wellbeing that can fit around my busy lifestyle", "https://talkingtherapiessouthwark.nhs.uk/" },
                    { 51, 43, "alcohol", "Headspace meditation and sleep app ", "Headspace", "Direct me to an app to help replace my drinking with something healthy (Headspace)", "https://www.headspace.com/" },
                    { 53, 45, "alcohol", "Talking Therapies Southwark NHS Trust", "SLAM IAPT", "Direct me to Southwark Talking Therapies, my local free self-referral NHS talking therapies service, to find support for my mental wellbeing", "https://slam-iapt.nhs.uk/" },
                    { 54, 46, "alcohol", "Drug and alcohol services - Southwark", "CGL", "Connect me to local services that can give me extra help in changing my drinking behaviours", "https://www.changegrowlive.org/drug-alcohol-service-southwark" },
                    { 57, 47, "mental", "Talking Therapies Southwark NHS Trust", "Talking therapies Southwark", "Direct me to Southwark Talking Therapies (a free NHS self-referral therapy service) to find a talking therapy that’s right for me", "https://talkingtherapiessouthwark.nhs.uk/" },
                    { 58, 48, "mental", "Southwark Wellbeing Hub", "Southwark Wellbeing Hub Peer Support", "Direct me to Southwark's Wellbeing Hub for information and support on mental wellbeing", "Southwark Wellbeing Hub - Southwark Wellbeing Hub (together-uk.org)" },
                    { 59, 49, "mental", "Look after your mental health using exercise", "Mental Health Foundation", "Show me how physical activity can be beneficial for my mental wellbeing", "https://www.mentalhealth.org.uk/publications/how-to-using-exercise" },
                    { 61, 50, "mental", "Good Thinking Digital NHS service", "Good thinking", "Direct me to a range of online helpful mental wellbeing resources that I can use to fit in with my busy lifestyle", "https://www.good-thinking.uk/" },
                    { 52, 44, "alcohol", "Drink Aware: how to cut down on alcohol at home", "Drink Aware Cut Down on Alcohol at Home", "Send me tips on how I can change my environment to help me cut down my alcohol consumption at home ", "https://www.drinkaware.co.uk/advice/how-to-reduce-your-drinking/how-to-cut-down-on-alcohol-at-home" },
                    { 158, 140, "bloodpressure", "21 Top Tips on Practicing Mindfulness for Beginners", "", "Send me tips on practicing mindfulness for beginners to help lower my anxiety on getting my results", "https://www.therapyforyou.co.uk/post/practicing-mindfulness-for-beginners" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BarrierHealthCheck_HealthChecksId",
                table: "BarrierHealthCheck",
                column: "HealthChecksId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodKitRequests_CheckId",
                table: "BloodKitRequests",
                column: "CheckId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomBarriers_HealthCheckId",
                table: "CustomBarriers",
                column: "HealthCheckId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCheckIntervention_HealthChecksId",
                table: "HealthCheckIntervention",
                column: "HealthChecksId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_BloodPressureFollowUpId",
                table: "HealthChecks",
                column: "BloodPressureFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_BloodSugarFollowUpId",
                table: "HealthChecks",
                column: "BloodSugarFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_BookGPAppointmentFollowUpId",
                table: "HealthChecks",
                column: "BookGPAppointmentFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_CholesterolFollowUpId",
                table: "HealthChecks",
                column: "CholesterolFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_DrinkLessFollowUpId",
                table: "HealthChecks",
                column: "DrinkLessFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_HealthyWeightFollowUpId",
                table: "HealthChecks",
                column: "HealthyWeightFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_HeightAndWeightFollowUpId",
                table: "HealthChecks",
                column: "HeightAndWeightFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_ImproveBloodPressureFollowUpId",
                table: "HealthChecks",
                column: "ImproveBloodPressureFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_ImproveBloodSugarFollowUpId",
                table: "HealthChecks",
                column: "ImproveBloodSugarFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_ImproveCholesterolFollowUpId",
                table: "HealthChecks",
                column: "ImproveCholesterolFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_MentalWellbeingFollowUpId",
                table: "HealthChecks",
                column: "MentalWellbeingFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_MoveMoreFollowUpId",
                table: "HealthChecks",
                column: "MoveMoreFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_QuitSmokingFollowUpId",
                table: "HealthChecks",
                column: "QuitSmokingFollowUpId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthChecks_VariantId",
                table: "HealthChecks",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_BarrierId",
                table: "Interventions",
                column: "BarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationErrors_HealthCheckId",
                table: "ValidationErrors",
                column: "HealthCheckId");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION dbo.IsHeartAgeHigh(@heartAge int, @age int)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @heartAge IS NULL THEN NULL
        
        WHEN @heartAge > @age + ROUND(((@age - 40) / 5)+5,0) THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION dbo.BodyMassIndex(@height float, @weight float)
RETURNS FLOAT
AS
BEGIN
    RETURN (@weight / (@height * @height))
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION dbo.IsHighBmiRiskEthnicity(@ethnicity int)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @ethnicity IS NULL THEN NULL
        WHEN @ethnicity IN (13,12,10,15,14,11,8,9,5,4,6) THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION dbo.BPRiskCategory(@systolicBP int, @diastolicBP int)
RETURNS INT
AS
BEGIN
    -- BP is recorded as Low if any is Low, and then is recorded as the highest risk of either Systolic or Diastolic. 
    -- So e.g. if Sys is Healthy but Dias is Slightly High, the result is Slightly High.
    -- 0 - too low
    -- 1 - healthy
    -- 2 - slightly high
    -- 3 - high
    -- 4 - very high
    RETURN CASE
        WHEN @systolicBP IS NULL OR @diastolicBP IS NULL THEN NULL
        WHEN @systolicBP < 90 THEN 0
        WHEN @diastolicBP < 60 THEN 0
        WHEN @systolicBP >= 90 AND @systolicBP < 120 AND @diastolicBP >= 60 AND @diastolicBP < 80 THEN 1
        WHEN @systolicBP >= 120 AND @systolicBP < 140 AND @diastolicBP >= 60 AND @diastolicBP < 90 THEN 2
        WHEN @diastolicBP >= 80 AND @diastolicBP < 90 AND @systolicBP >= 90 AND @systolicBP < 140 THEN 2
        WHEN @systolicBP >= 140 AND @systolicBP < 180 AND @diastolicBP >= 60 AND @diastolicBP < 110 THEN 3
        WHEN @diastolicBP >= 90 AND @diastolicBP < 110 AND @systolicBP >= 90 AND @systolicBP < 180 THEN 3
        WHEN @systolicBP >= 180 AND @diastolicBP >= 60 THEN 4
        WHEN @diastolicBP >= 110 AND @systolicBP >= 90 THEN 4
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION dbo.CholesterolRiskCategory(@totalCholesterol int)
RETURNS INT
AS
BEGIN
    -- 0 - low
    -- 1 - medium
    -- 2 - high
    RETURN CASE
        WHEN @totalCholesterol IS NULL THEN NULL
        WHEN @totalCholesterol <= 5 THEN 0
        WHEN @totalCholesterol > 5 AND @totalCholesterol < 7.5 THEN 1 
        WHEN @totalCholesterol >= 7.5 THEN 2
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION dbo.MentalRiskCategory(@gad2 int, @feelingDown bit, @disinterested bit)
RETURNS INT
AS
BEGIN
    RETURN CASE
        WHEN @gad2 IS NULL THEN NULL
        WHEN @gad2 < 2 AND @feelingDown = 0 AND @disinterested = 0 THEN 1
        WHEN (NOT (@gad2 < 2 AND @feelingDown = 0 AND @disinterested = 0) AND NOT (@gad2 >= 5 AND @feelingDown = 1 AND @disinterested = 1)) THEN 1
        WHEN @gad2 >= 5 AND @feelingDown = 1 AND @disinterested = 1 THEN 2
    END
END';");

migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsCVDHigh](@qrisk FLOAT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @qrisk IS NULL THEN NULL
        WHEN @qrisk >= 20 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsDiabetesHigh](@qdiabetes FLOAT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @qdiabetes IS NULL THEN NULL
        WHEN @qdiabetes >= 5.3 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsCholesterolHigh](@cholesterol FLOAT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @cholesterol IS NULL THEN NULL
        WHEN @cholesterol >= 7.5 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsBloodPressureHigh](@SystolicBloodPressure INT, @DiastolicBloodPressure INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @SystolicBloodPressure IS NULL OR @DiastolicBloodPressure IS NULL THEN NULL
        WHEN @SystolicBloodPressure >= 140 OR @DiastolicBloodPressure > 90 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsBloodSugarMedium](@hbA1c INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @hbA1c IS NULL THEN NULL
        WHEN @hbA1c BETWEEN 42 AND 47 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsBloodSugarHigh](@hbA1c INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @hbA1c IS NULL THEN NULL
        WHEN @hbA1c >= 48 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsMentalHealthRiskHigh](@GAD INT, @FeelingDown BIT, @Disinterested BIT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @GAD IS NULL OR @FeelingDown IS NULL OR @Disinterested IS NULL THEN NULL
        WHEN @GAD >= 5 AND @FeelingDown = 1 AND @Disinterested = 1 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsMentalHealthRiskMedium](@GAD INT, @FeelingDown BIT, @Disinterested BIT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @GAD IS NULL OR @FeelingDown IS NULL OR @Disinterested IS NULL THEN NULL
        WHEN @GAD >= 5 AND @FeelingDown = 1 AND @Disinterested = 1 THEN CONVERT(bit,0) -- high risk
        WHEN @GAD >= 2 OR @FeelingDown = 1 OR @Disinterested = 1 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsAuditMedium](@AUDIT INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @AUDIT IS NULL THEN NULL
        WHEN @AUDIT BETWEEN 8 AND 15 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsAuditHigh](@AUDIT INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @AUDIT IS NULL THEN NULL
        WHEN @AUDIT >= 16 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[IsSmoker](@SmokingStatus INT)
RETURNS BIT
AS
BEGIN
    RETURN CASE
        WHEN @SmokingStatus IS NULL THEN NULL
        WHEN @SmokingStatus >= 2 THEN CONVERT(bit,1)
        ELSE CONVERT(bit,0)
    END
END';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[EligibleForGPFollowUp](@SystolicBloodPressure INT, @DiastolicBloodPressure INT, @HbA1c FLOAT, @Height FLOAT, @Weight FLOAT, @QRISK FLOAT, @QDiabetes FLOAT)
    RETURNS BIT
    AS
    BEGIN
        RETURN CASE
            WHEN @SystolicBloodPressure IS NULL OR @DiastolicBloodPressure IS NULL OR @HbA1c IS NULL OR @Height IS NULL OR @Weight IS NULL OR @QRISK IS NULL OR @QDiabetes IS NULL THEN NULL
            WHEN dbo.IsBloodPressureHigh(@SystolicBloodPressure, @DiastolicBloodPressure) = 1 OR
                @hbA1c > 100 OR 
                dbo.BodyMassIndex(@height,@weight) < 16 OR
                @QRISK >= 10 OR
                dbo.IsDiabetesHigh(@QDiabetes) = 1 THEN CONVERT(bit,1)
            ELSE CONVERT(bit,0)
        END
    END';");

migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[RiskFactors](@id uniqueidentifier)
RETURNS INT
AS
BEGIN
	DECLARE @result INT

	SELECT @result = COUNT(1) FROM
	(
		SELECT CASE WHEN dbo.IsSmoker(hc.SmokingStatus) = 1 THEN 1 ELSE 0 END AS Val FROM dbo.HealthChecks hc WHERE hc.Id = @Id UNION ALL
		SELECT CASE WHEN dbo.IsAuditHigh(hc.[AUDIT]) = 1 OR dbo.IsAuditMedium(hc.[AUDIT]) = 1 THEN 1 ELSE 0 END AS Val FROM dbo.HealthChecks hc WHERE hc.Id = @Id UNION ALL
		SELECT CASE WHEN hc.GPPAQ <= 2 THEN 1 ELSE 0 END AS Val FROM dbo.HealthChecks hc WHERE hc.Id = @Id UNION ALL
		SELECT CASE WHEN (dbo.BodyMassIndex(hc.Height,hc.Weight) < 16) OR
			(dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 1 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 23 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) < 27.5) OR
			(dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 0 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 25 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) < 30) OR
			(dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 1 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 27.5) OR
			(dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 0 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 30) OR
			(dbo.IsBloodPressureHigh(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 1) OR
			(dbo.IsBloodPressureHigh(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 0 AND (hc.SystolicBloodPressure BETWEEN 120 AND 139 OR hc.DiastolicBloodPressure BETWEEN 80 AND 89))
			THEN 1 ELSE 0 END AS Val FROM dbo.HealthChecks hc WHERE hc.Id = @Id UNION ALL
		SELECT CASE WHEN (hc.TotalCholesterol IS NOT NULL AND (hc.QRISK >= 10 OR hc.TotalCholesterol >= 7.5)) OR
			(hc.TotalCholesterol IS NOT NULL AND hc.QRISK < 10 AND hc.TotalCholesterol < 7.5)
			THEN 1 ELSE 0 END AS Val FROM dbo.HealthChecks hc WHERE hc.Id = @Id UNION ALL
		SELECT CASE WHEN hc.QDiabetes >= 5.6 THEN 1 ELSE 0 END AS Val FROM dbo.HealthChecks hc WHERE hc.Id = @Id UNION ALL
		SELECT CASE WHEN hc.BloodSugar IS NOT NULL AND hc.BloodSugar >= 42 THEN 1 ELSE 0 END AS Val FROM dbo.HealthChecks hc WHERE hc.Id = @Id UNION ALL
		SELECT CASE WHEN hc.FamilyHistoryCVD = 1 OR
			hc.ChronicKidneyDisease = 1 OR
			hc.AtrialFibrillation = 1 OR
			hc.BloodPressureTreatment = 1 OR
			hc.Migraines = 1 OR
			hc.RheumatoidArthritis = 1 OR
			hc.SystemicLupusErythematosus = 0 OR
			hc.SevereMentalIllness = 0 OR
			hc.AtypicalAntipsychoticMedication = 0 
			THEN 1 ELSE 0 END AS Val FROM dbo.HealthChecks hc WHERE hc.Id = @Id
	) a WHERE a.Val = 1

	RETURN @result
END';");

migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[AnonymisedBarriers](@from DateTime NULL, @to DateTime NULL)
RETURNS TABLE
AS
RETURN
(
	SELECT
		hc.Id AS PatientIdentifier,
		b.Category AS Category,
		b.[Text] AS [Barrier]
	FROM dbo.BarrierHealthCheck bhc
		INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
		INNER JOIN dbo.HealthChecks hc ON bhc.HealthChecksId = hc.Id
	WHERE hc.CheckStartedDate BETWEEN COALESCE(@from, ''1753-01-01 00:00:00.000'') AND COALESCE(@to, ''9999-12-31 23:59:59.997'')
)';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[AnonymisedInterventions](@from DateTime NULL, @to DateTime NULL)
RETURNS TABLE
AS
RETURN
(
	SELECT
		hc.Id AS PatientIdentifier,
		i.Category AS Category,
		b.[Text] AS Barrier,
		i.[Text] AS [Intervention],
		i.[Url] AS [Url]
	FROM dbo.HealthCheckIntervention ihc
		INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
		INNER JOIN dbo.HealthChecks hc ON ihc.HealthChecksId = hc.Id
		LEFT JOIN dbo.Barriers b ON i.BarrierId = b.Id
	WHERE hc.CheckStartedDate BETWEEN COALESCE(@from, ''1753-01-01 00:00:00.000'') AND COALESCE(@to, ''9999-12-31 23:59:59.997'')
)';");

migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[UptakeRateReport](@from DateTime NULL, @to DateTime NULL)
RETURNS TABLE
AS
RETURN
(
    SELECT 
	    SexForResults,
	    Age,
	    Ethnicity,
	    hc.Postcode,
	    SmokingStatus,
        ts.IMDQuintile,
	    1 AS Invited,
	    1 AS OpenedLink,
	    CASE
		    WHEN hc.FirstHealthPriority IS NOT NULL OR hc.Height IS NOT NULL THEN 1
		    ELSE 0
	    END AS AnsweredAQuestion,
	    CASE
		    WHEN hc.Postcode IS NOT NULL THEN 1
		    ELSE 0
	    END AS [Validation],
	    CASE
		    WHEN hc.QRISK IS NOT NULL THEN 1
		    ELSE 0
	    END AS Results,
	    CASE
		    WHEN hc.FirstHealthPriorityAfterResults = ''bloodpressure'' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''improvebloodpressure'' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''bloodsugar'' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''improvebloodsugar'' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''smoking'' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''alcohol'' AND hc.DrinkingFrequency IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''weight'' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''cholesterol'' AND hc.CholesterolFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''improvecholesterol'' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''move'' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 1
		    WHEN hc.FirstHealthPriorityAfterResults = ''mental'' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 1
		    ELSE 0
	    END AS AtLeast1FollowUp,
	    CASE
		    WHEN (SELECT COUNT(1) FROM dbo.HealthCheckIntervention hci WHERE hci.HealthChecksId = hc.Id) > 0 THEN 1
		    ELSE 0
	    END AS AtLeast1Intervention,
	    CONVERT(int, hc.HealthCheckCompleted) AS HealthCheckCompleted,
	    CASE
		    WHEN hc.BloodPressureUpdated = 1 THEN 1
		    WHEN hc.BloodSugarUpdated = 1 THEN 1
		    WHEN hc.CholesterolUpdated = 1 THEN 1
		    WHEN hc.HeightAndWeightUpdated = 1 THEN 1
		    ELSE 0
	    END AS UpdatedMeasurements
    FROM dbo.HealthChecks hc 
    	LEFT JOIN Townsend ts ON ts.Postcode = hc.ValidationPostcode
	WHERE hc.CheckStartedDate BETWEEN COALESCE(@from, ''1753-01-01 00:00:00.000'') AND COALESCE(@to, ''9999-12-31 23:59:59.997'')
)';");

migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[BiometricAssessmentSuccessReport](@from DateTime NULL, @to DateTime NULL)
    RETURNS TABLE
    AS
    RETURN
    (
        SELECT 
    	    SexForResults,
    	    Age,
    	    Ethnicity,
    	    hc.Postcode,
    	    SmokingStatus,
            ts.IMDQuintile,
    	    CASE
                WHEN hc.SystolicBloodPressure IS NULL THEN 1
                ELSE 0
            END AS UnknownBP,
            CASE
                WHEN hc.TotalCholesterol IS NULL THEN 1
                ELSE 0
            END AS UnknownCholesterol,
            CASE
                WHEN hc.HeightAndWeightSkipped = 0 THEN 0
                ELSE 1
            END AS UnknownHeightAndWeight,
            CASE
                WHEN hc.BloodSugar IS NULL THEN 1
                ELSE 0
            END AS UnknownBloodSugar,
            CASE
                WHEN hc.SystolicBloodPressure IS NOT NULL AND
                    hc.TotalCholesterol IS NOT NULL AND
                    hc.Height IS NOT NULL AND
                    hc.Weight IS NOT NULL AND
                    hc.BloodSugar IS NOT NULL THEN 1
                ELSE 0
            END AS HasAllBiometrics,
			CASE
                WHEN hc.SystolicBloodPressure IS NOT NULL AND
                    hc.TotalCholesterol IS NOT NULL AND
                    hc.Height IS NOT NULL AND
                    hc.Weight IS NOT NULL AND
                    hc.BloodSugar IS NOT NULL THEN 0
                ELSE 1
            END AS NeedsAnyBiometric,
            CASE
                WHEN hc.BloodPressureUpdated = 1 AND 
                    hc.BloodPressureUpdatedDate IS NOT NULL AND 
                    DATEDIFF(day, hc.BloodPressureUpdatedDate, GETDATE()) <= 14 THEN 1
                ELSE 0
            END AS BloodPressureUpdated,
            CASE
                WHEN hc.CholesterolUpdated = 1 AND 
                    hc.CholesterolUpdatedDate IS NOT NULL AND 
                    DATEDIFF(day, hc.CholesterolUpdatedDate, GETDATE()) <= 14 THEN 1
                ELSE 0
            END AS CholesterolUpdated,
            CASE
                WHEN hc.BloodSugarUpdated = 1 AND 
                    hc.BloodSugarUpdatedDate IS NOT NULL AND 
                    DATEDIFF(day, hc.BloodSugarUpdatedDate, GETDATE()) <= 14 THEN 1
                ELSE 0
            END AS BloodSugarUpdated,
            CASE
                WHEN hc.HeightAndWeightUpdated = 1 AND 
                    hc.HeightAndWeightUpdatedDate IS NOT NULL AND 
                    DATEDIFF(day, hc.HeightAndWeightUpdatedDate, GETDATE()) <= 14 THEN 1
                ELSE 0
            END AS HeightAndWeightUpdated,
			CASE
                WHEN hc.BloodPressureUpdated = 1 AND 
                    hc.BloodPressureUpdatedDate IS NOT NULL AND 
                    DATEDIFF(day, hc.BloodPressureUpdatedDate, GETDATE()) <= 14 THEN 1
				WHEN hc.CholesterolUpdated = 1 AND 
                    hc.CholesterolUpdatedDate IS NOT NULL AND 
                    DATEDIFF(day, hc.CholesterolUpdatedDate, GETDATE()) <= 14 THEN 1
				WHEN hc.BloodSugarUpdated = 1 AND 
                    hc.BloodSugarUpdatedDate IS NOT NULL AND 
                    DATEDIFF(day, hc.BloodSugarUpdatedDate, GETDATE()) <= 14 THEN 1
				WHEN hc.HeightAndWeightUpdated = 1 AND 
                    hc.HeightAndWeightUpdatedDate IS NOT NULL AND 
                    DATEDIFF(day, hc.HeightAndWeightUpdatedDate, GETDATE()) <= 14 THEN 1
                ELSE 0
            END AS GotAtLeastOneBiometric
        FROM dbo.HealthChecks hc 
            LEFT JOIN Townsend ts ON ts.Postcode = hc.ValidationPostcode
        WHERE hc.SubmittedCheckYourAnswers = 1 AND
            hc.CheckStartedDate BETWEEN COALESCE(@from, ''1753-01-01 00:00:00.000'') AND COALESCE(@to, ''9999-12-31 23:59:59.997'')
    )';");

migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[InterventionEligibilityReport](@from DateTime NULL, @to DateTime NULL)
    RETURNS TABLE
    AS
    RETURN
    (
        SELECT
            SexForResults,
            Age,
            Ethnicity,
            hc.Postcode,
            SmokingStatus,
            ts.IMDQuintile,
            hc.HealthCheckCompleted,
            COALESCE(dbo.IsCVDHigh(hc.QRISK),0) AS HighCVD,
            COALESCE(dbo.IsDiabetesHigh(hc.QDiabetes),0) AS HighDiabetes,
            CASE
                    WHEN hc.GPPAQ = 0 THEN 1
                    ELSE 0
            END AS PhysicalActivity0,
            CASE
                    WHEN hc.GPPAQ = 1 THEN 1
                    ELSE 0
            END AS PhysicalActivity1,
            CASE
                    WHEN hc.GPPAQ = 2 THEN 1
                    ELSE 0
            END AS PhysicalActivity2,
            COALESCE(dbo.IsMentalHealthRiskHigh(hc.GAD2, hc.FeelingDown, hc.Disinterested),0) AS HighMentalHealthRisk,
            COALESCE(dbo.IsMentalHealthRiskMedium(hc.GAD2, hc.FeelingDown, hc.Disinterested),0) AS MediumMentalHealthRisk,
            COALESCE(dbo.IsAuditHigh(hc.AUDIT),0) AS HighAlcoholRisk,
            COALESCE(dbo.IsAuditMedium(hc.AUDIT),0) AS MediumAlcoholRisk,
            COALESCE(dbo.IsSmoker(hc.SmokingStatus),0) AS Smoker,
            CASE
                WHEN dbo.BodyMassIndex(hc.Height,hc.Weight) BETWEEN 25 AND 30 THEN 1
                ELSE 0
            END AS BMI25To30,
            CASE
                WHEN dbo.BodyMassIndex(hc.Height,hc.Weight) > 30 THEN 1
                ELSE 0
            END AS BMI30Plus,
            COALESCE(dbo.IsCholesterolHigh(hc.TotalCholesterol),0) AS HighCholesterol,
            CASE
                WHEN hc.SystolicBloodPressure >= 140 THEN 1
                ELSE 0
            END AS HighBloodPressure,
            COALESCE(dbo.IsBloodSugarMedium(hc.BloodSugar),0) AS MediumBloodSugar,
            COALESCE(dbo.IsBloodSugarHigh(hc.BloodSugar),0) AS HighBloodSugar,
            COALESCE(dbo.EligibleForGPFollowUp(hc.SystolicBloodPressure, hc.DiastolicBloodPressure, hc.BloodSugar, hc.Height, hc.Weight, hc.QRISK, hc.QDiabetes),0) AS EligibleForGPFollowUp,
			CASE
				WHEN dbo.IsCVDHigh(hc.QRISK) = 1 THEN 1
				WHEN dbo.IsDiabetesHigh(hc.QDiabetes) = 1 THEN 1
				WHEN dbo.IsSmoker(hc.SmokingStatus) = 1 THEN 1
				WHEN hc.SystolicBloodPressure >= 140 THEN 1
				ELSE 0
			END AS HighRiskBundle
        FROM dbo.HealthChecks hc
            LEFT JOIN Townsend ts ON ts.Postcode = hc.ValidationPostcode
        WHERE hc.QRISK IS NOT NULL AND
            hc.CheckStartedDate BETWEEN COALESCE(@from, ''1753-01-01 00:00:00.000'') AND COALESCE(@to, ''9999-12-31 23:59:59.997'')
    )';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[DetailedUptakeReport](@from DateTime NULL, @to DateTime NULL)
    RETURNS TABLE
    AS
    RETURN
    (
        SELECT
            SexForResults,
    	    Age,
    	    Ethnicity,
    	    a.Postcode,
    	    SmokingStatus,
            a.IMDQuintile,
            OpenedLink as OpenedLink,
            ClickedStartNow as ClickedStartNow,
            HeightAndWeight as HeightAndWeight,
            Sex as Sex,
            EthnicitySubmitted as EthnicitySubmitted,
            Smoking as Smoking,
            DrinksAlcohol as DrinksAlcohol,
            PhysicalActivityStarted as PhysicalActivityStarted,
            PhysicalActivityFinished as PhysicalActivityFinished,
            BloodSugar as BloodSugar,
            RiskFactors1 as RiskFactors1,
            RiskFactors2 as RiskFactors2,
            RiskFactors3 as RiskFactors3,
            BloodPressure as BloodPressure,
            Cholesterol as Cholesterol,
            [Validation] as [Validation],
            [Validation] & ~[SkippedValidation] as SuccessfulValidation,
            [SkippedValidation] as SkippedValidation,
            CheckYourAnswers as CheckYourAnswers,
            CheckYourAnswers & HeightAndWeight as CheckYourAnswersWithHeightAndWeight,
            CheckYourAnswers & ~HeightAndWeight as CheckYourAnswersWithoutHeightAndWeight,
            HeightAndWeightFollowUp as FollowUpHeightAndWeight,
            GeneratedResult as GeneratedResults,
            SubmittedResults as SubmittedHealthPrioritiesAfterResults,
            BookGPFollowUp as BookGPAppointmentFollowUp,
            FirstFollowUp as TotalFollowedUpOnFirstPriority,
            FirstFollowUp & ~BookGPFollowUp as FollowedUpOnFirstPriorityNoGPFollowUp,
            BookGPFollowUp & FirstFollowUp as FollowedUpOnFirstPriorityGPFollowUp,
            SecondFollowUp as TotalFollowedUpOnSecondPriority,
            SecondFollowUp & ~BookGPFollowUp as FollowedUpOnSecondPriorityNoGPFollowUp,
            BookGPFollowUp & SecondFollowUp as FollowedUpOnSecondPriorityGPFollowUp,
            ThirdFollowUp as TotalFollowedUpOnThreeOrMorePriorities,
            ThirdFollowUp & ~BookGPFollowUp as FollowedUpOnThirdPriorityNoGPFollowUp,
            BookGPFollowUp & ThirdFollowUp as FollowedUpOnThirdPriorityGPFollowUp,
            AtLeast1Barrier as AnyBarriers,
            AtLeast1Intervention as AnyInterventions,
            HealthCheckCompleted as Completed
        FROM (
            SELECT
                SexForResults,
                Age,
                Ethnicity,
                hc.Postcode,
                SmokingStatus,
                ts.IMDQuintile,
                1 AS OpenedLink,
                CONVERT(int, hc.ClickedStartNow) AS ClickedStartNow,
                CASE
                    WHEN hc.SmokingStatus IS NOT NULL THEN 1
                    ELSE 0
                END AS Smoking,
                CASE
                    WHEN hc.SexAtBirth IS NOT NULL THEN 1
                    ELSE 0
                END AS Sex,
                CASE
                    WHEN hc.Height IS NOT NULL THEN 1
                    ELSE 0
                END AS HeightAndWeight,
                CASE
                    WHEN hc.Ethnicity IS NOT NULL THEN 1
                    ELSE 0
                END AS EthnicitySubmitted,
                CASE
                    WHEN hc.DrinksAlcohol IS NOT NULL THEN 1
                    ELSE 0
                END AS DrinksAlcohol,
                CASE
                    WHEN hc.WorkActivity IS NOT NULL THEN 1
                    ELSE 0
                END AS PhysicalActivityStarted,
                CASE
                    WHEN hc.WalkingPace IS NOT NULL THEN 1
                    ELSE 0
                END AS PhysicalActivityFinished,
                CASE
                    WHEN hc.KnowYourHbA1c IS NOT NULL THEN 1
                    ELSE 0
                END AS BloodSugar,
                CASE
                    WHEN hc.KnowYourCholesterol IS NOT NULL THEN 1
                    ELSE 0
                END AS Cholesterol,
                CASE
                    WHEN hc.KnowYourBloodPressure IS NOT NULL THEN 1
                    ELSE 0
                END AS BloodPressure,
                CASE
                    WHEN hc.FamilyHistoryCVD IS NOT NULL THEN 1
                    ELSE 0
                END AS RiskFactors1,
                CASE
                    WHEN hc.BloodPressureTreatment IS NOT NULL THEN 1
                    ELSE 0
                END AS RiskFactors2,
                CASE
                    WHEN hc.SystemicLupusErythematosus IS NOT NULL THEN 1
                    ELSE 0
                END AS RiskFactors3,
                CASE
                    WHEN hc.Postcode IS NOT NULL THEN 1
                    ELSE 0
                END AS [Validation],
                CONVERT(int, hc.ValidationOverwritten) as SkippedValidation,
                CONVERT(int, hc.SubmittedCheckYourAnswers) as CheckYourAnswers,
                CASE
                    WHEN hc.HeightAndWeightFollowUpId IS NOT NULL THEN 1
                    ELSE 0
                END AS HeightAndWeightFollowUp,
                CASE
                    WHEN hc.QRISK IS NOT NULL THEN 1
                    ELSE 0
                END AS GeneratedResult,
                CASE
                    WHEN hc.FirstHealthPriorityAfterResults IS NOT NULL THEN 1
                    ELSE 0
                END AS SubmittedResults,
                CASE
                    WHEN hc.BookGPAppointmentFollowUpId IS NOT NULL THEN 1
                    ELSE 0
                END AS BookGPFollowUp,
                CASE
                    WHEN hc.FirstHealthPriorityAfterResults = ''bloodpressure'' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''improvebloodpressure'' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''bloodsugar'' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''improvebloodsugar'' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''smoking'' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''alcohol'' AND hc.DrinkLessFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''weight'' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''cholesterol'' AND hc.CholesterolFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''improvecholesterol'' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''move'' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults = ''mental'' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 1
                    ELSE 0
                END AS FirstFollowUp,
                CASE
                    WHEN hc.SecondHealthPriorityAfterResults = ''bloodpressure'' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''improvebloodpressure'' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''bloodsugar'' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''improvebloodsugar'' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''smoking'' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''alcohol'' AND hc.DrinkLessFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''weight'' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''cholesterol'' AND hc.CholesterolFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''improvecholesterol'' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''move'' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 1
                    WHEN hc.SecondHealthPriorityAfterResults = ''mental'' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 1
                    ELSE 0
                END AS SecondFollowUp,
                CASE
                    WHEN hc.FirstHealthPriorityAfterResults <> ''bloodpressure'' AND hc.SecondHealthPriorityAfterResults <> ''bloodpressure'' AND hc.BloodPressureFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''improvebloodpressure'' AND hc.SecondHealthPriorityAfterResults <> ''improvebloodpressure'' AND hc.ImproveBloodPressureFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''bloodsugar'' AND hc.SecondHealthPriorityAfterResults <> ''bloodsugar'' AND hc.BloodSugarFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''improvebloodsugar'' AND hc.SecondHealthPriorityAfterResults <> ''improvebloodsugar'' AND hc.ImproveBloodSugarFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''smoking'' AND hc.SecondHealthPriorityAfterResults <> ''smoking'' AND hc.QuitSmokingFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''alcohol'' AND hc.SecondHealthPriorityAfterResults <> ''alcohol'' AND hc.DrinkLessFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''weight'' AND hc.SecondHealthPriorityAfterResults <> ''weight'' AND hc.HealthyWeightFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''cholesterol'' AND hc.SecondHealthPriorityAfterResults <> ''cholesterol'' AND hc.CholesterolFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''improvecholesterol'' AND hc.SecondHealthPriorityAfterResults <> ''improvecholesterol'' AND hc.ImproveCholesterolFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''move'' AND hc.SecondHealthPriorityAfterResults <> ''move'' AND hc.MoveMoreFollowUpId IS NOT NULL THEN 1
                    WHEN hc.FirstHealthPriorityAfterResults <> ''mental'' AND hc.SecondHealthPriorityAfterResults <> ''mental'' AND hc.MentalWellbeingFollowUpId IS NOT NULL THEN 1
                    ELSE 0
                END AS ThirdFollowUp,
                CASE
                    WHEN (SELECT COUNT(1) FROM dbo.HealthCheckIntervention hci WHERE hci.HealthChecksId = hc.Id) > 0 THEN 1
                    ELSE 0
                END AS AtLeast1Intervention,
                CASE
                    WHEN (SELECT COUNT(1) FROM dbo.BarrierHealthCheck hcb WHERE hcb.HealthChecksId = hc.Id) > 0 THEN 1
                    ELSE 0
                END AS AtLeast1Barrier,
                CONVERT(int, hc.HealthCheckCompleted) AS HealthCheckCompleted
            FROM dbo.HealthChecks hc
                LEFT JOIN Townsend ts ON ts.Postcode = hc.ValidationPostcode
            WHERE hc.CheckStartedDate BETWEEN COALESCE(@from, ''1753-01-01 00:00:00.000'') AND COALESCE(@to, ''9999-12-31 23:59:59.997'')
        ) a
    )';");

            migrationBuilder.Sql(@"EXEC sp_executesql N'CREATE FUNCTION [dbo].[AnonymisedDataSet](@from DateTime NULL, @to DateTime NULL)
RETURNS TABLE
AS
RETURN
(
    SELECT
        hc.Id as Id,
		NULL as PredictedCVDRisk,
		NULL as PredictedDiabetesRisk,
		NULL as InviteDate,
		NULL as NHSHCTextLink,
		NULL as NHSHCQRCode,
		CONVERT(bit,1) AS ArrivedAtLandingPage,
		hc.ClickedStartNow AS StartedNHSHealthCheck,
		CASE
			WHEN hc.Height IS NOT NULL THEN CONVERT(bit,1)
			ELSE CONVERT(bit,0)
		END as HeightAndWeightKnown,
		hc.HeightAndWeightSkipped as HeightAndWeightNotKnown,
		hc.Height,
		hc.[Weight],
		dbo.BodyMassIndex(hc.Height,hc.[Weight]) as BMI,
		CASE
			WHEN hc.SexAtBirth = 0 THEN ''F''
			ELSE ''M''
		END AS SexAssignedAtBirth,
		NULL as PreferredSexForFirstViewOfResults, -- hc.InitialSexForResults will show this, but not enough data yet
		CASE
			WHEN hc.Ethnicity IS NULL THEN NULL
			WHEN hc.Ethnicity IN (0,1,2,3) THEN ''White''
			WHEN hc.Ethnicity IN (4,5,6,7) THEN ''Mixed''
			WHEN hc.Ethnicity IN (8,9,10,11,12) THEN ''Asian''
			WHEN hc.Ethnicity IN (13,14,15) THEN ''Black''
			WHEN hc.Ethnicity IN (16,17) THEN ''Other''
		END as Ethnicity,
		CASE
			WHEN hc.SmokingStatus IS NULL THEN NULL
			WHEN dbo.IsSmoker(hc.SmokingStatus) = 1 THEN ''Smoker''
			WHEN hc.SmokingStatus = 1 THEN ''Ex-Smoker''
			WHEN hc.SmokingStatus = 0 THEN ''Non-Smoker''
		END as SmokingStatus,
		CASE
			WHEN hc.SmokingStatus IS NULL THEN NULL
			WHEN dbo.IsSmoker(hc.SmokingStatus) = 0 THEN NULL
			WHEN hc.SmokingStatus = 2 THEN ''Light''
			WHEN hc.SmokingStatus = 3 THEN ''Moderate''
			WHEN hc.SmokingStatus = 4 THEN ''Heavy''
		END as SmokingBehaviour,
		hc.DrinksAlcohol,
		CASE
			WHEN hc.DrinkingFrequency IS NULL THEN NULL
			WHEN hc.DrinkingFrequency = 0 THEN ''Never''
			WHEN hc.DrinkingFrequency = 1 THEN ''Monthly or less''
			WHEN hc.DrinkingFrequency = 2 THEN ''Monthly or less''
			WHEN hc.DrinkingFrequency = 3 THEN ''2 to 4 times per month''
			WHEN hc.DrinkingFrequency = 4 THEN ''2 to 3 times per week''
			WHEN hc.DrinkingFrequency = 5 THEN ''4 times or more per week''
		END as DrinkingFrequency,
		CASE
			WHEN hc.TypicalDayAlcoholUnits IS NULL THEN NULL
			WHEN hc.TypicalDayAlcoholUnits = 2 THEN ''1-2''
			WHEN hc.TypicalDayAlcoholUnits = 4 THEN ''3-4''
			WHEN hc.TypicalDayAlcoholUnits = 6 THEN ''5-6''
			WHEN hc.TypicalDayAlcoholUnits = 9 THEN ''7-9''
			WHEN hc.TypicalDayAlcoholUnits = 10 THEN ''10+''
		END as DrinkingUnits,
		CASE
            WHEN hc.MSASQ IS NULL THEN NULL
            WHEN hc.MSASQ = 0 THEN ''Never''
            WHEN hc.MSASQ = 1 THEN ''Less than monthly''
            WHEN hc.MSASQ = 2 THEN ''Monthly''
            WHEN hc.MSASQ = 3 THEN ''Weekly''
            WHEN hc.MSASQ = 4 THEN ''Daily or almost daily''
        END AS MSASQ,
		CASE
            WHEN hc.UnableToStopDrinking IS NULL THEN NULL
            WHEN hc.UnableToStopDrinking = 0 THEN ''Never''
            WHEN hc.UnableToStopDrinking = 1 THEN ''Less than monthly''
            WHEN hc.UnableToStopDrinking = 2 THEN ''Monthly''
            WHEN hc.UnableToStopDrinking = 3 THEN ''Weekly''
            WHEN hc.UnableToStopDrinking = 4 THEN ''Daily or almost daily''
        END AS UnableToStop,
		CASE
            WHEN hc.FailedResponsibilityDueToAlcohol IS NULL THEN NULL
            WHEN hc.FailedResponsibilityDueToAlcohol = 0 THEN ''Never''
            WHEN hc.FailedResponsibilityDueToAlcohol = 1 THEN ''Less than monthly''
            WHEN hc.FailedResponsibilityDueToAlcohol = 2 THEN ''Monthly''
            WHEN hc.FailedResponsibilityDueToAlcohol = 3 THEN ''Weekly''
            WHEN hc.FailedResponsibilityDueToAlcohol = 4 THEN ''Daily or almost daily''
        END AS FailedResponsibilityDueToAlcohol,
		CASE
            WHEN hc.NeededToDrinkAlcoholMorningAfter IS NULL THEN NULL
            WHEN hc.NeededToDrinkAlcoholMorningAfter = 0 THEN ''Never''
            WHEN hc.NeededToDrinkAlcoholMorningAfter = 1 THEN ''Less than monthly''
            WHEN hc.NeededToDrinkAlcoholMorningAfter = 2 THEN ''Monthly''
            WHEN hc.NeededToDrinkAlcoholMorningAfter = 3 THEN ''Weekly''
            WHEN hc.NeededToDrinkAlcoholMorningAfter = 4 THEN ''Daily or almost daily''
        END AS NeededToDrinkAlcoholMorningAfter,
		CASE
            WHEN hc.GuiltAfterDrinking IS NULL THEN NULL
            WHEN hc.GuiltAfterDrinking = 0 THEN ''Never''
            WHEN hc.GuiltAfterDrinking = 1 THEN ''Less than monthly''
            WHEN hc.GuiltAfterDrinking = 2 THEN ''Monthly''
            WHEN hc.GuiltAfterDrinking = 3 THEN ''Weekly''
            WHEN hc.GuiltAfterDrinking = 4 THEN ''Daily or almost daily''
        END AS GuiltAfterDrinking,
		CASE
            WHEN hc.MemoryLossAfterDrinking IS NULL THEN NULL
            WHEN hc.MemoryLossAfterDrinking = 0 THEN ''Never''
            WHEN hc.MemoryLossAfterDrinking = 1 THEN ''Less than monthly''
            WHEN hc.MemoryLossAfterDrinking = 2 THEN ''Monthly''
            WHEN hc.MemoryLossAfterDrinking = 3 THEN ''Weekly''
            WHEN hc.MemoryLossAfterDrinking = 4 THEN ''Daily or almost daily''
        END AS MemoryLossAfterDrinking,
		CASE
            WHEN hc.InjuryCausedByDrinking IS NULL THEN NULL
            WHEN hc.InjuryCausedByDrinking = 0 THEN ''Never''
            WHEN hc.InjuryCausedByDrinking = 1 THEN ''Less than monthly''
            WHEN hc.InjuryCausedByDrinking = 2 THEN ''Monthly''
            WHEN hc.InjuryCausedByDrinking = 3 THEN ''Weekly''
            WHEN hc.InjuryCausedByDrinking = 4 THEN ''Daily or almost daily''
        END AS InjuryCausedByDrinking,
		CASE
            WHEN hc.ContactsConcernedByDrinking IS NULL THEN NULL
            WHEN hc.ContactsConcernedByDrinking = 0 THEN ''Never''
            WHEN hc.ContactsConcernedByDrinking = 1 THEN ''Less than monthly''
            WHEN hc.ContactsConcernedByDrinking = 2 THEN ''Monthly''
            WHEN hc.ContactsConcernedByDrinking = 3 THEN ''Weekly''
            WHEN hc.ContactsConcernedByDrinking = 4 THEN ''Daily or almost daily''
        END AS ContactsConcernedByDrinking,
		CASE
            WHEN hc.WorkActivity IS NULL THEN NULL
            WHEN hc.WorkActivity = 0 THEN ''Not in employment''
            WHEN hc.WorkActivity = 1 THEN ''Mostly sitting''
            WHEN hc.WorkActivity = 2 THEN ''Mostly standing or walking''
            WHEN hc.WorkActivity = 3 THEN ''Definite physical effort''
            WHEN hc.WorkActivity = 4 THEN ''Vigorous physical activity''
        END AS WorkActivity,
		CASE
            WHEN hc.PhysicalActivity IS NULL THEN NULL
            WHEN hc.PhysicalActivity = 0 THEN ''None''
            WHEN hc.PhysicalActivity = 1 THEN ''Less than 1 hour''
            WHEN hc.PhysicalActivity = 2 THEN ''Between 1 and 3 hours''
            WHEN hc.PhysicalActivity = 3 THEN ''3 hours or more''
        END AS PhysicalActivity,
		CASE
            WHEN hc.Cycling IS NULL THEN NULL
            WHEN hc.Cycling = 0 THEN ''None''
            WHEN hc.Cycling = 1 THEN ''Less than 1 hour''
            WHEN hc.Cycling = 2 THEN ''Between 1 and 3 hours''
            WHEN hc.Cycling = 3 THEN ''3 hours or more''
        END AS Cycling,
		CASE
            WHEN hc.Housework IS NULL THEN NULL
            WHEN hc.Housework = 0 THEN ''None''
            WHEN hc.Housework = 1 THEN ''Less than 1 hour''
            WHEN hc.Housework = 2 THEN ''Between 1 and 3 hours''
            WHEN hc.Housework = 3 THEN ''3 hours or more''
        END AS Housework,
		CASE
            WHEN hc.Gardening IS NULL THEN NULL
            WHEN hc.Gardening = 0 THEN ''None''
            WHEN hc.Gardening = 1 THEN ''Less than 1 hour''
            WHEN hc.Gardening = 2 THEN ''Between 1 and 3 hours''
            WHEN hc.Gardening = 3 THEN ''3 hours or more''
        END AS Gardening,
		CASE
            WHEN hc.Walking IS NULL THEN NULL
            WHEN hc.Walking = 0 THEN ''None''
            WHEN hc.Walking = 1 THEN ''Less than 1 hour''
            WHEN hc.Walking = 2 THEN ''Between 1 and 3 hours''
            WHEN hc.Walking = 3 THEN ''3 hours or more''
        END AS Walking,
		CASE
            WHEN hc.WalkingPace IS NULL THEN NULL
            WHEN hc.WalkingPace = 0 THEN ''Slow''
            WHEN hc.WalkingPace = 1 THEN ''Steady''
            WHEN hc.WalkingPace = 2 THEN ''Brisk''
            WHEN hc.WalkingPace = 3 THEN ''Fast''
        END AS WalkingPace,
		CASE
			WHEN hc.KnowYourHbA1c IS NULL THEN NULL
			WHEN hc.KnowYourHbA1c = 0 THEN ''Yes, remember the number''
			WHEN hc.KnowYourHbA1c = 1 THEN ''Yes, cannot remember the number''
			WHEN hc.KnowYourHbA1c = 2 THEN ''No''
		END AS KnowYourHbA1c,
		CASE
			WHEN hc.BloodSugar IS NULL THEN NULL
			WHEN dbo.IsBloodSugarMedium(hc.BloodSugar) = 1 THEN ''Slightly High (42 - 47)''
			WHEN hc.BloodSugar >= 48 AND hc.BloodSugar < 100 THEN ''High (48 - 100)''
			WHEN hc.BloodSugar > 100 THEN ''Severe (101+)''
			ELSE ''Healthy (< 42)''
		END as HbA1cMeasure,
		CASE
			WHEN hc.KnowYourHbA1c IS NULL THEN NULL
			ELSE hc.WentToFindBloodSugar
		END AS HbA1cMeasureFound,
		CASE
			WHEN hc.KnowYourBloodPressure IS NULL THEN NULL
			WHEN hc.KnowYourBloodPressure = 0 THEN ''Yes, remember the number''
			WHEN hc.KnowYourBloodPressure = 1 THEN ''Yes, cannot remember the number''
			WHEN hc.KnowYourBloodPressure = 2 THEN ''No''
		END AS KnowYourBloodPressure,
		CASE
			WHEN hc.SystolicBloodPressure IS NULL THEN NULL
			WHEN hc.SystolicBloodPressure < 90 THEN ''Low (< 90)''
			WHEN hc.SystolicBloodPressure >= 180 THEN ''Severe (180+)''
			WHEN hc.SystolicBloodPressure BETWEEN 140 AND 179 THEN ''High (between 140 and 180)''
			WHEN hc.SystolicBloodPressure BETWEEN 120 AND 139 THEN ''Slightly High (between 120 and 140)''
			ELSE ''Healthy (between 90 and 120)''
		END as BloodPressureMeasureSys,
		CASE
			WHEN hc.DiastolicBloodPressure IS NULL THEN NULL
			WHEN hc.DiastolicBloodPressure < 60 THEN ''Low (< 60)''
			WHEN hc.DiastolicBloodPressure >= 110 THEN ''Severe (> 110)''
			WHEN hc.DiastolicBloodPressure BETWEEN 90 AND 109 THEN ''High (between 90 and 110)''
			WHEN hc.DiastolicBloodPressure BETWEEN 80 AND 89 THEN ''Slightly High (between 80 and 90)''
			ELSE ''Healthy (between 60 and 80)''
		END as BloodPressureMeasureDias,
		CASE
			WHEN hc.KnowYourBloodPressure IS NULL THEN NULL
			ELSE hc.WentToFindBloodPressure
		END AS BloodPressureMeasureFound,
		CASE
			WHEN hc.KnowYourCholesterol IS NULL THEN NULL
			WHEN hc.KnowYourCholesterol = 0 THEN ''Yes, remember the number''
			WHEN hc.KnowYourCholesterol = 1 THEN ''Yes, cannot remember the number''
			WHEN hc.KnowYourCholesterol = 2 THEN ''No''
		END AS KnowYourCholesterol,
		CASE
			WHEN hc.TotalCholesterol IS NULL THEN NULL
			WHEN hc.TotalCholesterol <= 5 THEN ''Low (< 5.0)''
			WHEN hc.TotalCholesterol >= 7.5 THEN ''High ( 7.5+)''
			ELSE ''Medium (between 5 and 7.5)''
		END as CholesterolMeasureTotal,
		CASE
			WHEN hc.HDLCholesterol IS NULL THEN NULL
			WHEN hc.SexForResults = 0 AND hc.HDLCholesterol >= 21.6 THEN ''High (> 21.6, Female)''
			WHEN hc.SexForResults = 1 AND hc.HDLCholesterol >= 18 THEN ''High (> 18, Male)''
			ELSE ''Healthy''
		END as CholesterolMeasureHDL,
		CASE
			WHEN hc.KnowYourCholesterol IS NULL THEN NULL
			ELSE hc.WentToFindCholesterol
		END AS CholesterolMeasureFound,
		hc.FeelingDown,
		hc.Disinterested,
		CASE
			WHEN hc.Anxious IS NULL THEN NULL
            WHEN hc.Anxious = 0 THEN ''Not at all''
            WHEN hc.Anxious = 1 THEN ''Several days''
            WHEN hc.Anxious = 2 THEN ''More than half the days''
            WHEN hc.Anxious = 3 THEN ''Every day''
        END AS Anxious,
		CASE
            WHEN hc.[Control] IS NULL THEN NULL
            WHEN hc.[Control] = 0 THEN ''Not at all''
            WHEN hc.[Control] = 1 THEN ''Several days''
            WHEN hc.[Control] = 2 THEN ''More than half the days''
            WHEN hc.[Control] = 3 THEN ''Every day''
        END AS Worrying,
		hc.SkipMentalHealthQuestions,
		CASE
			WHEN hc.EmailAddress IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END as EmailProvided,
		CASE
			WHEN hc.PhoneNumber IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END as PhoneNumberProvided,
		CASE
			WHEN hc.SmsNumber IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END as SmsNumberProvided,
		ts.IMDQuintile,
		ts.Lsoa,
        hc.Age,
        CASE
            WHEN hc.Postcode IS NULL THEN CONVERT(bit,0)
            WHEN hc.Postcode IS NOT NULL AND hc.ValidationOverwritten = 1 THEN CONVERT(bit,0)
            ELSE CONVERT(bit,1)
        END AS [ValidationConfirmed],
		CASE
            WHEN hc.Postcode IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
        END AS [ValidationCompleted],
		hc.GPSurgery,
		hc.SubmittedCheckYourAnswers,
		-- Follow Up Begins Here
		CASE
            WHEN hc.HeightAndWeightFollowUpId IS NOT NULL THEN CONVERT(bit,1)
            ELSE CONVERT(bit,0)
        END AS HeightAndWeightFollowUp,
		hc.ReceivedHealthCheckIncompleteEmail,
		hwf.ImportantToChange as HeightAndWeightImportance,
		hwf.ConfidentToChange as HeightAndWeightConfidence,
		CASE
			WHEN hwf.SelectedOption IS NULL THEN NULL
			WHEN hwf.SelectedOption = ''kiosk'' THEN ''Health Kiosk''
			WHEN hwf.SelectedOption = ''leisurecentre'' THEN ''Local Leisure Centre''
			WHEN hwf.SelectedOption = ''pharmacy'' THEN ''Pharmacy''
			WHEN hwf.SelectedOption = ''home'' THEN ''At home''
		END AS HeightAndWeightMethodOfMeasurement,
		hwf.DoYouHavePlans AS HeightAndWeightIntentions,
		hwf.SetADate AS HeightAndWeightSetADate,
		hwf.BeReminded AS HeightAndWeightReminderRequest,
		-- Follow Up Ends Here
		CASE
			WHEN hc.QRISK IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS ViewsResults,

        CASE
            WHEN hc.QRISK IS NULL THEN NULL
            WHEN hc.QRISK < 10 THEN ''Low''
			WHEN hc.QRISK < 20 AND hc.QRISK >= 10 THEN ''Medium''
			WHEN hc.QRISK < 30 AND hc.QRISK >= 20 THEN ''High''
			WHEN hc.QRISK >= 30 THEN ''Very High''
        END AS [CVDRiskCategory],
		CASE
			WHEN hc.QRISK IS NULL THEN NULL
			WHEN dbo.IsSmoker(hc.SmokingStatus) = 1 THEN ''High''
			WHEN dbo.IsAuditHigh(hc.[AUDIT]) = 1 THEN ''High''
			WHEN dbo.IsAuditMedium(hc.[AUDIT]) = 1 THEN ''Medium''
			WHEN hc.GPPAQ = 2 THEN ''Medium''
			WHEN hc.GPPAQ < 2 THEN ''High''
			WHEN dbo.BodyMassIndex(hc.Height,hc.Weight) < 16 THEN ''High''
			WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 1 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 23 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) < 27.5 THEN ''Medium''
            WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 0 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 25 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) < 30 THEN ''Medium''
			WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 1 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 27.5 THEN ''High''
            WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 0 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 30 THEN ''High''
			WHEN dbo.IsBloodPressureHigh(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 1 THEN ''High''
			WHEN dbo.IsBloodPressureHigh(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 0 AND (hc.SystolicBloodPressure BETWEEN 120 AND 139 OR hc.DiastolicBloodPressure BETWEEN 80 AND 89) THEN ''Medium''
			WHEN hc.TotalCholesterol IS NOT NULL AND (hc.QRISK >= 10 OR hc.TotalCholesterol >= 7.5 ) THEN ''High''
			WHEN hc.TotalCholesterol IS NOT NULL AND hc.QRISK < 10 AND hc.TotalCholesterol < 7.5 THEN ''Medium''
			WHEN hc.QDiabetes >= 5.6 THEN ''High''
			WHEN hc.BloodSugar IS NOT NULL AND hc.BloodSugar >= 48 THEN ''High''
			WHEN hc.BloodSugar IS NOT NULL AND hc.BloodSugar >= 42 THEN ''Medium''
			WHEN hc.FamilyHistoryCVD = 1 THEN ''High''
			WHEN hc.ChronicKidneyDisease = 1 THEN ''High''
			WHEN hc.AtrialFibrillation = 1 THEN ''High''
			WHEN hc.BloodPressureTreatment = 1 THEN ''High''
			WHEN hc.Migraines = 1 THEN ''High''
			WHEN hc.RheumatoidArthritis = 1 THEN ''High''
			WHEN hc.SystemicLupusErythematosus = 0 THEN ''High''
			WHEN hc.SevereMentalIllness = 0 THEN ''High''
			WHEN hc.AtypicalAntipsychoticMedication = 0 THEN ''High''
			ELSE ''Low''
		END AS CVDBehaviours,
		hc.QRISK as CVDRiskScore,
		hc.HeartAge,
        CASE
            WHEN hc.HeartAge IS NULL THEN NULL    
            WHEN dbo.IsHeartAgeHigh(hc.HeartAge,hc.Age) = 0 THEN ''Low''
			WHEN dbo.IsHeartAgeHigh(hc.HeartAge,hc.Age) = 1 THEN ''High''
        END AS [HeartAgeRiskCategory],
        CASE
            WHEN hc.QDiabetes IS NULL THEN NULL
            WHEN hc.QDiabetes < 5.6 THEN ''Low''
            ELSE ''High''
        END AS [QDiabetesScoreCategory],
		hc.QDiabetes,
        CASE
            WHEN hc.[AUDIT] IS NULL THEN NULL
            WHEN hc.[AUDIT] < 8 THEN ''Low''
			WHEN hc.[AUDIT] >= 8 AND hc.[AUDIT] <= 15 THEN ''Medium''
			WHEN hc.[AUDIT] >= 16 THEN ''High''
        END AS Alcohol,
        CASE
            WHEN hc.[GPPAQ] IS NULL THEN NULL
			WHEN hc.Walking <= 1 AND hc.[GPPAQ] = 0 THEN ''Inactive''
            WHEN hc.Walking > 1 AND hc.[GPPAQ] = 0 THEN ''Inactive + Walking''
			WHEN hc.Walking <= 1 AND hc.[GPPAQ] = 1 THEN ''Moderately inactive''
            WHEN hc.Walking > 1 AND hc.[GPPAQ] = 1 THEN ''Moderately inactive + Walking''
			WHEN hc.Walking <= 1 AND hc.[GPPAQ] = 2 THEN ''Moderately active''
            WHEN hc.Walking > 1 AND hc.[GPPAQ] = 2 THEN ''Moderately active + Walking''
			WHEN hc.[GPPAQ] = 3 THEN ''Active''
        END AS PhysicalActivityLevel,
        CASE
            WHEN hc.Height IS NULL OR hc.Weight IS NULL THEN NULL
            WHEN dbo.BodyMassIndex(hc.Height,hc.Weight) < 16 THEN ''Severely Underweight''
			WHEN dbo.BodyMassIndex(hc.Height,hc.Weight) >= 16 AND dbo.BodyMassIndex(hc.Height,hc.Weight) < 18.5 THEN ''Underweight''
			WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 1 AND dbo.BodyMassIndex(hc.Height,hc.Weight) >= 18.5 AND dbo.BodyMassIndex(hc.Height,hc.Weight) < 23 THEN ''Healthy''
            WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 0 AND dbo.BodyMassIndex(hc.Height,hc.Weight) >= 18.5 AND dbo.BodyMassIndex(hc.Height,hc.Weight) < 25 THEN ''Healthy''
			WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 1 AND dbo.BodyMassIndex(hc.Height,hc.Weight) >= 23 AND dbo.BodyMassIndex(hc.Height,hc.Weight) < 27.5 THEN ''Overweight''
            WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 0 AND dbo.BodyMassIndex(hc.Height,hc.Weight) >= 25 AND dbo.BodyMassIndex(hc.Height,hc.Weight) < 30 THEN ''Overweight''
			WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 1 AND dbo.BodyMassIndex(hc.Height,hc.Weight) >= 27.5 THEN ''Obese''
            WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 0 AND dbo.BodyMassIndex(hc.Height,hc.Weight) >= 30 THEN ''Obese''
        END AS [BMICategory],
		CASE
            WHEN dbo.BPRiskCategory(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) IS NULL THEN NULL
            WHEN dbo.BPRiskCategory(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 0 THEN ''Moderate (Low)''
			WHEN dbo.BPRiskCategory(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 1 THEN ''Healthy''
			WHEN dbo.BPRiskCategory(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 2 THEN ''Moderate (High)''
			WHEN dbo.BPRiskCategory(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 3 THEN ''High''
			WHEN dbo.BPRiskCategory(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 4 THEN ''Very High''
        END AS [BloodPressureCategory],
        CASE
            WHEN hc.BloodSugar IS NULL THEN NULL
            WHEN hc.BloodSugar < 42 THEN ''Normal''
			WHEN hc.BloodSugar >= 42 AND hc.BloodSugar < 47 AND QDiabetes < 5.6 THEN ''At risk''
			WHEN hc.BloodSugar >= 42 AND hc.BloodSugar < 47 AND QDiabetes >= 5.6 THEN ''High risk''
			WHEN hc.BloodSugar >= 47 AND hc.BloodSugar < 100 THEN ''Possible Diabetes''
			WHEN hc.BloodSugar >= 100 THEN ''Extremely high''
        END AS [HbA1cCategory],
        CASE
            WHEN dbo.CholesterolRiskCategory(hc.TotalCholesterol) IS NULL THEN NULL
            WHEN dbo.CholesterolRiskCategory(hc.TotalCholesterol) = 0 THEN ''Healthy''
			WHEN dbo.CholesterolRiskCategory(hc.TotalCholesterol) = 1 AND hc.QRisk < 10 THEN ''Moderate''
			WHEN dbo.CholesterolRiskCategory(hc.TotalCholesterol) = 2 OR hc.QRisk >=10 THEN ''Poor''
        END AS [CholesterolCategory],
		dbo.RiskFactors(hc.Id) AS ModifiableFactorsCount,
		CASE
			WHEN hc.QRISK IS NULL THEN NULL
			WHEN hc.QRISK > 10 THEN CONVERT(bit,1)
			WHEN hc.QDiabetes >= 5.6 THEN CONVERT(bit,1)
			WHEN dbo.IsMentalHealthRiskHigh(hc.GAD2, hc.FeelingDown, hc.Disinterested) = 1 THEN CONVERT(bit,1)
			WHEN dbo.IsAuditHigh(hc.[AUDIT]) = 1 THEN CONVERT(bit,1)
			WHEN dbo.IsAuditMedium(hc.[AUDIT]) = 1 THEN CONVERT(bit,1)
			WHEN dbo.BodyMassIndex(hc.Height,hc.[Weight]) > 16 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) < 18.5 THEN CONVERT(bit,1)
			WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 1 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 27.5 THEN CONVERT(bit,1)
            WHEN dbo.IsHighBmiRiskEthnicity(hc.Ethnicity) = 0 AND dbo.BodyMassIndex(hc.Height,hc.[Weight]) >= 30 THEN CONVERT(bit,1)
			WHEN dbo.IsBloodPressureHigh(hc.SystolicBloodPressure, hc.DiastolicBloodPressure) = 1 THEN CONVERT(bit,1)
			WHEN hc.TotalCholesterol IS NOT NULL AND (hc.QRISK >= 10 OR hc.TotalCholesterol >= 7.5 ) THEN CONVERT(bit,1)
			WHEN hc.TotalCholesterol IS NOT NULL AND hc.QRISK < 10 AND hc.TotalCholesterol < 7.5 THEN CONVERT(bit,1)
			WHEN hc.QDiabetes >= 5.6 THEN CONVERT(bit,1)
			WHEN hc.BloodSugar IS NOT NULL AND hc.BloodSugar >= 42 THEN CONVERT(bit,1)
			ELSE CONVERT(bit,0)
		END as NonUrgentGPFollowUp,
		CASE
			WHEN hc.QRISK IS NULL THEN NULL
			WHEN dbo.BodyMassIndex(hc.Height,hc.Weight) < 16 THEN CONVERT(bit,1)
			WHEN hc.SystolicBloodPressure > 180 OR hc.DiastolicBloodPressure > 110 THEN CONVERT(bit,1)
			WHEN hc.BloodSugar IS NOT NULL AND hc.BloodSugar >= 100 THEN CONVERT(bit,1)
			ELSE CONVERT(bit,0)
		END as UrgentGPFollowUp,
		CASE
            WHEN hc.FirstHealthPriorityAfterResults = ''bloodpressure'' THEN ''Learn what my blood pressure is, and what this means for me''
            WHEN hc.FirstHealthPriorityAfterResults = ''improvebloodpressure'' THEN ''Improve my blood pressure''
            WHEN hc.FirstHealthPriorityAfterResults = ''bloodsugar'' THEN ''Learn what my blood sugar level is, and what this means for me''
            WHEN hc.FirstHealthPriorityAfterResults = ''improvebloodsugar'' THEN ''Improve my blood sugar level''
            WHEN hc.FirstHealthPriorityAfterResults = ''smoking'' THEN ''Stop smoking''
            WHEN hc.FirstHealthPriorityAfterResults = ''alcohol'' THEN ''Drink less alcohol''
            WHEN hc.FirstHealthPriorityAfterResults = ''weight'' THEN ''Healthy weight''
            WHEN hc.FirstHealthPriorityAfterResults = ''cholesterol'' THEN ''Learn what my cholesterol levels are, and what this means for me''
            WHEN hc.FirstHealthPriorityAfterResults = ''improvecholesterol'' THEN ''Improve my cholesterol levels''
            WHEN hc.FirstHealthPriorityAfterResults = ''move'' THEN ''Move more''
            WHEN hc.FirstHealthPriorityAfterResults = ''mental'' THEN ''Mental wellbeing''
            ELSE NULL
        END AS [FirstHealthPriorityAfterResults],
		CASE
            WHEN hc.SecondHealthPriorityAfterResults = ''bloodpressure'' THEN ''Learn what my blood pressure is, and what this means for me''
            WHEN hc.SecondHealthPriorityAfterResults = ''improvebloodpressure'' THEN ''Improve my blood pressure''
            WHEN hc.SecondHealthPriorityAfterResults = ''bloodsugar'' THEN ''Learn what my blood sugar level is, and what this means for me''
            WHEN hc.SecondHealthPriorityAfterResults = ''improvebloodsugar'' THEN ''Improve my blood sugar level''
            WHEN hc.SecondHealthPriorityAfterResults = ''smoking'' THEN ''Stop smoking''
            WHEN hc.SecondHealthPriorityAfterResults = ''alcohol'' THEN ''Drink less alcohol''
            WHEN hc.SecondHealthPriorityAfterResults = ''weight'' THEN ''Healthy weight''
            WHEN hc.SecondHealthPriorityAfterResults = ''cholesterol'' THEN ''Learn what my cholesterol levels are, and what this means for me''
            WHEN hc.SecondHealthPriorityAfterResults = ''improvecholesterol'' THEN ''Improve my cholesterol levels''
            WHEN hc.SecondHealthPriorityAfterResults = ''move'' THEN ''Move more''
            WHEN hc.SecondHealthPriorityAfterResults = ''mental'' THEN ''Mental wellbeing''
            ELSE NULL
        END AS [SecondHealthPriorityAfterResults],
		CASE
			WHEN hc.QRISK IS NULL THEN NULL
            WHEN hc.BookGPAppointmentFollowUpId IS NOT NULL THEN ''Follow-up - visit your GP clinic''
            WHEN hc.FirstHealthPriorityAfterResults IS NOT NULL THEN ''Follow-up - first health priority''
        END AS FirstFollowUpOptionSelected,
		CASE
            WHEN gpf.BeReminded IS NOT NULL THEN CONVERT(bit,1)
            ELSE CONVERT(bit,0)
        END AS BookGPFollowUpPageCompleted,
		gpf.ImportantToChange as GPFollowUpImportance,
		gpf.ConfidentToChange as GPFollowUpConfidence,
		gpf.DoYouHavePlans AS GPFollowUpIntentions,
		gpf.SetADate AS GPFollowUpSetADate,
		gpf.BeReminded AS GPFollowUpReminderRequest,
		CASE
			WHEN gpf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = gpf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS GPFollowUpNumberOfBarriers,
		CASE
			WHEN gpf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = gpf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS GPFollowUpNumberOfActions,

		CASE
			WHEN bpf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS MeasureBloodPressureFollowUpPageCompleted,
		bpf.ImportantToChange as MeasureBloodPressureFollowUpImportance,
		bpf.ConfidentToChange as MeasureBloodPressureFollowUpConfidence,
		CASE
			WHEN bpf.SelectedOption IS NULL THEN NULL
			WHEN bpf.SelectedOption = ''kiosk'' THEN ''Health Kiosk''
			WHEN bpf.SelectedOption = ''leisurecentre'' THEN ''Local Leisure Centre''
			WHEN bpf.SelectedOption = ''pharmacy'' THEN ''Pharmacy''
			WHEN bpf.SelectedOption = ''home'' THEN ''At home''
		END AS MeasureBloodPressureFollowUpMethodOfMeasurement,
		bpf.DoYouHavePlans AS MeasureBloodPressureFollowUpIntentions,
		bpf.SetADate AS MeasureBloodPressureFollowUpSetADate,
		bpf.BeReminded AS MeasureBloodPressureFollowUpReminderRequest,
		CASE
			WHEN bpf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = bpf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS MeasureBloodPressureFollowUpNumberOfBarriers,
		CASE
			WHEN bpf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = bpf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS MeasureBloodPressureFollowUpNumberOfActions,

		CASE
			WHEN ibpf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS ImproveBloodPressureFollowUpPageCompleted,
		ibpf.ImportantToChange as ImproveBloodPressureFollowUpImportance,
		ibpf.ConfidentToChange as ImproveBloodPressureFollowUpConfidence,
		ibpf.DoYouHavePlans AS ImproveBloodPressureFollowUpIntentions,
		ibpf.SetADate AS ImproveBloodPressureFollowUpSetADate,
		ibpf.BeReminded AS ImproveBloodPressureFollowUpReminderRequest,
		CASE
			WHEN ibpf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = ibpf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS ImproveBloodPressureFollowUpNumberOfBarriers,
		CASE
			WHEN ibpf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = ibpf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS ImproveBloodPressureFollowUpNumberOfActions,

		CASE
			WHEN bsf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS MeasureBloodSugarFollowUpPageCompleted,
		bsf.ImportantToChange as MeasureBloodSugarFollowUpImportance,
		bsf.ConfidentToChange as MeasureBloodSugarFollowUpConfidence,
		CASE
			WHEN bsf.SelectedOption IS NULL THEN NULL
			WHEN bsf.SelectedOption = ''kiosk'' THEN ''Health Kiosk''
			WHEN bsf.SelectedOption = ''leisurecentre'' THEN ''Local Leisure Centre''
			WHEN bsf.SelectedOption = ''pharmacy'' THEN ''Pharmacy''
			WHEN bsf.SelectedOption = ''home'' THEN ''At home''
		END AS MeasureBloodSugarFollowUpMethodOfMeasurement,
		bsf.DoYouHavePlans AS MeasureBloodSugarFollowUpIntentions,
		bsf.SetADate AS MeasureBloodSugarFollowUpSetADate,
		bsf.BeReminded AS MeasureBloodSugarFollowUpReminderRequest,
		CASE
			WHEN bsf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = bsf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS MeasureBloodSugarFollowUpNumberOfBarriers,
		CASE
			WHEN bsf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = bsf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS MeasureBloodSugarFollowUpNumberOfActions,

		CASE
			WHEN ibsf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS ImproveBloodSugarFollowUpPageCompleted,
		ibsf.ImportantToChange as ImproveBloodSugarFollowUpImportance,
		ibsf.ConfidentToChange as ImproveBloodSugarFollowUpConfidence,
		ibsf.DoYouHavePlans AS ImproveBloodSugarFollowUpIntentions,
		ibsf.SetADate AS ImproveBloodSugarFollowUpSetADate,
		ibsf.BeReminded AS ImproveBloodSugarFollowUpReminderRequest,
		CASE
			WHEN ibsf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = ibsf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS ImproveBloodSugarFollowUpNumberOfBarriers,
		CASE
			WHEN ibsf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = ibsf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS ImproveBloodSugarFollowUpNumberOfActions,

		CASE
			WHEN chf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS MeasureCholesterolFollowUpPageCompleted,
		chf.ImportantToChange as MeasureCholesterolFollowUpImportance,
		chf.ConfidentToChange as MeasureCholesterolFollowUpConfidence,
		CASE
			WHEN chf.SelectedOption IS NULL THEN NULL
			WHEN chf.SelectedOption = ''kiosk'' THEN ''Health Kiosk''
			WHEN chf.SelectedOption = ''leisurecentre'' THEN ''Local Leisure Centre''
			WHEN chf.SelectedOption = ''pharmacy'' THEN ''Pharmacy''
			WHEN chf.SelectedOption = ''home'' THEN ''At home''
		END AS MeasureCholesterolFollowUpMethodOfMeasurement,
		chf.DoYouHavePlans AS MeasureCholesterolFollowUpIntentions,
		chf.SetADate AS MeasureCholesterolFollowUpSetADate,
		chf.BeReminded AS MeasureCholesterolFollowUpReminderRequest,
		CASE
			WHEN chf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = chf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS MeasureCholesterolFollowUpNumberOfBarriers,
		CASE
			WHEN chf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = chf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS MeasureCholesterolFollowUpNumberOfActions,

		CASE
			WHEN ichf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS ImproveCholesterolFollowUpPageCompleted,
		ichf.ImportantToChange as ImproveCholesterolFollowUpImportance,
		ichf.ConfidentToChange as ImproveCholesterolFollowUpConfidence,
		ichf.DoYouHavePlans AS ImproveCholesterolFollowUpIntentions,
		ichf.SetADate AS ImproveCholesterolFollowUpSetADate,
		ichf.BeReminded AS ImproveCholesterolFollowUpReminderRequest,
		CASE
			WHEN ichf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = ichf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS ImproveCholesterolFollowUpNumberOfBarriers,
		CASE
			WHEN ichf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = ichf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS ImproveCholesterolFollowUpNumberOfActions,

		CASE
			WHEN smf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS StopSmokingFollowUpPageCompleted,
		smf.ImportantToChange as StopSmokingFollowUpImportance,
		smf.ConfidentToChange as StopSmokingFollowUpConfidence,
		smf.DoYouHavePlans AS StopSmokingFollowUpIntentions,
		smf.SetADate AS StopSmokingFollowUpSetADate,
		smf.BeReminded AS StopSmokingFollowUpReminderRequest,
		CASE
			WHEN smf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = smf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS StopSmokingFollowUpNumberOfBarriers,
		CASE
			WHEN smf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = smf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS StopSmokingFollowUpNumberOfActions,

		CASE
			WHEN alf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS QuitDrinkingFollowUpPageCompleted,
		alf.ImportantToChange as QuitDrinkingFollowUpImportance,
		alf.ConfidentToChange as QuitDrinkingFollowUpConfidence,
		alf.DoYouHavePlans AS QuitDrinkingFollowUpIntentions,
		alf.SetADate AS QuitDrinkingFollowUpSetADate,
		alf.BeReminded AS QuitDrinkingFollowUpReminderRequest,
		CASE
			WHEN alf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = alf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS QuitDrinkingFollowUpNumberOfBarriers,
		CASE
			WHEN alf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = alf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS QuitDrinkingFollowUpNumberOfActions,

		CASE
			WHEN wtf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS HealthyWeightFollowUpPageCompleted,
		wtf.ImportantToChange as HealthyWeightFollowUpImportance,
		wtf.ConfidentToChange as HealthyWeightFollowUpConfidence,
		wtf.DoYouHavePlans AS HealthyWeightFollowUpIntentions,
		wtf.SetADate AS HealthyWeightFollowUpSetADate,
		wtf.BeReminded AS HealthyWeightFollowUpReminderRequest,
		CASE
			WHEN wtf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = wtf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS HealthyWeightFollowUpNumberOfBarriers,
		CASE
			WHEN wtf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = wtf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS HealthyWeightFollowUpNumberOfActions,

		CASE
			WHEN mvf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS MoveMoreFollowUpPageCompleted,
		mvf.ImportantToChange as MoveMoreFollowUpImportance,
		mvf.ConfidentToChange as MoveMoreFollowUpConfidence,
		mvf.DoYouHavePlans AS MoveMoreFollowUpIntentions,
		mvf.SetADate AS MoveMoreFollowUpSetADate,
		mvf.BeReminded AS MoveMoreFollowUpReminderRequest,
		CASE
			WHEN mvf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = mvf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS MoveMoreFollowUpNumberOfBarriers,
		CASE
			WHEN mvf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = mvf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS MoveMoreFollowUpNumberOfActions,

		CASE
			WHEN mhf.BeReminded IS NULL THEN CONVERT(bit,0)
			ELSE CONVERT(bit,1)
		END AS MentalHealthFollowUpPageCompleted,
		mhf.ImportantToChange as MentalHealthFollowUpImportance,
		mhf.ConfidentToChange as MentalHealthFollowUpConfidence,
		mhf.DoYouHavePlans AS MentalHealthFollowUpIntentions,
		mhf.SetADate AS MentalHealthFollowUpSetADate,
		mhf.BeReminded AS MentalHealthFollowUpReminderRequest,
		CASE
			WHEN mhf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.BarrierHealthCheck bhc 
					INNER JOIN dbo.Barriers b ON bhc.ChosenBarriersId = b.Id
				WHERE b.Category = mhf.[Type] AND bhc.HealthChecksId = hc.Id
			)
		END AS MentalHealthFollowUpNumberOfBarriers,
		CASE
			WHEN mhf.BeReminded IS NULL THEN NULL
			ELSE (SELECT COUNT(1) 
				FROM dbo.HealthCheckIntervention ihc
					INNER JOIN dbo.Interventions i ON ihc.ChosenInterventionsId = i.Id
				WHERE i.Category = mhf.[Type] AND ihc.HealthChecksId = hc.Id
			)
		END AS MentalHealthFollowUpNumberOfActions,

		CASE
            WHEN hc.QRISK is NULL THEN NULL
            WHEN (SELECT COUNT(1) FROM dbo.HealthCheckIntervention hci WHERE hci.HealthChecksId = hc.Id AND hci.ChosenInterventionsId IN (11,28,45,56,60,112,124,136)) > 0 THEN CONVERT(bit,1)
            ELSE CONVERT(bit,0)
        END AS [PatientEligibleForEveryoneHealthReferral],
		CASE
			WHEN hc.PrefersToContactEveryoneHealth IS NULL THEN NULL
			WHEN hc.PrefersToContactEveryoneHealth = 1 THEN ''Self-referral''
			WHEN hc.EveryoneHealthConsent = 1 THEN ''Contacted''
		END AS MethodOfContactForEveryoneHealth,
		CASE
			WHEN hc.QRISK IS NULL THEN NULL
			WHEN bpf.BeReminded IS NOT NULL THEN CONVERT(bit, 1)
			WHEN bsf.BeReminded IS NOT NULL THEN CONVERT(bit, 1)
			WHEN hwf.BeReminded IS NOT NULL THEN CONVERT(bit, 1)
			WHEN chf.BeReminded IS NOT NULL THEN CONVERT(bit, 1)
			ELSE CONVERT(bit, 0)
		END AS RecommendedBiometricAssessments,
		hc.HealthCheckCompleted,
		hc.ContactInformationUpdatedOnComplete,
		hc.HeightAndWeightUpdated,
		hc.BloodSugarUpdated,
		hc.BloodPressureUpdated,
		hc.CholesterolUpdated,
		hc.FamilyHistoryDiabetes,
		hc.FamilyHistoryCVD,
		hc.ChronicKidneyDisease,
		hc.AtrialFibrillation,
		hc.BloodPressureTreatment
    FROM HealthChecks hc
		LEFT JOIN Townsend ts ON ts.Postcode = hc.ValidationPostcode
		LEFT JOIN FollowUp hwf ON hc.HeightAndWeightFollowUpId = hwf.Id
		LEFT JOIN FollowUp bpf ON hc.BloodPressureFollowUpId = bpf.Id
		LEFT JOIN FollowUp ibpf ON hc.ImproveBloodPressureFollowUpId = ibpf.Id
		LEFT JOIN FollowUp bsf ON hc.BloodSugarFollowUpId = bsf.Id
		LEFT JOIN FollowUp ibsf ON hc.ImproveBloodSugarFollowUpId = ibsf.Id
		LEFT JOIN FollowUp smf ON hc.QuitSmokingFollowUpId = smf.Id
		LEFT JOIN FollowUp alf ON hc.DrinkLessFollowUpId = alf.Id
		LEFT JOIN FollowUp wtf ON hc.HealthyWeightFollowUpId = wtf.Id
		LEFT JOIN FollowUp chf ON hc.CholesterolFollowUpId = chf.Id
		LEFT JOIN FollowUp ichf ON hc.ImproveCholesterolFollowUpId = ichf.Id
		LEFT JOIN FollowUp mvf ON hc.MoveMoreFollowUpId = mvf.Id
		LEFT JOIN FollowUp mhf ON hc.MentalWellbeingFollowUpId = mhf.Id
		LEFT JOIN FollowUp gpf ON hc.BookGPAppointmentFollowUpId = gpf.Id
    WHERE hc.CheckStartedDate BETWEEN COALESCE(@from, ''1753-01-01 00:00:00.000'') AND COALESCE(@to, ''9999-12-31 23:59:59.997'')
)';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarrierHealthCheck");

            migrationBuilder.DropTable(
                name: "BloodKitRequests");

            migrationBuilder.DropTable(
                name: "CustomBarriers");

            migrationBuilder.DropTable(
                name: "HealthCheckIntervention");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Townsend");

            migrationBuilder.DropTable(
                name: "ValidationErrors");

            migrationBuilder.DropTable(
                name: "Interventions");

            migrationBuilder.DropTable(
                name: "HealthChecks");

            migrationBuilder.DropTable(
                name: "Barriers");

            migrationBuilder.DropTable(
                name: "FollowUp");

            migrationBuilder.DropTable(
                name: "IdentityVariants");

            migrationBuilder.Sql("DROP FUNCTION dbo.DetailedUptakeReport");

            migrationBuilder.Sql("DROP FUNCTION dbo.BiometricAssessmentSuccessReport");

            migrationBuilder.Sql("DROP FUNCTION dbo.InterventionEligibilityReport");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsSmoker");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsAuditHigh");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsAuditMedium");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsMentalHealthRiskMedium");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsMentalHealthRiskHigh");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsBloodSugarHigh");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsBloodSugarMedium");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsBloodPressureHigh");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsCholesterolHigh");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsDiabetesHigh");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsCVDHigh");

            migrationBuilder.Sql("DROP FUNCTION dbo.AnonymisedDataSet");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsHeartAgeHigh");

            migrationBuilder.Sql("DROP FUNCTION dbo.BodyMassIndex");

            migrationBuilder.Sql("DROP FUNCTION dbo.EligibleForGPFollowUp");

            migrationBuilder.Sql("DROP FUNCTION dbo.IsHighBmiRiskEthnicity");

            migrationBuilder.Sql("DROP FUNCTION dbo.BPRiskCategory");

            migrationBuilder.Sql("DROP FUNCTION dbo.CholesterolRiskCategory");

            migrationBuilder.Sql("DROP FUNCTION dbo.MentalRiskCategory");

            migrationBuilder.Sql("DROP FUNCTION dbo.UptakeRateReport");

            migrationBuilder.Sql("DROP FUNCTION dbo.AnonymisedInterventions");

            migrationBuilder.Sql("DROP FUNCTION dbo.AnonymisedBarriers");

            migrationBuilder.Sql("DROP FUNCTION dbo.RiskFactors");
        }
    }
}
