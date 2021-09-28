using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPTreatmentOfferingMicroservices.Models
{
    public class IPTreatmentPackage
    {
        public enum AilmentCategory{
            Orthopaedics, Urology
        }
        public  AilmentCategory Ailment{ get; set; }
       public PackageDetail PackageDetail { get; set; }
    }
    
}
