using Electronic_WMS.Models.Entities;
using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface ICategoryService
    {
        public GetListCategory GetList(SearchVM search);
        public IEnumerable<CategoryCombobox> GetListCombobox();
        public Category GetById(int id);
        public ResponseModel Insert(Category cate);
        public ResponseModel Update(Category cate);
        public ResponseModel Delete(int id);
    }
}
