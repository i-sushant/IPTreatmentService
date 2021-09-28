using IPTreatmentMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPTreatmentOfferingMicroservices.Models
{
    public class SpecialistDetail
    {
        public int SpecialistId { get; set; }
        public string Name { get; set; }
        public AilmentCategory AreaOfExpertise { get; set; }
        public int ExperienceInYears { get; set; }
        public long ContactNumber { get; set; }
    }
}
