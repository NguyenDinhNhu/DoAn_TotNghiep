using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class SerialNumberModel
    {
        public int SerialId { get; set; }
        public string SerialNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public int ProductId { get; set; }
        public string? Location { get; set; }
        public int WareHouseId { get; set; }
        public int InventoryLineId { get; set; }
    }
}
