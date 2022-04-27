using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DigitalHealthCheckWeb.Model
{
    public class BooleanToYNConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) =>
            text switch
            {
                "N" => false,
                "Y" => true,
                _ => null
            };


        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData) =>
            value switch
            {
                null => "NULL",
                0 => "N",
                1 => "Y",
                false => "N",
                true => "Y",
                _ => throw new InvalidOperationException("oops!")
            };
    }
}
