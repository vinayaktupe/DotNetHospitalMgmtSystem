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
    public class CasePapersController : Controller
    {
        private readonly UserDbContext _context;

        public CasePapersController(UserDbContext context)
        {
            _context = context;
        }

        // GET: CasePapers
        public async Task<IActionResult> Index()
        {
            return View(await _context.CasePapers.ToListAsync());
        }

        // GET: CasePapers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var casePaper = await _context.CasePapers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (casePaper == null)
            {
                return NotFound();
            }

            return View(casePaper);
        }

        // GET: CasePapers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CasePapers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Doctor,Patient,PatientName,Description,ForSelf,IsSolved,CreatedAt,UpdatedAt,IsActive")] CasePaper casePaper)
        {
            if (ModelState.IsValid)
            {
                _context.Add(casePaper);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(casePaper);
        }

        // POST: CasePapers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Doctor,Patient,PatientName,Description,ForSelf,IsSolved,CreatedAt,UpdatedAt,IsActive")] CasePaper casePaper)
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
