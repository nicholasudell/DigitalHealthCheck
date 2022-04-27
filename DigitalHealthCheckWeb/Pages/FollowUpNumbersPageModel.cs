using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHealthCheckWeb.Pages
{
    public abstract class FollowUpNumbersPageModel : FollowUpPageModel
    {
        public new class UnsanitisedModel : FollowUpPageModel.UnsanitisedModel
        {
            public string SelectedOption { get; set; }
        }

        public new class SanitisedModel : FollowUpPageModel.SanitisedModel
        {
            public string SelectedOption { get; set; }
        }

        public new UnsanitisedModel Model { get; set; }

        public string SelectedOptionError { get; set; }

        protected abstract string SelectedOptionErrorMessage { get; }

        protected FollowUpNumbersPageModel(Database database, ICredentialsDecrypter credentialsDecrypter, IHealthPriorityRouter healthPriorityRouter, IPageFlow pageFlow)
            : base(database, credentialsDecrypter, healthPriorityRouter, pageFlow)
        {
        }

        protected override async Task LoadPageData()
        {
            await base.LoadPageData();

            if (Model is null) //FollowUp might already have been set, e.g. onPost
            {
                var followUp = await GetFollowUpAsync(await GetHealthCheckAsync());

                Model = new UnsanitisedModel()
                {
                    SetADate = base.Model.SetADate,
                    Barriers = base.Model.Barriers,
                    BeReminded = base.Model.BeReminded,
                    ConfidentToChange = base.Model.ConfidentToChange,
                    DoYouHavePlans = base.Model.DoYouHavePlans,
                    EmailAddress = base.Model.EmailAddress,
                    ImportantToChange = base.Model.ImportantToChange,
                    OtherBarrier = base.Model.OtherBarrier,
                    SelectedOption = followUp?.SelectedOption
                };
            }

            base.Model = Model;
        }

    protected virtual async Task<IActionResult> PostAsync(UnsanitisedModel model)
    {
        Model = model;

        //Need to have pagedata loaded to know if we require an email address
        await LoadPageData();

        var sanitisedModel = await ValidateAndSanitise(model);

        if (sanitisedModel == null)
        {
            return await Reload();
        }

        await SaveData(new FollowUp()
        {
            Type = CurrentRoute,
            BeReminded = sanitisedModel.BeReminded,
            ConfidentToChange = sanitisedModel.ConfidentToChange,
            DoYouHavePlans = sanitisedModel.DoYouHavePlans,
            ImportantToChange = sanitisedModel.ImportantToChange,
            SetADate = sanitisedModel.SetADate,
            SelectedOption = sanitisedModel.SelectedOption
        }, sanitisedModel.EmailAddress, sanitisedModel.SelectedBarriers, sanitisedModel.OtherBarrier);

        return ToNextPage(model.SubmitAction, await GetHealthCheckAsync());
    }

    protected virtual async Task<SanitisedModel> ValidateAndSanitise(UnsanitisedModel model)
    {
        var sanitisedBase = await base.ValidateAndSanitise(model);

        if (sanitisedBase is null)
        {
            return null;
        }

        var isValid = true;

        var sanitisedModel = new SanitisedModel
        {
            SetADate = sanitisedBase.SetADate,
            BeReminded = sanitisedBase.BeReminded,
            ConfidentToChange = sanitisedBase.ConfidentToChange,
            DoYouHavePlans = sanitisedBase.DoYouHavePlans,
            ImportantToChange = sanitisedBase.ImportantToChange,
            SelectedOption = model.SelectedOption,
            EmailAddress = model.EmailAddress,
            SelectedBarriers = sanitisedBase.SelectedBarriers,
            OtherBarrier = sanitisedBase.OtherBarrier
        };

        if (string.IsNullOrEmpty(model.SelectedOption))
        {
            SelectedOptionError = SelectedOptionErrorMessage;

            InsertErrorBefore(SelectedOptionError, "#options", "#important-to-change", "#confident-to-change");

            isValid = false;
        }
        else
        {
            sanitisedModel.SelectedOption = model.SelectedOption;
        }

        return isValid ? sanitisedModel : null;
    }
}
}