using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPTreatmentMicroservice.Models
{
    public class TreatmentPlan
    {
        public int PatientId { get; set; }
        public string PackageName { get; set; }
        public String TestDetails { get; set; }
        public int Cost { get; set; }
        public int SpecialistId { get; set; }
        public DateTime TreatmentCommencementDate { get; set; }
        public DateTime TreatmentEndDate { get; set; }

    }
}
