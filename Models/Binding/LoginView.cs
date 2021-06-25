using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZulfieP.Models.Binding
{
    public partial class LoginView
    {


        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
