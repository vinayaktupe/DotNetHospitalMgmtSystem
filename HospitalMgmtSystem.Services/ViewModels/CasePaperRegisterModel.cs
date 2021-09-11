using HospitalMgmtSystem.DAL.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalMgmtSystem.Services.ViewModels
{
    public class CasePaperRegisterModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Please Select Doctor")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Please Select Patient")]
        public int PatientID { get; set; }

        [Required(ErrorMessage ="Please Select Specialization")]
        public Specialization Specialization { get; set; }

        public string PatientName { get; set; }

        [Required(ErrorMessage ="Casepaper must have description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Are you creating case paper for youself?")]
        public bool ForSelf { get; set; }

        public bool? IsSolved { get; set; }
    }
}
