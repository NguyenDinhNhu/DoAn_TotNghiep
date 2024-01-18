using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Entities
{
    [Table("InventoryLine")]
    public class InventoryLineEntity
    {
        [Key]
        public int InventoryLineId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
    }
}
