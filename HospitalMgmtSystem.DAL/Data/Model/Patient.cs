using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HospitalMgmtSystem.DAL.Data.Model
{
    [Table("Patients")]
    public class Patient
    {
        public Patient()
        {
            CasePapers = new HashSet<CasePaper>();
        }

        [Key]
        public int ID { get; set; }

        //[ForeignKey("Id")]
        //public ApplicationUser User { get; set; }
        //public string User { get; set; }
        public string UserId { get; set; }


        [Required(ErrorMessage = "Please provide Blood Group")]
        public BloodGroup BloodGroup { get; set; }

        [Required(ErrorMessage = "Please provide Medical History")]
        public string MedicalHistory { get; set; }

        [StringLength(maximumLength: 250, ErrorMessage = "Additional information can be 250 characters long.")]
        public string AdditionalInfo { get; set; }

        public bool? IsActive { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Patients")]
        public virtual ApplicationUser Users { get; set; }


        [InverseProperty("Patients")]
        public virtual ICollection<CasePaper> CasePapers { get; set; }
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
