using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DigitalHealthCheckCommon.Mail;
using DigitalHealthCheckEF;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceComponents.Emails;

namespace DigitalHealthCheckService
{
    public abstract class PatientReminderEmail<TEmailComponent> : PatientEmail<TEmailComponent>
        where TEmailComponent : IComponent
    {
        protected PatientReminderEmail(ILogger logger, IConfiguration configuration, IMailNotificationEngine mailEngine, Database database) : base(logger, configuration, mailEngine, database)
        {
        }

        protected async System.Threading.Tasks.Task<IEnumerable<HealthCheck>> GetEligiblePatientsAsync(ReminderStatus currentReminderStatus, TimeSpan reminderGap)
        {
            var patientsWithReminders = await Database.HealthChecks
                .Include(c => c.BloodPressureFollowUp)
                .Include(c => c.BloodSugarFollowUp)
                .Include(c => c.CholesterolFollowUp)
                .Include(c => c.ImproveBloodPressureFollowUp)
                .Include(c => c.ImproveBloodSugarFollowUp)
                .Include(c => c.ImproveCholesterolFollowUp)
                .Include(c => c.BookGPAppointmentFollowUp)
                .Include(c => c.DrinkLessFollowUp)
                .Include(c => c.HealthyWeightFollowUp)
                .Include(c => c.HeightAndWeightFollowUp)
                .Include(c => c.MentalWellbeingFollowUp)
                .Include(c => c.MoveMoreFollowUp)
                .Include(c => c.QuitSmokingFollowUp)
                .Include(c => c.ChosenInterventions)
                .Where(x =>
                    (x.HeightAndWeightFollowUp != null && x.HeightAndWeightFollowUp.BeReminded == true) ||
                    (x.QRisk != null &&
                        (
                            (x.BloodPressureFollowUp != null && x.BloodPressureFollowUp.BeReminded == true) ||
                            (x.BloodSugarFollowUp != null && x.BloodSugarFollowUp.BeReminded == true) ||
                            (x.CholesterolFollowUp != null && x.CholesterolFollowUp.BeReminded == true) ||
                            (x.ImproveBloodPressureFollowUp != null && x.ImproveBloodPressureFollowUp.BeReminded == true) ||
                            (x.ImproveBloodSugarFollowUp != null && x.ImproveBloodSugarFollowUp.BeReminded == true) ||
                            (x.ImproveCholesterolFollowUp != null && x.ImproveCholesterolFollowUp.BeReminded == true) ||
                            (x.BookGPAppointmentFollowUp != null && x.BookGPAppointmentFollowUp.BeReminded == true) ||
                            (x.DrinkLessFollowUp != null && x.DrinkLessFollowUp.BeReminded == true) ||
                            (x.HealthyWeightFollowUp != null && x.HealthyWeightFollowUp.BeReminded == true) ||
                            (x.HeightAndWeightFollowUp != null && x.HeightAndWeightFollowUp.BeReminded == true) ||
                            (x.MentalWellbeingFollowUp != null && x.MentalWellbeingFollowUp.BeReminded == true) ||
                            (x.MoveMoreFollowUp != null && x.MoveMoreFollowUp.BeReminded == true) ||
                            (x.QuitSmokingFollowUp != null && x.QuitSmokingFollowUp.BeReminded == true)
                    )) &&
                    x.EmailAddress != null &&
                    x.ReminderStatus == currentReminderStatus)
                .ToListAsync();

            Logger.LogInformation($"{patientsWithReminders.Count} patients requested reminders");

            return patientsWithReminders
                .Where(x =>  x.LastContactDate + reminderGap <= DateTime.Now)
                .ToList();
        }

        protected static Collection<ReminderType> GetReminders(HealthCheck patient)
        {
            var reminders = new Collection<ReminderType>();

            static bool FollowUpNeedsReminder(FollowUp followup) => followup?.BeReminded == true;

            if (FollowUpNeedsReminder(patient.BloodPressureFollowUp))
            {
                reminders.Add(ReminderType.BloodPressure);
            }

            if (FollowUpNeedsReminder(patient.BloodSugarFollowUp))
            {
                reminders.Add(ReminderType.BloodSugar);
            }

            if (FollowUpNeedsReminder(patient.BookGPAppointmentFollowUp))
            {
                reminders.Add(ReminderType.VisitGP);
            }

            if (FollowUpNeedsReminder(patient.CholesterolFollowUp))
            {
                reminders.Add(ReminderType.Cholesterol);
            }

            if (FollowUpNeedsReminder(patient.DrinkLessFollowUp))
            {
                reminders.Add(ReminderType.Drinking);
            }

            if (FollowUpNeedsReminder(patient.HealthyWeightFollowUp))
            {
                reminders.Add(ReminderType.Weight);
            }

            if (FollowUpNeedsReminder(patient.HeightAndWeightFollowUp))
            {
                reminders.Add(ReminderType.HeightAndWeight);
            }

            if (FollowUpNeedsReminder(patient.ImproveBloodPressureFollowUp))
            {
                reminders.Add(ReminderType.ImproveBloodPressure);
            }

            if (FollowUpNeedsReminder(patient.ImproveBloodSugarFollowUp))
            {
                reminders.Add(ReminderType.ImproveBloodSugar);
            }

            if (FollowUpNeedsReminder(patient.ImproveCholesterolFollowUp))
            {
                reminders.Add(ReminderType.ImproveCholesterol);
            }

            if (FollowUpNeedsReminder(patient.MentalWellbeingFollowUp))
            {
                reminders.Add(ReminderType.Mental);
            }

            if (FollowUpNeedsReminder(patient.MoveMoreFollowUp))
            {
                reminders.Add(ReminderType.Move);
            }

            if (FollowUpNeedsReminder(patient.QuitSmokingFollowUp))
            {
                reminders.Add(ReminderType.Smoking);
            }

            return reminders;
        }

        protected void UpdateCheckAfterEmail(HealthCheck check, ReminderStatus updateReminderStatus)
        {
            check.ReminderStatus = updateReminderStatus;
            check.LastContactDate = DateTime.Now;
        }
    }
}
