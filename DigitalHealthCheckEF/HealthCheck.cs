using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalHealthCheckEF
{
    public class HealthCheck
    {
        public int? Age { get; set; }

        public MentalWellbeingFrequency? Anxious { get; set; }

        public bool? AtrialFibrillation { get; set; }

        public bool HealthCheckCompleted { get; set; }

        public bool HeightAndWeightUpdated { get; set; }
        public DateTime? HeightAndWeightUpdatedDate { get; set; }
        public bool BloodSugarUpdated { get; set; }
        public DateTime? BloodSugarUpdatedDate { get; set; }
        public bool BloodPressureUpdated { get; set; }
        public DateTime? BloodPressureUpdatedDate { get; set; }
        public bool CholesterolUpdated { get; set; }
        public DateTime? CholesterolUpdatedDate { get; set; }

        public DateTime? HealthCheckCompletedDate { get; set; }

        public YesNoSkip? AtypicalAntipsychoticMedication { get; set; }

        public int? AUDIT { get; set; }

        public bool? BloodPressureTreatment { get; set; }

        public float? BloodSugar { get; set; }

        public IdentityVariant Variant { get; set; }

        public bool? GenderAffirmation { get; set; }
        public ICollection<Barrier> ChosenBarriers { get; set; }

        public ICollection<Intervention> ChosenInterventions { get; set; }

        public bool? ChronicKidneyDisease { get; set; }

        public string Identity { get; set; }

        public AUDITYesNotInLastYearNo? ContactsConcernedByDrinking { get; set; }

        public MentalWellbeingFrequency? Control { get; set; }

        public ICollection<CustomBarrier> CustomBarriers { get; set; }

        public GPPAQActivityLevel? Cycling { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? DiastolicBloodPressure { get; set; }

        public bool? Disinterested { get; set; }

        public AUDITDrinkingFrequency? DrinkingFrequency { get; set; }

        public bool? DrinksAlcohol { get; set; }

        public string EmailAddress { get; set; }

        public Ethnicity? Ethnicity { get; set; }

        public AUDITFrequency? FailedResponsibilityDueToAlcohol { get; set; }

        public bool? FamilyHistoryCVD { get; set; }

        public bool? FamilyHistoryDiabetes { get; set; }

        public bool? FeelingDown { get; set; }

        public string FirstHealthPriority { get; set; }

        public string FirstHealthPriorityAfterResults { get; set; }

        // Only used for reports.
        public string FirstName { get; set; }

        public string FollowUpAddressCholesterol { get; set; }

        public FollowUp CholesterolFollowUp {get; set;}
        public FollowUp BloodPressureFollowUp { get; set;}
        public FollowUp BloodSugarFollowUp { get; set;}
        public FollowUp BookGPAppointmentFollowUp { get; set;}
        public FollowUp DrinkLessFollowUp { get; set;}
        public FollowUp HealthyWeightFollowUp { get; set;}
        public FollowUp ImproveBloodPressureFollowUp { get; set;}
        public FollowUp ImproveCholesterolFollowUp { get; set;}
        public FollowUp MentalWellbeingFollowUp { get; set;}
        public FollowUp ImproveBloodSugarFollowUp { get; set;}
        public FollowUp QuitSmokingFollowUp { get; set;}
        public FollowUp MoveMoreFollowUp { get; set; }
        public FollowUp HeightAndWeightFollowUp { get; set; }

        public int? GAD2 { get; set; }

        public GPPAQActivityLevel? Gardening { get; set; }

        public string CustomIdentity { get; set; }

        public bool? GestationalDiabetes { get; set; }

        // Set through the encrypted hash
        public string GPEmail { get; set; }

        public int? GPPAQ { get; set; }

        // Set through the encrypted hash
        public string GPSurgery { get; set; }

        public AUDITFrequency? GuiltAfterDrinking { get; set; }

        public float? HdlCholesterol { get; set; }

        public int? HeartAge { get; set; }

        public float? Height { get; set; }

        public GPPAQActivityLevel? Housework { get; set; }

        public Guid Id { get; set; }

        public AUDITYesNotInLastYearNo? InjuryCausedByDrinking { get; set; }

        public HaveYouBeenMeasured? KnowYourBloodPressure { get; set; }

        public HaveYouBeenMeasured? KnowYourCholesterol { get; set; }

        public HaveYouBeenMeasured? KnowYourHbA1c { get; set; }

        public AUDITFrequency? MemoryLossAfterDrinking { get; set; }

        public bool? Migraines { get; set; }

        public AUDITFrequency? MSASQ { get; set; }

        public AUDITFrequency? NeededToDrinkAlcoholMorningAfter { get; set; }

        // Set through the encrypted hash
        public string NHSNumber { get; set; }

        public string PhoneNumber { get; set; }

        public GPPAQActivityLevel? PhysicalActivity { get; set; }

        public bool? PolycysticOvaries { get; set; }

        public string Postcode { get; set; }

        public bool? PrefersImperialHeightAndWeight { get; set; }

        public double? QDiabetes { get; set; }

        public double? QRisk { get; set; }

        public bool? RheumatoidArthritis { get; set; }

        public string SecondHealthPriorityAfterResults { get; set; }

        public YesNoSkip? SevereMentalIllness { get; set; }

        public Sex? SexAtBirth { get; set; }

        public Sex? SexForResults { get; set; }

        public SmokingStatus? SmokingStatus { get; set; }

        public string SmsNumber { get; set; }

        public bool? Steroids { get; set; }

        public string Surname { get; set; }

        public YesNoSkip? SystemicLupusErythematosus { get; set; }

        public int? SystolicBloodPressure { get; set; }

        public float? TotalCholesterol { get; set; }

        public int? TypicalDayAlcoholUnits { get; set; }

        public AUDITFrequency? UnableToStopDrinking { get; set; }

        public bool? SkipMentalHealthQuestions { get; set; }

        public bool? UnderCare { get; set; }

        // Set through the encrypted hash
        public DateTime ValidationDateOfBirth { get; set; }

        //If true, the patient has skipped validation and we shouldn't send GP reports.
        public bool ValidationOverwritten { get; set; }

        // Set through the encrypted hash
        public string ValidationPostcode { get; set; }

        // Set through the encrypted hash
        public string ValidationSurname { get; set; }

        public GPPAQActivityLevel? Walking { get; set; }

        public GPPAQWalkingPace? WalkingPace { get; set; }

        public float? Weight { get; set; }

        public GPPAQEmploymentActivity? WorkActivity { get; set; }

        public DateTime? CalculatedDate { get; set; }
        public bool? EveryoneHealthConsent { get; set; }
        public bool? PrefersToContactEveryoneHealth { get; set; }

        public ReminderStatus ReminderStatus { get; set; }
        public DateTime? LastContactDate { get; set; }
        public bool ClickedStartNow { get; set; }
        public DateTime? CheckStartedDate { get; set; }
        public string InitialSexForResults { get; set; }
        public bool WentToFindBloodPressure { get; set; }
        public bool WentToFindBloodSugar { get; set; }
        public bool WentToFindCholesterol { get; set; }
        public bool SubmittedCheckYourAnswers { get; set; }
        public bool ReceivedHealthCheckIncompleteEmail { get; set; }
        public bool ReceivedHealthCheckCompleteEmail { get; set; }
        public bool EveryoneHealthReferralSent { get; set; }
        public bool GPEmailSent { get; set; }

        public bool CompletePageSubmitted {get;set;}

        public bool ContactInformationUpdatedOnComplete {get;set;}

        public bool HeightAndWeightSkipped {get;set;}
    }
}