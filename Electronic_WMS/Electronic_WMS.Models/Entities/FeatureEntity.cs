using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Entities
{
    [Table("Feature")]
    public class FeatureEntity
    {
        [Key]
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
    }
}
