using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class Professor
    {
        [Required]
        [Key, ForeignKey("CoreUser")]
        public Guid UserId { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Map> Maps { get; set; }
    }
}
