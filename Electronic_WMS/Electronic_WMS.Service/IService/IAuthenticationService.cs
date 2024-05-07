using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IAuthenticationService
    {
        public string Login(LoginModel login);
        public UserToken GetUserToken();
    }
}
