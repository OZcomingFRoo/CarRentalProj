using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarRentalProj.CSCode;
namespace CarRentalProj.Models
{
    public class SignInEmp
    {
        public string Identity { get; set; }
        public string Password { get; set; }
    }


    public class EmpValidation : ValidationAttribute
    {
        [Required]
        [IdentityValidation]
        [Range(8,10)]
        public string Identity { get; set; }

        [Required]
        [PasswordValidation]
        public string Password { get; set; }
    }

}