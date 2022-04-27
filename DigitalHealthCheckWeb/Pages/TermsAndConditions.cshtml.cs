using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalHealthCheckWeb.Pages
{
    public class TermsAndConditionsPageModel : HealthCheckPageModel
    {
        public TermsAndConditionsPageModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public void OnGet()
        {
        }
    }
}
