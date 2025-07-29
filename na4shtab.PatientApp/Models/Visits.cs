using System;
using System.Collections.Generic;

namespace na4shtab.PatientApp.Models
{
    public class Visit
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public DateTime Date { get; set; }
        
        public List<Procedure> SelectedProcedures { get; set; } = new();


        public decimal TotalCost { get; set; }
    }
}