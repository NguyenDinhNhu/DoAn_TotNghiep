using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class InventorySearch : SearchVM
    {
        public int Type { get; set; }
        public int Status { get; set; }
    }
    public class GetListInventory
    {
        public IEnumerable<InventoryVM> ListInventory { get; set; }
        public int Total { get; set; }
    }

    public class ChangeStatusInventory
    {
        public int InventoryId { get; set; }
        public int Status { get; set; }
    }
    public class InventoryVM
    {
        public int InventoryId { get; set; }
        public int SourceLocation { get; set; }
        public string WareHouseName { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int Quantity { get; set; }
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
        public List<InsertOrUpdateInventoryLine>? ListInventoryLine { get; set; }
    }

    public class InventoryDetail
    {
        public int InventoryId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public string WareHouseName { get; set; }
        public int WareHouseId { get; set; } // vị trí đích
        public int SourceLocation { get; set; } // vị trí nguồn
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public List<InventoryLineVM> ListInventoryLine { get; set; }
    }

    public class DashboardVM
    {
        public int CountReciptReady { get; set; }
        public int CountDeliveryReady { get; set; }
        public int CountProductOutOfStock { get; set; }
    }

    public class MonthlyRevenue
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenueImport { get; set; }
        public decimal TotalRevenueExport { get; set; }
    }
}
