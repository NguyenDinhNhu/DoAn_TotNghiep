﻿using Electronic_WMS.Models.Entities;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _iCategoryRepository;
        public CategoryService(ICategoryRepository iCategoryRepository)
        {
            _iCategoryRepository = iCategoryRepository;
        }

        public ResponseModel Delete(int id)
        {
            var cate = _iCategoryRepository.GetById(id);
            if(cate == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Category Not Found!"
                };
            }
            cate.Status = (int)CommonStatus.IsDelete;
            var status = _iCategoryRepository.Update(cate);
            if(status == 0)
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

        public Category GetById(int id)
        {
            var cate = _iCategoryRepository.GetById(id);
            var cateDetail = new Category
            {
                CateId = cate.CateId,
                CateName = cate.CateName,
                ParentId = cate.ParentId,
                Status = cate.Status
            };
            return cateDetail;
        }

        public GetListCategory GetList(SearchVM search)
        {
            var listCate =  from c in _iCategoryRepository.GetList()
                            select new CategoryVM
                            {
                                CateId = c.CateId,
                                CateName = c.CateName,
                                ParentName = c.ParentId == 0? "Highest level" : _iCategoryRepository.GetParentName(c.ParentId),
                                Status = c.Status,
                            };
            if (!string.IsNullOrEmpty(search.TextSearch))
            {
                listCate = listCate.Where(b => b.CateName.ToLower().Contains(search.TextSearch.ToLower()));
            }
            var total = listCate.Count();
            listCate = listCate.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            return new GetListCategory { ListCate = listCate, Total = total};
        }

        public IEnumerable<CategoryCombobox> GetListCombobox()
        {
            var listCate = from c in _iCategoryRepository.GetList()
                           select new CategoryCombobox
                           {
                               CateId = c.CateId,
                               CateName = c.CateName
                           };
            return listCate;
        } 
        public IEnumerable<CategoryCombobox> GetCategoryParentCombobox()
        {
            var listCate = from c in _iCategoryRepository.GetList()
                           where c.ParentId == 0
                           select new CategoryCombobox
                           {
                               CateId = c.CateId,
                               CateName = c.CateName
                           };
            return listCate;
        }

        public ResponseModel Insert(Category cate)
        {
            // Check CateName in database
            var checkCateName = _iCategoryRepository.GetByName(cate.CateName);
            if (checkCateName != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Category already exists!"
                };
            }

            // Insert Category 
            var category = new CategoryEntity
            {
                CateId = cate.CateId,
                CateName = cate.CateName,
                ParentId = cate.ParentId,
                Status = (int)CommonStatus.IsActive,
            };

            var status = _iCategoryRepository.Insert(category);
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

        public ResponseModel Update(Category cate)
        {
            var category = _iCategoryRepository.GetById(cate.CateId);
            if(category == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Category does not exists!"
                };
            }
            // Check CateName in database
            var checkCateName = _iCategoryRepository.GetByName(cate.CateName);
            if (checkCateName != null && checkCateName.CateId != cate.CateId)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Category already exists!"
                };
            }

            // Update Category 
            category.CateName = cate.CateName;
            category.ParentId = cate.ParentId;

            var status = _iCategoryRepository.Update(category);
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
