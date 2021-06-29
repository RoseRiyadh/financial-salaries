using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models
{
    public partial class Salary
    {
        [Key]
        [Display(Name = "ت")]
        public int Id { get; set; }
        [Display(Name = "الموظف")]
        public int EmployeeId { get; set; }
        [Display(Name = "الراتب الاسمي")]
        public decimal InitialSalary { get; set; }
        [Display(Name = "المخصصات الجامعية")]
        public decimal? CollegeAllotments { get; set; }
        [Display(Name = "مخصصات الدرجة الوظيفية")]
        public decimal? DegreeAllotments { get; set; }
        [Display(Name = "المخصصات الزوجية")]
        public decimal? MarrigeAllotments { get; set; }
        [Display(Name = "مخصصات الاطفال")]
        public decimal? KidAllotments { get; set; }
        [Display(Name = "مخصصات النقل")]
        public decimal? TransportationAllotments { get; set; }
        [Display(Name = "مخصصات اللقب الغلمي")]
        public decimal? PositionAllotments { get; set; }
        [Display(Name = "ضريبة الدخل")]
        public decimal? IncomeTax { get; set; }
        [Display(Name = "استقطاع التقاعد (10%)")]
        public decimal? RetirementAllotments { get; set; }
        [Display(Name = "استقطاعات اخرى")]
        public decimal AnotherAllotments { get; set; }
        [Display(Name = "فرق الراتب للمجازين")]
        public decimal VacationDistinction { get; set; }
        [Display(Name = "الراتب الكلي")]
        public decimal? TotalAmount { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
