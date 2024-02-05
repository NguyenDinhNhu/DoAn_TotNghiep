using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class ProductFeature
    {
        public int ProductFeatureId { get; set; }
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
    }
}
