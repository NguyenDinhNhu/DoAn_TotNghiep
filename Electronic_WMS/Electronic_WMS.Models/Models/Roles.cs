using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class GetListRole 
    {
        public IEnumerable<Roles> ListRole { get; set; }
        public int Total { get; set; }
    }
    public class Roles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int Status { get; set; }
    }
    public class RolesCombobox
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
