using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class WareHouseVM
    {
        public int WareHouseId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int Status { get; set; }
    }
    
    public class InsertUpdateWareHouse
    {
        public int WareHouseId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
    }
}
