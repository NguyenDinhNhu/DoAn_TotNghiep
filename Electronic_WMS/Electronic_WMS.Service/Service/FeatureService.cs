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
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _iFeatureRepository;
        public FeatureService(IFeatureRepository iFeatureRepository)
        {
            _iFeatureRepository = iFeatureRepository;
        }
        public ResponseModel Delete(int id)
        {
            var feature = _iFeatureRepository.GetById(id);
            if (feature == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Feature Not Found!"
                };
            }
            feature.Status = (int)CommonStatus.IsDelete;
            var status = _iFeatureRepository.Update(feature);
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

        public Feature GetById(int id)
        {
            var feature = _iFeatureRepository.GetById(id);
            var featureDetail = new Feature
            {
                FeatureId = feature.FeatureId,
                FeatureName = feature.FeatureName,
                Status = feature.Status
            };
            return featureDetail;
        }

        public GetListFeature GetList(SearchVM search)
        {
            var list = from f in _iFeatureRepository.GetList()
                            select new Feature
                            {
                                FeatureId = f.FeatureId,
                                FeatureName = f.FeatureName,
                                Status = f.Status
                            };
            var total = list.Count();
            if (search.TextSearch == null)
            {
                list = list.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            else
            {
                list = list.Where(x => x.FeatureName.ToLower().Contains(search.TextSearch.ToLower())).Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            }
            return new GetListFeature { ListFeature = list, Total = total};
        }

        public IEnumerable<FeatureCombobox> GetListCombobox()
        {
            var list = from f in _iFeatureRepository.GetList()
                       select new FeatureCombobox
                       {
                           FeatureId = f.FeatureId,
                           FeatureName = f.FeatureName,
                       };
            return list;
        }

        public ResponseModel Insert(Feature feature)
        {
            // Check FeatureName in database
            var checkFeatureName = _iFeatureRepository.GetByName(feature.FeatureName);
            if (checkFeatureName != null)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Feature already exists!"
                };
            }

            // Insert Feature 
            var featureEntity = new FeatureEntity
            {
                FeatureId = feature.FeatureId,
                FeatureName = feature.FeatureName,
                Status = (int)CommonStatus.IsActive,
            };

            var status = _iFeatureRepository.Insert(featureEntity);
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

        public ResponseModel Update(Feature feature)
        {
            var featureDetail = _iFeatureRepository.GetById(feature.FeatureId);
            if (featureDetail == null)
            {
                return new ResponseModel
                {
                    StatusCode = 404,
                    StatusMessage = "Feature does not exists!"
                };
            }
            // Check FeatureName in database
            var checkFeatureName = _iFeatureRepository.GetByName(feature.FeatureName);
            if (checkFeatureName != null && checkFeatureName.FeatureId != feature.FeatureId)
            {
                return new ResponseModel
                {
                    StatusCode = 400,
                    StatusMessage = "Feature already exists!"
                };
            }

            // Update Feature 
            featureDetail.FeatureName = feature.FeatureName;

            var status = _iFeatureRepository.Update(featureDetail);
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
