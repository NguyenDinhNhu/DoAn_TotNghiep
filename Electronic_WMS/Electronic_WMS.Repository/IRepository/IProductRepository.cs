using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IProductRepository
    {
        public IEnumerable<ProductEntity> GetList();
        public int Insert(ProductEntity prod);
        public int Update(ProductEntity prod);
        public int Delete(ProductEntity prod);
        public ProductEntity GetById(int id);
        public ProductEntity GetByUserName(string name);
    }
}
