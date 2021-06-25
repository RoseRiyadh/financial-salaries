using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZulfieP.Models.Binding
{
    public partial class CreateGrade
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Grade name:")]
        public string GradeName { get; set; }
    }
}
