using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace na4shtab.PatientApp.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public DateTime? BirthDate { get; set; }
        public string ContactInfo { get; set; }

        public List<Visit> Visits { get; set; } = new();
    }
}