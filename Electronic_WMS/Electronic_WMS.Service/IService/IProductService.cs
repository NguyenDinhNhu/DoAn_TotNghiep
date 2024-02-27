using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IProductService
    {
        public IEnumerable<ProductVM> GetList(SearchVM search);
        public ProductVM GetById(int id);
        public ResponseModel Insert(InsertOrUpdateProduct prod);
        public ResponseModel Update(InsertOrUpdateProduct prod);
        public ResponseModel Delete(int id);
    }
}
