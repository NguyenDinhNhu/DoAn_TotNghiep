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
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _iBrandRepository;
        public BrandService(IBrandRepository iBrandRepository)
        {
            _iBrandRepository = iBrandRepository;
        }
        public ResponseModel Delete(int id)
        {
            var brand = _iBrandRepository.GetById(id);
            if (brand == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Brand Not Found!"
                };
            }
            brand.Status = (int)CommonStatus.IsDelete;
            var status = _iBrandRepository.Update(brand);
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

        public Brand GetById(int id)
        {
            var brand = _iBrandRepository.GetById(id);
            var brandDetail = new Brand
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                ParentId = brand.ParentId,
                Status = brand.Status
            };
            return brandDetail;
        }

        public GetListBrand GetList(SearchVM search)
        {
            var listBrand = from b in _iBrandRepository.GetList()
                           select new BrandVM
                           {
                               BrandId = b.BrandId,
                               BrandName = b.BrandName,
                               ParentName = b.ParentId == 0 ? "Highest level" : _iBrandRepository.GetParentName(b.ParentId),
                               Status = b.Status
                           };
            var total = listBrand.Count();
            if (search.TextSearch == null)
            {
                listBrand = listBrand.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            else
            {
                listBrand = listBrand.Where(b => b.BrandName.ToLower().Contains(search.TextSearch.ToLower())).Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            return new GetListBrand { ListBrand = listBrand, Total = total};
        }

        public IEnumerable<BrandCombobox> GetListCombobox()
        {
            var listBrand = from b in _iBrandRepository.GetList()
                            select new BrandCombobox
                            {
                                BrandId = b.BrandId,
                                BrandName = b.BrandName
                            };
            return listBrand;
        }

        public ResponseModel Insert(Brand brand)
        {
            // Check BrandName in database
            var checkBrandName = _iBrandRepository.GetByName(brand.BrandName);
            if (checkBrandName != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Brand already exists!"
                };
            }

            // Insert Brand 
            var brandEntity = new BrandEntity
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                ParentId = brand.ParentId,
                Status = brand.Status
            };

            var status = _iBrandRepository.Insert(brandEntity);
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

        public ResponseModel Update(Brand brand)
        {
            var brandDetail = _iBrandRepository.GetById(brand.BrandId);
            if (brandDetail == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Brand does not exists!"
                };
            }
            // Check BrandName in database
            var checkBrandName = _iBrandRepository.GetByName(brand.BrandName);
            if (checkBrandName != null && (checkBrandName.BrandId != brand.BrandId && checkBrandName.BrandName == brand.BrandName))
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Brand already exists!"
                };
            }

            // Update Brand 
            brandDetail.BrandName = brand.BrandName;
            brandDetail.ParentId = brand.ParentId;
            brandDetail.Status = brand.Status;

            var status = _iBrandRepository.Update(brandDetail);
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
