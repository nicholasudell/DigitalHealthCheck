namespace DigitalHealthCheckEF
{
    public enum AUDITDrinkingFrequency
    {
        Never = 0,
        LessThanMonthly = 1,
        Monthly = 2,
        TwoToFourPerMonth = 3,
        TwoToThreePerWeek = 4,
        FourTimesWeeklyPlus = 5
    }

    public enum AUDITFrequency
    {
        Never = 0,
        LessThanMonthly = 1,
        Monthly = 2,
        Weekly = 3,
        Daily = 4
    }

    public enum AUDITYesNotInLastYearNo
    {
        No,
        NotInLastYear,
        Yes
    }

    public enum Ethnicity
    {
        WhiteBritish,
        Irish,
        GypsyOrTraveller,
        WhiteOther,
        WhiteBlackCaribbean,
        WhiteBlackAfrican,
        WhiteAsian,
        MixedOther,
        Indian,
        Pakistani,
        Bangladeshi,
        Chinese,
        AsianOther,
        African,
        Caribbean,
        BlackOther,
        Arab,
        OtherOther,
    }

    public enum GPPAQActivityLevel
    {
        None = 0,
        LessThanOneHour = 1,
        OneToThreeHours = 2,
        ThreeHoursOrMore = 3
    }

    public enum GPPAQEmploymentActivity
    {
        Unemployed = 0,
        Sitting = 1,
        Standing = 2,
        Definite = 3,
        Vigorous = 4
    }

    public enum HaveYouBeenMeasured
    {
        Yes,
        Forgot,
        No
    }

    public enum GPPAQWalkingPace
    {
        Slow,
        Steady,
        Brisk,
        Fast
    }

    public enum MentalWellbeingFrequency
    {
        NotAtAll = 0,
        SeveralDays = 1,
        MoreThanHalf = 2,
        EveryDay = 3
    }

    public enum ReminderStatus
    {
        Unsent = 0,
        FirstReminder = 1,

        SecondSurvey = 2,
        SecondReminder = 3,
        ThirdReminder = 4
    }

    public enum Sex
    {
        Female,
        Male
    }

    public enum SmokingStatus
    {
        NonSmoker,
        ExSmoker,
        Light,
        Moderate,
        Heavy
    }

    public enum YesNoSkip
    {
        Yes,
        No,
        Skip
    }
}