using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IUsersRepository
    {
        public IEnumerable<UsersEntity> GetList();
        public int Insert(UsersEntity user);
        public int Update(UsersEntity user);
        public int Delete(UsersEntity user);
        public UsersEntity GetById(int id);
        public UsersEntity GetByUserNameOrEmail(string value);
    }
}
