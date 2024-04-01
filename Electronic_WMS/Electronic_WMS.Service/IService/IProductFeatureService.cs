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
        public GetListProductFeature GetList(SearchVM search);
        public IEnumerable<ProductFeatureVM> GetListByProductId(int productId);
        public ProductFeatureVM GetById(int id);
        public ResponseModel Insert(List<ProductFeature> pf);
        public ResponseModel Update(ProductFeature pf);
        public ResponseModel Delete(int id);
    }
}
