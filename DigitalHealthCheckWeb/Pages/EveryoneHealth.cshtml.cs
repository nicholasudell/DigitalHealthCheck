using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;

namespace DigitalHealthCheckWeb.Pages
{
    public class EveryoneHealthEmailModel : HealthCheckPageModel
    {
        private readonly IEveryoneHealthReferralService everyoneHealthReferralService;

        public HealthCheck Check { get; set; }

        public string ReferralName { get; set; }

        public IEnumerable<Intervention> EveryoneHealthReferrals { get; private set; }

        public EveryoneHealthEmailModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageFlow pageFlow,
            IEveryoneHealthReferralService everyoneHealthReferralService
            ) :
            base(database, credentialsDecrypter, pageFlow)
        {
            this.everyoneHealthReferralService = everyoneHealthReferralService;
        }

        public async Task OnGetAsync()
        {
            Check = await GetHealthCheckAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();

            EveryoneHealthReferrals = everyoneHealthReferralService.GetEveryoneHealthReferrals(Check).ToList();
        }
    }
}