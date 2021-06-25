using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZulfieP.Models.Binding
{
    public partial class EmployeeCreate
    {
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

        public int InitialSalary { get; set; }
        public int? UniAllotments { get; set; }
        public int? DegreeAllotments { get; set; }
        public int? PositionAllotments { get; set; }
        public int? MarrigeAllotments { get; set; }
        public int? KidsAllotments { get; set; }
        public int? TransportationAllotments { get; set; }
        public int? RetirementSubtraction { get; set; }
        public int? OtherSubtractions { get; set; }
        public string Description { get; set; }
        public int ScientificTitleId { get; set; }
        public int? VacationDiff { get; set; }


    }
}
