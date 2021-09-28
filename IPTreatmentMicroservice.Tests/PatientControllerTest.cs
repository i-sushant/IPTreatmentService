using FluentAssertions;
using IPTreatmentMicroservice.Controllers;
using IPTreatmentMicroservice.Models;
using IPTreatmentMicroservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;

namespace IPTreatmentMicroservice.Tests
{
    public class Tests
    {
        private  Mock<ITreatmentPlan> _repositoryStub;
        private Mock<IConfiguration> _configuration;
        [SetUp]
        public void Setup()
        {
            _repositoryStub = new Mock<ITreatmentPlan>();
        }

        [Test]
        public void GetAllDetails_WhenPatientsExists_ReturnsListOfPatients()
        {
           var expectedItems =  new List<PatientDetail>()
            {
                new PatientDetail { Id = 1, Name = "Anu", Age = 45, Ailment = AilmentCategory.Orthopaedics, TreatmentPackageName = "Package 1", TreatmentCommencementDate = DateTime.Now },
                new PatientDetail { Id = 2, Name = "Ram", Age = 50, Ailment = AilmentCategory.Orthopaedics, TreatmentPackageName = "Package 2", TreatmentCommencementDate = Convert.ToDateTime("12/10/2021") },
                new PatientDetail { Id = 3, Name = "Ravi", Age = 37, Ailment = AilmentCategory.Urology, TreatmentPackageName = "Package 1", TreatmentCommencementDate = Convert.ToDateTime("10/10/2021") },
                new PatientDetail { Id = 4, Name = "Raghu", Age = 37, Ailment = AilmentCategory.Urology, TreatmentPackageName = "Package 2", TreatmentCommencementDate = Convert.ToDateTime("02/11/2021") },
            };

            _repositoryStub.Setup(repo => repo.GetAll()).Returns(expectedItems);

            var controller = new PatientController(_repositoryStub.Object, _configuration.Object);
            var response = controller.GetAllDetails();
            var result = response as OkObjectResult;
            result.Value.Should().BeEquivalentTo(expectedItems, options => options.ComparingByMembers<PatientDetail>());
        }

        [Test]
        public void GetAllDetails_WhenPatientsDoesNotExists_ReturnsNotFound()
        {
            _repositoryStub.Setup(repo => repo.GetAll()).Returns((IEnumerable<PatientDetail>)null);
            var controller = new PatientController(_repositoryStub.Object,  _configuration.Object);
            var response = controller.GetAllDetails();
            response.Should().BeOfType<NotFoundResult>();
            (response as NotFoundResult).StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            

        }

        [Test]
        public void GetPatientById_WhenPatientExists_ReturnsPatientDetails()
        {
            var expectedItem = new PatientDetail { Id = 1, Name = "Anu", Age = 45, Ailment = AilmentCategory.Orthopaedics, TreatmentPackageName = "Package 1", TreatmentCommencementDate = DateTime.Now };
            _repositoryStub.Setup(repo => repo.GetPatient(expectedItem.Id)).Returns(expectedItem);
            var controller = new PatientController(_repositoryStub.Object, _configuration.Object);
            var response = controller.GetPatientById(1);
            var result = response as OkObjectResult;
            result.Value.Should().BeEquivalentTo(expectedItem, options => options.ComparingByMembers<PatientDetail>());

        }

        [Test]
        public void GetPatientById_WhenPatientDoesNotExists_ReturnsNotFound()
        {
            _repositoryStub.Setup(repo => repo.GetPatient(2)).Returns((PatientDetail)null);
            var controller = new PatientController(_repositoryStub.Object, _configuration.Object);
            var response = controller.GetPatientById(2);
            response.Should().BeOfType<NotFoundResult>();
            (response as NotFoundResult).StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public void MarkTreatmentComplete_WhenPatientExists_ReturnsOk()
        {
            var expectedItem = new PatientDetail { Id = 1, Name = "Anu", Age = 45, Ailment = AilmentCategory.Orthopaedics, TreatmentPackageName = "Package 1", TreatmentCommencementDate = DateTime.Now };
            _repositoryStub.Setup(repo => repo.GetPatient(expectedItem.Id)).Returns(expectedItem);
            var controller = new PatientController(_repositoryStub.Object, _configuration.Object);
            var response = controller.MarkTreatmentComplete(1);
            response.Should().BeOfType<OkResult>();
            (response as OkResult).StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
        [Test]
        public void MarkTreatmentComplete_WhenPatientDoesNotExists_ReturnsBadRequest()
        {
            var expectedItem = new PatientDetail { Id = 1, Name = "Anu", Age = 45, Ailment = AilmentCategory.Orthopaedics, TreatmentPackageName = "Package 1", TreatmentCommencementDate = DateTime.Now };
            _repositoryStub.Setup(repo => repo.GetPatient(2)).Returns((PatientDetail)null);
            var controller = new PatientController(_repositoryStub.Object, _configuration.Object);
            var response = controller.MarkTreatmentComplete(2);
            response.Should().BeOfType<BadRequestResult>();
            (response as BadRequestResult).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
        [Test]
        public void GetTimeTableByPatientId_WhenPatientExists_ReturnsTreatmentPlan()
        {
            
            var patient = new PatientDetail { Id = 1, Name = "Anu", Age = 45, Ailment = AilmentCategory.Orthopaedics, TreatmentPackageName = "Package 1", TreatmentCommencementDate = DateTime.Now };
            var timeTable = new TreatmentPlan { Cost = 2000, PackageName = "Package 1", PatientId = 1, SpecialistId = 1, TestDetails = "OPD1,UPD1", TreatmentCommencementDate = DateTime.Today, TreatmentEndDate = DateTime.Today.AddDays(10) };
            _repositoryStub.Setup(repo => repo.GetTimeTable(patient.Id)).Returns(timeTable);
            var controller = new PatientController(_repositoryStub.Object, _configuration.Object);
            var response = controller.GetTimeTableByPatientId(1);
            var result = response as OkObjectResult;
            result.Value.Should().BeEquivalentTo(timeTable, options => options.ComparingByMembers<TreatmentPlan>());
        }
        [Test]
        public void GetTimeTableByPatientId_WhenPatientDoesNotExists_ReturnsNotFound()
        {
            _repositoryStub.Setup(repo => repo.GetTimeTable(1)).Returns((TreatmentPlan)null);
            var controller = new PatientController(_repositoryStub.Object, _configuration.Object);
            var response = controller.GetTimeTableByPatientId(1);
            response.Should().BeOfType<NotFoundResult>();
            (response as NotFoundResult).StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        
    }
}