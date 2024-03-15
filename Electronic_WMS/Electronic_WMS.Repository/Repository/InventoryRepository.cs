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

namespace Electronic_WMS.Repository.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly WMSDbContext _db;
        public InventoryRepository(WMSDbContext db)
        {
            _db = db;
        }
        public int Delete(InventoryEntity inv)
        {
            _db.InventoryEntities.Remove(inv);
            return _db.SaveChanges();
        }

        public InventoryEntity GetById(int id)
        {
            return _db.InventoryEntities.Find(id);
        }

        public IEnumerable<InventoryEntity> GetList()
        {
            return _db.InventoryEntities.Where(x => x.Status != (int)InventoryStatus.IsDelete).OrderBy(x => x.CreatedDate).ToList();
        }

        public int Insert(InventoryEntity inv)
        {
            _db.InventoryEntities.Add(inv);
            return _db.SaveChanges();
        }

        public int Update(InventoryEntity inv)
        {
            _db.Entry(inv).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
