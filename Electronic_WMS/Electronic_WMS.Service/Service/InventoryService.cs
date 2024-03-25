using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Utilities.Enum;
using Electronic_WMS.Utilities.Library;
using Newtonsoft.Json;
using System;
using System.Collections;
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
        private readonly IUsersRepository _iUserRepository;
        private readonly ISerialNumberRepository _iSerialNumberRepository;
        private readonly IProductRepository _iProductRepository;
        private readonly IWareHouseRepository _iWareHouseRepository;
        public InventoryService(IInventoryRepository iInventoryRepository, 
            IInventoryLineRepository iInventoryLineRepository, IUsersRepository iUserRepository,
            ISerialNumberRepository iSerialNumberRepository, IProductRepository iProductRepository,
            IWareHouseRepository iWareHouseRepository)
        {
            _iInventoryRepository = iInventoryRepository;
            _iInventoryLineRepository = iInventoryLineRepository;
            _iUserRepository = iUserRepository;
            _iSerialNumberRepository = iSerialNumberRepository;
            _iProductRepository = iProductRepository;
            _iWareHouseRepository = iWareHouseRepository;
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

        public InventoryDetail GetById(int id)
        {
            var inv = _iInventoryRepository.GetById(id);
            var invDetail = new InventoryDetail
            {
                InventoryId = inv.InventoryId,
                UserId = inv.UserId,
                UserName = _iUserRepository.GetById(inv.UserId).FullName,
                SourceLocation = inv.SourceLocation,
                CustomerName = _iUserRepository.GetById(inv.SourceLocation).FullName,
                WareHouseId = inv.WareHouseId,
                WareHouseName = _iWareHouseRepository.GetById(inv.WareHouseId).Name,
                CreatedDate = inv.CreatedDate,
                UpdatedDate = inv.UpdatedDate,
                UpdatedBy = inv.UpdatedBy,
                Type = inv.Type,
                Status = inv.Status,
                ListInventoryLine = (from il in _iInventoryLineRepository.GetListByInventoryId(inv.InventoryId)
                                     select new InventoryLineVM
                                     {
                                         InventoryLineId = il.InventoryLineId,
                                         Quantity = il.Quantity,
                                         Price = il.Price,
                                         InventoryId = il.InventoryId,
                                         ProductId = il.ProductId,
                                         ProductName = _iProductRepository.GetById(il.ProductId).ProductName,
                                         ListSerialNumber = (from seri in _iSerialNumberRepository.GetListByInventoryLineId(il.InventoryLineId)
                                                             select new SerialNumberModel
                                                             {
                                                                 SerialId = seri.SerialId,
                                                                 SerialNumber = seri.SerialNumber,
                                                                 CreatedDate = seri.CreatedDate,
                                                                 Status = seri.Status,
                                                                 ProductId = seri.ProductId,
                                                                 Location = seri.Location,
                                                                 WareHouseId = seri.WareHouseId,
                                                                 InventoryLineId = seri.InventoryLineId,
                                                             }).ToList(),
                                     }).ToList(),
            };
            return invDetail;
        }

        public IEnumerable<InventoryVM> GetListByType(InventorySearch search)
        {
            var list = from inv in _iInventoryRepository.GetList()
                       select new InventoryVM
                       {
                            InventoryId = inv.InventoryId,
                            SourceLocation = inv.SourceLocation,
                            CustomerName = _iUserRepository.GetById(inv.SourceLocation).FullName,
                            CreatedDate = inv.CreatedDate,
                            Type = inv.Type,
                            Status = inv.Status
                       };
            if(search.Type == 0)
            {
                list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            else
            {
                list = list.Where(x => x.Type == search.Type).Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }

            if (search.TextSearch == null)
            {
                list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            else
            {
                list = list.Where(x => x.CustomerName.ToLower().Contains(search.TextSearch.ToLower())).Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            return list;
        }

        public ResponseModel Insert(InsertOrUpdateInventory inv)
        {
            // Insert Inventory 
            var inventory = new InventoryEntity
            {
                InventoryId = inv.InventoryId,
                UserId = 1,
                WareHouseId = inv.WareHouseId,
                SourceLocation = inv.SourceLocation,
                CreatedDate = DateTime.Now,
                Type = inv.Type,
                Status = 0,
            };
            var status = _iInventoryRepository.Insert(inventory);
            if (status == 0)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error!"
                };
            }

            if (inv.ListInventoryLine.Count() > 0)
            {
                foreach (var invLine in inv.ListInventoryLine)
                {
                    var inventoryLine = new InventoryLineEntity
                    {
                        ProductId = invLine.ProductId,
                        Quantity = invLine.Quantity,
                        Price = invLine.Price,
                        InventoryId = inventory.InventoryId
                    };
                    //Insert InventoryLine
                    var res = _iInventoryLineRepository.Insert(inventoryLine);
                    if (res == 0)
                    {
                        return new ResponseModel
                        {
                            StatusCode = 500,
                            StatusMessage = "Error!"
                        };
                    }

                    if(invLine.ListSerialNumber.Count() > 0)
                    {
                        foreach (var seri in invLine.ListSerialNumber)
                        {
                            var serialNumber = new SerialNumberEntity
                            {
                                SerialNumber = seri.SerialNumber,
                                CreatedDate = DateTime.Now,
                                Status = (int)SeriStatus.IsStock,
                                ProductId = invLine.ProductId,
                                WareHouseId = inventory.WareHouseId,
                                InventoryLineId = invLine.InventoryLineId
                            };
                            //Insert SerialNumber
                            var result = _iSerialNumberRepository.Insert(serialNumber);
                            if (result == 0)
                            {
                                return new ResponseModel
                                {
                                    StatusCode = 500,
                                    StatusMessage = "Error!"
                                };
                            }
                        }
                    }
                }
            }
            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Create Successfully!"
            };
        }

        public ResponseModel Update(InsertOrUpdateInventory inv)
        {
            var invDetail = _iInventoryRepository.GetById(inv.InventoryId);
            if (invDetail == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Inventory does not exists!"
                };
            }

            // Update Product 
            invDetail.SourceLocation = inv.SourceLocation;
            invDetail.WareHouseId = inv.WareHouseId;
            invDetail.UpdatedDate = DateTime.Now;
            invDetail.UpdatedBy = 1;
            invDetail.Type = inv.Type;
            invDetail.Status = inv.Status;
            var status = _iInventoryRepository.Update(invDetail);
            if (status == 0)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error!"
                };
            }

            if (inv.ListInventoryLine.Count() > 0)
            {
                foreach (var invLine in inv.ListInventoryLine)
                {
                    var invLineDetail = _iInventoryLineRepository.GetById(invLine.InventoryLineId);
                    invLineDetail.ProductId = invLine.ProductId;
                    invLineDetail.Quantity = invLine.Quantity;
                    invLineDetail.Price = invLine.Price;
                    invLineDetail.InventoryId = invDetail.InventoryId;

                    var res = _iInventoryLineRepository.Update(invLineDetail);
                    if (res == 0)
                    {
                        return new ResponseModel
                        {
                            StatusCode = 500,
                            StatusMessage = "Error!"
                        };
                    }

                    if (invLine.ListSerialNumber.Count() > 0)
                    {
                        foreach (var seri in invLine.ListSerialNumber)
                        {
                            if (seri.SerialId > 0)
                            {
                                var serialNumber = _iSerialNumberRepository.GetById(seri.SerialId);
                                serialNumber.SerialNumber = seri.SerialNumber;
                                serialNumber.Status = (int)SeriStatus.IsStock;
                                serialNumber.ProductId = invLine.ProductId;
                                serialNumber.WareHouseId = invDetail.WareHouseId;
                                serialNumber.InventoryLineId = invLine.InventoryLineId;
                            }
                            else
                            {
                                var serialNumber = new SerialNumberEntity
                                {
                                    SerialNumber = seri.SerialNumber,
                                    CreatedDate = DateTime.Now,
                                    Status = (int)SeriStatus.IsStock,
                                    ProductId = invLine.ProductId,
                                    WareHouseId = invDetail.WareHouseId,
                                    InventoryLineId = invLine.InventoryLineId
                                };
                                //Insert SerialNumber
                                var result = _iSerialNumberRepository.Insert(serialNumber);
                                if (result == 0)
                                {
                                    return new ResponseModel
                                    {
                                        StatusCode = 500,
                                        StatusMessage = "Error!"
                                    };
                                }
                            }
                        }
                    }
                }
            }

            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Edit Successfully!"
            };
        }
    }
}
