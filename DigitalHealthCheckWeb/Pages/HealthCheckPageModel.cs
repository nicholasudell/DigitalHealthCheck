using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Components.GDS;
using DigitalHealthCheckWeb.Helpers;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckWeb.Pages
{
    public abstract class HealthCheckPageModel : PageModel
    {
        readonly Lazy<Credentials> credentials;

        private readonly IPageFlow pageFlow;

        public Credentials Credentials => credentials.Value;

        [FromQuery(Name = "hash")]
        public string Hash { get; set; }

        [FromQuery(Name = "id")]
        public string Id { get; set; }

        [FromQuery(Name = "variant")]
        public bool Variant { get; set; }

        public string LastPage { get; set; }

        [FromQuery(Name = "next")]
        public string NextPage { get; set; }

        /// <summary>
        /// What to show after the next page.
        /// </summary>
        /// <value>The then page.</value>
        [FromQuery(Name = "then")]
        public string ThenPage { get; set; }

        protected Database Database { get; private set; }

        protected virtual string NextPageRelative => string.IsNullOrEmpty(NextPage) ? null : $"./{NextPage}";

        protected Guid? UserId => string.IsNullOrEmpty(Id) ? Guid.NewGuid() : new Guid(Id);

        public HealthCheckPageModel(Database database, ICredentialsDecrypter credentialsDecrypter, IPageFlow pageFlow)
        {
            Database = database;
            this.pageFlow = pageFlow;
            credentials = new Lazy<Credentials>(() =>
                !string.IsNullOrEmpty(Hash) ?
                    credentialsDecrypter.Decrypt(Hash) :
                    null);
        }

        public IList<GDSErrorSummary.Item> ErrorList { get; set; } = new List<GDSErrorSummary.Item>();

        protected void AddError(string message, string inputId) => ErrorList.Add(new GDSErrorSummary.Item
        {
            Content = message,
            Href = inputId
        });

        protected async Task AddError(HealthCheck check, string message, string inputId)
        {
            await Database.ValidationErrors.AddAsync(new ValidationError()
            {
                HealthCheck = check,
                ErrorControl = inputId,
                ErrorMessage = message,
                Page = CurrentPage
            });

            AddError(message, inputId);
        }

        protected virtual async Task<HealthCheck> GetHealthCheckAsync()
        {
            var check = await GetOrCreateHealthCheck();

            await UpdateBackLink();

            return check;
        }

        protected async Task<HealthCheck> GetOrCreateHealthCheck()
        {
            var healthCheck = await Database.HealthChecks.FindAsync(UserId);

            if (healthCheck is not null || !UserId.HasValue)
            {
                if(Variant)
                {
                    await Database.Entry(healthCheck).Reference(c => c.Variant).LoadAsync();
                }

                return healthCheck;
            }

            var newId = UserId.Value;

            var check = new HealthCheck
            {
                Id = newId,
                CheckStartedDate = DateTime.Now
            };

            Id = newId.ToString();

            if (Credentials != null)
            {
                check.ValidationPostcode = Credentials.Postcode.Replace(" ","").ToUpper();
                check.ValidationSurname = Credentials.Surname;
                check.ValidationDateOfBirth = Credentials.DateOfBirth;
                check.NHSNumber = Credentials.NHSNumber;
                check.GPSurgery = Credentials.GPSurgery;
                check.GPEmail = Credentials.GPEmail;
            }

            await Database.HealthChecks.AddAsync(check);

            try
            {
                await Database.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                //Two concurrent requests from a validated user could cause this save to fail,
                //but in that case we should continue, because what we wanted to happen has happened.
            }

            return check;
        }

        protected void InsertErrorAfter(string message, string inputId, params string[] priorInputIds)
        {
            // We need to insert this error prior to any of the laterInputIds
            var index = -1;

            for (var i = 0; i < ErrorList.Count; i++)
            {
                var error = ErrorList[i];

                if (priorInputIds.Contains(error.Href))
                {
                    index = i + 1;
                    break;
                }
            }

            ErrorList.Insert(index == -1 ? 0 : index, new GDSErrorSummary.Item
            {
                Content = message,
                Href = inputId
            });
        }

        protected void InsertErrorBefore(string message, string inputId, params string[] laterInputIds)
        {
            // We need to insert this error prior to any of the laterInputIds
            var index = -1;

            for (var i = ErrorList.Count - 1; i >= 0; i--)
            {
                var error = ErrorList[i];

                if (laterInputIds.Contains(error.Href))
                {
                    index = i + 1;
                    break;
                }
            }

            ErrorList.Insert(index == -1 ? 0 : index, new GDSErrorSummary.Item
            {
                Content = message,
                Href = inputId
            });
        }

        protected bool IsValidated()
        {
            var validationStatus = HttpContext.Session.GetEnum<YesNoSkip>("Validated") ?? YesNoSkip.No;

            return validationStatus != YesNoSkip.No;
        }

        protected RedirectToPageResult RedirectToValidation() =>
            RedirectWithId("./Validation", next:HttpContext.Request.CurrentPage());

        protected RedirectToPageResult RedirectWithId(string pageName, string next = null, string then = null, bool? variant = null)
        {
            dynamic routeValues = new ExpandoObject();

            routeValues.id = UserId;
            routeValues.variant = variant ?? Variant;

            if (string.IsNullOrEmpty(next))
            {
                routeValues.next = ThenPage; //go to the then page next, if it exists.
            }
            else
            {
                routeValues.next = next;
                routeValues.then = then ?? ThenPage; //preserve the then page in cases of redirects while then is still on the stack
            }

            return RedirectToPage(NextPageRelative ?? pageName, routeValues);
        }

        protected virtual async Task<IActionResult> Reload()
        {
            await UpdateBackLink();
            return Page();
        }

        protected async Task UpdateBackLink()
        {
            var check = await Database.HealthChecks.FindAsync(UserId);

            ViewData["BackPage"] = await pageFlow.PreviousPage(check, CurrentPage);
        }

        protected string CurrentPage
        {
            get
            {
                var currentPage = HttpContext.Request.CurrentPage();

                if (currentPage.StartsWith("/"))
                {
                    currentPage = currentPage[1..];
                }

                return currentPage;
            }
        }
    }
}