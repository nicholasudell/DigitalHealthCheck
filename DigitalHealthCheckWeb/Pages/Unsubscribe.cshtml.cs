using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalHealthCheckWeb.Pages
{
    public class UnsubscribeModel : HealthCheckPageModel
    {
        public bool Success { get; set; }

        public UnsubscribeModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow) 
            : base(database, credentialsDecrypter, pageFlow)
        {
        }

        public async Task OnGet()
        {
            if(Guid.TryParse(Id, out var id))
            {
                var check = await Database.HealthChecks.FindAsync(id);

                if(check is null)
                {
                    Success = false;
                }
                else
                {
                    check.EmailAddress = null;

                    await Database.SaveChangesAsync();

                    Success = true;
                }
            }
            else
            {
                Success = false;
            }
        }
    }
}
