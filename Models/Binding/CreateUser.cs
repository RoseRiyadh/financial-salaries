using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZulfieP.Models.Binding
{
    public partial class CreateUser : PasswordChange
    {
        public int Id { get; set; }
        [Required, StringLength(100, MinimumLength = 3)]
        [Display(Name ="Full Name:")]
        public string FullName { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        [Display(Name = "UserName :")]
        public string UserName { get; set; }

        [Display(Name = "User Account Activity:")]
        public bool IsActive { get; set; }

    }

    public class PasswordChange
    {
        [Required, StringLength(15, MinimumLength = 6)]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, StringLength(15, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password:")]
        public string ConfirmPassword { get; set; }
    }
}
