using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NetMind.Areas.Admin.Models;
using NetMind.Models;



namespace NetMind.Areas.Site.Models
{
    public class Issue
    {
        [Display(Name = "#")]
        public int IssueID { get; set; }

        [Display(Name = "Creator")]
        public int CreatorID { get; set; }

        public User Creator { get; set; }

        public DateTime CreatedOn { get; set; }

        [Display(Name = "Assignee")]
        public int? AssigneeID { get; set; }

        public User Assignee { get; set; }

        public DateTime UpdatedOn { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Status")]
        [Required]
        public int StatusID { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Tracker")]
        [Required]
        public int TrackerID { get; set; }

        public Tracker Tracker { get; set; }

        [Display(Name = "Priority")]
        [Required]
        public int PriorityID { get; set; }

        public Priority Priority { get; set; }

        [Display(Name = "Is closed")]
        public bool IsClosed { get; set; }
    }
}
