using HospitalMgmtSystem.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace HospitalMgmtSystem.Services.Services
{
    public interface ISearchService<T> where T : class
    {
        public Task<List<T>> Search(string key);
    }
    public class SearchService<T> : ISearchService<T> where T : class
    {
        private readonly DbSet<T> _context;

        public SearchService()
        {
            _context = GenericRepository<T>.Inst.Set;
        }
        
        public Task<List<T>> Search(string key)
        {
            var res = typeof(T).GetProperties().Select(property => property.Name);
            //if (key == null)
            //{
            //    return null;
            //}

            //var data = await _context.Include("Users").ToListAsync();

            //key = key.ToLower();

            //var filteredResult = data
            //    .Where(patient =>
            //            patient.ID.ToString().ToLower().Contains(key)

            //            || patient.Name.ToLower().Contains(key)

            //            || patient.Number.ToLower().Contains(key)

            //            || patient.Email.ToLower().Contains(key)

            //            || patient.MedicalHistory.ToLower().Split(" ")
            //            .Any(el =>
            //                key.Split(" ")
            //                    .Any(keyEl =>
            //                        el.Contains(keyEl)))

            //            || patient.AdditionalInfo != null
            //            && patient.AdditionalInfo.ToLower().Split(" ")
            //            .Any(el =>
            //                key.Split(" ")
            //                    .Any(keyEl =>
            //                        el.Contains(keyEl)))

            //            || patient.Address != null
            //            && patient.Address.ToLower().Split(" ")
            //            .Any(el =>
            //                key.Split(" ")
            //                    .Any(keyEl =>
            //                        el.Contains(keyEl)))
            //    );
            return null;
        }
    }
}
