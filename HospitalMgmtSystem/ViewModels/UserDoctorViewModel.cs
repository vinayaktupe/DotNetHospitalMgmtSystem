using HospitalMgmtSystem.DAL.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.ViewModels
{
    public class UserDoctorViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public Specialization Specialization { get; set; }

        [Display(Name ="Years of Experience")]
        public int YearsOfExperience { get; set; }

        [Display(Name = "Additional Information")]
        public string AdditionalInformation { get; set; }
    }
}
