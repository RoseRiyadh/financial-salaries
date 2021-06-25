using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models.Entities
{
    public partial class Grades
    {
        public Grades()
        {
            EmployeesGrade = new HashSet<Employees>();
            
        }
        [Key]
        public int Id { get; set; }
        public string GradeName { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Employees> EmployeesGrade { get; set; }
    }
}
