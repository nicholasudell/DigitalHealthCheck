using System;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;

namespace DigitalHealthCheckWeb.Pages
{
    public class GPEmailModel : HealthCheckPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public HealthCheck Check { get; set; }

        public Result Result { get; set; }

        public GPEmailModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IPageFlow pageFlow,
            IHealthCheckResultFactory healthCheckResultFactory) :
            base(database, credentialsDecrypter, pageFlow) => this.healthCheckResultFactory = healthCheckResultFactory ?? throw new ArgumentNullException(nameof(healthCheckResultFactory));

        public async Task OnGetAsync()
        {
            Check = await GetHealthCheckAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();

            Result = healthCheckResultFactory.GetResult(Check, false);
        }
    }
}