using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models
{
    public partial class Degree
    {
        public Degree()
        {
            Employee = new HashSet<Employee>();
        }
        [Key]
        [Display(Name = "ت")]
        public int Id { get; set; }
        [Display(Name = "المرحلة الجامعية")]
        public string Name { get; set; }
        [Display(Name = "المبلغ المخصص (%) ")]
        public decimal Income { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
    
}
