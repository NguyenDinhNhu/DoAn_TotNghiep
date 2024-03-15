using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class InventoryVM
    {
        public int InventoryId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }

    public class InsertOrUpdateInventory
    {
        public int InventoryId { get; set; }
        public int UserId { get; set; }
        public int WareHouseId { get; set; } // vị trí đích
        public int SourceLocation { get; set; } // vị trí nguồn
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
    }

    public class InventoryDetail
    {
        public int InventoryId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public int WareHouseId { get; set; } // vị trí đích
        public int SourceLocation { get; set; } // vị trí nguồn
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public List<InventoryLine> InventoryLines { get; set; }
    }
}
