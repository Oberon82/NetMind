using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetMind.Areas.Admin.Models
{
    public class Priority: Ordable
    {
        public int PriorityID { get; set; }
        [Required(ErrorMessage = "Nav norādīts nosaukums")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Display(Name = "Is default")]
        public bool IsDefault { get; set; }

        public bool Active { get; set; }
    }
}
