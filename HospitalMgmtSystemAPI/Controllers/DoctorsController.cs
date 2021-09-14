using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.DAL.Repository;
using HospitalMgmtSystem.Services.Services;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HospitalMgmtSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IUserService _userService;

        public DoctorsController(IDoctorService doctorService, IUserService userService)
        {
            this._doctorService = doctorService;
            this._userService = userService;
        }

        // GET: api/<DoctorsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var doctors = await _doctorService.GetAllDoctors();
            if (doctors == null)
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
                results = doctors.Count(),
                data = doctors.ToList()
            });
        }

        // GET: api/<DoctorsController>/Specialization/{specialization}
        [HttpGet("Specialization/{specialization}")]
        public async Task<IActionResult> Specialization(int specialization)
        {
            var res = GenericRepository<Doctor>.
                    Inst
                    .Set
                    .Include(doc => doc.Users)
                    .Where(doc => doc.Specialization == (Specialization)specialization && doc.IsActive != false);

            if (res == null)
            {
                return BadRequest(new
                {
                    status = "fail",
                    results = 0
                });
            }

            var doctors = res.Select(doc => new UserDoctorViewModel
            {
                ID = doc.ID,
                Name = doc.Users.FirstName + " " + doc.Users.LastName,
                Email = doc.Users.Email,
                Number = doc.Users.Number,
                Address = doc.Users.Address,
                Specialization = doc.Specialization,
                YearsOfExperience = doc.YearsOfExperience,
                AdditionalInformation = doc.AdditionalInfo
            });

            return Ok(new
            {
                status = "success",
                results = doctors.Count(),
                data = doctors.ToList()
            });
        }

        // GET api/<DoctorsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var doctor = await _doctorService.GetDoctorByID(id);

            if (doctor == null)
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
                    doctor.ID,
                    Name = doctor.Users.FirstName + " " + doctor.Users.LastName,
                    doctor.Users.Email,
                    doctor.Users.Number,
                    doctor.Specialization,
                    doctor.YearsOfExperience,
                    doctor.AdditionalInfo,
                }
            });
        }

        // POST api/<DoctorsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDoctorRegisterModel receivedDoctor)
        {
            receivedDoctor.ID = 0;

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = receivedDoctor.FirstName,
                LastName = receivedDoctor.LastName,
                Number = receivedDoctor.Number,
                Address = receivedDoctor.Address,
                Email = receivedDoctor.Email,
                UserName = receivedDoctor.Email,
                IsActive = true,
                EmailConfirmed = true,
                Role = Role.Doctor
            };

            Doctor doctor = new Doctor()
            {
                Specialization = receivedDoctor.Specialization,
                YearsOfExperience = receivedDoctor.YearsOfExperience,
                AdditionalInfo = receivedDoctor.AdditionalInfo,
                IsActive = true
            };

            bool isAdded = await _userService.CreateUser(user, "Doctor");

            if (isAdded)
            {
                doctor.UserId = user.Id;
                await _doctorService.CreateDoctor(doctor);
                return NoContent();
            }

            return BadRequest();
        }

        // PUT api/<DoctorsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [Bind("Number,YearsOfExperience,Specialization,AdditionalInformation")] UserDoctorViewModel receivedDoctor)
        {
            var doctor = await _doctorService.GetDoctorByID(id);

            if (doctor == null)
            {
                return BadRequest();
            }

            doctor.Users.Number = receivedDoctor.Number ?? doctor.Users.Number;

            doctor.YearsOfExperience = receivedDoctor.YearsOfExperience != doctor.YearsOfExperience
                                    && receivedDoctor.YearsOfExperience > 0
                                    ? receivedDoctor.YearsOfExperience
                                    : doctor.YearsOfExperience;

            doctor.Specialization = receivedDoctor.Specialization == doctor.Specialization
                                    ? doctor.Specialization
                                    : receivedDoctor.Specialization;

            doctor.AdditionalInfo = receivedDoctor.AdditionalInformation ?? doctor.AdditionalInfo;

            await _doctorService.UpdateDoctor(doctor);

            return NoContent();
        }

        // DELETE api/<DoctorsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _doctorService.DeleteDoctor(id));
        }
    }
}
