using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IProductService
    {
        public GetListProduct GetList(SearchVM search);
        public GetListProductStock GetListProductStock(SearchVM search);
        public IEnumerable<ProductCombobox> GetListCombobox();
        public ProductDetailVM GetById(int id);
        public ResponseModel Insert(InsertOrUpdateProduct prod, UserToken userToken);
        public ResponseModel Update(InsertOrUpdateProduct prod, UserToken userToken);
        public ResponseModel Delete(int id);
        public byte[] ExportStockToExcel();
    }
}
