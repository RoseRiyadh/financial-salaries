using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models.Entities
{
    public partial class Employees
    {
        public Employees()
        {
            Salaries = new HashSet<Salaries>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Full employee name :")]
        public string FullName { get; set; }
        [Display(Name = "Birth date: :")]
        public DateTime Birthdate { get; set; }
        [Display(Name = "Identity Number :")]
        public string IdentityNumber { get; set; }
        [Display(Name = "Department :")]
        public int DepartmentId { get; set; }
        [Display(Name = "Section :")]
        public short Section { get; set; }
        [Display(Name = "Retirement status :")]
        public short IsRatired { get; set; }
        [Display(Name = "Kids' Number :")]
        public short KidsNumber { get; set; }
        [Display(Name = "Grade :")]
        public int GradeId { get; set; }
        [Display(Name = "Stage :")]
        public int StageId { get; set; }
        [Display(Name = "Marrige Status :")]
        public short MarrigeStatus { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Grades Grade { get; set; }
        public virtual Stages Stage { get; set; }
        public virtual ICollection<Salaries> Salaries { get; set; }
    }
}
