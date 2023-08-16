using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class Battle
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("Player1")]
        public Guid Player1Id { get; set; }
        public virtual Player Player1 { get; set; }
        public int Player1Points { get; set; }
        public double Player1Time{ get; set; }

        [ForeignKey("Player2")]
        public Guid Player2Id { get; set; }
        public virtual Player Player2 { get; set; }
        public int Player2Points { get; set; }
        public double Player2Time { get; set; }

        public Guid? WonId { get; set; }

        public int LevelNumber { get; set; }
        public string AssignmentIds { get; set; }
    }
}
