using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Entities
{
    [Table("SerialNumber")]
    public class SerialNumberEntity
    {
        [Key]
        public int SerialId { get; set; }
        public string SerialNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public int ProductId { get; set; }
    }
}
