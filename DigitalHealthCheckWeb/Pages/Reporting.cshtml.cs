using System;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using DigitalHealthCheckEF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CsvHelper.Configuration;
using System.Collections.Generic;
using DigitalHealthCheckWeb.Model;
using DigitalHealthCheckWeb.Components.GDS;

namespace DigitalHealthCheckWeb.Pages
{
    [Authorize]
    public partial class ReportingModel : PageModel
    {
        public class UnsanitisedModel
        {
            public string ReportKey { get; set; }
            public string FromDateDay { get; set; }
            public string FromDateMonth { get; set; }
            public string FromDateYear { get; set; }
            public string ToDateDay { get; set; }
            public string ToDateMonth { get; set; }
            public string ToDateYear { get; set; }
        }

        public IList<GDSErrorSummary.Item> ErrorList { get; set; } = new List<GDSErrorSummary.Item>();

        

        

        private class SanitisedModel
        {
            public string ReportKey { get; set; }

            public DateTime? FromDate { get; set; }

            public DateTime? ToDate { get; set; }
        }

        private readonly Database database;
        private readonly IPostcodeLookupService postcodeLookupService;

        public UnsanitisedModel Model { get; set; }

        public ReportingModel(Database database, IPostcodeLookupService postcodeLookupService)
        {
            this.database = database;
            this.postcodeLookupService = postcodeLookupService;
        }

        public void OnGet()
        {
            Model = new UnsanitisedModel();
        }

        protected void AddError(string message, string inputId) => ErrorList.Add(new GDSErrorSummary.Item
        {
            Content = message,
            Href = inputId
        });

        public bool ToDateDayInvalid { get; set; }

        public string ToDateError { get; set; }

        public bool ToDateMonthInvalid { get; set; }

        public bool ToDateYearInvalid { get; set; }

        public bool FromDateDayInvalid { get; set; }

        public string FromDateError { get; set; }

        public bool FromDateMonthInvalid { get; set; }

        public bool FromDateYearInvalid { get; set; }

        public async Task<IActionResult> OnPost(UnsanitisedModel model)
        {
            var sanitisedModel = ValidateAndSanitise(model);

            if(sanitisedModel is null)
            {
                return Page();
            }

            Report report = sanitisedModel.ReportKey switch
            {
                "AnonymisedDataSet" => new AnonymisedDatasetReport(database, sanitisedModel.FromDate, sanitisedModel.ToDate),
                "AnonymisedBarriers" => new AnonymisedBarriersReport(database, sanitisedModel.FromDate, sanitisedModel.ToDate),
                "AnonymisedInterventions" => new AnonymisedInterventionsReport(database, sanitisedModel.FromDate, sanitisedModel.ToDate),
                "Thriva" => new ThrivaReport(database, sanitisedModel.FromDate, sanitisedModel.ToDate),
                "UptakeRate" => new Model.UptakeRateReport(database, sanitisedModel.FromDate, sanitisedModel.ToDate),
                "InterventionEligibility" => new Model.InterventionEligibilityReport(database, sanitisedModel.FromDate, sanitisedModel.ToDate),
                "BiometricAssessmentSuccess" => new Model.BiometricAssessmentSuccessReport(database, sanitisedModel.FromDate, sanitisedModel.ToDate),
                "DetailedUptake" => new Model.DetailedUptakeReport(database, sanitisedModel.FromDate, sanitisedModel.ToDate),
                _ => throw new NotSupportedException($"Report type {model.ReportKey} is not supported.")
            };

            var data = await report.Generate();

            return File(data, "text/csv", GenerateFileName(sanitisedModel));
        }


