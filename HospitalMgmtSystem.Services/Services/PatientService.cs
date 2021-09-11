using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalMgmtSystem.Services.ViewModels;


namespace HospitalMgmtSystem.Services.Services
{
    public interface IPatientService
    {
        public Task<IEnumerable<UserPatientViewModel>> GetAllPatients();
        public Task<Patient> GetPatientByID(int Id);
        public Task<Patient> CreatePatient(Patient patient);
        public Task<Patient> UpdatePatient(Patient patient);
        public Task<bool> DeletePatient(int Id);
    }
    public class PatientService : IPatientService
    {
        public async Task<Patient> CreatePatient(Patient patient)
        {
            bool isAdded = new GenericRepository<Patient>().Create(patient);
            if (isAdded)
            {
                return patient;
            }
            return null;
        }

        public async Task<bool> DeletePatient(int Id)
        {
            GenericRepository<Patient> genericRepository = new GenericRepository<Patient>();
            Patient patient = genericRepository.GetById(Id);
            if (patient != null)
            {
                patient.IsActive = false;
                patient.Users.IsActive = false;
                genericRepository.Update(patient);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<UserPatientViewModel>> GetAllPatients()
        {
            var res = new GenericRepository<Patient>().GetAll();

            var patients = res.Where(doc => doc.IsActive != false).Select(o => new UserPatientViewModel
            {
                ID = o.ID,
                Name = o.Users.FirstName + " " + o.Users.LastName,
                Email = o.Users.Email,
                Number = o.Users.Number,
                Address = o.Users.Address,
                BloodGroup = o.BloodGroup,
                MedicalHistory = o.MedicalHistory,
                AdditionalInfo = o.AdditionalInfo
            });

            return patients;
        }

        public async Task<Patient> GetPatientByID(int Id)
        {
            return new GenericRepository<Patient>().GetByCondition(patient => patient.ID == Id && patient.IsActive != false).SingleOrDefault();
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            patient.Users.UpdatedAt = DateTime.Now;
            if (new GenericRepository<Patient>().Update(patient))
            {
                return patient;
            }

            return null;
        }
    }
}
