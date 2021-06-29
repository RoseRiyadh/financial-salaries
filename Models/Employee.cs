using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Salary = new HashSet<Salary>();
        }
        [Key]
        [Display(Name = "ت")]
        public int Id { get; set; }
        [Display(Name = "اسم الموظف الكامل")]
        public string FullName { get; set; }
        [Display(Name = "تاريخ الميلاد")]
        public DateTime Birthdate { get; set; }
        [Display(Name = "عدد الاطفال (ان وجد)")]
        public short? KidsNumber { get; set; }
        [Display(Name = "تاريخ بداية الخدمة")]
        public DateTime StartingDate { get; set; }
        [Display(Name = "الحالة الزوجية")]
        public int MarrigeStatusId { get; set; }
        [Display(Name = "اللقب العلمي")]
        public int PositionId { get; set; }
        [Display(Name = "المرحلة الوظيفية")]
        public int DegreeId { get; set; }
        [Display(Name = "متخرج من ")]
        public int CollegeId { get; set; }
        [Display(Name = "الدرجة الوظيفية")]
        public int GradeId { get; set; }
        [Display(Name = "تابع لقسم ")]
        public int RoomId { get; set; }
        [Display(Name = "نوع الوظيفة")]
        public int JobTitleId { get; set; }

        public virtual College College { get; set; }
        public virtual Degree Degree { get; set; }
        public virtual Grade Grade { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        public virtual MarrigeStatus MarrigeStatus { get; set; }
        public virtual Position Position { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<Salary> Salary { get; set; }
    }
}
