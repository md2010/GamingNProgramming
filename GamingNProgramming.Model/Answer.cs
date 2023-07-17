using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class Answer
    {
        [Required, Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string OfferedAnswer { get; set; }
        public bool IsCorrect { get; set; }

        [ForeignKey("AssignmentId")]
        public Guid? AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }

    }
}
