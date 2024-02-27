using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Utilities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Electronic_WMS.Repository.Repository
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly WMSDbContext _db;
        public FeatureRepository(WMSDbContext db)
        {
            _db = db;
        }
        public int Delete(FeatureEntity feature)
        {
            _db.FeatureEntities.Remove(feature);
            return _db.SaveChanges();
        }

        public FeatureEntity GetById(int id)
        {
            return _db.FeatureEntities.Find(id);
        }

        public FeatureEntity GetByName(string name)
        {
            return _db.FeatureEntities.Where(f => f.Status == (int)CommonStatus.IsActive && f.FeatureName.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<FeatureEntity> GetList()
        {
            return _db.FeatureEntities.Where(f => f.Status == (int)CommonStatus.IsActive).ToList();
        }

        public int Insert(FeatureEntity feature)
        {
            _db.FeatureEntities.Add(feature);
            return _db.SaveChanges();
        }

        public int Update(FeatureEntity feature)
        {
            _db.Entry(feature).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
