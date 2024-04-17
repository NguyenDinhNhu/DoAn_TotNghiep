using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Utilities.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.Service
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _iRolesRepository;
        public RolesService(IRolesRepository iRolesRepository)
        {
            _iRolesRepository = iRolesRepository;
        }

        public ResponseModel Delete(int id)
        {
            var feature = _iRolesRepository.GetById(id);
            if (feature == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Role Not Found!"
                };
            }
            feature.Status = (int)CommonStatus.IsDelete;
            var status = _iRolesRepository.Update(feature);
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

        public Roles GetById(int id)
        {
            var role = _iRolesRepository.GetById(id);
            var roleDetail = new Roles
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Status = role.Status
            };
            return roleDetail;
        }

        public GetListRole GetList(SearchVM search)
        {
            var list = from r in _iRolesRepository.GetList()
                       select new Roles
                       {
                           RoleId = r.RoleId,
                           RoleName = r.RoleName,
                           Status = r.Status
                       };
            if (!string.IsNullOrEmpty(search.TextSearch))
            {
                list = list.Where(x => x.RoleName.ToLower().Contains(search.TextSearch.ToLower()));
            }
            var total = list.Count();
            list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            return new GetListRole { ListRole = list, Total = total};
        }

        public IEnumerable<RolesCombobox> GetListCombobox()
        {
            var list = from r in _iRolesRepository.GetList()
                       select new RolesCombobox
                       {
                           RoleId = r.RoleId,
                           RoleName = r.RoleName,
                       };
            return list;
        }

        public ResponseModel Insert(Roles role)
        {
            // Check RoleName in database
            var checkRoleName = _iRolesRepository.GetByName(role.RoleName);
            if (checkRoleName != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Role already exists!"
                };
            }

            // Insert Role 
            var roleEntity = new RolesEntity
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Status = (int)CommonStatus.IsActive,
            };

            var status = _iRolesRepository.Insert(roleEntity);
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

        public ResponseModel Update(Roles role)
        {
            var roleDetail = _iRolesRepository.GetById(role.RoleId);
            if (roleDetail == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Role does not exists!"
                };
            }
            // Check RoleName in database
            var checkRoleName = _iRolesRepository.GetByName(role.RoleName);
            if (checkRoleName != null && checkRoleName.RoleId != role.RoleId)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Role already exists!"
                };
            }

            // Update Role 
            roleDetail.RoleName = role.RoleName;

            var status = _iRolesRepository.Update(roleDetail);
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
                StatusMessage = "Update Successfully!"
            };
        }
    }
}
