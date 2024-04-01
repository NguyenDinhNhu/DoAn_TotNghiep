using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class GetListFeature
    {
        public IEnumerable<Feature> ListFeature { get; set; }
        public int Total { get; set; }
    }
    public class Feature
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int Status { get; set; }
    }

    public class FeatureCombobox
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
    }
}
