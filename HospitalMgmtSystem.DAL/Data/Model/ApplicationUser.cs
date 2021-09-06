using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalMgmtSystem.DAL.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Please provide First Name")]
        [StringLength(maximumLength: 50, ErrorMessage = "First Name cannot be more than 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide Last Name")]
        [StringLength(maximumLength: 50, ErrorMessage = "Last Name cannot be more than 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please provide Number")]
        [StringLength(maximumLength: 15, ErrorMessage = "Number cannot be more than 15 digits")]
        [MinLength(10, ErrorMessage = "Number cannot be less than 10 digits")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Please provide Address")]
        [StringLength(maximumLength: 250, ErrorMessage = "Address cannot be more than 250 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select Role")]
        public Role Role { get; set; }

        public bool? IsAdmin { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? IsActive { get; set; }

    }

    public enum Role
    {
        Doctor,
        Patient
    }
}
