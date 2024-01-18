using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Entities
{
    [Table("Roles")]
    public class RolesEntity
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
