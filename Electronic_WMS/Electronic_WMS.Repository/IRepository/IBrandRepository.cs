using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IBrandRepository
    {
        public IEnumerable<BrandEntity> GetList();
        public int Insert(BrandEntity brand);
        public int Update(BrandEntity brand);
        public int Delete(BrandEntity brand);
        public BrandEntity GetById(int id);
        public BrandEntity GetByName(string name);
        public string GetParentName(int parentId);
    }
}
