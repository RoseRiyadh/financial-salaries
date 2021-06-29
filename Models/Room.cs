using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models
{
    public partial class Room
    {
        public Room()
        {
            Employee = new HashSet<Employee>();
        }
        [Key]
        [Display(Name = "ت")]
        public int Id { get; set; }
        [Display(Name = "اسم القسم")]
        public string Name { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
