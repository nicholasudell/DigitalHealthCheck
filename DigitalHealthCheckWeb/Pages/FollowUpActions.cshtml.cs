using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHealthCheckCommon;
using DigitalHealthCheckEF;
using DigitalHealthCheckWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalHealthCheckWeb.Pages
{
    public class FollowUpActionsModel : HealthCheckPageModel
    {
        public class UnsanitisedModel
        {
            public IEnumerable<string> Alcohol { get; set; }

            public IEnumerable<string> BloodPressure { get; set; }

            public IEnumerable<string> BloodSugar { get; set; }

            public IEnumerable<string> Cholesterol { get; set; }

            public IEnumerable<string> GP { get; set; }

            public IEnumerable<string> ImproveBloodPressure { get; set; }

            public IEnumerable<string> ImproveBloodSugar { get; set; }

            public IEnumerable<string> ImproveCholesterol { get; set; }

            public IEnumerable<string> Mental { get; set; }

            public IEnumerable<string> Move { get; set; }

            public IEnumerable<string> Smoking { get; set; }

            public IEnumerable<string> Weight { get; set; }
        }

        private class SanitisedModel
        {
            public IEnumerable<Intervention> Alcohol { get; set; }

            public IEnumerable<Intervention> BloodPressure { get; set; }

            public IEnumerable<Intervention> BloodSugar { get; set; }

            public IEnumerable<Intervention> Cholesterol { get; set; }

            public IEnumerable<Intervention> GP { get; set; }

            public IEnumerable<Intervention> ImproveBloodPressure { get; set; }

            public IEnumerable<Intervention> ImproveBloodSugar { get; set; }

            public IEnumerable<Intervention> ImproveCholesterol { get; set; }

            public IEnumerable<Intervention> Mental { get; set; }

            public IEnumerable<Intervention> Move { get; set; }

            public IEnumerable<Intervention> Smoking { get; set; }

            public IEnumerable<Intervention> Weight { get; set; }
        }

        private readonly IBodyMassIndexCalculator bodyMassIndexCalculator;
        private readonly IHealthCheckResultFactory healthCheckResultFactory;
        private readonly IEveryoneHealthReferralService everyoneHealthReferralService;

        public IList<Intervention> AllInterventions { get; set; }

        public IList<CustomBarrier> CustomBarriers { get; set; }

        public IList<int> SelectedInterventions { get; set; }

        public FollowUpActionsModel(
            Database database,
            ICredentialsDecrypter credentialsDecrypter,
            IBodyMassIndexCalculator bodyMassIndexCalculator,
            IPageFlow pageFlow,
            IHealthCheckResultFactory healthCheckResultFactory,
            IEveryoneHealthReferralService everyoneHealthReferralService)
            : base(database, credentialsDecrypter, pageFlow)
        {
            this.bodyMassIndexCalculator = bodyMassIndexCalculator;
            this.healthCheckResultFactory = healthCheckResultFactory;
            this.everyoneHealthReferralService = everyoneHealthReferralService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsValidated())
            {
                return RedirectToValidation();
            }

            await LoadData();

            if (!AllInterventions.Any())
            {
                var check = await GetHealthCheckAsync();

                var result = healthCheckResultFactory.GetResult(check, false);

                if (result.Alcohol == DefaultStatus.Healthy &&
                    (result.BloodPressure is null || result.BloodPressure == BloodPressureStatus.Healthy) &&
                    (result.BloodSugar is null || result.BloodSugar == BloodSugarStatus.Healthy) &&
                    result.BodyMassIndex == BodyMassIndexStatus.Healthy &&
                    (result.Cholesterol is null || result.Cholesterol == DefaultStatus.Healthy) &&
                    result.Diabetes == DefaultStatus.Healthy &&
                    result.HeartAge == DefaultStatus.Healthy &&
                    result.HeartDisease == DefaultStatus.Healthy &&
                    result.PhysicalActivity == PhysicalActivityStatus.Active &&
                    result.Smoker == DefaultStatus.Healthy)
                {
                    //No interventions necessary, go straight to complete.

                    return RedirectWithId("./HealthCheckComplete");
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(UnsanitisedModel actions)
        {
            var sanitisedActions = await ValidateAndSanitiseAsync(actions);

            if (sanitisedActions == null)
            {
                await LoadData();

                return await Reload();
            }

            var check = await GetHealthCheckAsync();

            check.ChosenInterventions = (sanitisedActions.Alcohol ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.Move ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.Smoking ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.Weight ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.GP ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.Mental ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.BloodPressure ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.BloodSugar ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.Cholesterol ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.ImproveBloodPressure ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.ImproveBloodSugar ?? Enumerable.Empty<Intervention>())
                .Concat(sanitisedActions.ImproveCholesterol ?? Enumerable.Empty<Intervention>())
                .ToList();

            await Database.SaveChangesAsync();

            if(everyoneHealthReferralService.HasEveryoneHealthReferrals(check))
            {
                return RedirectWithId("./EveryoneHealthConsent");
            }

            return RedirectWithId("./HealthCheckComplete");
        }

        protected override async Task<HealthCheck> GetHealthCheckAsync()
        {
            var check = await base.GetHealthCheckAsync();

            await Database.Entry(check).Collection(c => c.ChosenInterventions).LoadAsync();

            return check;
        }

        private async Task LoadData()
        {
            var check = await GetHealthCheckAsync();

            await Database.Entry(check).Collection(c => c.ChosenBarriers).LoadAsync();

            await Database.Entry(check).Collection(c => c.CustomBarriers).LoadAsync();

            CustomBarriers = check.CustomBarriers.ToList();

            var barriers = check.ChosenBarriers;

            if (!barriers.Any())
            {
                // If no barriers have been selected and the patient has any health risks We show
                // the interventions for those risks

                var result = healthCheckResultFactory.GetResult(check, false);

                void LoadBarriers(string category)
                {
                    foreach (var barrier in Database.Barriers.Where(x => x.Category == category))
                    {
                        barriers.Add(barrier);
                    }
                }

                if (result.Alcohol != DefaultStatus.Healthy)
                {
                    LoadBarriers("alcohol");
                }

                if (result.BloodPressure is null)
                {
                    LoadBarriers("bloodpressure");
                }
                else if (result.BloodPressure is not null && result.BloodPressure != BloodPressureStatus.Healthy)
                {
                    LoadBarriers("improvebloodpressure");
                }

                if (result.BloodSugar is null)
                {
                    LoadBarriers("bloodsugar");
                }
                else if (result.BloodSugar is not null && result.BloodSugar != BloodSugarStatus.Healthy)
                {
                    LoadBarriers("improvebloodsugar");
                }

                if (result.BodyMassIndex != BodyMassIndexStatus.Healthy)
                {
                    LoadBarriers("weight");
                }

                if (result.Cholesterol is null)
                {
                    LoadBarriers("cholesterol");
                }
                else if (result.Cholesterol is not null && result.Cholesterol != DefaultStatus.Healthy)
                {
                    LoadBarriers("improvecholesterol");
                }

                if (result.PhysicalActivity != PhysicalActivityStatus.Active)
                {
                    LoadBarriers("move");
                }

                if (result.Smoker != DefaultStatus.Healthy)
                {
                    LoadBarriers("smoking");
                }
            }

            foreach (var barrier in barriers)
            {
                await Database.Entry(barrier).Collection(c => c.Interventions).LoadAsync();
            }

            //Some interventions appear regardless of barriers, but we only want to
            //show interventions for the categories of barriers people have chosen.

            var additionalInterventions = Database.Interventions.Where(x => x.Barrier == null && barriers.Select(y => y.Category).Contains(x.Category));

            //Some barrierless interventions have extra requirements, so we filter on those.

            var bmi = bodyMassIndexCalculator.CalculateBodyMassIndex(check.Height.Value, check.Weight.Value);

            if (bmi >= 40)
            {
                additionalInterventions = additionalInterventions.Where(x => x.Id != Database.BmiOver30InterventionId);
            }
            else if (bmi > 30)
            {
                additionalInterventions = additionalInterventions.Where(x => x.Id != Database.BmiOver40InterventionId);
            }
            else
            {
                additionalInterventions = additionalInterventions.Where(x => x.Id != Database.BmiOver30InterventionId && x.Id != Database.BmiOver40InterventionId);
            }

            if (check.BloodSugar < 42 || check.BloodSugar > 47)
            {
                additionalInterventions = additionalInterventions.Where(x => x.Id != Database.NationalDiabetesPreventionProgramInterventionId);
            }

            AllInterventions = barriers
                .SelectMany(x => x.Interventions)
                .Concat(additionalInterventions)
                .ToList();

            SelectedInterventions = check.ChosenInterventions.Select(x => x.Id).ToList();
        }

        async Task<IList<Intervention>> LoadInterventionsAsync(IEnumerable<string> unsanitisedIds)
        {
            if (unsanitisedIds is null)
            {
                return null;
            }

            // There is an exclusive "none" option on the interventions checkboxes that's there for
            // if you don't want to pick any items. This is clearer for some users than simply
            // leaving the control blank.

            if (unsanitisedIds.Count() == 1 && unsanitisedIds.Single() == "none")
            {
                return new List<Intervention>();
            }

            var results = new List<Intervention>();

            var sanitisedIds = unsanitisedIds.Select(x => int.Parse(x));

            foreach (var unsanitisedId in unsanitisedIds)
            {
                if (int.TryParse(unsanitisedId, out var sanitisedId))
                {
                    results.Add(await Database.Interventions.FindAsync(sanitisedId));
                }
                else
                {
                    return null;
                }
            }

            return results;
        }

        async Task<SanitisedModel> ValidateAndSanitiseAsync(UnsanitisedModel actions)
        {
            var alcohol = await LoadInterventionsAsync(actions.Alcohol);

            var smoking = await LoadInterventionsAsync(actions.Smoking);

            var weight = await LoadInterventionsAsync(actions.Weight);

            var move = await LoadInterventionsAsync(actions.Move);

            var mental = await LoadInterventionsAsync(actions.Mental);

            var gp = await LoadInterventionsAsync(actions.GP);

            var bloodPressure = await LoadInterventionsAsync(actions.BloodPressure);

            var bloodSugar = await LoadInterventionsAsync(actions.BloodSugar);

            var cholesterol = await LoadInterventionsAsync(actions.Cholesterol);

            var improveBloodPressure = await LoadInterventionsAsync(actions.ImproveBloodPressure);

            var improveBloodSugar = await LoadInterventionsAsync(actions.ImproveBloodSugar);

            var improveCholesterol = await LoadInterventionsAsync(actions.ImproveCholesterol);

            return new SanitisedModel
            {
                Alcohol = alcohol,
                Smoking = smoking,
                Weight = weight,
                Move = move,
                GP = gp,
                Mental = mental,
                BloodPressure = bloodPressure,
                BloodSugar = bloodSugar,
                Cholesterol = cholesterol,
                ImproveBloodPressure = improveBloodPressure,
                ImproveBloodSugar = improveBloodSugar,
                ImproveCholesterol = improveCholesterol
            };
        }
    }
}