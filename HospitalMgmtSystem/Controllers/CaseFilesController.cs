using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalMgmtSystem.DAL.Data;
using HospitalMgmtSystem.DAL.Data.Model;

namespace HospitalMgmtSystem.Controllers
{
    public class CaseFilesController : Controller
    {
        private readonly UserDbContext _context;

        public CaseFilesController(UserDbContext context)
        {
            _context = context;
        }

        // GET: CaseFiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaseFiles.ToListAsync());
        }

        // GET: CaseFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseFile = await _context.CaseFiles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (caseFile == null)
            {
                return NotFound();
            }

            return View(caseFile);
        }

        // GET: CaseFiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaseFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FileType,Fields,CreatedBy,UpdatedBy,CreatedAt,UpdatedAt")] CaseFile caseFile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caseFile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caseFile);
        }

        // GET: CaseFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseFile = await _context.CaseFiles.FindAsync(id);
            if (caseFile == null)
            {
                return NotFound();
            }
            return View(caseFile);
        }

        // POST: CaseFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FileType,Fields,CreatedBy,UpdatedBy,CreatedAt,UpdatedAt")] CaseFile caseFile)
        {
            if (id != caseFile.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseFileExists(caseFile.ID))
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
            return View(caseFile);
        }

        // GET: CaseFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseFile = await _context.CaseFiles
                .FirstOrDefaultAsync(m => m.ID == id);
            if (caseFile == null)
            {
                return NotFound();
            }

            return View(caseFile);
        }

        // POST: CaseFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseFile = await _context.CaseFiles.FindAsync(id);
            _context.CaseFiles.Remove(caseFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseFileExists(int id)
        {
            return _context.CaseFiles.Any(e => e.ID == id);
        }
    }
}
