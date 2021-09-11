using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalMgmtSystem.DAL.Data;
using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.Services.Services;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HospitalMgmtSystem.DAL.Repository;

namespace HospitalMgmtSystem.Controllers
{
    public class CasePapersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly ICasePaperService _casePaperService;

        public CasePapersController(ApplicationDbContext context, ILogger<CasePapersController> logger, IDoctorService doctorService, IPatientService patientService, ICasePaperService casePaperService)
        {
            _context = context;
            this._logger = logger;
            this._doctorService = doctorService;
            this._patientService = patientService;
            this._casePaperService = casePaperService;
        }

        //GET: CasePapers/GetDoctorBySpecialization/specialization
        [Route("CasePapers/GetDoctorBySpecialization/{specialization}")]
        public async Task<IActionResult> GetDoctorBySpecialization(int specialization)
        {
            try
            {
                var doctor = GenericRepository<Doctor>.
                    Inst
                    .Set
                    .Include("Users")
                    .Where(doc => doc.Specialization == (Specialization)specialization && doc.IsActive != false)
                    .Select(doc => new { doc.ID, Name = doc.Users.FirstName + " " + doc.Users.LastName })
                    .ToArray();

                return Json(new { status = 200, count = doctor.Count(), data = doctor });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Source} - {ex.Message}");
            }
            return Json(new
            {
                status = 400,
                count = 0,
                data = new { }
            });
        }

        // GET: CasePapers
        public async Task<IActionResult> Index()
        {
            //var applicationDbContext = _context.CasePapers.Include(c => c.Doctors).Include(c => c.Patients);
            var list = await _casePaperService.GetAllCasePapers();
            return View(list);
        }

        // GET: CasePapers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var casePaper = await _casePaperService.GetCasePaperByID(id);

            if (casePaper == null)
            {
                return NotFound();
            }

            return View(new CasePaperViewModel()
            {
                ID = casePaper.ID,
                DoctorName = casePaper.Doctors.Users.FirstName + " " + casePaper.Doctors.Users.LastName,
                SelfName = casePaper.Patients.Users.FirstName + " " + casePaper.Patients.Users.LastName,
                ForSelf = casePaper.ForSelf,
                MedicalHistory = casePaper.Patients.MedicalHistory,
                PatientName = casePaper.PatientName,
                Description = casePaper.Description,
                IsSolved = casePaper.IsSolved,
                Specialization = casePaper.Doctors.Specialization
            });
        }

        // GET: CasePapers/Create
        public async Task<IActionResult> Create()
        {
            ViewData["PatientID"] = new SelectList(await _patientService.GetAllPatients(), "ID", "Name");
            return View();
        }

        // POST: CasePapers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorID,PatientID,PatientName,Description,ForSelf")] CasePaperRegisterModel receivedCasePaper)
        {
            if (ModelState.IsValid)
            {
                CasePaper casePaper = new CasePaper()
                {
                    DoctorID = receivedCasePaper.DoctorID,
                    PatientID = receivedCasePaper.PatientID,
                    Description = receivedCasePaper.Description,
                    ForSelf = receivedCasePaper.ForSelf,
                    PatientName = receivedCasePaper.PatientName
                };

                var res = _casePaperService.CreateCasePaper(casePaper);

                if (res == null)
                {
                    return View(receivedCasePaper);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(await _patientService.GetAllPatients(), "ID", "Name");


            return View(receivedCasePaper);
        }

        // GET: CasePapers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _casePaperService.GetCasePaperByID(id);

            if (res == null)
            {
                return NotFound();
            }

            ViewData["PatientID"] = new SelectList(await _patientService.GetAllPatients(), "ID", "Name");
            var casePaper = new CasePaperRegisterModel()
            {
                ID = res.ID,
                Specialization = res.Doctors.Specialization,
                DoctorID = res.Doctors.ID,
                Description = res.Description,
                ForSelf = (bool)res.ForSelf,
                PatientName = res.PatientName,
                IsSolved = res.IsSolved
            };

            return View(casePaper);
        }

        // POST: CasePapers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Specialization,DoctorID,PatientID,PatientName,Description,ForSelf")] CasePaperRegisterModel receivedCasePaper)
        {
            if (id != receivedCasePaper.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CasePaper casePaper = await _casePaperService.GetCasePaperByID(id);

                    casePaper.DoctorID = receivedCasePaper.DoctorID;
                    casePaper.PatientID = receivedCasePaper.PatientID;
                    casePaper.Description = receivedCasePaper.Description;
                    casePaper.ForSelf = receivedCasePaper.ForSelf;
                    casePaper.PatientName = receivedCasePaper.PatientName;

                    var res = _casePaperService.UpdateCasePaper(casePaper);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasePaperExists(receivedCasePaper.ID))
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
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "ID", "ID", receivedCasePaper.DoctorID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "MedicalHistory", receivedCasePaper.PatientID);
            return View(receivedCasePaper);
        }

        // GET: CasePapers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var casePaper = await _casePaperService.GetCasePaperByID(id);

            if (casePaper == null)
            {
                return NotFound();
            }


            return View(new CasePaperViewModel()
            {
                ID = casePaper.ID,
                DoctorName = casePaper.Doctors.Users.FirstName + " " + casePaper.Doctors.Users.LastName,
                SelfName = casePaper.Patients.Users.FirstName + " " + casePaper.Patients.Users.LastName,
                ForSelf = casePaper.ForSelf,
                MedicalHistory = casePaper.Patients.MedicalHistory,
                PatientName = casePaper.PatientName,
                Description = casePaper.Description,
                IsSolved = casePaper.IsSolved,
                Specialization = casePaper.Doctors.Specialization
            });
        }

        // POST: CasePapers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var casePaper = await _casePaperService.GetCasePaperByID(id);

            casePaper.IsActive = false;
            await _casePaperService.UpdateCasePaper(casePaper);

            return RedirectToAction(nameof(Index));
        }

        private bool CasePaperExists(int id)
        {
            return _context.CasePapers.Any(e => e.ID == id);
        }
    }
}
