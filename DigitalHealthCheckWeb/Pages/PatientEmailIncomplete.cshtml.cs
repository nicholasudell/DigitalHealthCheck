using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;

namespace DigitalHealthCheckWeb.Pages
{
    public class PatientEmailIncompleteModel : HealthCheckPageModel
    {
        private readonly IHealthCheckResultFactory healthCheckResultFactory;

        public HealthCheck Check { get; set; }

        public Result Result { get; set; }

        public bool Validated { get; set; }

        public PatientEmailIncompleteModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IHealthCheckResultFactory healthCheckResultFactory,
            IPageFlow pageFlow) :
            base(database, credentialsDecrypter, pageFlow) => this.healthCheckResultFactory = healthCheckResultFactory;

        public async Task OnGetAsync()
        {
            Check = await GetHealthCheckAsync();

            await Database.Entry(Check).Collection(c => c.ChosenInterventions).LoadAsync();

            Result = healthCheckResultFactory.GetResult(Check, false);

            Validated = base.IsValidated();
        }
    }
}