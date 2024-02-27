using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Utilities.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly WMSDbContext _db;
        public CategoryRepository(WMSDbContext db)
        {
            _db = db;
        }

        public int Delete(CategoryEntity category)
        {
            _db.CategoryEntities.Remove(category);
            return _db.SaveChanges();
        }

        public CategoryEntity GetById(int id)
        {
            return _db.CategoryEntities.Find(id);
        }

        public CategoryEntity GetByName(string name)
        {
            return _db.CategoryEntities.Where(c => c.Status == (int)CommonStatus.IsActive && c.CateName.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<CategoryEntity> GetList()
        {
            return _db.CategoryEntities.Where(c => c.Status == (int)CommonStatus.IsActive).ToList();
        }

        public string GetParentName(int parentId)
        {
            return _db.CategoryEntities.Where(c => c.CateId == parentId).FirstOrDefault().CateName;
        }

        public int Insert(CategoryEntity category)
        {
            _db.CategoryEntities.Add(category);
            return _db.SaveChanges();
        }

        public int Update(CategoryEntity category)
        {
            _db.Entry(category).State = EntityState.Modified;
            return _db.SaveChanges();
        }
    }
}
