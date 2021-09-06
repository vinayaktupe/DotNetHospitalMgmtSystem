using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HospitalMgmtSystem.DAL.Data.Model
{
    public class Patient
    {
        [Key]
        public int ID { get; set; }

        //[ForeignKey("Id")]
        //public ApplicationUser User { get; set; }
        public string User { get; set; }


        [Required(ErrorMessage = "Please provide Blood Group")]
        public BloodGroup BloodGroup { get; set; }

        [Required(ErrorMessage = "Please provide Medical History")]
        public string MedicalHistory { get; set; }

        [StringLength(maximumLength: 250, ErrorMessage = "Additional information can be 250 characters long.")]
        public string AdditionalInfo { get; set; }

        public bool? IsActive { get; set; }


    }

    public enum BloodGroup
    {
        A_Positive,
        A_Negative,
        B_Negative,
        B_Positive,
        O_Positive,
        O_Negative,
        AB_Negative,
        AB_Positive
    }
}
