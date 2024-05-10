using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class GetListUser
    {
        public IEnumerable<UsersVM> ListUser { get; set; }
        public int Total { get; set; }
    }
    public class UsersVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class SupplierOrShopCombobox
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
    public class InsertUpdateUsers
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public int RoleId { get; set; }
        public IFormFile FileImage { get; set; }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }

    public class UserToken
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string RoleName { get; set; }
    }
}
