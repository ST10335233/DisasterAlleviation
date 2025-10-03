using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class TaskRegistration
    {
        public int Id { get; set; }
        public string VolunteerName { get; set; }
        public string IncidentTitle { get; set; }
   
    }

}
