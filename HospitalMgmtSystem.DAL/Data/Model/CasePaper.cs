using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgmtSystem.DAL.Data.Model
{
    [Table("CasePapers")]
    public class CasePaper
    {
        public CasePaper()
        {
            CaseFiles = new HashSet<CaseFile>();
        }

        [Key]
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }

        //public string Doctor { get; set; }
        //public string Patient { get; set; }

        public string PatientName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Are you creating case paper for youself?")]
        public bool? ForSelf { get; set; }

        public bool? IsSolved { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? IsActive { get; set; }

        [ForeignKey(nameof(DoctorID))]
        [InverseProperty("CasePapers")]
        public virtual Doctor Doctors { get; set; }

        [ForeignKey(nameof(PatientID))]
        [InverseProperty("CasePapers")]
        public virtual Patient Patients { get; set; }

        [InverseProperty("CasePapers")]
        public virtual ICollection<CaseFile> CaseFiles { get; set; }
    }
}
