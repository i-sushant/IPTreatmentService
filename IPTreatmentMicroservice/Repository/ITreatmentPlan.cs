using IPTreatmentMicroservice.Models;
using IPTreatmentOfferingMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPTreatmentMicroservice.Repository
{
    public interface ITreatmentPlan
    {
        int Register(PatientDetail obj);
        //List<PatientDetail> GetPatient();
        IEnumerable<PatientDetail> GetAll();
        PatientDetail GetPatient(int id);
        TreatmentPlan GetTimeTable(int patientId);
        TreatmentPlan AddTimeTable(IPTreatmentPackage package, int specialistId,PatientDetail patient);
        void MarkTreatmentComplete(int patientId);
    }
}
