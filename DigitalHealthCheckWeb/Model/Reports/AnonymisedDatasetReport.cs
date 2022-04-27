using System;
using System.Threading.Tasks;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using CsvHelper.Configuration;
using System.Collections.Generic;

namespace DigitalHealthCheckWeb.Model
{

    public class AnonymisedDatasetReport : Report<AnonymisedPatient>
    {
        private readonly Database database;
        private readonly DateTime? from;
        private readonly DateTime? to;

        public AnonymisedDatasetReport(Database database, DateTime? from = null, DateTime? to = null) : base
        (
            new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = "\t"
            }, csv =>
            {
                csv.Context.RegisterClassMap<AnonymisedDataSetMap>();
            }
        )
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.from = from;
            this.to = to;
        }

        protected override async Task<IList<AnonymisedPatient>> GetRecordsAsync() 
        {
            database.Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
            return await database.AnonymisedDataSet(from,to).ToListAsync();
        }

        private sealed class AnonymisedDataSetMap : ClassMap<AnonymisedPatient>
        {
            public AnonymisedDataSetMap()
            {
                Map(x=> x.Id).Name("PatientIdentifier");
                Map(x=> x.PredictedCVDRisk).Name("Predicted CVD Risk");
                Map(x=> x.PredictedDiabetesRisk).Name("Predicted Diabetes Risk");
                Map(x=> x.InviteDate).Name("InviteDate");
                Map(x=> x.NHSHCTextLink).Name("NHS HC Text Link");
                Map(x=> x.NHSHCQRCode).Name("NHS HC QR Code");
                Map(x=> x.ArrivedAtLandingPage).Name("Arrived at the landing page").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.StartedNHSHealthCheck).Name("Started NHS Health Check").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HeightAndWeightKnown).Name("Height and weight known").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HeightAndWeightNotKnown).Name("Height and weight not known").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.Height).Name("Height");
                Map(x=> x.Weight).Name("Weight");
                Map(x=> x.BMI).Name("BMI");
                Map(x=> x.SexAssignedAtBirth).Name("Sex assigned at birth");
                Map(x=> x.PreferredSexForFirstViewOfResults).Name("Preferred sex for first view of results");
                Map(x=> x.Ethnicity).Name("Ethnicity");
                Map(x=> x.SmokingStatus).Name("Smoking status");
                Map(x=> x.SmokingBehaviour).Name("Smoking behaviour");
                Map(x=> x.DrinksAlcohol).Name("Drink alcohol").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.DrinkingFrequency).Name("Drink alcohol - frequency");
                Map(x=> x.DrinkingUnits).Name("Drink alcohol - units");
                Map(x=> x.MSASQ).Name("Drink alcohol - frequency of greater than reccomended units drunk in one sitting in past year");
                Map(x=> x.UnableToStop).Name("Drink alcohol - frequency of not being able to stop drinking in past year");
                Map(x=> x.FailedResponsibilityDueToAlcohol).Name("Drink alcohol - failed to do what was normally expected  because of drinking in past year");
                Map(x=> x.NeededToDrinkAlcoholMorningAfter).Name("Drink alcohol - needed an alcoholic drink in the morning after a heavy drinking session to get going because of drinking in past year");
                Map(x=> x.GuiltAfterDrinking).Name("Drink alcohol - feeling of guilt or remorse after drinking because of drinking in past year");
                Map(x=> x.MemoryLossAfterDrinking).Name("Drink alcohol - unable to remember what happened the night before because of drinking in past year");
                Map(x=> x.InjuryCausedByDrinking).Name("Drink alcohol - personally injured or somebody else injured as a result of drinking");
                Map(x=> x.ContactsConcernedByDrinking).Name("Drink alcohol - relative or friend, doctor or other health worker concerned about drinking or suggested need to cut down");
                Map(x=> x.WorkActivity).Name("Physical Activity - type and amount of physical activity involved in work");
                Map(x=> x.PhysicalActivity).Name("Physical Activity - Physical exercise hours on activity in last week");
                Map(x=> x.Cycling).Name("Physical Activity - Cycling hours on activity in last week");
                Map(x=> x.Housework).Name("Physical Activity - Housework/childcare hours on activity in last week");
                Map(x=> x.Gardening).Name("Physical Activity - Gardening/DIY hours on activity in last week");
                Map(x=> x.Walking).Name("Physical Activity - Walking hours on activity in last week");
                Map(x=> x.WalkingPace).Name("Physical Activity - Walking pace");
                Map(x=> x.KnowYourHbA1c).Name("HbA1c measured within the last six months");
                Map(x=> x.HbA1cMeasure).Name("HbA1c measure");
                Map(x=> x.HbA1cMeasureFound).Name("HbA1c measure found").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.KnowYourBloodPressure).Name("Blood pressure measured within the last six months");
                Map(x=> x.BloodPressureMeasureSys).Name("Blood pressure measure (sys)");
                Map(x=> x.BloodPressureMeasureDias).Name("Blood pressure measure (dias)");
                Map(x=> x.BloodPressureMeasureFound).Name("Blood pressure measure found").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.KnowYourCholesterol).Name("Cholesterol measured within the last six months");
                Map(x=> x.CholesterolMeasureTotal).Name("Total cholesterol measure");
                Map(x=> x.CholesterolMeasureHDL).Name("HDL measure");
                Map(x=> x.CholesterolMeasureFound).Name("Cholesterol measure found").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.FeelingDown).Name("Mental wellbeing Whooley 1").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.Disinterested).Name("Mental wellbeing Whooley 2").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.Anxious).Name("Mental wellbeing GAD2 1");
                Map(x=> x.Worrying).Name("Mental wellbeing GAD2 2");
                Map(x=> x.SkipMentalHealthQuestions).Name("Mental wellbeing questions passed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.EmailProvided).Name("Email provided to recieve results and follow up services").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.PhoneNumberProvided).Name("Provided phone number for possibility of opting in for telephone service later in NHS Health Check (Everyone Health)").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.SmsNumberProvided).Name("Provided mobile number for possibility of opting in to text reminders").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.IMDQuintile).Name("Index of Multiple Deprivation (IMD) Quintile");
                Map(x=> x.Lsoa).Name("LSOA");
                Map(x=> x.Age).Name("Age");
                Map(x=> x.ValidationConfirmed).Name("Patient validation confirmed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ValidationCompleted).Name("Complete validation page").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.GPSurgery).Name("GP Surgery");
                Map(x=> x.SubmittedCheckYourAnswers).Name("Accept and submit answers").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HeightAndWeightFollowUp).Name("Health and Weight Follow-up Page Completion").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ReceivedHealthCheckIncompleteEmail).Name("NHS Health Check in progress email").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HeightAndWeightImportance).Name("Height and weight Follow Up - importance ");
                Map(x=> x.HeightAndWeightConfidence).Name("Height and weight Follow Up - confidence");
                Map(x=> x.HeightAndWeightMethodOfMeasurement).Name("Height and weight Follow Up - method of measurement");
                Map(x=> x.HeightAndWeightIntentions).Name("Height and weight Follow Up - intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HeightAndWeightSetADate).Name("Height and weight Follow Up - contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HeightAndWeightReminderRequest).Name("Height and weight Follow Up - reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ViewsResults).Name("Views Results").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.CVDRiskCategory).Name("CVDRiskCategory");
                Map(x=> x.CVDBehaviours).Name("CVD behaviours");
                Map(x=> x.CVDRiskScore).Name("CVDRisk Score");
                Map(x=> x.HeartAge).Name("HeartAge");
                Map(x=> x.HeartAgeRiskCategory).Name("HeartAge Risk Category ");
                Map(x=> x.QDiabetesScoreCategory).Name("QDiabetesScore Category");
                Map(x=> x.QDiabetes).Name("QDiabetesScore");
                Map(x=> x.Alcohol).Name("Alcohol");
                Map(x=> x.PhysicalActivityLevel).Name("Physical Activity Level");
                Map(x=> x.BMICategory).Name("BMICategory");
                Map(x=> x.BloodPressureCategory).Name("BloodPressure Category");
                Map(x=> x.HbA1cCategory).Name("HbA1c Category");
                Map(x=> x.CholesterolCategory).Name("Cholesterol Category");
                Map(x=> x.ModifiableFactorsCount).Name("Number of CVD modifiable risk factors");
                Map(x=> x.NonUrgentGPFollowUp).Name("Patient recommended for non-urgent GP follow up");
                Map(x=> x.UrgentGPFollowUp).Name("Patient recommended for urgent GP follow up");
                Map(x=> x.FirstHealthPriorityAfterResults).Name("Health Priority 1 (follow up)");
                Map(x=> x.SecondHealthPriorityAfterResults).Name("Health Priority 2 (follow up)");
                Map(x=> x.FirstFollowUpOptionSelected).Name("First follow up option selected");
                Map(x=> x.BookGPFollowUpPageCompleted).Name("Book GP Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.GPFollowUpImportance).Name("Book GP Follow Up Importance");
                Map(x=> x.GPFollowUpConfidence).Name("Book GP Follow Up Confidence");
                Map(x=> x.GPFollowUpIntentions).Name("Book GP Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.GPFollowUpSetADate).Name("Book GP Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.GPFollowUpReminderRequest).Name("Book GP Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.GPFollowUpNumberOfBarriers).Name("Book GP Follow Up number of barriers");
                Map(x=> x.GPFollowUpNumberOfActions).Name("Book GP Follow Up number of actions");
                Map(x=> x.MeasureBloodPressureFollowUpPageCompleted).Name("Measure Blood Pressure Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureBloodPressureFollowUpImportance).Name("Measure Blood Pressure Follow Up Importance");
                Map(x=> x.MeasureBloodPressureFollowUpConfidence).Name("Measure Blood Pressure Follow Up Confidence");
                Map(x=> x.MeasureBloodPressureFollowUpMethodOfMeasurement).Name("Measure Blood Pressure Follow Up MethodOfMeasurement");
                Map(x=> x.MeasureBloodPressureFollowUpIntentions).Name("Measure Blood Pressure Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureBloodPressureFollowUpSetADate).Name("Measure Blood Pressure Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureBloodPressureFollowUpReminderRequest).Name("Measure Blood Pressure Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureBloodPressureFollowUpNumberOfBarriers).Name("Measure Blood Pressure Follow Up number of barriers");
                Map(x=> x.MeasureBloodPressureFollowUpNumberOfActions).Name("Measure Blood Pressure Follow Up number of actions");
                Map(x=> x.ImproveBloodPressureFollowUpPageCompleted).Name("Improve Blood Pressure Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveBloodPressureFollowUpImportance).Name("Improve Blood Pressure Follow Up Importance");
                Map(x=> x.ImproveBloodPressureFollowUpConfidence).Name("Improve Blood Pressure Follow Up Confidence");
                Map(x=> x.ImproveBloodPressureFollowUpIntentions).Name("Improve Blood Pressure Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveBloodPressureFollowUpSetADate).Name("Improve Blood Pressure Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveBloodPressureFollowUpReminderRequest).Name("Improve Blood Pressure Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveBloodPressureFollowUpNumberOfBarriers).Name("Improve Blood Pressure Follow Up number of barriers");
                Map(x=> x.ImproveBloodPressureFollowUpNumberOfActions).Name("Improve Blood Pressure Follow Up number of actions");
                Map(x=> x.MeasureBloodSugarFollowUpPageCompleted).Name("Measure Blood Sugar Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureBloodSugarFollowUpImportance).Name("Measure Blood Sugar Follow Up Importance");
                Map(x=> x.MeasureBloodSugarFollowUpConfidence).Name("Measure Blood Sugar Follow Up Confidence");
                Map(x=> x.MeasureBloodSugarFollowUpMethodOfMeasurement).Name("Measure Blood Sugar Follow Up MethodOfMeasurement");
                Map(x=> x.MeasureBloodSugarFollowUpIntentions).Name("Measure Blood Sugar Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureBloodSugarFollowUpSetADate).Name("Measure Blood Sugar Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureBloodSugarFollowUpReminderRequest).Name("Measure Blood Sugar Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureBloodSugarFollowUpNumberOfBarriers).Name("Measure Blood Sugar Follow Up number of barriers");
                Map(x=> x.MeasureBloodSugarFollowUpNumberOfActions).Name("Measure Blood Sugar Follow Up number of actions");
                Map(x=> x.ImproveBloodSugarFollowUpPageCompleted).Name("Improve Blood Sugar Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveBloodSugarFollowUpImportance).Name("Improve Blood Sugar Follow Up Importance");
                Map(x=> x.ImproveBloodSugarFollowUpConfidence).Name("Improve Blood Sugar Follow Up Confidence");
                Map(x=> x.ImproveBloodSugarFollowUpIntentions).Name("Improve Blood Sugar Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveBloodSugarFollowUpSetADate).Name("Improve Blood Sugar Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveBloodSugarFollowUpReminderRequest).Name("Improve Blood Sugar Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveBloodSugarFollowUpNumberOfBarriers).Name("Improve Blood Sugar Follow Up number of barriers");
                Map(x=> x.ImproveBloodSugarFollowUpNumberOfActions).Name("Improve Blood Sugar Follow Up number of actions");
                Map(x=> x.MeasureCholesterolFollowUpPageCompleted).Name("Measure Cholesterol Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureCholesterolFollowUpImportance).Name("Measure Cholesterol Follow Up Importance");
                Map(x=> x.MeasureCholesterolFollowUpConfidence).Name("Measure Cholesterol Follow Up Confidence");
                Map(x=> x.MeasureCholesterolFollowUpMethodOfMeasurement).Name("Measure Cholesterol Follow Up MethodOfMeasurement");
                Map(x=> x.MeasureCholesterolFollowUpIntentions).Name("Measure Cholesterol Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureCholesterolFollowUpSetADate).Name("Measure Cholesterol Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureCholesterolFollowUpReminderRequest).Name("Measure Cholesterol Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MeasureCholesterolFollowUpNumberOfBarriers).Name("Measure Cholesterol Follow Up number of barriers");
                Map(x=> x.MeasureCholesterolFollowUpNumberOfActions).Name("Measure Cholesterol Follow Up number of actions");
                Map(x=> x.ImproveCholesterolFollowUpPageCompleted).Name("Improve Cholesterol Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveCholesterolFollowUpImportance).Name("Improve Cholesterol Follow Up Importance");
                Map(x=> x.ImproveCholesterolFollowUpConfidence).Name("Improve Cholesterol Follow Up Confidence");
                Map(x=> x.ImproveCholesterolFollowUpIntentions).Name("Improve Cholesterol Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveCholesterolFollowUpSetADate).Name("Improve Cholesterol Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveCholesterolFollowUpReminderRequest).Name("Improve Cholesterol Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ImproveCholesterolFollowUpNumberOfBarriers).Name("Improve Cholesterol Follow Up number of barriers");
                Map(x=> x.ImproveCholesterolFollowUpNumberOfActions).Name("Improve Cholesterol Follow Up number of actions");
                Map(x=> x.StopSmokingFollowUpPageCompleted).Name("Stop Smoking Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.StopSmokingFollowUpImportance).Name("Stop Smoking Follow Up Importance");
                Map(x=> x.StopSmokingFollowUpConfidence).Name("Stop Smoking Follow Up Confidence");
                Map(x=> x.StopSmokingFollowUpIntentions).Name("Stop Smoking Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.StopSmokingFollowUpSetADate).Name("Stop Smoking Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.StopSmokingFollowUpReminderRequest).Name("Stop Smoking Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.StopSmokingFollowUpNumberOfBarriers).Name("Stop Smoking Follow Up number of barriers");
                Map(x=> x.StopSmokingFollowUpNumberOfActions).Name("Stop Smoking Follow Up number of actions");
                Map(x=> x.QuitDrinkingFollowUpPageCompleted).Name("Quit Drinking Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.QuitDrinkingFollowUpImportance).Name("Quit Drinking Follow Up Importance");
                Map(x=> x.QuitDrinkingFollowUpConfidence).Name("Quit Drinking Follow Up Confidence");
                Map(x=> x.QuitDrinkingFollowUpIntentions).Name("Quit Drinking Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.QuitDrinkingFollowUpSetADate).Name("Quit Drinking Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.QuitDrinkingFollowUpReminderRequest).Name("Quit Drinking Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.QuitDrinkingFollowUpNumberOfBarriers).Name("Quit Drinking Follow Up number of barriers");
                Map(x=> x.QuitDrinkingFollowUpNumberOfActions).Name("Quit Drinking Follow Up number of actions");
                Map(x=> x.HealthyWeightFollowUpPageCompleted).Name("Healthy Weight Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HealthyWeightFollowUpImportance).Name("Healthy Weight Follow Up Importance");
                Map(x=> x.HealthyWeightFollowUpConfidence).Name("Healthy Weight Follow Up Confidence");
                Map(x=> x.HealthyWeightFollowUpIntentions).Name("Healthy Weight Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HealthyWeightFollowUpSetADate).Name("Healthy Weight Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HealthyWeightFollowUpReminderRequest).Name("Healthy Weight Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HealthyWeightFollowUpNumberOfBarriers).Name("Healthy Weight Follow Up number of barriers");
                Map(x=> x.HealthyWeightFollowUpNumberOfActions).Name("Healthy Weight Follow Up number of actions");
                Map(x=> x.MoveMoreFollowUpPageCompleted).Name("Move More Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MoveMoreFollowUpImportance).Name("Move More Follow Up Importance");
                Map(x=> x.MoveMoreFollowUpConfidence).Name("Move More Follow Up Confidence");
                Map(x=> x.MoveMoreFollowUpIntentions).Name("Move More Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MoveMoreFollowUpSetADate).Name("Move More Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MoveMoreFollowUpReminderRequest).Name("Move More Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MoveMoreFollowUpNumberOfBarriers).Name("Move More Follow Up number of barriers");
                Map(x=> x.MoveMoreFollowUpNumberOfActions).Name("Move More Follow Up number of actions");
                Map(x=> x.MentalHealthFollowUpPageCompleted).Name("Mental Health Follow Up page completed").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MentalHealthFollowUpImportance).Name("Mental Health Follow Up Importance");
                Map(x=> x.MentalHealthFollowUpConfidence).Name("Mental Health Follow Up Confidence");
                Map(x=> x.MentalHealthFollowUpIntentions).Name("Mental Health Follow Up Intentions").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MentalHealthFollowUpSetADate).Name("Mental Health Follow Up contact or set a date").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MentalHealthFollowUpReminderRequest).Name("Mental Health Follow Up reminder request").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MentalHealthFollowUpNumberOfBarriers).Name("Mental Health Follow Up number of barriers");
                Map(x=> x.MentalHealthFollowUpNumberOfActions).Name("Mental Health Follow Up number of actions");
                Map(x=> x.PatientEligibleForEveryoneHealthReferral).Name("Eligible for EH Referral").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.MethodOfContactForEveryoneHealth).Name("Method of contact for EH");
                Map(x=> x.RecommendedBiometricAssessments).Name("Recommended biometric assessments").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HealthCheckCompleted).Name("Completion").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ContactInformationUpdatedOnComplete).Name("Opts in to recieve results").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.HeightAndWeightUpdated).Name("Re-enters H and W").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.BloodSugarUpdated).Name("Re-enters HbA1c").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.BloodPressureUpdated).Name("Re-enters blood pressure").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.CholesterolUpdated).Name("Re-enters cholesterol").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.FamilyHistoryDiabetes).Name("Close family member have Type 2 diabetes").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.FamilyHistoryCVD).Name("Close family member have angina or heart attack under the age of 60").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.ChronicKidneyDisease).Name("Diagnosed with chronic kidney disease").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.AtrialFibrillation).Name("Diagnosed with atrial fibrillation").TypeConverter<BooleanToYNConverter>();
                Map(x=> x.BloodPressureTreatment).Name("On blood pressure treatment").TypeConverter<BooleanToYNConverter>();
            }

        }
    }
}
