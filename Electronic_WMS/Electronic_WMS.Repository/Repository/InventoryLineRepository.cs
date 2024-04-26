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
    public class InventoryLineRepository : IInventoryLineRepository
    {
        private readonly WMSDbContext _db;
        public InventoryLineRepository(WMSDbContext db)
        {
            _db = db;
        }
        public int Delete(InventoryLineEntity inv)
        {
            _db.InventoryLineEntities.Remove(inv);
            return _db.SaveChanges();
        }

        public InventoryLineEntity GetById(int id)
        {
            return _db.InventoryLineEntities.Find(id);
        }

        public IEnumerable<InventoryLineEntity> GetListByInventoryId(int invId)
        {
            return _db.InventoryLineEntities.Where(x => x.InventoryId == invId).ToList();
        }
        public IEnumerable<InventoryLineEntity> GetList()
        {
            return _db.InventoryLineEntities.ToList();
        }

        public int Insert(InventoryLineEntity inv)
        {
            _db.InventoryLineEntities.Add(inv);
            return _db.SaveChanges();
        }

        public int Update(InventoryLineEntity inv)
        {
            _db.Entry(inv).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
