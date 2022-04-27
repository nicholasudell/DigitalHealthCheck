using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Components.GDS;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class ValidationModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string DayOfBirth { get; set; }

            public string FirstName { get; set; }

            public string MonthOfBirth { get; set; }

            public string Postcode { get; set; }

            public string SubmitAction { get; set; }

            public string Surname { get; set; }

            public string YearOfBirth { get; set; }
        }

        private class SanitisedModel
        {
            public DateTime DateOfBirth { get; set; }

            public string FirstName { get; set; }

            public string Postcode { get; set; }

            public string Surname { get; set; }
        }

        public bool CouldNotValidate { get; set; }

        public bool DateOfBirthDayInvalid { get; set; }

        public string DateOfBirthError { get; set; }

        public bool DateOfBirthMonthInvalid { get; set; }

        public bool DateOfBirthYearInvalid { get; set; }

        public string FirstNameError { get; set; }

        public string PostcodeError { get; set; }

        public string SurnameError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public ValidationModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            Model = new UnsanitisedModel();

            await UpdateBackLink();
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            var healthCheck = await GetHealthCheckAsync();

            var sanitisedModel = await ValidateAndSanitise(healthCheck, model);

            if (sanitisedModel is null)
            {
                await Database.SaveChangesAsync();
                await UpdateBackLink();
                return await Reload();
            }

            if (model.SubmitAction == "Skip")
            {
                return await Skip(sanitisedModel);
            }
            else
            {
                return await Authenticate(sanitisedModel);
            }
        }

        static bool CompareStrings(string one, string two) =>
            string.Equals(
                one?.Replace(" ", string.Empty),
                two?.Replace(" ", string.Empty),
                StringComparison.OrdinalIgnoreCase
            );

        private async Task<IActionResult> Authenticate(SanitisedModel sanitisedModel)
        {
            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is null)
            {
                CouldNotValidate = true;
                return await Reload();
            }
            else
            {
                var isValid = true;

                isValid &= CompareStrings(healthCheck.ValidationPostcode, sanitisedModel.Postcode);
                isValid &= CompareStrings(healthCheck.ValidationSurname, sanitisedModel.Surname);
                isValid &= healthCheck.ValidationDateOfBirth == sanitisedModel.DateOfBirth;

                if (isValid)
                {
                    //We keep measurements we use for validation and measurements entered separate
                    //to prevent us from accidentally revealing patient information to the wrong person.

                    healthCheck.Postcode = sanitisedModel.Postcode;
                    healthCheck.DateOfBirth = sanitisedModel.DateOfBirth;
                    healthCheck.Surname = sanitisedModel.Surname;
                    healthCheck.FirstName = sanitisedModel.FirstName;
                    healthCheck.Age = CalculateAge(sanitisedModel.DateOfBirth);

                    await Database.SaveChangesAsync();

                    HttpContext.Session.SetString("Validated", healthCheck.ValidationOverwritten ?
                        YesNoSkip.Skip.ToString() :
                        YesNoSkip.Yes.ToString());

                    return RedirectWithId("./CheckYourAnswers");
                }
                else
                {
                    CouldNotValidate = true;

                    ErrorList.Add(new GDSErrorSummary.Item
                    {
                        Content = "We have been unable to verify your identity. This means we can't send your results to your GP clinic to update your patient records. Please continue with your NHS Health Check and get your results. You can then share these results with your GP clinic directly."
                    });

                    return await Reload();
                }
            }
        }

        static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;

            var age = today.Year - birthDate.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        private async Task<IActionResult> Skip(SanitisedModel sanitisedModel)
        {
            // User cannot authenticate and is going to just view the results

            var healthCheck = await GetHealthCheckAsync();

            if (healthCheck is not null)
            {
                // Because we won't have gotten validation data from the invites, and we need a
                // postcode to do QRISK, we set this now

                healthCheck.Postcode = sanitisedModel.Postcode;
                healthCheck.DateOfBirth = sanitisedModel.DateOfBirth;
                healthCheck.Surname = sanitisedModel.Surname;
                healthCheck.FirstName = sanitisedModel.FirstName;
                healthCheck.Age = CalculateAge(sanitisedModel.DateOfBirth);

                // Set this data for validation in the future on return links, e.g.

                healthCheck.ValidationDateOfBirth = sanitisedModel.DateOfBirth;
                healthCheck.ValidationPostcode = sanitisedModel.Postcode;
                healthCheck.ValidationSurname = sanitisedModel.Surname;
                healthCheck.ValidationOverwritten = true;

                await Database.SaveChangesAsync();
            }

            HttpContext.Session.SetString("Validated", YesNoSkip.Skip.ToString());

            return RedirectWithId("./CheckYourAnswers");
        }

        async Task<SanitisedModel> ValidateAndSanitise(HealthCheck check, UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (string.IsNullOrEmpty(model.Postcode))
            {
                PostcodeError = "Enter your postcode.";
                AddError(PostcodeError, "#postcode");

                isValid = false;
            }
            else if (!model.Postcode.IsValidPostcode())
            {
                isValid = false;
                PostcodeError = "Enter a real postcode.";
                await AddError(check, PostcodeError, "#postcode");
            }
            else
            {
                sanitisedModel.Postcode = model.Postcode.Replace(" ","").ToUpper();
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                FirstNameError = "Enter your first name.";
                AddError(FirstNameError, "#firstname");

                isValid = false;
            }
            else
            {
                sanitisedModel.FirstName = model.FirstName.Trim();
            }

            if (string.IsNullOrEmpty(model.Surname))
            {
                SurnameError = "Enter your surname.";
                AddError(SurnameError, "#surname");

                isValid = false;
            }
            else
            {
                sanitisedModel.Surname = model.Surname.Trim();
            }

            if (string.IsNullOrEmpty(model.YearOfBirth) && string.IsNullOrEmpty(model.MonthOfBirth) && string.IsNullOrEmpty(model.DayOfBirth))
            {
                DateOfBirthError = "Enter your date of birth.";
                DateOfBirthDayInvalid = true;
                DateOfBirthMonthInvalid = true;
                DateOfBirthYearInvalid = true;
                AddError(DateOfBirthError, "#day-of-birth");

                isValid = false;
            }
            else
            {
                var dateIsValid = true;

                if (string.IsNullOrEmpty(model.DayOfBirth))
                {
                    dateIsValid = false;

                    if (string.IsNullOrEmpty(model.MonthOfBirth))
                    {
                        DateOfBirthError = "Your date of birth must include a day and month.";

                        DateOfBirthDayInvalid = true;
                        DateOfBirthMonthInvalid = true;
                        DateOfBirthYearInvalid = true;

                        await AddError(check, DateOfBirthError, "#day-of-birth");
                    }
                    else if (string.IsNullOrEmpty(model.YearOfBirth))
                    {
                        DateOfBirthError = "Your date of birth must include a day and year.";

                        DateOfBirthDayInvalid = true;
                        DateOfBirthMonthInvalid = true;
                        DateOfBirthYearInvalid = true;

                        await AddError(check, DateOfBirthError, "#day-of-birth");
                    }
                    else
                    {
                        DateOfBirthDayInvalid = true;
                        DateOfBirthError = "Your date of birth must include a day.";
                        await AddError(check, DateOfBirthError, "#day-of-birth");
                    }
                } else if (string.IsNullOrEmpty(model.MonthOfBirth))
                {

                    if (string.IsNullOrEmpty(model.YearOfBirth))
                    {
                        DateOfBirthError = "Your date of birth must include a month and year.";

                        DateOfBirthDayInvalid = true;
                        DateOfBirthMonthInvalid = true;
                        DateOfBirthYearInvalid = true;

                        await AddError(check, DateOfBirthError, "#day-of-birth");
                    }
                    else
                    {
                        DateOfBirthError = "Your date of birth must include a month.";
                        DateOfBirthMonthInvalid = true;
                        await AddError(check, DateOfBirthError, "#month-of-birth");
                    }

                    dateIsValid = false;
                } else if (string.IsNullOrEmpty(model.YearOfBirth))
                {
                    DateOfBirthError = "Your date of birth must include a year.";

                    DateOfBirthYearInvalid = true;
                    
                    await AddError(check, DateOfBirthError, "#year-of-birth");

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                if (!int.TryParse(model.DayOfBirth, out var dayOfBirthSanitised))
                {
                    DateOfBirthError = "Your date of birth must be three numbers like 1 1 1995";
                    await AddError(check, DateOfBirthError, "#day-of-birth");

                    DateOfBirthDayInvalid = true;

                    dateIsValid = false;
                }

                if (!int.TryParse(model.MonthOfBirth, out var monthOfBirthSanitised))
                {
                    if (dateIsValid)
                    {
                        DateOfBirthError = "Your date of birth must be three numbers like 1 1 1995";
                        await AddError(check, DateOfBirthError, "#month-of-birth");
                    }

                    DateOfBirthMonthInvalid = true;

                    dateIsValid = false;
                }

                if (!int.TryParse(model.YearOfBirth, out var yearOfBirthSanitised))
                {
                    if (dateIsValid)
                    {
                        DateOfBirthError = "Your date of birth must be three numbers like 1 1 1995";
                        await AddError(check, DateOfBirthError, "#year-of-birth");
                    }

                    DateOfBirthYearInvalid = true;

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                if (dayOfBirthSanitised > 31 || dayOfBirthSanitised < 1)
                {
                    DateOfBirthError = "Your date of birth must be a real date";
                    await AddError(check, DateOfBirthError, "#day-of-birth");

                    DateOfBirthDayInvalid = true;
                    dateIsValid = false;
                }

                if (monthOfBirthSanitised > 12 || monthOfBirthSanitised < 1)
                {
                    if (dateIsValid)
                    {
                        DateOfBirthError = "Your date of birth must be a real date";
                        await AddError(check, DateOfBirthError, "#month-of-birth");
                    }

                    DateOfBirthMonthInvalid = true;

                    dateIsValid = false;
                }

                if (!dateIsValid)
                {
                    return null;
                }

                var sanitisedDateTime = new DateTime(yearOfBirthSanitised, monthOfBirthSanitised, dayOfBirthSanitised);

                if (DateTime.UtcNow < sanitisedDateTime)
                {
                    DateOfBirthError = "Your date of birth must be in the past.";

                    await AddError(check, DateOfBirthError, "#day-of-birth");

                    DateOfBirthYearInvalid = true;
                    DateOfBirthMonthInvalid = true;
                    DateOfBirthDayInvalid = true;

                    isValid = false;
                }
                else
                {
                    sanitisedModel.DateOfBirth = sanitisedDateTime;
                }
            }

            return isValid ? sanitisedModel : null;
        }
    }
}