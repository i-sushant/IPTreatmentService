using IPTreatmentMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using IPTreatmentOfferingMicroservices.Models;

namespace IPTreatmentMicroservice.Repository
{
    public class TreatmentPlanRepository : ITreatmentPlan
    {
        private List<PatientDetail> _patlist;
        private List<TreatmentPlan> _treatmentPlans;

        public TreatmentPlanRepository()
        {
            //_patlist = new List<PatientDetail>()
            //{
            //    new PatientDetail { Id = 1, Name = "Anu", Age = 45, Ailment = AilmentCategory.Orthopaedics, TreatmentPackageName = "Package 1", TreatmentCommencementDate = DateTime.Now },
            //    new PatientDetail { Id = 2, Name = "Ram", Age = 50, Ailment = AilmentCategory.Orthopaedics, TreatmentPackageName = "Package 2", TreatmentCommencementDate = Convert.ToDateTime("12/10/2021") },
            //    new PatientDetail { Id = 3, Name = "Ravi", Age = 37, Ailment = AilmentCategory.Urology, TreatmentPackageName = "Package 1", TreatmentCommencementDate = Convert.ToDateTime("10/10/2021") },
            //    new PatientDetail { Id = 4, Name = "Raghu", Age = 37, Ailment = AilmentCategory.Urology, TreatmentPackageName = "Package 2", TreatmentCommencementDate = Convert.ToDateTime("02/11/2021") },
            //};
            _patlist = new List<PatientDetail>();
            _treatmentPlans = new List<TreatmentPlan>();
        }
        public int Register(PatientDetail obj)
        {
            obj.Id = _patlist.Count() + 1;
            _patlist.Add(obj);
            return obj.Id;
           
        }
        public PatientDetail GetPatient(int id)
        {
            return _patlist.Find(p => p.Id == id);
        }

        public IEnumerable<PatientDetail> GetAll()
        {
            return _patlist;
        }
        public void AddTimeTable(TreatmentPlan treatmentPlan)
        {
            _treatmentPlans.Add(treatmentPlan);
        }

        public TreatmentPlan GetTimeTable(int patientId)
        {
            return _treatmentPlans.Find(plan => plan.PatientId == patientId);
        }

        public void MarkTreatmentComplete(int patientId)
        {
            _patlist.Find(patient => patient.Id == patientId).TreatmentCompletionStatus = true;
        }

        public TreatmentPlan AddTimeTable(IPTreatmentPackage package, int specialistId, PatientDetail patient)
        {
            var treatmentTimeTable = new TreatmentPlan()
            {
                Cost = package.PackageDetail.Cost,
                PackageName = package.PackageDetail.TreatmentPackageName,
                PatientId = patient.Id,
                SpecialistId = specialistId,
                TestDetails = package.PackageDetail.TestDetails,
                TreatmentCommencementDate = patient.TreatmentCommencementDate,
                TreatmentEndDate = patient.TreatmentCommencementDate.AddDays(package.PackageDetail.TreatmentDuration * 7)
            };
            _treatmentPlans.Add(treatmentTimeTable);
            return treatmentTimeTable;
        }

        
    }
}
