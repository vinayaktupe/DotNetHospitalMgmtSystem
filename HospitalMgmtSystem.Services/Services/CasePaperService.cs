using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.DAL.Repository;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Services.Services
{
    public interface ICasePaperService
    {
        public Task<IEnumerable<CasePaperViewModel>> GetAllCasePapers();
        public Task<CasePaper> GetCasePaperByID(int Id);
        public Task<CasePaper> CreateCasePaper(CasePaper casePaper);
        public Task<CasePaper> UpdateCasePaper(CasePaper casePaper);
        public Task<bool> DeleteCasePaper(int Id);
    }
    public class CasePaperService : ICasePaperService
    {
        private readonly IQueryable<CasePaper> _context;

        public CasePaperService()
        {
            _context = GenericRepository<CasePaper>
                .Inst
                .Set
                .Include("Doctors")
                .Include("Patients");
        }
        public async Task<CasePaper> CreateCasePaper(CasePaper casePaper)
        {
            bool isAdded = new GenericRepository<CasePaper>().Create(casePaper);
            if (isAdded)
            {
                return casePaper;
            }
            return null;
        }

        public async Task<bool> DeleteCasePaper(int Id)
        {
            GenericRepository<CasePaper> genericRepository = new GenericRepository<CasePaper>();
            CasePaper casePaper = genericRepository.GetById(Id);
            if (casePaper != null)
            {
                casePaper.IsActive = false;
                genericRepository.Update(casePaper);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<CasePaperViewModel>> GetAllCasePapers()
        {


            var casePapers = _context.
                Where(paper => paper.IsActive != false)
                .Select(o => new CasePaperViewModel
                {
                    ID = o.ID,
                    DoctorName = o.Doctors.Users.FirstName + " " + o.Doctors.Users.LastName,
                    SelfName = o.Patients.Users.FirstName + " " + o.Patients.Users.LastName,
                    PatientName = o.PatientName,
                    Specialization = o.Doctors.Specialization,
                    Description = o.Description,
                    ForSelf = o.ForSelf,
                    IsSolved = o.IsSolved,
                    MedicalHistory = o.Patients.MedicalHistory
                });

            return casePapers;
        }

        public async Task<CasePaper> GetCasePaperByID(int Id)
        {
            var casePaper = _context
                .Include(paper => paper.Doctors.Users)
                .Include(paper => paper.Patients.Users)
                .Where(casePaper => casePaper.ID == Id && casePaper.IsActive == true);

            return await casePaper.SingleOrDefaultAsync();
        }

        public async Task<CasePaper> UpdateCasePaper(CasePaper casePaper)
        {
            casePaper.UpdatedAt = DateTime.Now;
            if (new GenericRepository<CasePaper>().Update(casePaper))
            {
                return casePaper;
            }

            return null;
        }
    }
}
