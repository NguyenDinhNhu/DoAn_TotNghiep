using Electronic_WMS.Models.Entities;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Utilities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.Repository
{
    public class ProductFeatureRepository : IProductFeatureRepository
    {
        private readonly WMSDbContext _db;
        public ProductFeatureRepository(WMSDbContext db)
        {
            _db = db;
        }
        public int Delete(ProductFeatureEntity pf)
        {
            _db.ProductFeatureEntities.Remove(pf);
            return _db.SaveChanges();
        }

        public ProductFeatureEntity GetById(int id)
        {
            return _db.ProductFeatureEntities.Find(id);
        }

        public IEnumerable<ProductFeatureEntity> GetList()
        {
            return _db.ProductFeatureEntities.ToList();
        }

        public IEnumerable<ProductFeatureEntity> GetListByProductId(int productId)
        {
            return _db.ProductFeatureEntities.Where(x => x.ProductId == productId).ToList();
        }

        public int Insert(ProductFeatureEntity pf)
        {
            _db.ProductFeatureEntities.Add(pf);
            return _db.SaveChanges();
        }

        public int Update(ProductFeatureEntity pf)
        {
            _db.Entry(pf).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
