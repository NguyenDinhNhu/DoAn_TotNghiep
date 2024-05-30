using ClosedXML.Excel;
using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Utilities.Enum;
using Electronic_WMS.Utilities.Library;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _iProductRepository;
        private readonly ICategoryRepository _iCategoryRepository;
        private readonly IBrandRepository _iBrandRepository;
        private readonly IProductFeatureRepository _iProductFeatureRepository;
        private readonly IFeatureRepository _iFeatureRepository;
        private readonly IInventoryLineRepository _iInventoryLineRepository;
        private readonly IInventoryRepository _iInventoryRepository;
        public ProductService(IProductRepository iProductRepository, ICategoryRepository iCategoryRepository,
            IBrandRepository iBrandRepository, IProductFeatureRepository iProductFeatureRepository, IFeatureRepository iFeatureRepository,
            IInventoryLineRepository iInventoryLineRepository, IInventoryRepository iInventoryRepository)
        {
            _iProductRepository = iProductRepository;
            _iCategoryRepository = iCategoryRepository;
            _iBrandRepository = iBrandRepository;
            _iProductFeatureRepository = iProductFeatureRepository;
            _iFeatureRepository = iFeatureRepository;
            _iInventoryRepository = iInventoryRepository;
            _iInventoryLineRepository = iInventoryLineRepository;
        }
        public ResponseModel Delete(int id)
        {
            var user = _iProductRepository.GetById(id);
            if (user == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Product Not Found!"
                };
            }
            user.Status = (int)CommonStatus.IsDelete;
            var status = _iProductRepository.Update(user);
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

        public ProductDetailVM GetById(int id)
        {
            var prod = _iProductRepository.GetById(id);
            var prodDetail = new ProductDetailVM
            {
                ProductId = prod.ProductId,
                ProductName = prod.ProductName,
                Image = prod.Image,
                CreatedDate = prod.CreatedDate,
                UpdatedDate = prod.UpdatedDate,
                CreatedBy = prod.CreatedBy,
                UpdatedBy = prod.UpdatedBy,
                Description = prod.Description,
                Price = prod.Price,
                Unit = prod.Unit,
                Quantity = prod.Quantity,
                Status = prod.Status,
                CateId = prod.CateId,
                BrandId = prod.BrandId,
                BrandName = _iBrandRepository.GetById(prod.BrandId).BrandName,
                CateName = _iCategoryRepository.GetById(prod.CateId).CateName,
                ListProductFeature = (from pf in _iProductFeatureRepository.GetListByProductId(prod.ProductId)
                                      select new ProductFeatureVM {
                                          ProductFeatureId = pf.ProductFeatureId,
                                          ProductId = pf.ProductId,
                                          FeatureId = pf.FeatureId,
                                          ProductName = prod.ProductName,
                                          FeatureName = _iFeatureRepository.GetById(pf.FeatureId).FeatureName,
                                          Value = pf.Value,
                                      }).ToList(),
            };
            return prodDetail;
        }

        public GetListProduct GetList(ProductSearch search)
        {
            var list = from prod in _iProductRepository.GetList()
                       join c in _iCategoryRepository.GetList() on prod.CateId equals c.CateId
                       join b in _iBrandRepository.GetList() on prod.BrandId equals b.BrandId
                       where c.Status == (int)CommonStatus.IsActive && b.Status == (int)CommonStatus.IsActive
                       select new ProductVM
                       {
                           ProductId = prod.ProductId,
                           ProductName = prod.ProductName,
                           Image = prod.Image,
                           CreatedDate = prod.CreatedDate,
                           Price = prod.Price,
                           Quantity = prod.Quantity,
                           BrandId = prod.BrandId,
                           CateId = prod.CateId,
                           Status = prod.Status,
                       };
            if (!string.IsNullOrEmpty(search.TextSearch))
            {
                list = list.Where(x => x.ProductName.ToLower().Contains(search.TextSearch.ToLower()));
            }
            if (search.CateId > 0)
            {
                list = from prod in list
                       join c in _iCategoryRepository.GetList() on prod.CateId equals c.CateId
                       where c.CateId == search.CateId || c.ParentId == search.CateId
                       select new ProductVM
                       {
                           ProductId = prod.ProductId,
                           ProductName = prod.ProductName,
                           Image = prod.Image,
                           CreatedDate = prod.CreatedDate,
                           Price = prod.Price,
                           Quantity = prod.Quantity,
                           BrandId = prod.BrandId,
                           CateId = prod.CateId,
                           Status = prod.Status,
                       };
            }
            if (search.BrandId > 0)
            {
                list = from prod in list
                       join b in _iBrandRepository.GetList() on prod.BrandId equals b.BrandId
                       where b.BrandId == search.BrandId || b.ParentId == search.BrandId
                       select new ProductVM
                       {
                           ProductId = prod.ProductId,
                           ProductName = prod.ProductName,
                           Image = prod.Image,
                           CreatedDate = prod.CreatedDate,
                           Price = prod.Price,
                           Quantity = prod.Quantity,
                           BrandId = prod.BrandId,
                           CateId = prod.CateId,
                           Status = prod.Status,
                       };
            }
            if (search.CheckStock > 0)
            {
                if (search.CheckStock == 1) 
                {
                    list = list.Where(x => x.Quantity == 0);
                }
                else if (search.CheckStock == 2)
                {
                    list = list.Where(x => x.Quantity > 0);
                }
            }
            var total = list.Count();
            list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            return new GetListProduct { ListProduct = list, Total = total };
        }
        public GetListProductStock GetListProductStock(SearchVM search)
        {
            var list = from prod in _iProductRepository.GetList()
                       join c in _iCategoryRepository.GetList() on prod.CateId equals c.CateId
                       join b in _iBrandRepository.GetList() on prod.BrandId equals b.BrandId
                       where c.Status == (int)CommonStatus.IsActive && b.Status == (int)CommonStatus.IsActive
                       select new ProductStock
                       {
                           ProductId = prod.ProductId,
                           ProductName = prod.ProductName,
                           QuantityStock = prod.Quantity,
                           QuantityExported = (from inv in _iInventoryRepository.GetList()
                                               join invLine in _iInventoryLineRepository.GetList()
                                               on inv.InventoryId equals invLine.InventoryId
                                               where inv.Type == 2 && inv.Status == (int)InventoryStatus.IsComplete
                                               && invLine.ProductId == prod.ProductId
                                               select inv.InventoryId).ToList().Count(),
                           Incoming = (from inv in _iInventoryRepository.GetList()
                                       join invLine in _iInventoryLineRepository.GetList()
                                       on inv.InventoryId equals invLine.InventoryId
                                       where inv.Type == 1 && inv.Status == (int)InventoryStatus.IsReady
                                       && invLine.ProductId == prod.ProductId
                                       select inv.InventoryId).ToList().Count(),
                           Outgoing = (from inv in _iInventoryRepository.GetList()
                                       join invLine in _iInventoryLineRepository.GetList()
                                       on inv.InventoryId equals invLine.InventoryId
                                       where inv.Type == 2 && inv.Status == (int)InventoryStatus.IsReady
                                       && invLine.ProductId == prod.ProductId
                                       select inv.InventoryId).ToList().Count(),
                       };
            if (!string.IsNullOrEmpty(search.TextSearch))
            {
                list = list.Where(x => x.ProductName.ToLower().Contains(search.TextSearch.ToLower()));
            }
            var total = list.Count();
            list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            return new GetListProductStock { ListProduct = list, Total = total};
        }

        public IEnumerable<ProductCombobox> GetListCombobox()
        {
            var list = from prod in _iProductRepository.GetList()
                       select new ProductCombobox
                       {
                           ProductId = prod.ProductId,
                           ProductName = prod.ProductName,
                       };
            return list;
        }

        public ResponseModel Insert(InsertOrUpdateProduct prod, UserToken userToken)
        {
            // Check ProductName in database
            //var checkProdName = _iProductRepository.GetByUserName(prod.ProductName);
            //if (checkProdName != null)
            //{
            //    return new ResponseModel
            //    {
            //        StatusCode = 400,
            //        StatusMessage = "ProductName already exists!"
            //    };
            //}
            
            // Insert Product 
            var prodEntity = new ProductEntity
            {
                ProductId = prod.ProductId,
                ProductName = prod.ProductName,
                CreatedDate = DateTime.Now,
                CreatedBy = userToken.UserId,
                Description = prod.Description,
                Price = prod.Price,
                Unit = prod.Unit,
                Quantity = 0,
                Status = (int)CommonStatus.IsActive,
                CateId = prod.CateId,
                BrandId = prod.BrandId,
            };
            String avatar = XString.ToAscii(prod.ProductName);
            if (prod.FileImage != null)
            {
                String fileName = avatar + prod.FileImage.FileName.Substring(prod.FileImage.FileName.LastIndexOf('.'));
                var path = Path.Combine("E:\\Nam2ki2\\Nam4Ki2\\DoAn_TotNghiep\\Electronic_WMS_Angular\\src\\assets\\img\\product", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    prod.FileImage.CopyTo(stream);
                }
                prodEntity.Image = fileName;
            }
            else
            {
                prodEntity.Image = null;
            }
            var status = _iProductRepository.Insert(prodEntity);
            if (status == 0)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error!"
                };
            }

            List<ProductFeature> PFList = JsonConvert.DeserializeObject<List<ProductFeature>>(prod.ListProductFeature);
            if (PFList.Count() > 0)
            {
                foreach (var pf in PFList)
                {
                    var productFeature = new ProductFeatureEntity
                    {
                        ProductId = prodEntity.ProductId,
                        FeatureId = pf.FeatureId,
                        Value = pf.Value,
                    };
                    var res = _iProductFeatureRepository.Insert(productFeature);
                    if (res == 0)
                    {
                        return new ResponseModel
                        {
                            StatusCode = 500,
                            StatusMessage = "Error!"
                        };
                    }
                }
            }
            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Create Successfully!"
            };
        }

        public ResponseModel Update(InsertOrUpdateProduct prod, UserToken userToken)
        {
            var prodDetail = _iProductRepository.GetById(prod.ProductId);
            if (prodDetail == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Product does not exists!"
                };
            }

            // Check ProductName in database
            //var checkProdName = _iProductRepository.GetByUserName(prod.ProductName);
            //if (checkProdName != null && (checkProdName.ProductId != prod.ProductId && checkProdName.ProductName == prod.ProductName))
            //{
            //    return new ResponseModel
            //    {
            //        StatusCode = 400,
            //        StatusMessage = "ProductName already exists!"
            //    };
            //}

            // Update Product 
            prodDetail.ProductName = prod.ProductName;
            prodDetail.UpdatedDate = DateTime.Now;
            prodDetail.UpdatedBy = userToken.UserId;
            prodDetail.Description = prod.Description;
            prodDetail.Price = prod.Price;
            prodDetail.Unit = prod.Unit;
            prodDetail.Quantity = prod.Quantity;
            prodDetail.CateId = prod.CateId;
            prodDetail.BrandId = prod.BrandId;
            String avatar = XString.ToAscii(prod.ProductName);
            if (prod.FileImage != null)
            {
                String fileName = avatar + prod.FileImage.FileName.Substring(prod.FileImage.FileName.LastIndexOf('.'));
                var path = Path.Combine("E:\\Nam2ki2\\Nam4Ki2\\DoAn_TotNghiep\\Electronic_WMS_Angular\\src\\assets\\img\\product", fileName);
                using (var stream = System.IO.File.Create(path))
                {
                    prod.FileImage.CopyTo(stream);
                }
                prodDetail.Image = fileName;
            }
            var status = _iProductRepository.Update(prodDetail);
            if (status == 0)
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error!"
                };
            }

            List<ProductFeature> PFList = JsonConvert.DeserializeObject<List<ProductFeature>>(prod.ListProductFeature);
            if (PFList.Count() > 0)
            {
                foreach (var pf in PFList)
                {
                    var productFeature = _iProductFeatureRepository.GetById(pf.ProductFeatureId);
                    productFeature.ProductId = prodDetail.ProductId;
                    productFeature.FeatureId = pf.FeatureId;
                    productFeature.Value = pf.Value;
                    
                    var res = _iProductFeatureRepository.Update(productFeature);
                    if (res == 0)
                    {
                        return new ResponseModel
                        {
                            StatusCode = 500,
                            StatusMessage = "Error!"
                        };
                    }
                }
            }
            return new ResponseModel
            {
                StatusCode = 200,
                StatusMessage = "Edit Successfully!"
            };
        }
        public byte[] ExportStockToExcel()
        {
            var list = from prod in _iProductRepository.GetList()
                    join c in _iCategoryRepository.GetList() on prod.CateId equals c.CateId
                    join b in _iBrandRepository.GetList() on prod.BrandId equals b.BrandId
                    where c.Status == (int)CommonStatus.IsActive && b.Status == (int)CommonStatus.IsActive
                    select new ProductStock
                    {
                        ProductId = prod.ProductId,
                        ProductName = prod.ProductName,
                        QuantityStock = prod.Quantity,
                        QuantityExported = (from inv in _iInventoryRepository.GetList()
                                            join invLine in _iInventoryLineRepository.GetList()
                                            on inv.InventoryId equals invLine.InventoryId
                                            where inv.Type == 2 && inv.Status == (int)InventoryStatus.IsComplete
                                            && invLine.ProductId == prod.ProductId
                                            select inv.InventoryId).ToList().Count(),
                        Incoming = (from inv in _iInventoryRepository.GetList()
                                    join invLine in _iInventoryLineRepository.GetList()
                                    on inv.InventoryId equals invLine.InventoryId
                                    where inv.Type == 1 && inv.Status == (int)InventoryStatus.IsReady
                                    && invLine.ProductId == prod.ProductId
                                    select inv.InventoryId).ToList().Count(),
                        Outgoing = (from inv in _iInventoryRepository.GetList()
                                    join invLine in _iInventoryLineRepository.GetList()
                                    on inv.InventoryId equals invLine.InventoryId
                                    where inv.Type == 2 && inv.Status == (int)InventoryStatus.IsReady
                                    && invLine.ProductId == prod.ProductId
                                    select inv.InventoryId).ToList().Count(),
                    };
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Stock");

                // Gộp các ô từ cột 1 đến cột 7 trong dòng đầu tiên
                var titleHeader = worksheet.Range(1, 1, 1, 6);
                titleHeader.Merge();
                titleHeader.Value = "List of products in stock";
                titleHeader.Style.Font.Bold = true;
                titleHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleHeader.Style.Font.FontSize = 18;

                var exportedDate = worksheet.Range(2, 1, 2, 6);
                exportedDate.Merge();
                exportedDate.Value = "Exported Date: " + DateTime.Now.ToString("dd-MM-yyyy");
                exportedDate.Style.Font.Bold = true;
                exportedDate.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                exportedDate.Style.Font.FontSize = 16;

                // Đặt header
                worksheet.Cell(3, 1).Value = "ID";
                worksheet.Cell(3, 2).Value = "Product Name";
                worksheet.Cell(3, 3).Value = "Quantity Stock";
                worksheet.Cell(3, 4).Value = "Quantity Released";
                worksheet.Cell(3, 5).Value = "Incoming";
                worksheet.Cell(3, 6).Value = "Outgoing";

                // Thêm border cho header
                for (int col = 1; col <= 6; col++)
                {
                    var cell = worksheet.Cell(3, col);
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Font.Bold = true;
                }

                // Đổ dữ liệu từ danh sách object vào file Excel
                int row = 4;
                foreach (var item in list)
                {
                    worksheet.Cell(row, 1).Value = item.ProductId;
                    worksheet.Cell(row, 2).Value = item.ProductName;
                    worksheet.Cell(row, 3).Value = item.QuantityStock;
                    worksheet.Cell(row, 4).Value = item.QuantityExported;
                    worksheet.Cell(row, 5).Value = item.Incoming;
                    worksheet.Cell(row, 6).Value = item.Outgoing;

                    // Thêm border cho các ô dữ liệu
                    for (int col = 1; col <= 6; col++)
                    {
                        var cell = worksheet.Cell(row, col);
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    }

                    row++;
                }

                // Chỉnh kích thước của các cột
                worksheet.Column(1).Width = 20;
                worksheet.Column(2).Width = 80;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 20;

                // Lưu workbook vào MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
