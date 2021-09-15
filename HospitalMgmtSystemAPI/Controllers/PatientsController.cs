using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.Services.Services;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HospitalMgmtSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IUserService _userService;

        public PatientsController(IPatientService patientService, IUserService userService)
        {
            this._patientService = patientService;
            this._userService = userService;
        }

        // GET: api/<PatientsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var patients = await _patientService.GetAllPatients();

            if (patients == null)
            {
                return BadRequest(new
                {
                    status = "fail",
                    results = 0
                });
            }

            return Ok(new
            {
                status = "success",
                results = patients.Count(),
                data = patients.ToList()
            });
        }

        // GET api/<PatientsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _patientService.GetPatientByID(id);

            if (patient == null)
            {
                return BadRequest(new
                {
                    status = "fail",
                    results = 0
                });
            }

            return Ok(new
            {
                status = "success",
                data = new
                {
                    patient.ID,
                    Name = patient.Users.FirstName + " " + patient.Users.LastName,
                    patient.Users.Email,
                    patient.Users.Number,
                    patient.Users.Address,
                    patient.BloodGroup,
                    patient.MedicalHistory,
                    patient.AdditionalInfo
                }
            });
        }

        // POST api/<PatientsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserPatientRegisterModel receivedPatient)
        {
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = receivedPatient.FirstName,
                LastName = receivedPatient.LastName,
                Number = receivedPatient.Number,
                Address = receivedPatient.Address,
                Email = receivedPatient.Email,
                UserName = receivedPatient.Email,
                IsActive = true,
                EmailConfirmed = true,
                Role = Role.Patient
            };

            Patient patient = new Patient()
            {
                MedicalHistory = receivedPatient.MedicalHistory,
                BloodGroup = receivedPatient.BloodGroup,
                AdditionalInfo = receivedPatient.AdditionalInfo,
                IsActive = true
            };

            bool isAdded = await _userService.CreateUser(user, "Patient");

            if (isAdded)
            {
                patient.UserId = user.Id;
                await _patientService.CreatePatient(patient);

                return NoContent();
            }

            return BadRequest();
        }

        // PUT api/<PatientsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [Bind("Number,MedicalHistory,BloodGroup,AdditionalInfo")] UserPatientViewModel receivedPatient)
        {
            var patient = await _patientService.GetPatientByID(id);

            if (patient == null)
            {
                return BadRequest();
            }

            patient.Users.Number = receivedPatient.Number ?? patient.Users.Number;

            patient.MedicalHistory = receivedPatient.MedicalHistory ?? patient.MedicalHistory;

            patient.BloodGroup = receivedPatient.BloodGroup == patient.BloodGroup
                                    ? patient.BloodGroup
                                    : receivedPatient.BloodGroup;

            patient.AdditionalInfo = receivedPatient.AdditionalInfo ?? patient.AdditionalInfo;

            await _patientService.UpdatePatient(patient);

            return NoContent();

        }

        // DELETE api/<PatientsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _patientService.DeletePatient(id));
        }

        [HttpGet("search/{key}")]
        public async Task<IActionResult> Search(string? key)
        {
            if (key == null)
            {
                return BadRequest(new { status = "fail" });
            }

            var filteredResult = await _patientService.Search(key);


            if (filteredResult == null)
            {
                return BadRequest(new { status = "fail" });
            }

            return Ok(new
            {
                status = "success",
                results = filteredResult.Count(),
                data = filteredResult
            });
        }
    }
}
