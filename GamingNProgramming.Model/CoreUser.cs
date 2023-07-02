using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamingNProgramming.Model
{
    public class CoreUser
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] PasswordSalt { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        
    }
}