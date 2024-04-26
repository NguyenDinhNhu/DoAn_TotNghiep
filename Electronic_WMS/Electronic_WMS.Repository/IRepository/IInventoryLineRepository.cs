using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IInventoryLineRepository
    {
        public IEnumerable<InventoryLineEntity> GetListByInventoryId(int invId);
        public IEnumerable<InventoryLineEntity> GetList();
        public InventoryLineEntity GetById(int id);
        public int Insert(InventoryLineEntity inv);
        public int Update(InventoryLineEntity inv);
        public int Delete(InventoryLineEntity inv);
    }
}
