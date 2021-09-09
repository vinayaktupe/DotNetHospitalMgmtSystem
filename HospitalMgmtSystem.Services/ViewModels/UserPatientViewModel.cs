using HospitalMgmtSystem.DAL.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalMgmtSystem.Services.ViewModels
{
    public class UserPatientViewModel
    {

        public string Email { get; set; }

        public int ID { get; set; }

       public string Name { get; set; }

        public string Number { get; set; }

        public string Address { get; set; }

        [Display(Name = "Blood Group")]
        public BloodGroup BloodGroup { get; set; }

        [Display(Name = "Medical History")]
        public string MedicalHistory { get; set; }

        [Display(Name ="Additional Information")]
        public string AdditionalInfo { get; set; }
    }
}
