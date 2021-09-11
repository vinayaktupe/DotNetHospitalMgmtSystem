using HospitalMgmtSystem.DAL.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalMgmtSystem.Services.ViewModels
{
    public class CasePaperViewModel
    {
        public int DocId { get; set; }

        [Display(Name ="Case ID")]
        public int ID { get; set; }
        [Display(Name = "Doctor Name")]
        public string DoctorName { get; set; }

        [Display(Name = "Patient Name")]
        public string SelfName { get; set; }

        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Display(Name = "Department")]
        public Specialization Specialization { get; set; }

        [Display(Name = "Patient Medical History")]
        public string MedicalHistory { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool? ForSelf { get; set; }

        public bool? IsSolved { get; set; }
    }
}
