using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Services.ViewModels
{
    public class UserRolesViewModel
    {
     
        [Required]
        public string RoleName { get; set; }
    }
}
