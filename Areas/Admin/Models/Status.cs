using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetMind.Areas.Admin.Models
{
    public class Status: Ordable
    {
        
        public int StatusID { get; set; }
        [Required(ErrorMessage = "Nav norādīts nosaukums")]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Is default")]
        public bool IsDefault { get; set; }
        [Display(Name = "Is closed")]
        public bool IsClosed { get; set; }
    }
}
