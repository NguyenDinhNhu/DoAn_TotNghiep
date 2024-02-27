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
    public class ProductRepository : IProductRepository
    {
        private readonly WMSDbContext _db;
        public ProductRepository(WMSDbContext db)
        {
            _db = db;
        }
        public int Delete(ProductEntity prod)
        {
            _db.ProductEntities.Remove(prod);
            return _db.SaveChanges();
        }

        public ProductEntity GetById(int id)
        {
            return _db.ProductEntities.Find(id);
        }

        public ProductEntity GetByUserName(string name)
        {
            return _db.ProductEntities.Where(x => x.Status == (int)CommonStatus.IsActive && (x.ProductName.ToLower() == name.ToLower())).FirstOrDefault();
        }

        public IEnumerable<ProductEntity> GetList()
        {
            return _db.ProductEntities.Where(x => x.Status == (int)CommonStatus.IsActive).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public int Insert(ProductEntity prod)
        {
            _db.ProductEntities.Add(prod);
            return _db.SaveChanges();
        }

        public int Update(ProductEntity prod)
        {
            _db.Entry(prod).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
