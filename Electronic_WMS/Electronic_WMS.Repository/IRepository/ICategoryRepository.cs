using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public IEnumerable<CategoryEntity> GetList();
        public int Insert(CategoryEntity category);
        public int Update(CategoryEntity category);
        public int Delete(CategoryEntity category);
        public CategoryEntity GetById(int id);
        public CategoryEntity GetByName(string name);
        public string GetParentName(int parentId);
    }
}
