using System;
using System.Collections.Generic;
using System.Linq;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using NUnit.Framework;
using Shouldly;

namespace DigitalHealthCheckTests.Model
{
    [TestFixture]
    public class HealthScoreCalculatorTests
    {
        //There will be some variance when comparing floating point numbers. Anything up to this amount is acceptable.
        const double AcceptableVariance = 0.1d;

        [TestCaseSource(nameof(AUDITTestData))]
        public void AUDIT_ShouldMatchForValues(HealthCheck check, int expected)
        {
            var calculator = CreateCalculator();

            var actual = calculator.CalculateAuditScore(check,false);

            actual.ShouldBe
            (
                expected,
                $"Got {actual} instead of {expected} for the following check:{Environment.NewLine}{check.Print()}"
            );
        }

        [TestCaseSource(nameof(CVDTestData))]
        public void CVD_Score_ShouldMatchForValues(HealthCheck check, double townsend, double expected)
        {
            var calculator = CreateCalculator();

            var actual = calculator.Calculate10YearCVDRiskScore(check, townsend, false);

            actual.ShouldBe
            (
                expected,
                AcceptableVariance,
                $"Got {actual} instead of {expected} for townsend {townsend} and the following check:{Environment.NewLine}{check.Print()}"
            );
        }

        [TestCaseSource(nameof(DiabetesTestData))]
        public void Diabetes_Score_ShouldMatchForValues(HealthCheck check, double townsend, double expected)
        {
            var calculator = CreateCalculator();

            var actual = calculator.Calculate10YearDiabetesRiskScore(check, townsend, false);

            actual.ShouldBe
            (
                expected,
                AcceptableVariance,
                $"Got {actual} instead of {expected} for townsend {townsend} and the following check:{Environment.NewLine}{check.Print()}"
            );
        }

        [TestCaseSource(nameof(GPPAQTestData))]
        public void GPPAQ_ShouldMatchForValues(HealthCheck check, int expected)
        {
            var calculator = CreateCalculator();

            var actual = calculator.CalculateGPPAQScore(check, false);

            actual.ShouldBe
            (
                expected,
                $"Got {actual} instead of {expected} for the following check:{Environment.NewLine}{check.Print()}"
            );
        }

        [TestCaseSource(nameof(HeartAgeTestData))]
        public void HeartAge_ShouldMatchForValues(HealthCheck check, double cvd, int expected)
        {
            var calculator = CreateCalculator();

            var actual = calculator.CalculateHeartAge(check, cvd, false);

            actual.ShouldBe
            (
                expected,
                $"Got {actual} instead of {expected} for cvd {cvd} and the following check:{Environment.NewLine}{check.Print()}"
            );
        }

        //Todo: Put your implementation here.
        static StubRiskScoreCalculator CreateCalculator() => new();

        #region DiabetesTestData

