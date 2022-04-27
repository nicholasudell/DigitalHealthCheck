using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DigitalHealthCheckEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace UrlAvailabilityTests
{
    public class WebsiteAvailabilityTests : PageTest
    {
        [TestCaseSource(nameof(TestData))]
        public async Task UrlCanBeReached(string url, int interventionId)
        {
            if(url.EndsWith("pdf"))
            {
                //Playwright can't load PDFs so we use a lower tech solution
                await CheckUrlBasic(url);
            }
            else
            {
                await CheckUrlWithPlaywright(url, interventionId);
            }
        }

        [Test]
        [Explicit("This is a utility test for checking a URL that's not found in the interventions DB")]
        public async Task CustomUrlCanBeReached()
        {
            var url = "";
            var interventionId = 999;


            if (url.EndsWith("pdf"))
            {
                //Playwright can't load PDFs so we use a lower tech solution
                await CheckUrlBasic(url);
            }
            else
            {
                await CheckUrlWithPlaywright(url, interventionId);
            }
        }

        async Task CheckUrlBasic(string url)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;

                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "GET";

                using var response = await request.GetResponseAsync() as HttpWebResponse;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Assert.Fail($"Expected a successful status code from {url} but got {response.StatusCode}: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                throw new System.Exception($"Expected a successful status code from {url}", ex);
            }
        }

        async Task CheckUrlWithPlaywright(string url, int interventionId)
        {
            var response = await Page.GotoAsync(url);

            await Page.ScreenshotAsync(new PageScreenshotOptions { Path = $"screenshots/{interventionId}.png" });

            if (!response.Ok)
            {
                Assert.Fail($"Expected a successful status code from {url} but got {response.Status}: {response.StatusText}");
            }
        }




        static Database CreateDatabase()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(currentDirectory + "/appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<Database>();
            var connectionString = configuration.GetConnectionString("DatabaseConnection");

            builder.UseSqlServer(connectionString);

            return new Database(builder.Options);
        }

        static IEnumerable<TestCaseData> TestData()
        {
            using var database = CreateDatabase();

            return database.Interventions.Select(x =>
                new TestCaseData(x.Url, x.Id)
                    .SetDescription($"The URL at {x.Url} should return a website.")
                    .SetName($"Url_for_intervention_id_{x.Id}_exists"))
                .ToList();
        }
    }
}