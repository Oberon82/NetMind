using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace NetMind.Areas.Admin.Models
{
    public class Tracker: Ordable
    {
        public int TrackerID { get; set; }

        [Required(ErrorMessage = "Nav norādīts nosaukums")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
