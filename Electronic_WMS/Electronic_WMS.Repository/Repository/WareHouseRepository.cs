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
    public class WareHouseRepository : IWareHouseRepository
    {
        private readonly WMSDbContext _db;
        public WareHouseRepository(WMSDbContext db)
        {
            _db = db;
        }

        public int Delete(WareHouseEntity wh)
        {
            _db.WareHouseEntities.Remove(wh);
            return _db.SaveChanges();
        }

        public WareHouseEntity GetById(int id)
        {
            return _db.WareHouseEntities.Find(id);
        }

        public WareHouseEntity GetByName(string name)
        {
            return _db.WareHouseEntities.Where(x => x.Status != (int)CommonStatus.IsDelete && x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<WareHouseEntity> GetList()
        {
            return _db.WareHouseEntities.Where(x => x.Status != (int)CommonStatus.IsDelete).ToList();
        }

        public int Insert(WareHouseEntity wh)
        {
            _db.WareHouseEntities.Add(wh);
            return _db.SaveChanges();
        }

        public int Update(WareHouseEntity wh)
        {
            _db.Entry(wh).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
