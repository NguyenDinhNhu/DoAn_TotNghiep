using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IProductFeatureService
    {
        public IEnumerable<ProductFeatureVM> GetList(SearchVM search);
        public IEnumerable<ProductFeatureVM> GetListByProductId(int productId);
        public ProductFeatureVM GetById(int id);
        public ResponseModel Insert(ProductFeature prod);
        public ResponseModel Update(ProductFeature prod);
        public ResponseModel Delete(int id);
    }
}
