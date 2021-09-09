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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using HospitalMgmtSystem.Services.Services;

namespace HospitalMgmtSystem.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserDoctorRegisterModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IDoctorService _doctorService;

        //private readonly UserDbContext _context;

        public DoctorsController(UserManager<ApplicationUser> userManager, ILogger<UserDoctorRegisterModel> logger, ApplicationDbContext appContext, IDoctorService doctorService)
        {
            this._userManager = userManager;
            this._logger = logger;
            _context = appContext;
            this._doctorService = doctorService;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var res = await _doctorService.GetAllDoctors();
            return View(res);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var doc = await _doctorService.GetDoctorByID(id);

            if (doc == null)
            {
                return NotFound();
            }

            var doctor = new UserDoctorViewModel()
            {
                ID = doc.ID,
                Name = doc.Users.FirstName + " " + doc.Users.LastName,
                Address = doc.Users.Address,
                Email = doc.Users.Email,
                Number = doc.Users.Number,
                Specialization = doc.Specialization,
                YearsOfExperience = doc.YearsOfExperience,
                AdditionalInformation = doc.AdditionalInfo,
            };

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, FirstName, LastName, Number, Address, Email, Specialization, YearsOfExperience, AdditionalInfo")] UserDoctorRegisterModel receivedDoctor)
        {
            receivedDoctor.ID = 0;


            if (ModelState.IsValid)
            {
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

                var result = await _userManager.CreateAsync(user, "User@123");

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "Doctor");
                    doctor.UserId = user.Id;

                    await _doctorService.CreateDoctor(doctor);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Error: {error.Code} - {error.Description}");
                    }
                }
            }
            return View();
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _doctorService.GetDoctorByID(id);
            var doctor = new UserDoctorRegisterModel()
            {
                ID = res.ID,
                YearsOfExperience = res.YearsOfExperience,
                AdditionalInfo = res.AdditionalInfo,
                Specialization = res.Specialization,

                FirstName = res.Users.FirstName,
                LastName = res.Users.LastName,
                Number = res.Users.Number,
                Address = res.Users.Address,
                Email = res.Users.Email
            };

            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, FirstName, LastName, Number, Address, Email, Specialization, YearsOfExperience, AdditionalInfo")] UserDoctorRegisterModel receivedDoctor)
        {
            if (id != receivedDoctor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var doctor = await _doctorService.GetDoctorByID(receivedDoctor.ID);

                    doctor.Users.FirstName = receivedDoctor.FirstName;
                    doctor.Users.LastName = receivedDoctor.LastName;
                    doctor.Users.Number = receivedDoctor.Number;
                    doctor.Users.Address = receivedDoctor.Address;

                    doctor.Specialization = receivedDoctor.Specialization;
                    doctor.YearsOfExperience = receivedDoctor.YearsOfExperience;
                    doctor.AdditionalInfo = receivedDoctor.AdditionalInfo;

                    await _doctorService.UpdateDoctor(doctor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(receivedDoctor.ID))
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
            return View(receivedDoctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
           var doctor = await _doctorService.GetDoctorByID(id);

            UserDoctorViewModel model = new UserDoctorViewModel()
            {
                ID = doctor.ID,
                Name = doctor.Users.FirstName + " " + doctor.Users.LastName,
                Specialization = doctor.Specialization,
                YearsOfExperience = doctor.YearsOfExperience,
                Email = doctor.Users.Email,
                AdditionalInformation = doctor.AdditionalInfo,
                Number = doctor.Users.Number
            };

            if (doctor == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var doctor = await _doctorService.GetDoctorByID(id);


            doctor.IsActive = false;
            doctor.Users.IsActive = false;

            await _doctorService.UpdateDoctor(doctor);

            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.ID == id);
        }
    }
}
