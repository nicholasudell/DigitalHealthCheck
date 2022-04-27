using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using DigitalHealthCheckCommon;
using LinkGeneratorCommon;
using QMSUK.DigitalHealthCheck.Encryption;

namespace GenerateLinks
{

    internal class Program
    {
        const string AesHexKey = "<256-bit-hex-key>";

        static IEnumerable<string> GenerateLinks(string sourceFile, string linkFormat)
        {
            var records = new CredentialsFileLoader().LoadCredentials(sourceFile);

            var linkGenerator = new LinkGenerator(
                new HealthCheckCredentialsEncrypter(new UrlOptimisedAesEncrypter(AesHexKey), new NewtonsoftJsonSerializationWrapper<Credentials>()),
                linkFormat
            );

            return records.Select(x => linkGenerator.GenerateLink(Guid.NewGuid(), x)).ToList();
        }

        static void Main(string[] args)
        {
            var sourceFile = args[0];
            var linkFormat = args.ArgOrDefault(1, "<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/?id={0}&hash={1}");
            var outputFile = args.ArgOrDefault(2, "links.txt");

            var links = GenerateLinks(sourceFile, linkFormat);

            File.WriteAllLines(outputFile, links);
        }
    }
}