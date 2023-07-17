using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class PlayerTask
    {
        [Required, Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("Player")]
        public Guid PlayerId { get; set; }
        public virtual Player Player { get; set; }

        [ForeignKey("AssignmentId")]
        public Guid AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }

        public double ScoredPoints { get; set; }
        public double Percentage { get; set; }

        public string PlayersCode { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
