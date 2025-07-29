using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace na4shtab.PatientApp.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        public List<Visit> Visits { get; set; } = new();
    }
}