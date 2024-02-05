using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Service.Service;

namespace Electronic_WMS.API.Configurations
{
    public static class Configuration
    {
        public static void ConfigureService(this IServiceCollection services)
        {
            services.AddMvc();

            // Config Repository
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IWareHouseRepository, WareHouseRepository>();


            // Config Service
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IFeatureService, FeatureService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IWareHouseService, WareHouseService>();
        }
    }
}
