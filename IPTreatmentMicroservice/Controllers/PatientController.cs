using IPTreatmentMicroservice.Models;
using IPTreatmentMicroservice.Repository;
using IPTreatmentOfferingMicroservices.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IPTreatmentMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly ITreatmentPlan _repository;
        private readonly IConfiguration _configuration;

        public PatientController(ITreatmentPlan repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetAllDetails()
        {
            var records = _repository.GetAll();
            if(records==null)
                return NotFound();
            return Ok(records);
        }
        
        [HttpPatch]
        [Route("MarkTreatmentComplete/{id}")]
        public IActionResult MarkTreatmentComplete(int id)
        {
            if(_repository.GetPatient(id) != null)
            {
                _repository.MarkTreatmentComplete(id);
                return Ok();
            }
            return BadRequest();
                
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPatientById([FromRoute] int id)
        {
            var pat = _repository.GetPatient(id);
            if (pat == null)
            {
                return NotFound();
            }
            return Ok(pat);
        }
        [HttpGet]
        [Route("GetTimeTableByPatientId/{id}")]
        public IActionResult GetTimeTableByPatientId([FromRoute] int id)
        {
            var pat = _repository.GetTimeTable(id);
            if (pat == null)
            {
                return NotFound();
            }
            return Ok(pat);
        }


        [HttpPost]
        [Route("FormulateTreatmentTimeTable")]
        public async Task<IActionResult> FormulateTreatmentTimeTable(PatientDetail patientObj)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            IPTreatmentPackage package = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"{_configuration["IPTreatmentOfferingBaseUri"]}/IPTreatmentPackages/IPTreatmentPackageByName?ailment={patientObj.Ailment}&treatmentPackageName={patientObj.TreatmentPackageName}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    package = JsonConvert.DeserializeObject<IPTreatmentPackage>(result);
                } else
                {
                    return BadRequest();
                }
            }
            SpecialistDetail specialist = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"{_configuration["IPTreatmentOfferingBaseUri"]}/IPTreatmentPackages/SpecialistDetail");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var specialists = JsonConvert.DeserializeObject<List<SpecialistDetail>>(result);
                    specialist = specialists.Find(currentSpecialist => currentSpecialist.AreaOfExpertise == patientObj.Ailment);
                }
                else
                {
                    return BadRequest();
                }
            }
            if (!ValidatePatient(patientObj))
            {
                return BadRequest();
            }
            int patient = _repository.Register(patientObj);

            var treatmentTimeTable = _repository.AddTimeTable(package, specialist.SpecialistId, patientObj);
            return Ok(treatmentTimeTable);
        }

        private bool ValidatePatient(PatientDetail patientDetail)
        {
            bool ailmentTest = patientDetail.Ailment == AilmentCategory.Orthopaedics || patientDetail.Ailment == AilmentCategory.Urology;
            bool commencementDateTest = patientDetail.TreatmentCommencementDate >= DateTime.Today;
            return ailmentTest && commencementDateTest;
        }
    }
}
