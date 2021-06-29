using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models
{
    public partial class Grade
    {
        public Grade()
        {
            Employee = new HashSet<Employee>();
        }
        [Key]
        [Display(Name = "ت")]
        public int Id { get; set; }
        [Display(Name = "الدرجة الوظيفية")]
        public string Name { get; set; }
        [Display(Name = "المبلغ المخصص(%)")]
        public decimal Income { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
