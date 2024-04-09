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
    public class WareHouseService : IWareHouseService
    {
        private readonly IWareHouseRepository _iWareHouseRepository;
        public WareHouseService(IWareHouseRepository iWareHouseRepository)
        {
            _iWareHouseRepository = iWareHouseRepository;
        }

        public ResponseModel Delete(int id)
        {
            var wh = _iWareHouseRepository.GetById(id);
            if (wh == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "WareHouse Not Found!"
                };
            }
            wh.Status = (int)CommonStatus.IsDelete;
            var status = _iWareHouseRepository.Update(wh);
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

        public WareHouseVM GetById(int id)
        {
            var wh = _iWareHouseRepository.GetById(id);
            var whDetail = new WareHouseVM
            {
                WareHouseId = wh.WareHouseId,
                Name = wh.Name,
                Address = wh.Address,
                CreatedDate = wh.CreatedDate,
                UpdatedDate = wh.UpdatedDate,
                CreatedBy = wh.CreatedBy,
                UpdatedBy = wh.UpdatedBy,
                Status = wh.Status
            };
            return whDetail;
        }

        public GetListWareHouse GetList(SearchVM search)
        {
            var list = from wh in _iWareHouseRepository.GetList()
                       select new WareHouseVM
                       {
                           WareHouseId = wh.WareHouseId,
                           Name = wh.Name,
                           Address = wh.Address,
                           CreatedDate = wh.CreatedDate,
                           UpdatedDate = wh.UpdatedDate,
                           CreatedBy = wh.CreatedBy,
                           UpdatedBy = wh.UpdatedBy,
                           Status = wh.Status
                       };
            var total = list.Count();
            if (search.TextSearch == null)
            {
                list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            else
            {
                list = list.Where(x => x.Name.ToLower().Contains(search.TextSearch.ToLower())).Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            return new GetListWareHouse { ListWareHouse = list, Total = total};
        }

        public IEnumerable<WareHouseCombobox> GetListCombobox()
        {
            var list = from wh in _iWareHouseRepository.GetList()
                       select new WareHouseCombobox
                       {
                           WareHouseId = wh.WareHouseId,
                           Name = wh.Name
                       };
            return list;
        }

        public ResponseModel Insert(InsertUpdateWareHouse wh)
        {
            // Check WareHouseName in database
            var checkWHName = _iWareHouseRepository.GetByName(wh.Name);
            if (checkWHName != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Warehouse already exists!"
                };
            }

            // Insert WareHouse 
            var whEntity = new WareHouseEntity
            {
                WareHouseId = wh.WareHouseId,
                Name = wh.Name,
                Address = wh.Address,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                Status = (int)CommonStatus.IsActive
            };

            var status = _iWareHouseRepository.Insert(whEntity);
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

        public ResponseModel Update(InsertUpdateWareHouse wh)
        {
            var whDetail = _iWareHouseRepository.GetById(wh.WareHouseId);
            if (whDetail == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "WareHouse does not exists!"
                };
            }
            // Check WareHouseName in database
            var checkWHName = _iWareHouseRepository.GetByName(wh.Name);
            if (checkWHName != null && checkWHName.WareHouseId != wh.WareHouseId)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "WareHouse already exists!"
                };
            }

            // Update WareHouse 
            whDetail.Name = wh.Name;
            whDetail.Address = wh.Address;
            whDetail.UpdatedDate = DateTime.Now;
            whDetail.UpdatedBy = 1;

            var status = _iWareHouseRepository.Update(whDetail);
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
