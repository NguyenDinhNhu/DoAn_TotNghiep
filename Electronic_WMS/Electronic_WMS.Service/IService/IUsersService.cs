using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IUsersService
    {
        public IEnumerable<UsersVM> GetList(SearchVM search);
        public IEnumerable<SupplierOrShopCombobox> GetListSupplierOrShop(int rolesId);
        public UsersVM GetById(int id);
        public ResponseModel Insert(InsertUpdateUsers user);
        public ResponseModel Update(InsertUpdateUsers user);
        public ResponseModel Delete(int id);
    }
}
