using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Utilities.Enum;
using Electronic_WMS.Utilities.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.Service
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _iUsersRepository;
        private readonly IRolesRepository _iRolesRepository;
        public UsersService(IUsersRepository iUsersRepository, IRolesRepository iRolesRepository)
        {
            _iUsersRepository = iUsersRepository;
            _iRolesRepository = iRolesRepository;
        }
        public ResponseModel Delete(int id)
        {
            var user = _iUsersRepository.GetById(id);
            if (user == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "User Not Found!"
                };
            }
            user.Status = (int)CommonStatus.IsDelete;
            var status = _iUsersRepository.Update(user);
            if (status == 0)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error!"
                };
            }
            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Delete Successfully!"
            };
        }

        public UsersVM GetById(int id)
        {
            var user = _iUsersRepository.GetById(id);
            var userDetail = new UsersVM
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = user.Password,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                Image = user.Image,
                Status = user.Status,
                CreatedDate = user.CreatedDate,
                CreatedBy = user.CreatedBy,
                UpdatedDate = user.UpdatedDate,
                UpdatedBy = user.UpdatedBy,
                RoleId = user.RoleId,
                RoleName = _iRolesRepository.GetById(user.UserId).RoleName,
            };
            return userDetail;
        }

        public GetListUser GetList(SearchVM search)
        {
            var list = from user in _iUsersRepository.GetList()
                       select new UsersVM
                       {
                           UserId = user.UserId,
                           UserName = user.UserName,
                           Password = user.Password,
                           FullName = user.FullName,
                           Email = user.Email,
                           Address = user.Address,
                           Phone = user.Phone,
                           Image = user.Image,
                           Status = user.Status,
                           CreatedDate = user.CreatedDate,
                           CreatedBy = user.CreatedBy,
                           UpdatedDate = user.UpdatedDate,
                           UpdatedBy = user.UpdatedBy,
                           RoleId = user.RoleId,
                           RoleName = _iRolesRepository.GetById(user.UserId).RoleName,
                       };
            var total = list.Count();
            if (search.TextSearch == null)
            {
                list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            else
            {
                list = list.Where(x => x.FullName.ToLower().Contains(search.TextSearch.ToLower())).Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            return new GetListUser { ListUser = list, Total = total};
        }

        public IEnumerable<SupplierOrShopCombobox> GetListSupplierOrShop(int rolesId)
        {
            var list = from user in _iUsersRepository.GetList()
                       where user.RoleId == rolesId
                       select new SupplierOrShopCombobox
                       {
                           UserId = user.UserId,
                           FullName = user.FullName,
                       };
            return list;
        }

        public ResponseModel Insert(InsertUpdateUsers user)
        {
            // Check UserName in database
            var checkUserName = _iUsersRepository.GetByUserNameOrEmail(user.UserName);
            if (checkUserName != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "UserName already exists!"
                };
            }

            // Check Email in database
            var checkEmail = _iUsersRepository.GetByUserNameOrEmail(user.Email);
            if (checkEmail != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Email already exists!"
                };
            }

            // Insert User 
            var userEntity = new UsersEntity
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = XString.ToMD5(user.Password),
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                Status = (int)CommonStatus.IsActive,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                RoleId = user.RoleId,
            };
            String avatar = XString.ToAscii(user.FullName);
            if (user.FileImage != null)
            {
                String fileName = avatar + user.FileImage.FileName.Substring(user.FileImage.FileName.LastIndexOf('.'));
                var path = Path.Combine("E:\\Nam2ki2\\Nam4Ki2\\DoAn_TotNghiep\\Electronic_WMS_Angular\\src\\assets\\img\\user", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    user.FileImage.CopyTo(stream);
                }
                userEntity.Image = fileName;
            }
            else
            {
                userEntity.Image = null;
            }
            var status = _iUsersRepository.Insert(userEntity);
            if (status == 0)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error!"
                };
            }
            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Create Successfully!"
            };
        }

        public ResponseModel Update(InsertUpdateUsers user)
        {
            var uDetail = _iUsersRepository.GetById(user.UserId);
            if (uDetail == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "User does not exists!"
                };
            }

            // Check UserName in database
            var checkUserName = _iUsersRepository.GetByUserNameOrEmail(user.UserName);
            if (checkUserName != null && checkUserName.UserId != user.UserId)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "UserName already exists!"
                };
            }

            // Check Email in database
            var checkEmail = _iUsersRepository.GetByUserNameOrEmail(user.Email);
            if (checkEmail != null && checkEmail.Email != user.Email)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Email already exists!"
                };
            }

            // Update User 
            uDetail.UserName = user.UserName;
            uDetail.Password = XString.ToMD5(user.Password);
            uDetail.FullName = user.FullName;
            uDetail.Email = user.Email;
            uDetail.Address = user.Address;
            uDetail.Phone = user.Phone;
            uDetail.UpdatedDate = DateTime.Now;
            uDetail.UpdatedBy = 1;
            uDetail.RoleId = user.RoleId;
            String avatar = XString.ToAscii(user.FullName);
            if (user.FileImage != null)
            {
                String fileName = avatar + user.FileImage.FileName.Substring(user.FileImage.FileName.LastIndexOf('.'));
                var path = Path.Combine("E:\\Nam2ki2\\Nam4Ki2\\DoAn_TotNghiep\\Electronic_WMS_Angular\\src\\assets\\img\\user", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    user.FileImage.CopyTo(stream);
                }
                uDetail.Image = fileName;
            }
            else
            {
                uDetail.Image = null;
            }
            var status = _iUsersRepository.Update(uDetail);
            if (status == 0)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error!"
                };
            }
            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Edit Successfully!"
            };
        }
    }
}
