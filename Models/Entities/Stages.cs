using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models.Entities
{
    public partial class Stages
    {

        public Stages()
        {
            EmployeesStage = new HashSet<Employees>();
        }

        [Key]
        public int Id { get; set; }
        public string StageName { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Employees> EmployeesStage { get; set; }

    }
}
