using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface ISerialNumberService
    {
        public GetListSerialByProductId GetListByProductId(SearchSeriVM search);
        public IEnumerable<ListSerialCombobox> GetListSerialCombobox(SearchListSerialCombobox search);
        public ResponseModel UpdateLocation(List<UpdateLocation> listSeri);
    }
}
