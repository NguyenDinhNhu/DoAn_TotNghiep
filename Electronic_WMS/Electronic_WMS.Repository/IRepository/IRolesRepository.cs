using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IRolesRepository
    {
        public IEnumerable<RolesEntity> GetList();
        public int Insert(RolesEntity role);
        public int Update(RolesEntity role);
        public int Delete(RolesEntity role);
        public RolesEntity GetById(int id);
        public RolesEntity GetByName(string name);
    }
}
