using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Services.Services
{
    public interface ICaseFileService
    {
        public Task<IEnumerable<CaseFile>> GetCaseFileByCaseID(int Id);
        public Task<CaseFile> CreateCaseFile(CaseFile caseFile);
        public Task<CaseFile> UpdateCaseFile(CaseFile caseFile);
        public Task<bool> DeleteCaseFile(int Id);
        public Task<IEnumerable<CaseFile>> GetCaseFileByCondition(Func<CaseFile, bool> condition);
    }
    public class CaseFileService : ICaseFileService
    {
        private readonly IQueryable<CaseFile> _context;

        public CaseFileService()
        {
            _context = GenericRepository<CaseFile>
                .Inst
                .Set
                .Include(file => file.CasePapers)
                .Include(file => file.CasePapers.Doctors)
                .Include(file => file.CasePapers.Patients);
                //.Include(file => file.CasePapers.Doctors.Users)
                //.Include(file => file.CasePapers.Patients.Users);
        }
        public async Task<CaseFile> CreateCaseFile(CaseFile caseFile)
        {
            bool isAdded = new GenericRepository<CaseFile>().Create(caseFile);
            if (isAdded)
            {
                return caseFile;
            }
            return null;
        }

        public async Task<bool> DeleteCaseFile(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CaseFile>> GetCaseFileByCaseID(int Id)
        {
            return _context.Where(file => file.CasePapers.ID == Id).AsEnumerable();
        }

        public async Task<IEnumerable<CaseFile>> GetCaseFileByCondition(Func<CaseFile, bool> condition)
        {
            return _context.Where(condition);
        }

        public async Task<CaseFile> UpdateCaseFile(CaseFile caseFile)
        {
            caseFile.UpdatedAt = DateTime.Now;
            if (new GenericRepository<CaseFile>().Update(caseFile))
            {
                return caseFile;
            }

            return null;
        }
    }
}
