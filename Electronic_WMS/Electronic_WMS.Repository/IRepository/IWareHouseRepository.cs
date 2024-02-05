using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IWareHouseRepository
    {
        public IEnumerable<WareHouseEntity> GetList();
        public int Insert(WareHouseEntity wh);
        public int Update(WareHouseEntity wh);
        public int Delete(WareHouseEntity wh);
        public WareHouseEntity GetById(int id);
        public WareHouseEntity GetByName(string name);
    }
}
