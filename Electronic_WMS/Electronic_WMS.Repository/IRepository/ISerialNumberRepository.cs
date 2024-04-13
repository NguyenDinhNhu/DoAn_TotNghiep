using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface ISerialNumberRepository
    {
        public IEnumerable<SerialNumberEntity> GetListByProductId(int productId);
        public IEnumerable<SerialNumberEntity> GetListByInventoryLineId(int inventoryLineId);
        public SerialNumberEntity GetById(int id);
        public SerialNumberEntity GetByLocation(string location);
        public int Insert(SerialNumberEntity seri);
        public int Update(SerialNumberEntity seri);
        public int Delete(SerialNumberEntity seri);
    }
}
