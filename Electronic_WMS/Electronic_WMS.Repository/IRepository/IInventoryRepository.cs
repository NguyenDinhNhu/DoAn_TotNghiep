
using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IInventoryRepository
    {
        public IEnumerable<InventoryEntity> GetList();
        public InventoryEntity GetById(int id);
        public int Insert(InventoryEntity inv);
        public int Update(InventoryEntity inv);
        public int Delete(InventoryEntity inv);
    }
}
