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
using MedHelper_API.Dto.Doctor;
using Microsoft.AspNetCore.Http;

namespace MedHelper_Tests
{
    public class DoctorTests
    {
        private readonly Mock<IDoctorService> _doctorService = new();

        private Doctor doctor = new Doctor()
        {
            DoctorID = 100,
            FirstName = "TestDoctor",
            LastName = "TestDoctor",
            Email = "testdoctor@gmail.com",
            Pass = "password"
        };

        [Fact]
        public async Task UpdatePatient_ReturnsUnauthorized()
        {
            UpdateDoctorDto update = new UpdateDoctorDto()
            {
                FirstName = "Test",
                LastName = "Doctor",
                Email = "testdoctor@gmail.com",
                Pass = "pass"
            };

            ClaimsPrincipal fakeDoctor = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));

            _doctorService.Setup(serv => serv.Update(update, It.IsAny<int>()));
            var controller = new DoctorController(_doctorService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = fakeDoctor }
            };

            var result = await controller.Update(update);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task UpdatePatient_ReturnsSuccess()
        {
            UpdateDoctorDto update = new UpdateDoctorDto()
            {
                FirstName = "Test",
                LastName = "Doctor",
                Email = "testdoctor@gmail.com",
                Pass = "pass"
            };

            ClaimsPrincipal Doctor = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("DoctorID", "1"),
            }, "mock"));

            _doctorService.Setup(serv => serv.Update(update, It.IsAny<int>()));
            var controller = new DoctorController(_doctorService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = Doctor }
            };

            var result = await controller.Update(update);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
