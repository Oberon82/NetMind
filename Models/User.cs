using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetMind.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }
        
        [MaxLength(100)]
        public string Email { get; set; }
        
        [MaxLength(24)]
        public string Salt { get; set; }
        
        [MaxLength(64)]
        public string PasswordHash { get; set; }
    }
}
