using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.Services.Services;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DoctorsController : Controller
    {
        private readonly ILogger<UserDoctorRegisterModel> _logger;
        private readonly IDoctorService _doctorService;
        private readonly IUserService _userService;

        public DoctorsController(ILogger<UserDoctorRegisterModel> logger, IDoctorService doctorService, IUserService userService)
        {
            this._logger = logger;
            this._doctorService = doctorService;
            this._userService = userService;
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

                bool isAdded = await _userService.CreateUser(user, "Doctor");

                if (isAdded)
                {
                    doctor.UserId = user.Id;
                    await _doctorService.CreateDoctor(doctor);
                    return RedirectToAction(nameof(Index));
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
            return _doctorService.GetDoctorByID(id) != null;
        }
    }
}
