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
    public class SerialNumberRepository : ISerialNumberRepository
    {
        private readonly WMSDbContext _db;
        public SerialNumberRepository(WMSDbContext db)
        {
            _db = db;
        }
        public int Delete(SerialNumberEntity seri)
        {
            _db.SerialNumberEntities.Remove(seri);
            return _db.SaveChanges();
        }

        public IEnumerable<SerialNumberEntity> GetListByProductId(int productId)
        {
            return _db.SerialNumberEntities.Where(x => x.Status == (int)CommonStatus.IsActive && x.ProductId != productId).ToList();
        }

        public int Insert(SerialNumberEntity seri)
        {
            _db.SerialNumberEntities.Add(seri);
            return _db.SaveChanges();
        }

        public int Update(SerialNumberEntity seri)
        {
            _db.Entry(seri).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
