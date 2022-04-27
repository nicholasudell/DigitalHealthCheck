using Microsoft.AspNetCore.Components;

namespace DigitalHealthCheckWeb.Components.Pages
{
    public abstract class FollowUpPageBase : Page
    {
        [Parameter]
        public bool AskForEmailAddress { get; set; }

        [Parameter]
        public string BeReminded { get; set; }

        [Parameter]
        public string BeRemindedError { get; set; }

        [Parameter]
        public string ConfidentToChange { get; set; }

        [Parameter]
        public string ConfidentToChangeError { get; set; }

        [Parameter]
        public string DoYouHavePlans { get; set; }

        [Parameter]
        public string DoYouHavePlansError { get; set; }

        [Parameter]
        public string EmailAddress { get; set; }

        [Parameter]
        public string EmailAddressError { get; set; }

        [Parameter]
        public string ImportantToChange { get; set; }

        [Parameter]
        public string ImportantToChangeError { get; set; }

        [Parameter]
        public bool IsFinalFollowUp { get; set; }

        [Parameter]
        public string NextHealthPriorityName { get; set; }

        [Parameter]
        public int NextHealthPriorityNumber { get; set; }

        [Parameter]
        public string SetADate { get; set; }

        [Parameter]
        public string SetADateError { get; set; }

        [Parameter]
        public bool ShowSkipFollowUps { get; set; }
    }
}