using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class Assignment
    {
        [Required, Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public int Number { get; set; }
        public bool IsCoding { get; set; }
        public bool IsTimeMeasured { get; set; }
        public int Seconds { get; set; } = 0;
        public bool HasArgs { get; set; } = false;
        public string InitialCode { get; set; }

        public bool IsMultiSelect { get; set; } 
        
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }

        public bool HasBadge { get; set; }       


        [ForeignKey("LevelId")]
        public Guid LevelId { get; set; }
        public virtual Level Level { get; set; }

        [ForeignKey("BadgeId")]
        public Guid? BadgeId { get; set; }
        public virtual Badge Badge { get; set; }

        public virtual ICollection<TestCase> TestCases { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
