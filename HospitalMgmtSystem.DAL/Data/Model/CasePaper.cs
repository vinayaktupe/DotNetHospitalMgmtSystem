using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HospitalMgmtSystem.DAL.Data.Model
{
    public class CasePaper
    {
        [Key]
        public int ID { get; set; }

        //[ForeignKey("Id")]
        //public ApplicationUser Doctor { get; set; }
        public string Doctor { get; set; }

        //[ForeignKey("Id")]
        //public ApplicationUser Patient { get; set; }
        public string Patient { get; set; }

        public string PatientName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Are you creating case paper for youself?")]
        public bool ForSelf { get; set; }

        public bool? IsSolved { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? IsActive { get; set; }
    }
}
