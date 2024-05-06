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
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.EntityFrameworkCore;
using iText.Layout;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using Table = iText.Layout.Element.Table;

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
                                    seriUpdate.Location = "Is Realeased";
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
                            WareHouseName = _iWareHouseRepository.GetById(inv.WareHouseId).Name,
                            CreatedDate = inv.CreatedDate,
                            UpdatedDate = inv.UpdatedDate,
                            Quantity = _iInventoryLineRepository.GetList().Where(x => x.InventoryId ==  inv.InventoryId).Sum(x => x.Quantity),
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
                if (inv.ListInventoryLine.Count > 0)
                {
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
            if (inv.Type == 1)
            {
                List<string> lstSeriError = new List<string>();
                if (inv.ListInventoryLine.Count > 0)
                {
                    foreach (var item in inv.ListInventoryLine)
                    {
                        if (item.ListSerialNumber.Count > 0)
                        {
                            foreach (var i in item.ListSerialNumber)
                            {
                                var checkSeri = _iSerialNumberRepository.GetBySerialNumber(i.SerialNumber);
                                if (checkSeri != null)
                                {
                                    lstSeriError.Add(i.SerialNumber);
                                }
                            }
                        }
                    }
                }
                if (lstSeriError.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("Existing serial numbers:");
                    foreach (var error in lstSeriError)
                    {
                        errorMessage.AppendLine($"- {error}");
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
                                serialItem.InventoryLineId2 = inventoryLine.InventoryLineId;
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
                if (inv.ListInventoryLine.Count > 0)
                {
                    foreach (var item in inv.ListInventoryLine)
                    {
                        var quantityStock = _iSerialNumberRepository.GetList()
                            .Where(x => x.WareHouseId == inv.WareHouseId && x.ProductId == item.ProductId
                                && (x.Status == (int)SeriStatus.IsStock || x.Status == (int)SeriStatus.IsProcessing)).Count();
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
            if (inv.Type == 1)
            {
                List<string> lstSeriError = new List<string>();
                if (inv.ListInventoryLine.Count > 0)
                {
                    foreach (var item in inv.ListInventoryLine)
                    {
                        if (item.ListSerialNumber.Count > 0)
                        {
                            foreach (var i in item.ListSerialNumber)
                            {
                                var checkSeri = _iSerialNumberRepository.GetBySerialNumber(i.SerialNumber);
                                if (checkSeri != null && i.SerialId != checkSeri.SerialId)
                                {
                                    lstSeriError.Add(i.SerialNumber);
                                }
                            }
                        }
                    }
                }
                if (lstSeriError.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("Existing serial numbers:");
                    foreach (var error in lstSeriError)
                    {
                        errorMessage.AppendLine($"- Serial Number: {error}");
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
                                    serialNumber.InventoryLineId2 = invLineDetail.InventoryLineId;
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
                            Price = _iProductRepository.GetById(invLine.ProductId).Price,
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
                                    serialNumber.InventoryLineId2 = inventoryLine.InventoryLineId;
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

        public byte[] GenerateInventoryPDF(int inventoryId)
        {
            var invDetail = GetById(inventoryId);

            if (invDetail == null)
            {
                // Handle case when invoice is not found
                return null;
            }

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                var document = new Document(pdf);

                // Add invoice information
                document.Add(new Paragraph($"Transaction invoice:"));
                document.Add(new Paragraph($"Created Date: {invDetail.CreatedDate.ToString("HH:mm:ss dd-MM-yyyy")}"));
                document.Add(new Paragraph($"Reception Date: {invDetail.UpdatedDate?.ToString("HH:mm:ss dd-MM-yyyy")}"));
                document.Add(new Paragraph($"WareHouse Name: {invDetail.WareHouseName}"));
                if (invDetail.Type == 1)
                {
                    document.Add(new Paragraph($"Supplier: {invDetail.CustomerName}"));
                }
                else if (invDetail.Type == 2)
                {
                    document.Add(new Paragraph($"Shop Name: {invDetail.CustomerName}"));
                }

                // Create a table with 3 columns
                var table = new Table(3);
                table.SetWidth(UnitValue.CreatePercentValue(100));

                // Add header row
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph("Product")));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph("Quantity")));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph("Serial Numbers")));

                // Add invoice details
                foreach (var invLine in invDetail.ListInventoryLine)
                {
                    // Add product name in the first column
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(invLine.ProductName)));

                    // Add quantity in the second column
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(invLine.Quantity.ToString())));

                    // Add serial numbers in the third column
                    var serialNumbers = string.Join(", ", invLine.ListSerialNumber.Select(s => s.SerialNumber));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(serialNumbers)));
                }

                // Add the table to the document
                document.Add(table);

                // Close the document
                document.Close();

                return stream.ToArray();
            }
        }

        public byte[] ExportMoveHistoryToExcel(int type)
        {
            var inventory = from inv in _iInventoryRepository.GetList()
                            where inv.Status == (int)InventoryStatus.IsComplete
                            select new InventoryVM
                            {
                                InventoryId = inv.InventoryId,
                                SourceLocation = inv.SourceLocation,
                                CustomerName = _iUserRepository.GetById(inv.SourceLocation).FullName,
                                WareHouseName = _iWareHouseRepository.GetById(inv.WareHouseId).Name,
                                CreatedDate = inv.CreatedDate,
                                UpdatedDate = inv.UpdatedDate,
                                Quantity = _iInventoryLineRepository.GetList().Where(x => x.InventoryId == inv.InventoryId).Sum(x => x.Quantity),
                                Type = inv.Type,
                                Status = inv.Status
                            };
            if (type != 0)
            {
                inventory = inventory.Where(x => x.Type == type);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("MoveHistory");

                // Đặt header
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Customer Name";
                worksheet.Cell(1, 3).Value = "WareHouse Name";
                worksheet.Cell(1, 4).Value = "Created Date";
                worksheet.Cell(1, 5).Value = "Reception Date";
                worksheet.Cell(1, 6).Value = "Quantity";
                worksheet.Cell(1, 7).Value = "Type";

                // Đổ dữ liệu từ danh sách object vào file Excel
                int row = 2;
                foreach (var item in inventory)
                {
                    worksheet.Cell(row, 1).Value = item.InventoryId;
                    worksheet.Cell(row, 2).Value = item.CustomerName;
                    worksheet.Cell(row, 3).Value = item.WareHouseName;
                    worksheet.Cell(row, 4).Value = item.CreatedDate.ToString("dd-MM-yyyy");
                    worksheet.Cell(row, 5).Value = item.UpdatedDate?.ToString("dd-MM-yyyy");
                    worksheet.Cell(row, 6).Value = item.Quantity;
                    if (item.Type == 1)
                    {
                        worksheet.Cell(row, 7).Value = "Receipt";
                    }
                    else if (item.Type == 2)
                    {
                        worksheet.Cell(row, 7).Value = "Delivery";
                    }
                    row++;
                }

                // Chỉnh kích thước của các cột
                worksheet.Column(1).Width = 10; 
                worksheet.Column(2).Width = 45; 
                worksheet.Column(3).Width = 45; 
                worksheet.Column(4).Width = 25; 
                worksheet.Column(5).Width = 25; 
                worksheet.Column(6).Width = 15; 
                worksheet.Column(7).Width = 15; 

                // Lưu workbook vào MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public DashboardVM GetVMDashBoard()
        {
            var countReceiptReady = _iInventoryRepository.GetList().Where(x => x.Type == 1 && x.Status == (int)InventoryStatus.IsReady).Count();
            var countDeliveryReady = _iInventoryRepository.GetList().Where(x => x.Type == 2 && x.Status == (int)InventoryStatus.IsReady).Count();
            var countProductOutOfStock = _iProductRepository.GetList().Where(x => x.Quantity == 0).Count();

            return new DashboardVM
            {
                CountReciptReady = countReceiptReady,
                CountDeliveryReady = countDeliveryReady,
                CountProductOutOfStock = countProductOutOfStock
            };
        }
    }
}
