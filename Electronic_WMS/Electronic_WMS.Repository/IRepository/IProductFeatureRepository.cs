using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IProductFeatureRepository
    {
        public IEnumerable<ProductFeatureEntity> GetList();
        public IEnumerable<ProductFeatureEntity> GetListByProductId(int productId);
        public ProductFeatureEntity GetById(int id);
        public int Insert(ProductFeatureEntity pf);
        public int Update(ProductFeatureEntity pf);
        public int Delete(ProductFeatureEntity pf);
    }
}
