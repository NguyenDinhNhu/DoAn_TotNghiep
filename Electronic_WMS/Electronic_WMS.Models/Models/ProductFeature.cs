using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class GetListProductFeature
    {
        public IEnumerable<ProductFeatureVM> ListProductFeature { get; set; }
        public int Total { get; set; }
    }
    public class ProductFeature
    {
        public int ProductFeatureId { get; set; }
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
        public string Value { get; set; }
    }

    public class ProductFeatureVM
    {
        public int ProductFeatureId { get; set; }
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
        public string ProductName { get; set; }
        public string FeatureName { get; set; }
        public string Value { get; set; }
    }
}
