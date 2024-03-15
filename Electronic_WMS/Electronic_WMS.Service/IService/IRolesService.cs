using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IRolesService
    {
        public IEnumerable<Roles> GetList(SearchVM search);
        public IEnumerable<RolesCombobox> GetListCombobox();
        public Roles GetById(int id);
        public ResponseModel Insert(Roles role);
        public ResponseModel Update(Roles role);
        public ResponseModel Delete(int id);
    }
}
