using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Utilities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.Service
{
    public class ProductFeatureService : IProductFeatureService
    {
        private readonly IProductFeatureRepository _iProductFeatureRepository;
        private readonly IProductRepository _iProductRepository;
        private readonly IFeatureRepository _iFeatureRepository;
        public ProductFeatureService(IProductFeatureRepository iProductFeatureRepository, IProductRepository iProductRepository, IFeatureRepository iFeatureRepository)
        {
            _iProductFeatureRepository = iProductFeatureRepository;
            _iProductRepository = iProductRepository;
            _iFeatureRepository = iFeatureRepository;
        }
        public ResponseModel Delete(int id)
        {
            var pf = _iProductFeatureRepository.GetById(id);
            if (pf == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Product Not Found!"
                };
            }
            var status = _iProductFeatureRepository.Delete(pf);
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

        public ProductFeatureVM GetById(int id)
        {
            var pf = _iProductFeatureRepository.GetById(id);
            var productFeature = new ProductFeatureVM
            {
                ProductFeatureId = pf.ProductFeatureId,
                ProductId = pf.ProductId,
                FeatureId = pf.FeatureId,
                ProductName = _iProductRepository.GetById(pf.ProductId).ProductName,
                FeatureName = _iFeatureRepository.GetById(pf.FeatureId).FeatureName,
                Value = pf.Value,
            };
            return productFeature;
        }

        public GetListProductFeature GetList(SearchVM search)
        {
            var list = from pf in _iProductFeatureRepository.GetList()
                       select new ProductFeatureVM
                       {
                           ProductFeatureId = pf.ProductFeatureId,
                           ProductId = pf.ProductId,
                           FeatureId = pf.FeatureId,
                           ProductName = _iProductRepository.GetById(pf.ProductId).ProductName,
                           FeatureName = _iFeatureRepository.GetById(pf.FeatureId).FeatureName,
                           Value = pf.Value,
                       };
            var total = list.Count();
            if (search.TextSearch == null)
            {
                list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            else
            {
                list = list.Where(x => x.Value.ToLower().Contains(search.TextSearch.ToLower())).Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            return new GetListProductFeature { ListProductFeature = list, Total = total };
        }

        public IEnumerable<ProductFeatureVM> GetListByProductId(int productId)
        {
            var list = from pf in _iProductFeatureRepository.GetListByProductId(productId)
                       select new ProductFeatureVM
                       {
                           ProductFeatureId = pf.ProductFeatureId,
                           ProductId = pf.ProductId,
                           FeatureId = pf.FeatureId,
                           ProductName = _iProductRepository.GetById(pf.ProductId).ProductName,
                           FeatureName = _iFeatureRepository.GetById(pf.FeatureId).FeatureName,
                           Value = pf.Value,
                       };
            return list;
        }

        public ResponseModel Insert(ProductFeature pf)
        {
            // Check Product Feature in database
            var checkPFValue = _iProductFeatureRepository.GetList()
                .Where(x => x.ProductId == pf.ProductId && x.FeatureId == pf.FeatureId).FirstOrDefault();
            if (checkPFValue != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Product already exists this feature: " + _iFeatureRepository.GetById(pf.FeatureId).FeatureName
                };
            }

            // Insert Product Feature 
            var pfEntity = new ProductFeatureEntity
            {
                ProductId = pf.ProductId,
                FeatureId = pf.FeatureId,
                Value = pf.Value
            };

            var status = _iProductFeatureRepository.Insert(pfEntity);
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

        public ResponseModel Update(ProductFeature pf)
        {
            var pfDetail = _iProductFeatureRepository.GetById(pf.ProductFeatureId);
            if (pfDetail == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Product Feature does not exists!"
                };
            }

            // Update Product Feature  
            pfDetail.Value = pf.Value;

            var status = _iProductFeatureRepository.Update(pfDetail);
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
