namespace DisasterAlleviationFoundation.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public string DonorName { get; set; } = "";
        public string Item { get; set; } = "";
        public int Quantity { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
