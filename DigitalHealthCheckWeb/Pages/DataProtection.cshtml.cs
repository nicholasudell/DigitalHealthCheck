using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;

namespace DigitalHealthCheckWeb.Pages
{
    public class DataProtectionPageModel : HealthCheckPageModel
    {
        public DataProtectionPageModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow)
        {
        }

        public void OnGet()
        {
        }
    }
}