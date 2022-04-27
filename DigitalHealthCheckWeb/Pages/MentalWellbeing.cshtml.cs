using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public class MentalWellbeingModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public string Anxious { get; set; }

            public string Control { get; set; }

            public string Disinterested { get; set; }

            public string FeelingDown { get; set; }

            public string UnderCare { get; set; }

            public string SubmitAction { get; set; }
        }

        private class SanitisedModel
        {
            public MentalWellbeingFrequency Anxious { get; set; }

            public MentalWellbeingFrequency Control { get; set; }

            public bool Disinterested { get; set; }

            public bool FeelingDown { get; set; }

            public bool UnderCare { get; set; }
        }

        public string AnxiousError { get; set; }

        public string ControlError { get; set; }

        public string DisinterestedError { get; set; }

        public string FeelingDownError { get; set; }

        public UnsanitisedModel Model { get; set; }

        public string UnderCareError { get; set; }

        public MentalWellbeingModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGetAsync()
        {
            var healthCheck = await GetHealthCheckAsync();

            Model = new UnsanitisedModel
            {
                UnderCare = healthCheck?.UnderCare.AsYesNoOrNull(),
                Disinterested = healthCheck?.Disinterested.AsYesNoOrNull(),
                FeelingDown = healthCheck?.FeelingDown.AsYesNoOrNull(),
                Anxious = healthCheck?.Anxious?.ToString().ToLowerInvariant(),
                Control = healthCheck?.Control?.ToString().ToLowerInvariant(),
            };
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel model)
        {
            Model = model;

            if (model.SubmitAction == "Submit")
            {

                var sanitisedModel = ValidateAndSanitise(model);

                if (sanitisedModel is null)
                {
                    return await Reload();
                }

                var healthCheck = await GetHealthCheckAsync();

                if (healthCheck is not null)
                {
                    healthCheck.UnderCare = sanitisedModel.UnderCare;
                    healthCheck.Disinterested = sanitisedModel.Disinterested;
                    healthCheck.FeelingDown = sanitisedModel.FeelingDown;
                    healthCheck.Anxious = sanitisedModel.Anxious;
                    healthCheck.Control = sanitisedModel.Control;
                    healthCheck.SkipMentalHealthQuestions = false;
                }

                await Database.SaveChangesAsync();
            }
            else
            { //User is skipping these Qs
                var healthCheck = await GetHealthCheckAsync();

                if (healthCheck is not null)
                {
                    healthCheck.UnderCare = null;
                    healthCheck.Disinterested = null;
                    healthCheck.FeelingDown = null;
                    healthCheck.Anxious = null;
                    healthCheck.Control = null;
                    healthCheck.SkipMentalHealthQuestions = true;
                }

                await Database.SaveChangesAsync();
            }

            return RedirectWithId("./Complete");
        }

        SanitisedModel ValidateAndSanitise(UnsanitisedModel model)
        {
            var isValid = true;

            var sanitisedModel = new SanitisedModel();

            if (model.FeelingDown.SanitiseYesNo(out var sanitisedFeelingDown))
            {
                sanitisedModel.FeelingDown = sanitisedFeelingDown;
            }
            else
            {
                FeelingDownError = $"Select yes if you have been bothered by feeling down, depressed or hopeless during the past month";
                AddError(FeelingDownError, "#feeling-down");
                isValid = false;
            }

            if (model.Disinterested.SanitiseYesNo(out var sanitisedDisinterested))
            {
                sanitisedModel.Disinterested = sanitisedDisinterested;
            }
            else
            {
                DisinterestedError = $"Select yes if you have often been bothered by little interest or pleasure in doing things during the past month";
                AddError(DisinterestedError, "#disinterested");
                isValid = false;
            }

            if (string.IsNullOrEmpty(model.Anxious) ||
                !Enum.TryParse<MentalWellbeingFrequency>(model.Anxious, true, out var sanitisedAnxious))
            {
                AnxiousError = $"Select how often over the last two weeks you have been bothered by feeling nervous, anxious or on edge";
                AddError(AnxiousError, "#anxious");
                isValid = false;
            }
            else
            {
                sanitisedModel.Anxious = sanitisedAnxious;
            }

            if (string.IsNullOrEmpty(model.Control) ||
                !Enum.TryParse<MentalWellbeingFrequency>(model.Control, true, out var sanitisedControl))
            {
                ControlError = $"Select how often over the last two weeks you have not been able to stop or control worrying";
                AddError(ControlError, "#control");
                isValid = false;
            }
            else
            {
                sanitisedModel.Control = sanitisedControl;
            }

            if (model.UnderCare.SanitiseYesNo(out var sanitisedUnderCare))
            {
                sanitisedModel.UnderCare = sanitisedUnderCare;
            }
            else
            {
                UnderCareError = $"Select yes if you are currently under the care of a psychologist or doctor for mental health problems";
                AddError(UnderCareError, "#under-care");
                isValid = false;
            }

            return isValid ? sanitisedModel : null;
        }
    }
}