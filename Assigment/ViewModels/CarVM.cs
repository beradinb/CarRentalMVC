using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assigment.ViewModels
{
    public class CarVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Car Make")]
        [Display(Name = "Make")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Please Enter Car Model")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Please Enter Car Cost p/Day")]
        [Display(Name = "CostPD")]
        public int CostPD { get; set; }

        [Required(ErrorMessage = "Please Enter Car Quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }


    }
}
