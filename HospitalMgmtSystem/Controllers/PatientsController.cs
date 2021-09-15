using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalMgmtSystem.DAL.Data;
using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.Extensions.Logging;
using HospitalMgmtSystem.Services.Services;
using Microsoft.AspNetCore.Authorization;

namespace HospitalMgmtSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PatientsController : Controller
    {
        private readonly ILogger<UserPatientRegisterModel> _logger;
        private readonly IPatientService _patientService;
        private readonly IUserService _userService;

        public PatientsController(ILogger<UserPatientRegisterModel> logger, IPatientService patientService, IUserService userService)
        {
            this._logger = logger;
            this._patientService = patientService;
            this._userService = userService;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _patientService.GetAllPatients());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var res = await _patientService.GetPatientByID(id);
            if (res == null)
            {
                return NotFound();
            }
            var patient = new UserPatientViewModel()
            {
                ID = res.ID,
                Name = res.Users.FirstName + " " + res.Users.LastName,
                Email = res.Users.Email,
                Address = res.Users.Address,
                Number = res.Users.Number,
                BloodGroup = res.BloodGroup,
                MedicalHistory = res.MedicalHistory,
                AdditionalInfo = res.AdditionalInfo
            };

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, FirstName, LastName, Number, Address, Email,BloodGroup,MedicalHistory,AdditionalInfo")] UserPatientRegisterModel receivedPatient)
        {
            receivedPatient.ID = 0;

            if (ModelState.IsValid)
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

                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            return View(receivedPatient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _patientService.GetPatientByID(id);

            if (res == null)
            {
                return NotFound();
            }

            var patient = new UserPatientRegisterModel()
            {
                ID = res.ID,
                MedicalHistory = res.MedicalHistory,
                BloodGroup = res.BloodGroup,
                AdditionalInfo = res.AdditionalInfo,

                FirstName = res.Users.FirstName,
                LastName = res.Users.LastName,
                Number = res.Users.Number,
                Address = res.Users.Address,
                Email = res.Users.Email
            };

            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, FirstName, LastName, Number, Address, Email,BloodGroup,MedicalHistory,AdditionalInfo")] UserPatientRegisterModel receivedPatient)
        {
            if (id != receivedPatient.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var patient = await _patientService.GetPatientByID(receivedPatient.ID);
                    patient.Users.FirstName = receivedPatient.FirstName;
                    patient.Users.LastName = receivedPatient.LastName;
                    patient.Users.Number = receivedPatient.Number;
                    patient.Users.Address = receivedPatient.Address;

                    patient.MedicalHistory = receivedPatient.MedicalHistory;
                    patient.BloodGroup = receivedPatient.BloodGroup;
                    patient.AdditionalInfo = receivedPatient.AdditionalInfo;

                    await _patientService.UpdatePatient(patient);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(receivedPatient.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(receivedPatient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _patientService.GetPatientByID(id);

            if (res == null)
            {
                return NotFound();
            }

            UserPatientViewModel patient = new UserPatientViewModel()
            {
                ID = res.ID,
                Name = res.Users.FirstName + " " + res.Users.LastName,
                Email = res.Users.Email,
                Address = res.Users.Address,
                Number = res.Users.Number,
                BloodGroup = res.BloodGroup,
                MedicalHistory = res.MedicalHistory,
                AdditionalInfo = res.AdditionalInfo
            };

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _patientService.GetPatientByID(id);

            patient.IsActive = false;
            patient.Users.IsActive = false;

            await _patientService.UpdatePatient(patient);

            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _patientService.GetPatientByID(id) != null;
        }

        public async Task<IActionResult> Search(string id)
        {
            if (id == null)
            {
                return BadRequest(new { status = "fail" });
            }

            var filteredResult = await _patientService.Search(id);


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
