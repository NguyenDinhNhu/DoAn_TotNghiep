using Electronic_WMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Repository.IRepository
{
    public interface IFeatureRepository
    {
        public IEnumerable<FeatureEntity> GetList();
        public int Insert(FeatureEntity feature);
        public int Update(FeatureEntity feature);
        public int Delete(FeatureEntity feature);
        public FeatureEntity GetById(int id);
        public FeatureEntity GetByName(string name);
    }
}
