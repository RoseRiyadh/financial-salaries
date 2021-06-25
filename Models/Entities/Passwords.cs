using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models.Entities
{
    public partial class Passwords
    {
        public Passwords()
        {
            Users = new HashSet<Users>();
        }
        [Key]
        public int Id { get; set; }
        public string HashedPassword { get; set; }
        public string PasswordSalt { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
