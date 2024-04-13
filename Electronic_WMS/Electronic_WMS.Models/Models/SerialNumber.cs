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

    public class GetListSerialByProductId
    {
        public IEnumerable<SerialNumberVM> ListSerial { get; set; }
        public int Total { get; set; }
    }

    public class SerialNumberVM
    {
        public int SerialId { get; set; }
        public string SerialNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Location { get; set; }
        public int WareHouseId { get; set; }
        public string WareHouseName { get; set; }
    }
        
    public class SearchSeriVM : SearchVM
    {
        public int ProductId { get; set;}
    }

    public class UpdateLocation
    {
        public int SerialId { get; set;}
        public string Location { get; set; } 
    }
}
