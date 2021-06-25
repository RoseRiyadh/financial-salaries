using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models.Entities
{
    public partial class Salaries
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int InitialSalary { get; set; }
        public int? UniAllotments { get; set; }
        public int? DegreeAllotments { get; set; }
        public int? PositionAllotments { get; set; }
        public int? MarrigeAllotments { get; set; }
        public int? KidsAllotments { get; set; }
        public int? TransportationAllotments { get; set; }
        public int? RetirementSubtraction { get; set; }
        public int? IncomeTax { get; set; }
        public int? OtherSubtractions { get; set; }
        public string Description { get; set; }
        public int ScientificTitleId { get; set; }
        public int? VacationDiff { get; set; }

        public int TotalAmount { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Employees Employee { get; set; }
        public virtual ScientificTitles ScientificTitles { get; set; }
    }
}
