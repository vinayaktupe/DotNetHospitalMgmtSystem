using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HospitalMgmtSystem.DAL.Data.Model
{
    [Table("Doctors")]
    public class Doctor
    {
        public Doctor()
        {
            CasePapers = new HashSet<CasePaper>();
            //CaseFiles = new HashSet<CaseFile>();
        }

        [Key]
        public int ID { get; set; }

        //[ForeignKey("Id")]
        //public ApplicationUser User { get; set; }
        //public string User { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Please select Specialization")]
        public Specialization Specialization { get; set; }

        [Required(ErrorMessage = "Please provide Years of Experience")]
        public int YearsOfExperience { get; set; }

        [StringLength(maximumLength: 250, ErrorMessage = "Additional information can be 250 characters long.")]
        public string AdditionalInfo { get; set; }

        public bool? IsActive { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Doctors")]
        public virtual ApplicationUser Users { get; set; }

        [InverseProperty("Doctors")]
        public virtual ICollection<CasePaper> CasePapers { get; set; }

        //[InverseProperty("Doctors")]
        //public virtual ICollection<CaseFile> CaseFiles { get; set; }


    }

    public enum Specialization
    {
        Allergists_Immunologists,
        Anesthesiologists,
        Cardiologists,
        Colon_and_Rectal_Surgeons,
        Critical_Care_Medicine_Specialists,
        Dermatologists,
        Endocrinologists,
        Emergency_Medicine_Specialists,
        Family_Physicians,
        Gastroenterologists,
        Geriatric_Medicine_Specialists,
        Hematologists,
        Hospice_and_Palliative_Medicine_Specialists,
        Infectious_Disease_Specialists,
        Internists,
        Medical_Geneticists,
        Nephrologists,
        Neurologists,
        Obstetricians_and_Gynecologists,
        Oncologists,
        Ophthalmologists,
        Osteopaths,
        Otolaryngologists,
        Pathologists,
        Pediatricians,
        Physiatrists,
        Plastic_Surgeons,
        Podiatrists,
        Preventive_Medicine_Specialists,
        Psychiatrists,
        Pulmonologists,
        Radiologists,
        Rheumatologists,
        Sleep_Medicine_Specialists,
        Sports_Medicine_Specialists,
        General_Surgeons,
        Urologists
    }
}
