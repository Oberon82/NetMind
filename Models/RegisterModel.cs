using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetMind.Models
{
    public class RegisterModel
    {
        [Display(Name = "Lietotājs")]
        [Required(ErrorMessage = "Nav norādīts lietotājs")]
        public string UserName { get; set; }

        [Display(Name = "E-pasts")]
        [Required(ErrorMessage ="Nav norādīts e-pasts")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Display(Name = "Parole")]
        [Required(ErrorMessage = "Nav norādīta parole")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Paroles atkārtojums")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Nav norādīta parole")]
        [Compare("Password", ErrorMessage = "Nav ievadīta korekta parole")]
        public string ConfirmPassword { get; set; }
    }
}
