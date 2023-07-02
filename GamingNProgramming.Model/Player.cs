using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class Player
    {
        [Required]
        [Key, ForeignKey("CoreUser")]
        public Guid UserId { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string Username { get; set; }
        public int Points { get; set; }
        public int XPs { get; set; }

        [ForeignKey("Professor")]
        public Guid? ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }

        public virtual CoreUser CoreUser { get; set; }

        [ForeignKey("Avatar")]
        public Guid AvatarId { get; set; }

        public virtual Avatar Avatar { get; set; }
    }
}
