using Electronic_WMS.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.IService
{
    public interface IFeatureService
    {
        public GetListFeature GetList(SearchVM search);
        public IEnumerable<FeatureCombobox> GetListCombobox();
        public Feature GetById(int id);
        public ResponseModel Insert(Feature feature);
        public ResponseModel Update(Feature feature);
        public ResponseModel Delete(int id);
    }
}
