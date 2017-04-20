using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assigment.ViewModels
{
    public class UserRegistrationVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Your Firstname")]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Surname")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Remote("doesUserNameExist", "User", HttpMethod = "POST", ErrorMessage = "User name already exists. Please enter a different user name.")]
        [Required(ErrorMessage = "Please Enter Username")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please Enter a Password")]
        [StringLength(10, ErrorMessage = "Please choose a password with a mimimum length of 7 characters", MinimumLength = 7)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter an Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter a valid Email Address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

    }
}