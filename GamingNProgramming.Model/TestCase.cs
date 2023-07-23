using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class TestCase
    {
        [Required, Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }

        [ForeignKey("AssignmentId")]
        public Guid? AssignmentId { get; set; }
    }
}
