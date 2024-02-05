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
    public class BrandRepository : IBrandRepository
    {
        private readonly WMSDbContext _db;
        public BrandRepository(WMSDbContext db)
        {
            _db = db;
        }

        public int Delete(BrandEntity brand)
        {
            _db.BrandEntities.Remove(brand);
            return _db.SaveChanges();
        }

        public BrandEntity GetById(int id)
        {
            return _db.BrandEntities.Find(id);
        }

        public BrandEntity GetByName(string name)
        {
            return _db.BrandEntities.Where(b => b.Status != (int)CommonStatus.IsDelete && b.BrandName.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<BrandEntity> GetList()
        {
            return _db.BrandEntities.Where(b => b.Status != (int)CommonStatus.IsDelete).ToList();
        }

        public string GetParentName(int parentId)
        {
            return _db.BrandEntities.Where(b => b.BrandId == parentId).FirstOrDefault().BrandName;
        }

        public int Insert(BrandEntity brand)
        {
            _db.BrandEntities.Add(brand);
            return _db.SaveChanges();
        }

        public int Update(BrandEntity brand)
        {
            _db.Entry(brand).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
