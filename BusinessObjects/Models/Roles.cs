using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = null!;

        public virtual ICollection<Users> Users { get; set; } = new List<Users>();
    }
}