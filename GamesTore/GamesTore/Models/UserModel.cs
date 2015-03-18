using GamesTore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamesToreAPI.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="First Name")]
        [Required(ErrorMessage="Please enter a first name.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter a last name.")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter a valid email.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ApiKey { get; set; }

        public Roles Role { get; set; }
        public virtual ICollection<SalesModel> Purchases { get; set; }
    }
}