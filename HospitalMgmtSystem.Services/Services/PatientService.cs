using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgmtSystem.Services.Services
{
    public interface IPatientService
    {
        public Task<IEnumerable<UserPatientViewModel>> GetAllPatients();
        public Task<Patient> GetPatientByID(int Id);
        public Task<Patient> CreatePatient(Patient patient);
        public Task<Patient> UpdatePatient(Patient patient);
        public Task<bool> DeletePatient(int Id);

        public Task<IEnumerable<UserPatientViewModel>> Search(string? key);
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
            }).OrderBy(patient=>patient.ID);

            return patients;
        }

        public async Task<Patient> GetPatientByID(int Id)
        {
            return GenericRepository<Patient>
                .Inst
                .Set
                .Include(patient => patient.Users)
                .Where(patient => patient.ID == Id && patient.IsActive != false).SingleOrDefault();
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            patient.Users.UpdatedAt = DateTime.Now;
            patient.Users.IsActive = true;
            patient.IsActive = true;

            if (new GenericRepository<Patient>().Update(patient))
            {
                return patient;
            }

            return null;
        }

        public async Task<IEnumerable<UserPatientViewModel>> Search(string? key)
        {
            if (key == null)
            {
                return null;
            }

            var data = await GetAllPatients();
            data = data.ToList();

            key = key.ToLower();

            var filteredResult = data
                .Where(patient =>
                        patient.ID.ToString().ToLower().Contains(key)

                        || patient.Name.ToLower().Contains(key)

                        || patient.Number.ToLower().Contains(key)

                        || patient.Email.ToLower().Contains(key)

                        || patient.MedicalHistory.ToLower().Split(" ")
                        .Any(el =>
                            key.Split(" ")
                                .Any(keyEl =>
                                    el.Contains(keyEl)))

                        || patient.AdditionalInfo != null
                        && patient.AdditionalInfo.ToLower().Split(" ")
                        .Any(el =>
                            key.Split(" ")
                                .Any(keyEl =>
                                    el.Contains(keyEl)))

                        || patient.Address != null
                        && patient.Address.ToLower().Split(" ")
                        .Any(el =>
                            key.Split(" ")
                                .Any(keyEl =>
                                    el.Contains(keyEl)))
                );

            return filteredResult;
        }
    }
}
