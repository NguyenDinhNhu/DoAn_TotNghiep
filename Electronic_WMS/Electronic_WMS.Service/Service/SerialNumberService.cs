using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.Service
{
    public class SerialNumberService : ISerialNumberService
    {
        private readonly ISerialNumberRepository _iSerialNumberRepository;
        private readonly IWareHouseRepository _iWareHouseRepository;
        public SerialNumberService(ISerialNumberRepository iSerialNumberRepository, IWareHouseRepository iWareHouseRepository)
        {
            _iSerialNumberRepository = iSerialNumberRepository;
            _iWareHouseRepository = iWareHouseRepository;
        }
        public GetListSerialByProductId GetListByProductId(SearchSeriVM search)
        {
            var listSeri = from s in _iSerialNumberRepository.GetListByProductId(search.ProductId)
                            select new SerialNumberVM
                            {
                                SerialId = s.SerialId,
                                SerialNumber = s.SerialNumber,
                                CreatedDate = s.CreatedDate,
                                Location = s.Location,
                                WareHouseId = s.WareHouseId,
                                WareHouseName = _iWareHouseRepository.GetById(s.WareHouseId).Name,
                            };
            if (!string.IsNullOrEmpty(search.TextSearch))
            {
                listSeri = listSeri.Where(s => s.SerialNumber.ToLower().Contains(search.TextSearch.ToLower()));
            }
            var total = listSeri.Count();
            listSeri = listSeri.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            return new GetListSerialByProductId { ListSerial = listSeri, Total = total };
        }

        public ResponseModel UpdateLocation(UpdateLocation location)
        {
            var seri = _iSerialNumberRepository.GetById(location.SerialId);
            if (seri == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Serial does not exists!"
                };
            }
            // Check Location in database
            var checkLocation = _iSerialNumberRepository.GetByLocation(location.Location);
            if (checkLocation != null && checkLocation.SerialId != location.SerialId)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "This location cannot be selected!"
                };
            }

            // Update Brand 
            seri.Location = location.Location;

            var status = _iSerialNumberRepository.Update(seri);
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
