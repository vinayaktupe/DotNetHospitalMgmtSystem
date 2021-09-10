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

        public CasePapersController(ApplicationDbContext context, ILogger<CasePapersController> logger, IDoctorService doctorService, IPatientService patientService)
        {
            _context = context;
            this._logger = logger;
            this._doctorService = doctorService;
            this._patientService = patientService;
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
                    .Where(doc => doc.Specialization == (Specialization) specialization && doc.IsActive != false)
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
            var applicationDbContext = _context.CasePapers.Include(c => c.Doctors).Include(c => c.Patients);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CasePapers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePaper = await _context.CasePapers
                .Include(c => c.Doctors)
                .Include(c => c.Patients)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (casePaper == null)
            {
                return NotFound();
            }

            return View(casePaper);
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
        public async Task<IActionResult> Create([Bind("DoctorID,PatientID,PatientName,Description,ForSelf")] CasePaperRegisterModel casePaper, IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                _context.Add(casePaper);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(await _patientService.GetAllPatients(), "ID", "Name");


            return View(casePaper);
        }

        // GET: CasePapers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePaper = await _context.CasePapers.FindAsync(id);
            if (casePaper == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(await _patientService.GetAllPatients(), "ID", "Name");

            return View(casePaper);
        }

        // POST: CasePapers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DoctorID,PatientID,PatientName,Description,ForSelf,IsSolved,CreatedAt,UpdatedAt,IsActive")] CasePaper casePaper)
        {
            if (id != casePaper.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(casePaper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasePaperExists(casePaper.ID))
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
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "ID", "ID", casePaper.DoctorID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "MedicalHistory", casePaper.PatientID);
            return View(casePaper);
        }

        // GET: CasePapers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePaper = await _context.CasePapers
                .Include(c => c.Doctors)
                .Include(c => c.Patients)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (casePaper == null)
            {
                return NotFound();
            }

            return View(casePaper);
        }

        // POST: CasePapers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var casePaper = await _context.CasePapers.FindAsync(id);
            _context.CasePapers.Remove(casePaper);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasePaperExists(int id)
        {
            return _context.CasePapers.Any(e => e.ID == id);
        }
    }
}
