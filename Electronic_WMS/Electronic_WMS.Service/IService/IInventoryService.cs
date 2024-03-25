using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IInventoryService
    {
        public IEnumerable<InventoryVM> GetListByType(InventorySearch search);
        public InventoryDetail GetById(int id);
        public ResponseModel Insert(InsertOrUpdateInventory inv);
        public ResponseModel Update(InsertOrUpdateInventory inv);
        public ResponseModel Delete(int id);
    }
}
