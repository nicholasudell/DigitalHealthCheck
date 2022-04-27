using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DigitalHealthCheckEF;

namespace QRiskEstimator
{
    public class SexConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) =>
            text switch
            {
                "F" => Sex.Female,
                "M" => Sex.Male,
                _ => null
            };


        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) =>
            value switch
            {
                null => "",
                Sex.Female => "F",
                Sex.Male => "M",
                _ => throw new InvalidOperationException("oops!")
            };
    }
}
