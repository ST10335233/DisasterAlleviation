namespace DisasterAlleviationFoundation.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsRegistered { get; set; } = true;
    }
}

 