using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class Incident
    {
        public int Id { get; set; }                  // Unique ID
        public string Title { get; set; } = "";      // Short Title of Incident
        public string Description { get; set; } = "";// Details of Incident
        public string Location { get; set; } = "";   // Where it happened
        public DateTime DateReported { get; set; } = DateTime.Now; // Auto-filled
    }

}
