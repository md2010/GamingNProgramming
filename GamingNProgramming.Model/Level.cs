using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GamingNProgramming.Model
{
    public class Level
    {
        [Required, Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfTasks { get; set; }

        [ForeignKey("MapId")]
        public Guid? MapId { get; set; }
        public virtual Map Map { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
