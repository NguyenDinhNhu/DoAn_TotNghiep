using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Entities
{
    [Table("Category")]
    public class CategoryEntity
    {
        [Key]
        public int CateId { get; set; }
        public string CateName { get; set; }
        public int ParentId { get; set; }
        public int Status { get; set; }
    }
}
