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

        public ResponseModel ChangeStatus(ChangeStatusInventory change)
        {
            var inv = _iInventoryRepository.GetById(change.InventoryId);
            if (inv == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Inventory Not Found!"
                };
            }
            if (change.Status == (int)InventoryStatus.IsCancel)
            {
                inv.Status = change.Status;
                var invDetail = _iInventoryLineRepository.GetListByInventoryId(change.InventoryId).ToList();
                if (invDetail.Count() > 0)
                {
                    foreach (var i in invDetail)
                    {
                        var lstSeri = _iSerialNumberRepository.GetListByInventoryLineId(i.InventoryLineId).ToList();
                        if (inv.Type == 1)
                        {
                            if (lstSeri.Count > 0)
                            {
                                foreach (var s in lstSeri)
                                {
                                    var seriUpdate = _iSerialNumberRepository.GetById(s.SerialId);
                                    seriUpdate.Status = (int)SeriStatus.IsDelete;
                                    var updateSerial = _iSerialNumberRepository.Update(seriUpdate);
                                    if (updateSerial == 0)
                                    {
                                        return new ResponseModel
                                        {
                                            StatusCode = 500,
                                            StatusMessage = "Eror!"
                                        };
                                    }
                                }
                            }
                        }
                        else if (inv.Type == 2)
                        {
                            foreach (var s in lstSeri)
                            {
                                var seriUpdate = _iSerialNumberRepository.GetById(s.SerialId);
                                seriUpdate.Status = (int)SeriStatus.IsStock;
                                var updateSerial = _iSerialNumberRepository.Update(seriUpdate);
                                if (updateSerial == 0)
                                {
                                    return new ResponseModel
                                    {
                                        StatusCode = 500,
                                        StatusMessage = "Eror!"
                                    };
                                }
                            }
                        }
                    }
                }
            }
            else if (change.Status == (int)InventoryStatus.IsComplete)
            {
                inv.Status = change.Status;
                var invDetail = _iInventoryLineRepository.GetListByInventoryId(change.InventoryId).ToList();
                if (invDetail.Count() > 0)
                {
                    foreach (var i in invDetail)
                    {
                        var product = _iProductRepository.GetById(i.ProductId);
                        var lstSeri = _iSerialNumberRepository.GetListByInventoryLineId(i.InventoryLineId).ToList();
                        if (inv.Type == 1) // Receipts
                        {
                            product.Quantity += i.Quantity;
                            if (lstSeri.Count() > 0)
                            {
                                foreach (var s in lstSeri)
                                {
                                    var seriUpdate = _iSerialNumberRepository.GetById(s.SerialId);
                                    seriUpdate.Status = (int)SeriStatus.IsStock;
                                    var updateSerial = _iSerialNumberRepository.Update(seriUpdate);
                                    if (updateSerial == 0)
                                    {
                                        return new ResponseModel
                                        {
                                            StatusCode = 500,
                                            StatusMessage = "Eror!"
                                        };
                                    }
                                }
                            }
                        }
                        else if (inv.Type == 2) //Deliveries
                        {
                            product.Quantity -= i.Quantity;
                            if (lstSeri.Count() > 0)
                            {
                                foreach (var s in lstSeri)
                                {
                                    var seriUpdate = _iSerialNumberRepository.GetById(s.SerialId);
                                    seriUpdate.Status = (int)SeriStatus.IsReleased;
                                    var updateSerial = _iSerialNumberRepository.Update(seriUpdate);
                                    if (updateSerial == 0)
                                    {
                                        return new ResponseModel
                                        {
                                            StatusCode = 500,
                                            StatusMessage = "Eror!"
                                        };
                                    }
                                }
                            }
                        }
                        var updateProduct = _iProductRepository.Update(product);
                        if (updateProduct == 0)
                        {
                            return new ResponseModel
                            {
                                StatusCode = 500,
                                StatusMessage = "Eror!"
                            };
                        }
                    }
                }
            }
            inv.UpdatedDate = DateTime.Now;
            inv.UpdatedBy = 1;
            var res = _iInventoryRepository.Update(inv);
            if (res == 0)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Eror!"
                };

            }
            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Status change successful!"
            };
        }

        public ResponseModel Delete(int id)
        {
            var inv = _iInventoryRepository.GetById(id);
            if (inv == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Inventory Not Found!"
                };
            }
            inv.Status = (int)InventoryStatus.IsDelete;
            var status = _iInventoryRepository.Update(inv);
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

        public GetListInventory GetListByType(InventorySearch search)
        {
            var list = from inv in _iInventoryRepository.GetList()
                       select new InventoryVM
                       {
                            InventoryId = inv.InventoryId,
                            SourceLocation = inv.SourceLocation,
                            CustomerName = _iUserRepository.GetById(inv.SourceLocation).FullName,
                            WareHouseName = _iWareHouseRepository.GetById(inv.SourceLocation).Name,
                            CreatedDate = inv.CreatedDate,
                            Type = inv.Type,
                            Status = inv.Status
                       };
            if (search.Type != 0)
            {
                list = list.Where(x => x.Type == search.Type);
            }

            if (search.Status != 0)
            {
                list = list.Where(x => x.Status == search.Status);
            }

            if (!string.IsNullOrEmpty(search.TextSearch))
            {
                list = list.Where(x => x.CustomerName.ToLower().Contains(search.TextSearch.ToLower()));
            }
            var total = list.Count();
            list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            return new GetListInventory { ListInventory = list, Total = total};
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
                Status = (int)InventoryStatus.IsReady,
            };

            if (inv.Type == 2)
            {
                List<CheckQuantity> lstProductError = new List<CheckQuantity>();
                foreach (var item in inv.ListInventoryLine)
                {
                    var quantityStock = _iSerialNumberRepository.GetListByProductId(item.ProductId)
                        .Where(x => x.WareHouseId == inv.WareHouseId).Count();
                    if (item.Quantity > quantityStock)
                    {
                        var productError = new CheckQuantity
                        {
                            ProductName = _iProductRepository.GetById(item.ProductId).ProductName,
                            Quantity = quantityStock,
                        };
                        lstProductError.Add(productError);
                    }
                }
                if (lstProductError.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("Quantity is not enough for the following products:");
                    foreach (var error in lstProductError)
                    {
                        errorMessage.AppendLine($"- Product: {error.ProductName} remaining {error.Quantity}");
                    }
                    return new ResponseModel
                    {
                        StatusCode = 400,
                        StatusMessage = errorMessage.ToString()
                    };
                }
            }

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
                        Price = _iProductRepository.GetById(invLine.ProductId).Price,
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
                            if (seri.SerialId > 0)
                            {
                                var serialItem = _iSerialNumberRepository.GetById(seri.SerialId);
                                serialItem.Status = (int)SeriStatus.IsProcessing;
                                //Update SerialNumber when inventory type = 2: deliveries
                                var result = _iSerialNumberRepository.Update(serialItem);
                                if (result == 0)
                                {
                                    return new ResponseModel
                                    {
                                        StatusCode = 500,
                                        StatusMessage = "Error!"
                                    };
                                }
                            }
                            else
                            {
                                var serialNumber = new SerialNumberEntity
                                {
                                    SerialNumber = seri.SerialNumber,
                                    CreatedDate = DateTime.Now,
                                    Status = (int)SeriStatus.IsCreate,
                                    ProductId = inventoryLine.ProductId,
                                    WareHouseId = inventory.WareHouseId,
                                    InventoryLineId = inventoryLine.InventoryLineId
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

            // Update  
            invDetail.SourceLocation = inv.SourceLocation;
            invDetail.WareHouseId = inv.WareHouseId;
            invDetail.UpdatedDate = DateTime.Now;
            invDetail.UpdatedBy = 1;
            invDetail.Type = inv.Type;

            if (inv.Type == 2)
            {
                List<CheckQuantity> lstProductError = new List<CheckQuantity>();
                foreach (var item in inv.ListInventoryLine)
                {
                    var quantityStock = _iSerialNumberRepository.GetListByProductId(item.ProductId)
                        .Where(x => x.WareHouseId == inv.WareHouseId).Count();
                    if (item.Quantity > quantityStock)
                    {
                        var productError = new CheckQuantity
                        {
                            ProductName = _iProductRepository.GetById(item.ProductId).ProductName,
                            Quantity = quantityStock,
                        };
                        lstProductError.Add(productError);
                    }
                }
                if (lstProductError.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("Quantity is not enough for the following products:");
                    foreach (var error in lstProductError)
                    {
                        errorMessage.AppendLine($"- Product: {error.ProductName} remaining {error.Quantity}");
                    }
                    return new ResponseModel
                    {
                        StatusCode = 400,
                        StatusMessage = errorMessage.ToString()
                    };
                }
            }

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
                    if (invLine.InventoryLineId >  0)
                    {
                        var invLineDetail = _iInventoryLineRepository.GetById(invLine.InventoryLineId);
                        invLineDetail.ProductId = invLine.ProductId;
                        invLineDetail.Quantity = invLine.Quantity;
                        invLineDetail.Price = _iProductRepository.GetById(invLine.ProductId).Price;
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
                            if (invDetail.Type == 1)
                            {
                                foreach (var seri in invLine.ListSerialNumber)
                                {
                                    if (seri.SerialId > 0)
                                    {
                                        var serialNumber = _iSerialNumberRepository.GetById(seri.SerialId);
                                        serialNumber.SerialNumber = seri.SerialNumber;
                                        serialNumber.ProductId = invLineDetail.ProductId;
                                        serialNumber.WareHouseId = invDetail.WareHouseId;
                                        serialNumber.InventoryLineId = invLineDetail.InventoryLineId;
                                        //Update SerialNumber
                                        var result = _iSerialNumberRepository.Update(serialNumber);
                                        if (result == 0)
                                        {
                                            return new ResponseModel
                                            {
                                                StatusCode = 500,
                                                StatusMessage = "Error!"
                                            };
                                        }
                                    }
                                    else
                                    {
                                        var serialNumber = new SerialNumberEntity
                                        {
                                            SerialNumber = seri.SerialNumber,
                                            CreatedDate = DateTime.Now,
                                            Status = (int)SeriStatus.IsCreate,
                                            ProductId = invLineDetail.ProductId,
                                            WareHouseId = invDetail.WareHouseId,
                                            InventoryLineId = invLineDetail.InventoryLineId
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
                            else if (invDetail.Type == 2)
                            {
                                foreach (var seri in invLine.ListSerialNumber)
                                {
                                    var serialNumber = _iSerialNumberRepository.GetById(seri.SerialId);
                                    serialNumber.Status = (int)SeriStatus.IsProcessing;
                                    //Update SerialNumber
                                    var result = _iSerialNumberRepository.Update(serialNumber);
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
                    else
                    {
                        var inventoryLine = new InventoryLineEntity
                        {
                            ProductId = invLine.ProductId,
                            Quantity = invLine.Quantity,
                            Price = invLine.Price,
                            InventoryId = invDetail.InventoryId
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
                        if (invLine.ListSerialNumber.Count() > 0)
                        {
                            foreach (var seri in invLine.ListSerialNumber)
                            {
                                if (seri.SerialId > 0)
                                {
                                    var serialNumber = _iSerialNumberRepository.GetById(seri.SerialId);
                                    serialNumber.Status = (int)SeriStatus.IsProcessing;
                                    //Update SerialNumber
                                    var result = _iSerialNumberRepository.Update(serialNumber);
                                    if (result == 0)
                                    {
                                        return new ResponseModel
                                        {
                                            StatusCode = 500,
                                            StatusMessage = "Error!"
                                        };
                                    }
                                }
                                else
                                {
                                    var serialNumber = new SerialNumberEntity
                                    {
                                        SerialNumber = seri.SerialNumber,
                                        CreatedDate = DateTime.Now,
                                        Status = (int)SeriStatus.IsCreate,
                                        ProductId = inventoryLine.ProductId,
                                        WareHouseId = invDetail.WareHouseId,
                                        InventoryLineId = inventoryLine.InventoryLineId
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
            }

            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Edit Successfully!"
            };
        }

        
    }
}
