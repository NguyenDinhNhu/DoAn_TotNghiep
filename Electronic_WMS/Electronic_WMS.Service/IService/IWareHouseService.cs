using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IWareHouseService
    {
        public IEnumerable<WareHouseVM> GetList(SearchVM search);
        public IEnumerable<WareHouseCombobox> GetListCombobox();
        public WareHouseVM GetById(int id);
        public ResponseModel Insert(InsertUpdateWareHouse wh);
        public ResponseModel Update(InsertUpdateWareHouse wh);
        public ResponseModel Delete(int id);
    }
}
