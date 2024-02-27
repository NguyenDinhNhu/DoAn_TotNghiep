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
    public class RolesRepository : IRolesRepository
    {
        private readonly WMSDbContext _db;
        public RolesRepository(WMSDbContext db)
        {
            _db = db;
        }
        public int Delete(RolesEntity role)
        {
            _db.RolesEntities.Remove(role);
            return _db.SaveChanges();
        }

        public RolesEntity GetById(int id)
        {
            return _db.RolesEntities.Find(id);
        }

        public RolesEntity GetByName(string name)
        {
            return _db.RolesEntities.Where(x => x.Status == (int)CommonStatus.IsActive && x.RoleName.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<RolesEntity> GetList()
        {
            return _db.RolesEntities.Where(x => x.Status == (int)CommonStatus.IsActive).ToList();
        }

        public int Insert(RolesEntity role)
        {
            _db.RolesEntities.Add(role);
            return _db.SaveChanges();
        }

        public int Update(RolesEntity role)
        {
            _db.Entry(role).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
