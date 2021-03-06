using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace HospitalMgmtSystem.DAL.Data.Model
{
    [Table("CaseFiles")]
    public class CaseFile
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage ="Please provide name for the file")]
        public string Name { get; set; }
        public int CaseID { get; set; }

        [Required(ErrorMessage = "Please Provide File Type")]
        public FileType FileType { get; set; }

        public string Fields { get; set; }

        public bool? IsActive { get; set; }

        //[Required]
        //public string CreatedByID { get; set; }

        ////[Required]
        //public string UpdatedByID { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CaseID))]
        [InverseProperty("CaseFiles")]
        public virtual CasePaper CasePapers { get; set; }

        //[ForeignKey(nameof(CreatedByID))]
        //[InverseProperty("CaseFiles")]
        //public virtual Doctor CreatedBy { get; set; }

        //[ForeignKey(nameof(UpdatedByID))]
        //[InverseProperty("CaseFiles")]
        //public virtual Doctor UpdatedBy { get; set; }
    }

    public enum FileType
    {
        Report,
        Prescription
    }
}
