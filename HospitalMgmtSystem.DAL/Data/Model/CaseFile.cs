using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HospitalMgmtSystem.DAL.Data.Model
{
    public class CaseFile
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public CasePaper CasePaper { get; set; }

        [Required(ErrorMessage = "Please Provide File Type")]
        public FileType FileType { get; set; }

        public string Fields { get; set; }

        //[Required]
        //public ApplicationUser CreatedBy { get; set; }
        public string CreatedBy { get; set; }

        //[Required]
        //public ApplicationUser UpdatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public enum FileType
    {
        Report,
        Prescription
    }
}
