using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GamingNProgramming.Model
{
    public class Map
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public int Number { get; set; }
        public bool IsVisible { get; set; } = false;
        public string Title { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int Points { get; set; }

        [ForeignKey("ProfessorId")]
        public Guid? ProfessorId { get; set; } = null;

        public virtual ICollection<Level> Levels { get; set; }
    }
}
