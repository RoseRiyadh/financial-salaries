using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models.Entities
{
    public partial class ScientificTitles
    {
        public ScientificTitles()
        {
                        Salaries = new HashSet<Salaries>();

        }
        [Key]
        public int Id { get; set; }
        public string ScientificTitle { get; set; }
        public int Income { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Salaries> Salaries { get; set; }
    }


}
