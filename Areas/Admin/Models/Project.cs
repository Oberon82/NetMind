using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetMind.Areas.Admin.Models
{
    public class Project
    {
        public int ProjectID { get; set; }

        public int? ParentID { get; set; }
        
        [Required(ErrorMessage = "Nav norādīts nosaukums")]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool Public { get; set; }

        public string Description { get; set; }
    }
}
