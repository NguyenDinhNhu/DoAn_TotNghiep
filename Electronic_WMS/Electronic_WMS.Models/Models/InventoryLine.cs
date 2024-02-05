﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class InventoryLine
    {
        public int InventoryLineId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
    }
}