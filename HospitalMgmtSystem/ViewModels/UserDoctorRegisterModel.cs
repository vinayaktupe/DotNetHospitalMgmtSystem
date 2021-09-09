﻿using HospitalMgmtSystem.DAL.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.ViewModels
{
    public class UserDoctorRegisterModel
    {
        //USER MODEL
        public string Email { get; set; }

        public int ID { get; set; }

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

        //Doctor MODEL
        [Required(ErrorMessage = "Please select Specialization")]
        public Specialization Specialization { get; set; }

        [Required(ErrorMessage = "Please provide Years of Experience")]
        public int YearsOfExperience { get; set; }

        [StringLength(maximumLength: 250, ErrorMessage = "Additional information can be 250 characters long.")]
        public string AdditionalInfo { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
