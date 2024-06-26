﻿using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IUsersService
    {
        public GetListUser GetList(SearchVM search);
        public IEnumerable<SupplierOrShopCombobox> GetListSupplierOrShop(int rolesId);
        public UsersVM GetById(int id);
        public ResponseModel Insert(InsertUpdateUsers user, UserToken userToken);
        public ResponseModel Update(InsertUpdateUsers user, UserToken userToken);
        public ResponseModel Delete(int id);
    }
}
