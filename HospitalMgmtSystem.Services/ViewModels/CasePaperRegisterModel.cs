using HospitalMgmtSystem.DAL.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalMgmtSystem.Services.ViewModels
{
    public class CasePaperRegisterModel
    {
        [Required]
        public int DoctorID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public Specialization Specialization { get; set; }

        public string PatientName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Are you creating case paper for youself?")]
        public bool ForSelf { get; set; }

        public bool? IsSolved { get; set; }
    }
}
