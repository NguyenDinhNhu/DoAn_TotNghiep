using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Entities
{
    [Table("ProductFeature")]
    public class ProductFeatureEntity
    {
        [Key]
        public int ProductFeatureId { get; set; }
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
    }
}
