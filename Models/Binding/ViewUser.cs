using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZulfieP.Models.Binding
{
    public partial class ViewUser
    {

        public int Id { get; set; }
        [Display(Name = "Full Name:")]
        public string FullName { get; set; }
        [Required, StringLength(100, MinimumLength = 3)]
        [Display(Name = "UserName:")]
        public string UserName { get; set; }
        [Display(Name = "User Account Activity:")]
        public bool IsActive { get; set; }
    }
}
