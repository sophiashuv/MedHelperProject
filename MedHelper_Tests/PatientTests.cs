using System;
using Xunit;
using MedHelper_API.Controllers;
using MedHelper_API.Repository;
using MedHelper_API.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MedHelper_EF.Models;
using Moq;
using System.Threading.Tasks;
using MedHelper_API.Responses;
using MyApi.Controllers;
using Moq.Protected;
using System.Security.Claims;
using System.Collections.Generic;
using MedHelper_API.Dto.Patient;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using MedHelper_API.Service;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace MedHelper_Tests
{
    public class PatientTests
    {
        private readonly Mock<IPatientService> _patientService = new();
        private readonly Mock<ILogger<PatientController>> _logger = new();
        private readonly Mock<MedHelperDB> _context = new();

        private ClaimsPrincipal Doctor = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "example name"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim("DoctorID", "1"),
        }, "mock"));

        private ClaimsPrincipal fakeDoctor = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "example name"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }, "mock"));

        private PatientResponse TestPatient1 = new PatientResponse()
        {
            UserName = "Username",
            Gender = "female",
            Birthdate = new DateTime(2000, 7, 20),
            PatientID = 100,
            DoctorID = 1
        };

        private PatientResponse TestPatient2 = new PatientResponse()
        {
            UserName = "Testname",
            Gender = "male",
            Birthdate = new DateTime(1998, 7, 12),
            PatientID = 101,
            DoctorID = 1
        };

        [Fact]
        public Task GetPatient_ReturnsUnauthorized()
        {

            //_patientService.Setup(serv => serv.GetOne(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((PatientResponse)null);

            var controller = new PatientController(_patientService.Object, _context.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = fakeDoctor }
            };

            var result = controller.GetOne(1200);
            Assert.IsType<UnauthorizedObjectResult>(result.Result);
            return Task.CompletedTask;
        }

        [Fact]
        public Task GetPatients_ReturnsUnauthorized()
        {
            List<PatientResponse> patientList = new List<PatientResponse>() { };
            patientList.Add(TestPatient1);
            patientList.Add(TestPatient2);
            //var response = _patientService.Setup(serv => serv.GetAll(It.IsAny<int>())).ReturnsAsync(patientList);

            var controller = new PatientController(_patientService.Object, _context.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = fakeDoctor }
            };

            var result = controller.GetAllPatients();
            Assert.IsType<UnauthorizedObjectResult>(result.Result);
            return Task.CompletedTask;
        }

        [Fact]
        public async Task CreatePatient_ReturnsUnauthorized()
        {
            CreatePatientDto createPatient = new CreatePatientDto()
            {
                UserName = "CreateUser",
                Gender = "male",
                Birthdate = new DateTime(2002, 6, 1)
            };
            PatientResponse patient = new PatientResponse()
            {
                PatientID = 102,
                UserName = "CreateUser",
                Gender = "male",
                Birthdate = new DateTime(2002, 6, 1)
            };

            _patientService.Setup(serv => serv.Create(createPatient, It.IsAny<int>())).ReturnsAsync((PatientResponse)patient);
            var controller = new PatientController(_patientService.Object, _context.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = fakeDoctor }
            };

            var result = await controller.Create(createPatient);
            Assert.IsType<UnauthorizedObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreatePatient_ReturnsSuccess()
        {
            CreatePatientDto createPatient = new CreatePatientDto()
            {
                UserName = "CreateUser",
                Gender = "male",
                Birthdate = new DateTime(2002, 6, 1)
            };
            PatientResponse patient = new PatientResponse()
            {
                PatientID = 102,
                UserName = "CreateUser",
                Gender = "male",
                Birthdate = new DateTime(2002, 6, 1)
            };

            _patientService.Setup(serv => serv.Create(createPatient, It.IsAny<int>())).ReturnsAsync((PatientResponse)patient);
            var controller = new PatientController(_patientService.Object, _context.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = Doctor }
            };

            var result = await controller.Create(createPatient);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task DeletePatient_ReturnsUnauthorized()
        {
            var expectedProduct = TestPatient1;
            _patientService.Setup(serv => serv.Delete(It.IsAny<int>(), It.IsAny<int>()));
            var controller = new PatientController(_patientService.Object, _context.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = fakeDoctor }
            };

            var result = await controller.Delete(expectedProduct.PatientID);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task DeletePatient_ReturnsNoContent()
        {
            var expectedProduct = TestPatient1;
            _patientService.Setup(serv => serv.Delete(It.IsAny<int>(), It.IsAny<int>()));
            var controller = new PatientController(_patientService.Object, _context.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = Doctor }
            };

            var result = await controller.Delete(expectedProduct.PatientID);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePation_ReturnsUnauthorized()
        {
            UpdatePatientDto createPatient = new UpdatePatientDto()
            {
                UserName = "UpdateUser",
                Gender = "male",
                Birthdate = new DateTime(2002, 6, 1)
            };
            TestPatient1.UserName = createPatient.UserName;
            TestPatient1.Birthdate = createPatient.Birthdate;
            TestPatient1.Gender = createPatient.Gender;

            _patientService.Setup(serv => serv.Update(TestPatient1.PatientID, createPatient, It.IsAny<int>()));
            var controller = new PatientController(_patientService.Object, _context.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = fakeDoctor }
            };

            var result = await controller.Update(TestPatient1.PatientID, createPatient);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }


        [Fact]
        public async Task UpdatePation_ReturnsSuccess()
        {
            UpdatePatientDto createPatient = new UpdatePatientDto()
            {
                UserName = "UpdateUser",
                Gender = "male",
                Birthdate = new DateTime(2002, 6, 1)
            };
            TestPatient1.UserName = createPatient.UserName;
            TestPatient1.Birthdate = createPatient.Birthdate;
            TestPatient1.Gender = createPatient.Gender;

            _patientService.Setup(serv => serv.Update(TestPatient1.PatientID, createPatient, It.IsAny<int>()));
            var controller = new PatientController(_patientService.Object, _context.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = Doctor }
            };

            var result = await controller.Update(TestPatient1.PatientID, createPatient);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
