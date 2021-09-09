using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalMgmtSystem.DAL.Data;
using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace HospitalMgmtSystem.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserDoctorRegisterModel> _logger;
        private readonly ApplicationDbContext _context;
        //private readonly UserDbContext _context;

        public DoctorsController(UserManager<ApplicationUser> userManager, ILogger<UserDoctorRegisterModel> logger, ApplicationDbContext appContext/*, UserDbContext context*/)
        {
            this._userManager = userManager;
            this._logger = logger;
            //this._appContext = appContext;
            _context = appContext;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            //var res = _appContext.Users.Where(user => user.IsActive != false)
            //    .Join(
            //    _context.Doctors,
            //    user => user.Id,
            //    doctor => doctor.User,
            //    (appuser, doc) => new
            //    {
            //        doc.ID,
            //        Name = appuser.FirstName + " " + appuser.LastName,
            //        appuser.Address,
            //        appuser.Email,
            //        appuser.Number,
            //        doc.Specialization,
            //        doc.YearsOfExperience,
            //        AdditionalInformation = doc.AdditionalInfo,
            //    }
            //    ).Cast<UserDoctorViewModel>().OrderBy(model => new { model.Specialization, model.YearsOfExperience, model.Name }).AsEnumerable();

            //var appUser = await Task.Run(() => _appContext.Users.Where(user => user.IsActive != false && user.Role == Role.Doctor).AsEnumerable());

            //var doctor = await Task.Run(() => _context.Doctors.Where(doctor => doctor.IsActive != false).AsEnumerable());

            //var res = appUser;
            //(from user in appUser
            //           join doc in doctor
            //           on user.Id equals doc.User
            //           orderby doc.Specialization, doc.YearsOfExperience
            //           select new UserDoctorViewModel
            //           {
            //               ID = doc.ID,
            //               Name = user.FirstName + " " + user.LastName,
            //               Address = user.Address,
            //               Email = user.Email,
            //               Number = user.Number,
            //               Specialization = doc.Specialization,
            //               YearsOfExperience = doc.YearsOfExperience,
            //               AdditionalInformation = doc.AdditionalInfo,
            //           });

            //return View(await _context.Doctors.ToListAsync());
            var res =
                _context.Doctors.Where(doc => doc.IsActive != false && doc.Users.IsActive != false).Select(doc => new UserDoctorViewModel
                {
                    ID = doc.ID,
                    Name = doc.Users.FirstName + " " + doc.Users.LastName,
                    Number = doc.Users.Number,
                    Email = doc.Users.Email,
                    Address = doc.Users.Address,
                    Specialization = doc.Specialization,
                    YearsOfExperience = doc.YearsOfExperience,
                    AdditionalInformation = doc.AdditionalInfo
                })
                .AsEnumerable();

            return View(res);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doc = await _context.Doctors
                .Include("Users")
                .FirstOrDefaultAsync(m => m.ID == id && m.IsActive != false);

            if (doc == null)
            {
                return NotFound();
            }
            //var user = await _appContext.Users.FirstOrDefaultAsync(appUser => appUser.Id == doc.User && appUser.IsActive != false);

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
            //UserDoctorRegisterModel model = new UserDoctorRegisterModel() { User = user, Doctor = doctor };
            //user.UserName = user.Email;
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
                    _context.Doctors.Add(doctor);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var res = await _context.Doctors.Include("Users").FirstOrDefaultAsync(doc => doc.ID == id);
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
                    var doctor = await _context.Doctors.FindAsync(receivedDoctor.ID);
                    var user = await _context.Users.FindAsync(doctor.UserId);

                    user.FirstName = receivedDoctor.FirstName;
                    user.LastName = receivedDoctor.LastName;
                    user.Number = receivedDoctor.Number;
                    user.Address = receivedDoctor.Address;

                    doctor.Specialization = receivedDoctor.Specialization;
                    doctor.YearsOfExperience = receivedDoctor.YearsOfExperience;
                    doctor.AdditionalInfo = receivedDoctor.AdditionalInfo;

                    _context.Doctors.Update(doctor);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include("Users")
                .FirstOrDefaultAsync(m => m.ID == id);
            //var user = await _appContext.Users.FirstOrDefaultAsync(m => m.Id == doctor.User);
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
            var doctor = await _context.Doctors
                .Include("Users")
                .FirstOrDefaultAsync(doc => doc.ID == id);
            //var user = await _appContext.Users.FirstOrDefaultAsync(m => m.Id == doctor.User);

            doctor.IsActive = false;
            //user.IsActive = false;
            doctor.Users.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.ID == id);
        }
    }
}
