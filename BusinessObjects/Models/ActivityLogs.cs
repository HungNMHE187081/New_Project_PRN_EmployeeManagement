using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class ActivityLogs
    {
        [Key]
        public int LogID { get; set; }

        public int? UserID { get; set; }

        [StringLength(100)]
        public string Action { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }

        public DateTime LogDate { get; set; } = DateTime.Now;

        [ForeignKey("UserID")]
        public virtual Users? Users { get; set; }
    }
}