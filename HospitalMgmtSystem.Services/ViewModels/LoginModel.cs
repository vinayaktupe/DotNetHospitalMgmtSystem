using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HospitalMgmtSystem.Services.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Please Provide Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please Provide Password")]
        public string Password { get; set; }
        public bool RememberMe{ get; set; }
    }
}
