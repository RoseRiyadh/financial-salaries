using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ZulfieP.Models
{
    public partial class User
    {
        [Key]
        [Display(Name = "ت")]
        public int Id { get; set; }
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }
        [Display(Name = "نوع الحساب ")]
        public short Permission { get; set; }
    }
}
