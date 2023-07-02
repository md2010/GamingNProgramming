using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Model
{
    public class Avatar
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
