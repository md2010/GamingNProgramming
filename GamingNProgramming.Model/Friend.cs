using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class Friend
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("Player1")]
        public Guid Player1Id { get; set; }
        public virtual Player Player1 { get; set; }

        [ForeignKey("Player2")]
        public Guid Player2Id { get; set; }        
        public virtual Player Player2 { get; set; }
    }
}
