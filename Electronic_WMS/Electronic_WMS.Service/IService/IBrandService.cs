using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IBrandService
    {
        public IEnumerable<BrandVM> GetList(SearchVM search);
        public IEnumerable<BrandCombobox> GetListCombobox();
        public Brand GetById(int id);
        public ResponseModel Insert(Brand brand);
        public ResponseModel Update(Brand brand);
        public ResponseModel Delete(int id);
    }
}
