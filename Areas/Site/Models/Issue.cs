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
        public int IssueID { get; set; }

        [Display(Name = "Project")]
        public int ProjectID { get; set; }

        public Project Project { get; set; }

        [Display(Name = "Creator")]
        public int CreatorID { get; set; }

        public User Creator { get; set; }

        public DateTime CreatedOn { get; set; }

        [Display(Name = "Assignee")]
        public int AssigneeID { get; set; }

        public User Assignee { get; set; }

        public DateTime UpdatedOn { get; set; }
        
        [MaxLength(1000)]
        public string Title { get; set; }

        public string Description { get; set; }
        [Display(Name = "Status")]
        public int StatusID { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Tracker")]
        public int TrackerID { get; set; }

        public Tracker Tracker { get; set; }

        [Display(Name = "Priority")]
        public int PriorityID { get; set; }

        public Priority Priority { get; set; }
    }
}
