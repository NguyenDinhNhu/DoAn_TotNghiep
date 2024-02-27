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
    public class UsersRepository : IUsersRepository
    {
        private readonly WMSDbContext _db;
        public UsersRepository(WMSDbContext db)
        {
            _db = db;
        }
        public int Delete(UsersEntity user)
        {
            _db.UsersEntities.Remove(user);
            return _db.SaveChanges();
        }

        public UsersEntity GetById(int id)
        {
            return _db.UsersEntities.Find(id);
        }

        public UsersEntity GetByUserNameOrEmail(string value)
        {
            return _db.UsersEntities.Where(x => x.Status == (int)CommonStatus.IsActive && (x.UserName.ToLower() == value.ToLower() || x.Email == value)).FirstOrDefault();
        }

        public IEnumerable<UsersEntity> GetList()
        {
            return _db.UsersEntities.Where(x => x.Status == (int)CommonStatus.IsActive).ToList();
        }

        public int Insert(UsersEntity user)
        {
            _db.UsersEntities.Add(user);
            return _db.SaveChanges();
        }

        public int Update(UsersEntity user)
        {
            _db.Entry(user).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
