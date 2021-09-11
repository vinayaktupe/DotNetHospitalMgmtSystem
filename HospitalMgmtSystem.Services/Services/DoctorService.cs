using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.DAL.Repository;
using HospitalMgmtSystem.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgmtSystem.Services.Services
{
    public interface IDoctorService
    {
        public Task<IEnumerable<UserDoctorViewModel>> GetAllDoctors();
        public Task<Doctor> GetDoctorByID(int Id);
        public Task<Doctor> CreateDoctor(Doctor doctor);
        public Task<Doctor> UpdateDoctor(Doctor doctor);
        public Task<bool> DeleteDoctor(int Id);
        public Task<IEnumerable<Doctor>> GetDoctorByCondition(Func<Doctor, bool> condition);
    }
    public class DoctorService : IDoctorService
    {
        public async Task<Doctor> CreateDoctor(Doctor doctor)
        {
            bool isAdded = new GenericRepository<Doctor>().Create(doctor);
            if (isAdded)
            {
                return doctor;
            }
            return null;
        }

        public async Task<bool> DeleteDoctor(int Id)
        {
            GenericRepository<Doctor> genericRepository = new GenericRepository<Doctor>();
            Doctor doctor = genericRepository.GetById(Id);
            if (doctor != null)
            {
                doctor.IsActive = false;
                doctor.Users.IsActive = false;
                genericRepository.Update(doctor);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<UserDoctorViewModel>> GetAllDoctors()
        {
            var res = new GenericRepository<Doctor>().GetAll();
            var doctors = res.Where(doc => doc.IsActive != false).Select(o => new UserDoctorViewModel
            {
                ID = o.ID,
                Name = o.Users.FirstName + " " + o.Users.LastName,
                Email = o.Users.Email,
                Number = o.Users.Number,
                Address = o.Users.Address,
                Specialization = o.Specialization,
                YearsOfExperience = o.YearsOfExperience,
                AdditionalInformation = o.AdditionalInfo
            });

            return doctors;
        }

        public async Task<Doctor> GetDoctorByID(int Id)
        {
            return new GenericRepository<Doctor>().GetByCondition(doc => doc.ID == Id && doc.IsActive != false).SingleOrDefault();
        }

        public async Task<IEnumerable<Doctor>> GetDoctorByCondition(Func<Doctor, bool> condition)
        {
            var res = GenericRepository<Doctor>.Inst.Set.Include(doc=>doc.Users).Where(condition);
            return res;
        }

        public async Task<Doctor> UpdateDoctor(Doctor doctor)
        {
            doctor.Users.UpdatedAt = DateTime.Now;
            if (new GenericRepository<Doctor>().Update(doctor))
            {
                return doctor;
            }

            return null;
        }


    }
}
