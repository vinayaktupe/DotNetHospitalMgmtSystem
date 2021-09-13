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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace HospitalMgmtSystem.Controllers
{
    [Authorize]
    public class CaseFilesController : Controller
    {
        private readonly ILogger<CaseFilesController> _logger;
        private readonly ICaseFileService _caseFileService;

        public CaseFilesController(ILogger<CaseFilesController> logger, ICaseFileService caseFileService)
        {
            this._logger = logger;
            this._caseFileService = caseFileService;
        }

        // GET: CaseFiles
        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        if (User.IsInRole("Admin"))
        //        {
        //            var res = await _caseFileService.GetCaseFileByCaseID();
        //        }
        //        else if (User.IsInRole("Doctor"))
        //        {

        //        }
        //        else if (User.IsInRole("Patient"))
        //        {
        //        }
        //        else
        //        {
        //            return Unauthorized();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error: {ex.Source} - {ex.Message}");
        //    }

        //    return BadRequest();
        //}

        // GET: CaseFiles/Details/5
        public async Task<IActionResult> Details(int Id)
        {
            IEnumerable<CaseFile> res = null;
            try
            {
                if (User.IsInRole("Admin"))
                {
                    res = await _caseFileService.GetCaseFileByCaseID(Id);
                }
                else if (User.IsInRole("Doctor"))
                {
                    res = await _caseFileService
                        .GetCaseFileByCondition(file =>
                        file.CaseID == Id &&
                        file.CasePapers.Doctors.Users.Email == User.Identity.Name);

                }
                else if (User.IsInRole("Patient"))
                {
                    res = await _caseFileService
                        .GetCaseFileByCondition(file =>
                        file.CaseID == Id &&
                        file.CasePapers.Patients.Users.Email == User.Identity.Name);
                }
                else
                {
                    return Unauthorized("Yo do not have permission to access this route");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Source} - {ex.Message}");
                return BadRequest();
            }

            return Json(new { Count = res.Count(), Data = res.ToList() });
        }

        // GET: CaseFiles/Create/id
        [Authorize(Roles ="Doctor")]
        public IActionResult Create(int id)
        {
            ViewData["CaseID"] = id;
            return PartialView();
        }

        // POST: CaseFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseID,Name,FileType,Fields")] CaseFile caseFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _caseFileService.CreateCaseFile(caseFile);
                }
                else
                {
                    BadRequest("Please provide all requested fields");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Source} - {ex.Message}");
                return BadRequest();
            }

            return Ok();
        }

        public async Task<IActionResult> Download(int id) {
            var res = await _caseFileService.GetCaseFileByID(id);
            if (res==null)
            {
                return BadRequest();
            }

            return PartialView(res);
        }



        // GET: CaseFiles/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var caseFile = await _context.CaseFiles.FindAsync(id);
        //    if (caseFile == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CaseID"] = new SelectList(_context.CasePapers, "ID", "ID", caseFile.CaseID);
        //    return View(caseFile);
        //}

        // POST: CaseFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,CaseID,FileType,Fields,CreatedAt,UpdatedAt")] CaseFile caseFile)
        //{
        //    if (id != caseFile.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(caseFile);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CaseFileExists(caseFile.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CaseID"] = new SelectList(_context.CasePapers, "ID", "ID", caseFile.CaseID);
        //    return View(caseFile);
        //}

        // GET: CaseFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var res = await _caseFileService.GetCaseFileByCondition(file => file.ID == id);
            if (res == null)
            {
                return NotFound();
            }

            var caseFile = res.SingleOrDefault();
            caseFile.IsActive = false;
            await _caseFileService.UpdateCaseFile(caseFile);

            return Ok();
        }

        // POST: CaseFiles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    //var caseFile = await _context.CaseFiles.FindAsync(id);
        //    //_context.CaseFiles.Remove(caseFile);
        //    //await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private async Task<bool> CaseFileExists(int id)
        {
            var res = await _caseFileService.GetCaseFileByCaseID(id);
            return res != null;
        }
    }
}
