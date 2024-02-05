using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class CategoryVM
    {
        public int CateId { get; set; }
        public string CateName { get; set; }
        public string ParentName { get; set; }
        public int Status { get; set; }
    }
    public class Category
    {
        public int CateId { get; set; }
        public string CateName { get; set; }
        public int ParentId { get; set; }
        public int Status { get; set; }
    }
}
