using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class PlayerTaskAnswer
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("Player")]
        public Guid PlayerTaskId { get; set; }
        public virtual PlayerTask Player { get; set; }

        [ForeignKey("Answer")]
        public Guid AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
    }
}
