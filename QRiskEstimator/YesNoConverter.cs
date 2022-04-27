using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DigitalHealthCheckEF;

namespace QRiskEstimator
{
    public class YesNoConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) =>
            text switch
            {
                "N" => YesNoSkip.No,
                "Y" => YesNoSkip.Yes,
                "" => YesNoSkip.Skip,
                _ => YesNoSkip.Skip
            };


        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) =>
            value switch
            {
                null => "",
                1 => "N",
                0 => "Y",
                false => "N",
                YesNoSkip.No => "N",
                YesNoSkip.Yes => "Y",
                YesNoSkip.Skip => "",
                true => "Y",
                _ => throw new InvalidOperationException("oops!")
            };
    }
}
