using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class GetListBrand
    {
        public IEnumerable<BrandVM> ListBrand { get; set; }
        public int Total { get; set; }
    }
    public class BrandVM
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string ParentName { get; set; }
        public int Status { get; set; }
    }

    public class BrandCombobox
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }

    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int ParentId { get; set; }
        public int Status { get; set; }
    }
}