        static string GenerateFileName(SanitisedModel model)
        {
            var extension = model.ReportKey == "AnonymisedDataSet" ? "tsv" : "csv";

            var dateString = DateTime.Today.ToString("dd-MM-yyyy");

            if(model.FromDate.HasValue)
            {
                if(model.ToDate.HasValue)
                {
                    dateString = $"from_{model.FromDate.Value.Date:dd-MM-yyyy}_to_{model.ToDate.Value.Date:dd-MM-yyyy}";
                }
                else
                {
                    dateString = $"from_{model.FromDate.Value.Date:dd-MM-yyyy}";
                }
            }
            else if(model.ToDate.HasValue)
            {
                dateString = $"to_{model.ToDate.Value.Date:dd-MM-yyyy}";
            }

            return $"NHSDHC_{model.ReportKey}_Report_{dateString}.{extension}";
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel()
            {
                 ReportKey = model.ReportKey
            };

            

            if (string.IsNullOrEmpty(model.FromDateYear) && string.IsNullOrEmpty(model.FromDateMonth) && string.IsNullOrEmpty(model.FromDateDay))
            {
                // It is valid to not specify a from date
            }
            else
            {
                var dateIsValid = true;

                if (string.IsNullOrEmpty(model.FromDateDay))
                {
                    dateIsValid = false;

                    if (string.IsNullOrEmpty(model.FromDateMonth))
                    {
                        FromDateError = "The from date must include a day and month.";

                        FromDateDayInvalid = true;
                        FromDateMonthInvalid = true;
                        FromDateYearInvalid = true;

                        AddError(FromDateError, "#from-date-day");
                    }
                    else if (string.IsNullOrEmpty(model.FromDateYear))
                    {
                        FromDateError = "The from date must include a day and year.";

                        FromDateDayInvalid = true;
                        FromDateMonthInvalid = true;
                        FromDateYearInvalid = true;

                        AddError(FromDateError, "#from-date-day");
                    }
                    else
                    {
                        FromDateDayInvalid = true;
                        FromDateError = "The from date must include a day.";
                        AddError(FromDateError, "#from-date-day");
                    }
                }
                else if (string.IsNullOrEmpty(model.FromDateMonth))
                {

                    if (string.IsNullOrEmpty(model.FromDateYear))
                    {
                        FromDateError = "The from date must include a month and year.";

                        FromDateDayInvalid = true;
                        FromDateMonthInvalid = true;
                        FromDateYearInvalid = true;

                        AddError(FromDateError, "#from-date-day");
                    }
                    else
                    {
                        FromDateError = "The from date must include a month.";
                        FromDateMonthInvalid = true;
                        AddError(FromDateError, "#from-date-month");
                    }

                    dateIsValid = false;
                }
                else if (string.IsNullOrEmpty(model.FromDateYear))
                {
                    FromDateError = "The from date must include a year.";

                    FromDateYearInvalid = true;

                    AddError(FromDateError, "#from-date-year");

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                if (!int.TryParse(model.FromDateDay, out var FromDateDaySanitised))
                {
                    FromDateError = "The from date must be three numbers like 1 1 1995";
                    AddError(FromDateError, "#from-date-day");

                    FromDateDayInvalid = true;

                    dateIsValid = false;
                }

                if (!int.TryParse(model.FromDateMonth, out var FromDateMonthSanitised))
                {
                    if (dateIsValid)
                    {
                        FromDateError = "The from date must be three numbers like 1 1 1995";
                        AddError(FromDateError, "#from-date-month");
                    }

                    FromDateMonthInvalid = true;

                    dateIsValid = false;
                }

                if (!int.TryParse(model.FromDateYear, out var FromDateYearSanitised))
                {
                    if (dateIsValid)
                    {
                        FromDateError = "The from date must be three numbers like 1 1 1995";
                        AddError(FromDateError, "#from-date-year");
                    }

                    FromDateYearInvalid = true;

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                if (FromDateDaySanitised > 31 || FromDateDaySanitised < 1)
                {
                    FromDateError = "The from date must be a real date";
                    AddError(FromDateError, "#from-date-day");

                    FromDateDayInvalid = true;
                    dateIsValid = false;
                }

                if (FromDateMonthSanitised > 12 || FromDateMonthSanitised < 1)
                {
                    if (dateIsValid)
                    {
                        FromDateError = "The from date must be a real date";
                        AddError(FromDateError, "#from-date-month");
                    }

                    FromDateMonthInvalid = true;

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                var sanitisedDateTime = new DateTime(FromDateYearSanitised, FromDateMonthSanitised, FromDateDaySanitised);

                if (DateTime.UtcNow < sanitisedDateTime)
                {
                    FromDateError = "The from date must be in the past.";

                    AddError(FromDateError, "#from-date-day");

                    FromDateYearInvalid = true;
                    FromDateMonthInvalid = true;
                    FromDateDayInvalid = true;

                    isValid = false;
                }
                else
                {
                    sanitisedModel.FromDate = sanitisedDateTime;
                }
            }



            if (string.IsNullOrEmpty(model.ToDateYear) && string.IsNullOrEmpty(model.ToDateMonth) && string.IsNullOrEmpty(model.ToDateDay))
            {
                // It is valid to not specify a To date
            }
            else
            {
                var dateIsValid = true;

                if (string.IsNullOrEmpty(model.ToDateDay))
                {
                    dateIsValid = false;

                    if (string.IsNullOrEmpty(model.ToDateMonth))
                    {
                        ToDateError = "The To date must include a day and month.";

                        ToDateDayInvalid = true;
                        ToDateMonthInvalid = true;
                        ToDateYearInvalid = true;

                        AddError(ToDateError, "#To-date-day");
                    }
                    else if (string.IsNullOrEmpty(model.ToDateYear))
                    {
                        ToDateError = "The To date must include a day and year.";

                        ToDateDayInvalid = true;
                        ToDateMonthInvalid = true;
                        ToDateYearInvalid = true;

                        AddError(ToDateError, "#To-date-day");
                    }
                    else
                    {
                        ToDateDayInvalid = true;
                        ToDateError = "The To date must include a day.";
                        AddError(ToDateError, "#To-date-day");
                    }
                }
                else if (string.IsNullOrEmpty(model.ToDateMonth))
                {

                    if (string.IsNullOrEmpty(model.ToDateYear))
                    {
                        ToDateError = "The To date must include a month and year.";

                        ToDateDayInvalid = true;
                        ToDateMonthInvalid = true;
                        ToDateYearInvalid = true;

                        AddError(ToDateError, "#To-date-day");
                    }
                    else
                    {
                        ToDateError = "The To date must include a month.";
                        ToDateMonthInvalid = true;
                        AddError(ToDateError, "#To-date-month");
                    }

                    dateIsValid = false;
                }
                else if (string.IsNullOrEmpty(model.ToDateYear))
                {
                    ToDateError = "The To date must include a year.";

                    ToDateYearInvalid = true;

                    AddError(ToDateError, "#To-date-year");

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                if (!int.TryParse(model.ToDateDay, out var ToDateDaySanitised))
                {
                    ToDateError = "The To date must be three numbers like 1 1 1995";
                    AddError(ToDateError, "#To-date-day");

                    ToDateDayInvalid = true;

                    dateIsValid = false;
                }

                if (!int.TryParse(model.ToDateMonth, out var ToDateMonthSanitised))
                {
                    if (dateIsValid)
                    {
                        ToDateError = "The To date must be three numbers like 1 1 1995";
                        AddError(ToDateError, "#To-date-month");
                    }

                    ToDateMonthInvalid = true;

                    dateIsValid = false;
                }

                if (!int.TryParse(model.ToDateYear, out var ToDateYearSanitised))
                {
                    if (dateIsValid)
                    {
                        ToDateError = "The To date must be three numbers like 1 1 1995";
                        AddError(ToDateError, "#To-date-year");
                    }

                    ToDateYearInvalid = true;

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                if (ToDateDaySanitised > 31 || ToDateDaySanitised < 1)
                {
                    ToDateError = "The To date must be a real date";
                    AddError(ToDateError, "#To-date-day");

                    ToDateDayInvalid = true;
                    dateIsValid = false;
                }

                if (ToDateMonthSanitised > 12 || ToDateMonthSanitised < 1)
                {
                    if (dateIsValid)
                    {
                        ToDateError = "The To date must be a real date";
                        AddError(ToDateError, "#To-date-month");
                    }

                    ToDateMonthInvalid = true;

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                var sanitisedDateTime = new DateTime(ToDateYearSanitised, ToDateMonthSanitised, ToDateDaySanitised);

                sanitisedModel.ToDate = sanitisedDateTime;
            }



            return isValid ? sanitisedModel : null;
        }
    }
}
