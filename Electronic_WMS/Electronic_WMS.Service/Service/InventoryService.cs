using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Utilities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _iInventoryRepository;
        private readonly IInventoryLineRepository _iInventoryLineRepository;
        public InventoryService(IInventoryRepository iInventoryRepository, IInventoryLineRepository iInventoryLineRepository)
        {
            _iInventoryRepository = iInventoryRepository;
            _iInventoryLineRepository = iInventoryLineRepository;
        }
        public ResponseModel Delete(int id)
        {
            var user = _iInventoryRepository.GetById(id);
            if (user == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Product Not Found!"
                };
            }
            user.Status = (int)InventoryStatus.IsDelete;
            var status = _iInventoryRepository.Update(user);
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

        public ProductVM GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InventoryVM> GetList(SearchVM search)
        {
            throw new NotImplementedException();
        }

        public ResponseModel Insert(InsertOrUpdateInventory inv)
        {
            throw new NotImplementedException();
        }

        public ResponseModel Update(InsertOrUpdateInventory inv)
        {
            throw new NotImplementedException();
        }
    }
}
