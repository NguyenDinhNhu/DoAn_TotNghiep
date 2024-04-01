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
        public ProductService(IProductRepository iProductRepository, ICategoryRepository iCategoryRepository, 
            IBrandRepository iBrandRepository, IProductFeatureRepository iProductFeatureRepository)
        {
            _iProductRepository = iProductRepository;
            _iCategoryRepository = iCategoryRepository;
            _iBrandRepository = iBrandRepository;
            _iProductFeatureRepository = iProductFeatureRepository;
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

        public ProductVM GetById(int id)
        {
            var prod = _iProductRepository.GetById(id);
            var prodDetail = new ProductVM
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
            };
            return prodDetail;
        }

        public GetListProduct GetList(SearchVM search)
        {
            var list = from prod in _iProductRepository.GetList()
                       select new ProductVM
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
                       };
            var total = list.Count();
            if (search.TextSearch == null)
            {
                list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            else
            {
                list = list.Where(x => x.ProductName.ToLower().Contains(search.TextSearch.ToLower())).Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            return new GetListProduct { ListProduct = list, Total = total};
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

        public ResponseModel Insert(InsertOrUpdateProduct prod)
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
                CreatedBy = 1,
                Description = prod.Description,
                Price = prod.Price,
                Unit = prod.Unit,
                Quantity = 0,
                Status = prod.Status,
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

        public ResponseModel Update(InsertOrUpdateProduct prod)
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
            prodDetail.ProductName = prod.ProductName;
            prodDetail.UpdatedDate = DateTime.Now;
            prodDetail.UpdatedBy = 1;
            prodDetail.Description = prod.Description;
            prodDetail.Price = prod.Price;
            prodDetail.Unit = prod.Unit;
            prodDetail.Quantity = prod.Quantity;
            prodDetail.Status = prod.Status;
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
            else
            {
                prodDetail.Image = null;
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
    }
}