        static IEnumerable<TestCaseData> DiabetesTestData()
            => new[]
            {
                // Normal spread, all possible values filled

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=64,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                    BloodSugar=25f,
                },0,0.2f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=50,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                    BloodSugar=25f,
                },0,0.1f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=75,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                    BloodSugar=25f,
                },0,0.2f),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=64,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=75,
                    BloodSugar=35f,
                },0,6.5f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=50,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=75,
                    BloodSugar=35f,
                },0,5.5f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=75,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=75,
                    BloodSugar=35f,
                },0,4.9f),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=64,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=100,
                    BloodSugar=45f,
                },0,46f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=50,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=100,
                    BloodSugar=45f,
                },0,46f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=75,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=100,
                    BloodSugar=45f,
                },0,35.4f),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=64,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                    BloodSugar=25f,
                },0,0.1f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=50,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                    BloodSugar=25f,
                },0,0.1f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=75,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                    BloodSugar=25f,
                },0,0.1f),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=64,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=75,
                    BloodSugar=35f,
                },0,3.4f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=50,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=75,
                    BloodSugar=35f,
                },0,3.2f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=75,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=75,
                    BloodSugar=35f,
                },0,2.4f),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=64,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=100,
                    BloodSugar=45f,
                },0,33f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=50,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=100,
                    BloodSugar=45f,
                },0,34.1f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=75,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=100,
                    BloodSugar=45f,
                },0,24.2f),

                //Normal spread, no Blood Sugar filled

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=64,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                },0,2.3f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=50,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                },0,1.6f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=75,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                },0,2.3f),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=64,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                },0,1.6f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=50,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                },0,1.1f),
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=75,
                    Steroids=false,
                    BloodPressureTreatment=false,
                    FamilyHistoryDiabetes=false,
                    PolycysticOvaries = false,
                    GestationalDiabetes = false,
                    Ethnicity= Ethnicity.WhiteBritish,
                    SmokingStatus = SmokingStatus.NonSmoker,
                    Height=1.5f,
                    Weight=50,
                },0,1.6f),
            }.Select(x =>
                new TestCaseData(x.Item1, x.Item2, x.Item3)
                    .SetDescription($"Diabetes should be {x.Item3} for Townsend {x.Item2} and check {x.Item1.Print(x => x.Key != "Id", ", ")}")
                    .SetName($"Diabetes_ForTownsend_{x.Item2.ToString().Replace('.', '_')}_Check_{x.Item1.PrintForTestName(x => x.Key != "Id")}")
            ).ToList();

        #endregion DiabetesTestData

        #region CVDTestData

        static IEnumerable<TestCaseData> CVDTestData()
            => new[]
            {
                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=45,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=100,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,1.6d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=64,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=100,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,7.2d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=75,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=100,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,15.9d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=45,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,1.3d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=64,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,7.1d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=75,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,16.4d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=45,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=100,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,0.9d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=64,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=100,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,4.8d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=75,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=100,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,11.7d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=45,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,0.7d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=64,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,4.5d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=75,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SystolicBloodPressure = 90,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,12.2d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=47,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.75f,
                    Weight=80,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 1,
                    TotalCholesterol = 4,
                    SystolicBloodPressure = 125,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0f,1.6d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=62,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.6f,
                    Weight=60,
                    Ethnicity= Ethnicity.Indian,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 2,
                    TotalCholesterol = 4,
                    SystolicBloodPressure = 120,
                    SmokingStatus = SmokingStatus.Light
                },0f,12.3d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=62,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.6f,
                    Weight=60,
                    Ethnicity= Ethnicity.Indian,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 1,
                    TotalCholesterol = 4,
                    SystolicBloodPressure = 120,
                    SmokingStatus = SmokingStatus.Light
                },0f,17d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=62,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.6f,
                    Weight=60,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 2,
                    TotalCholesterol = 4,
                    SystolicBloodPressure = 120,
                    SmokingStatus = SmokingStatus.Light
                },0f,9.5d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=62,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.6f,
                    Weight=60,
                    Ethnicity= Ethnicity.Indian,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 2,
                    TotalCholesterol = 4,
                    SystolicBloodPressure = 120,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0f,8.7d),



                // No blood pressure

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=64,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,9.9d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=64,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 4,
                    TotalCholesterol = 12,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,6.1d),

                // No cholesterol

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=64,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    SystolicBloodPressure = 120,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,11.5d),

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Female,
                    Age=64,
                    AtrialFibrillation=false,
                    Steroids=false,
                    Migraines=false,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.5f,
                    Weight=50,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    SystolicBloodPressure = 120,
                    SmokingStatus = SmokingStatus.NonSmoker
                },0,6.5d),

                // Clinical Example from QRISK paper

                new Tuple<HealthCheck, double, double>(new HealthCheck
                {
                    SexForResults=Sex.Male,
                    Age=44,
                    AtrialFibrillation=false,
                    Steroids=true,
                    Migraines=true,
                    RheumatoidArthritis=false,
                    ChronicKidneyDisease=false,
                    BloodPressureTreatment=false,
                    Height=1.65f,
                    Weight=85,
                    Ethnicity= Ethnicity.WhiteBritish,
                    FamilyHistoryCVD = false,
                    HdlCholesterol = 2,
                    TotalCholesterol = 4,
                    SystolicBloodPressure = 132,
                    SmokingStatus = SmokingStatus.Heavy
                },0,7.5d),
            }.Select(x =>
                new TestCaseData(x.Item1, x.Item2, x.Item3)
                    .SetDescription($"CVD should be {x.Item3} for Townsend {x.Item2} and check {x.Item1.Print(x => x.Key != "Id", ", ")}")
                    .SetName($"CVD_ForTownsend_{x.Item2.ToString().Replace('.', '_')}_Check_{x.Item1.PrintForTestName(x => x.Key != "Id")}")
            ).ToList();

        #endregion CVDTestData

        #region HeartAgeTestData

        static IEnumerable<TestCaseData> HeartAgeTestData()
            => new[]
            {
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },0,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },0.1,26),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },0.2,29),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },0.5,33),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },1,37),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },2,43),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },3,47),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },5,52),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },7,57),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },10,61),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },12,64),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },15,67),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },18,70),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },20,72),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },25,75),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },30,78),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },50,84),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.WhiteBritish
                },100,84),




                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },0,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },0.1,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },0.2,27),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },0.5,32),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },1,36),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },2,41),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },3,44),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },5,49),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },7,53),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },10,58),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },12,60),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },15,64),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },18,66),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },20,68),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },25,71),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },30,74),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },50,83),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Indian
                },100,84),



                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },0,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },0.1,27),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },0.2,30),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },0.5,35),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },1,40),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },2,46),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },3,51),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },5,57),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },7,61),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },10,66),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },12,69),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },15,72),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },18,75),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },20,77),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },25,80),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },30,83),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },50,84),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Male,
                    Ethnicity = Ethnicity.Caribbean
                },100,84),



                 new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },0,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },0.1,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },0.2,28),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },0.5,35),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },1,42),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },2,49),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },3,53),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },5,59),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },7,63),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },10,67),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },12,70),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },15,72),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },18,75),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },20,76),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },25,79),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },30,82),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },50,84),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.WhiteBritish
                },100,84),




                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },0,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },0.1,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },0.2,27),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },0.5,33),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },1,39),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },2,46),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },3,50),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },5,56),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },7,60),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },10,64),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },12,66),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },15,69),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },18,72),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },20,73),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },25,76),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },30,78),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },50,84),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Indian
                },100,84),



                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },0,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },0.1,25),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },0.2,30),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },0.5,37),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },1,43),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },2,51),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },3,55),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },5,61),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },7,65),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },10,69),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },12,72),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },15,75),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },18,77),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },20,78),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },25,81),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },30,84),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },50,84),
                new Tuple<HealthCheck, double, int>(new HealthCheck
                {
                    SexForResults = Sex.Female,
                    Ethnicity = Ethnicity.Caribbean
                },100,84)



            }.Select(x =>
                new TestCaseData(x.Item1, x.Item2, x.Item3)
                    .SetDescription($"Heart Age should be {x.Item3} for CVD risk {x.Item2} and check {x.Item1.Print(x => x.Key != "Id", ", ")}")
                    .SetName($"HeartAge_ForCheck_{x.Item1.PrintForTestName(x => x.Key != "Id")}_ForCVD_{x.Item2.ToString().Replace('.', '_')}")
            ).ToList();

        #endregion HeartAgeTestData

        #region GPPAQTestData

        static IEnumerable<TestCaseData> GPPAQTestData()
            => new[]
            {
                new Tuple<HealthCheck, int>(new HealthCheck
                {
                    PhysicalActivity = GPPAQActivityLevel.None,
                    Cycling = GPPAQActivityLevel.None,
                    WorkActivity = GPPAQEmploymentActivity.Unemployed
                },0)
            }.Select(x =>
                new TestCaseData(x.Item1, x.Item2)
                    .SetDescription($"GPPAQ should be {x.Item2} for check {x.Item1.Print(x => x.Key != "Id", ", ")}")
                    .SetName($"GPPAQ_For_{x.Item1.PrintForTestName(x => x.Key != "Id")}")
            ).ToList();

        #endregion GPPAQTestData

        #region AUDITTestData

        static IEnumerable<TestCaseData> AUDITTestData()
            => new[]
            {
                new Tuple<HealthCheck, int>(new HealthCheck
                {
                    DrinksAlcohol= true,
                    FailedResponsibilityDueToAlcohol = AUDITFrequency.Never,
                    DrinkingFrequency = AUDITDrinkingFrequency.Never,
                    MSASQ = AUDITFrequency.Monthly,
                    GuiltAfterDrinking = AUDITFrequency.Never,
                    MemoryLossAfterDrinking = AUDITFrequency.Never,
                    NeededToDrinkAlcoholMorningAfter = AUDITFrequency.Never,
                    UnableToStopDrinking = AUDITFrequency.Never,
                    ContactsConcernedByDrinking = AUDITYesNotInLastYearNo.No,
                    InjuryCausedByDrinking = AUDITYesNotInLastYearNo.No,
                    TypicalDayAlcoholUnits=0
                }, 2),
                new Tuple<HealthCheck, int>(new HealthCheck
                {
                    DrinksAlcohol= false,
                },-1),
                new Tuple<HealthCheck, int>(new HealthCheck
                {
                    DrinksAlcohol= true,
                    MSASQ = AUDITFrequency.Never
                },-1),
                new Tuple<HealthCheck, int>(new HealthCheck
                {
                    DrinksAlcohol= true,
                    MSASQ = AUDITFrequency.LessThanMonthly
                },-1)
            }.Select(x =>
                new TestCaseData(x.Item1, x.Item2)
                    .SetDescription($"AUDIT should be {x.Item2} for check {x.Item1.Print(x => x.Key != "Id", ", ")}")
                    .SetName($"AUDIT_For_{x.Item1.PrintForTestName(x => x.Key != "Id")}")
            ).ToList();

        #endregion AUDITTestData
    }
}