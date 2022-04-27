namespace DigitalHealthCheckEF
{
    public class FollowUp
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool? BeReminded { get; set; }
        public string ConfidentToChange { get; set; }
        public bool? SetADate { get; set; }
        public bool? DoYouHavePlans { get; set; }
        public string ImportantToChange { get; set; }
        public string SelectedOption { get; set; }
    }
}