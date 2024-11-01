using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Departments
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = null!;

        public virtual ICollection<Employees> Employees { get; set; } = new List<Employees>();
        public virtual ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
    }
}