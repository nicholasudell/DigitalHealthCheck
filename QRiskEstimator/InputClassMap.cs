using CsvHelper.Configuration;

namespace QRiskEstimator
{
    public class InputClassMap : ClassMap<OutputFileRecord>
    {
        public InputClassMap()
        {
            Map(x=> x.NHSNumber);
            Map(x=> x.UniqueLink);
            Map(x=> x.Age);
            Map(x=> x.AtrialFibrillation).TypeConverter<YesNoConverter>();
            Map(x=> x.AtypicalAntipsychoticMedication).TypeConverter<YesNoConverter>();
            Map(x=> x.Steroids).TypeConverter<YesNoConverter>();
            Map(x=> x.Migraines).TypeConverter<YesNoConverter>();
            Map(x=> x.RheumatoidArthritis).TypeConverter<YesNoConverter>();
            Map(x=> x.ChronicKidneyDisease).TypeConverter<YesNoConverter>();
            Map(x=> x.SevereMentalIllness).TypeConverter<YesNoConverter>();
            Map(x=> x.ErectileDysfunction).TypeConverter<YesNoConverter>();
            Map(x=> x.SystemicLupusErythematosus).TypeConverter<YesNoConverter>();
            Map(x=> x.BloodPressureTreatment).TypeConverter<YesNoConverter>();
            Map(x=> x.Type1Diabetes).TypeConverter<YesNoConverter>();
            Map(x=> x.Type2Diabetes).TypeConverter<YesNoConverter>();
            Map(x=> x.BMI);
            Map(x=> x.SexAtBirth).TypeConverter<SexConverter>();
            Map(x=> x.Ethnicity);
            Map(x=> x.FamilyHistoryCVD).TypeConverter<YesNoConverter>();
            Map(x=> x.CholesterolRatio);
            Map(x=> x.SystolicBloodPressure);
            Map(x=> x.SmokingStatus);
            Map(x=> x.Postcode);
            Map(x=> x.QRISK).Ignore();
        }
    }
}
