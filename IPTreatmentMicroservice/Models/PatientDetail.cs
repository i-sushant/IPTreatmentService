using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IPTreatmentMicroservice.Models
{
    
    public class PatientDetail
    {

        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public int Age { get; set; }
        
        public AilmentCategory Ailment { get; set; }
        public string TreatmentPackageName { get; set; }
        public DateTime TreatmentCommencementDate { get; set; }
        public bool TreatmentCompletionStatus { get; set; } = false;

    }
}
