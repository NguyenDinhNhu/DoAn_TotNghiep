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
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductFeatureRepository, ProductFeatureRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IInventoryLineRepository, InventoryLineRepository>();
            services.AddScoped<ISerialNumberRepository, SerialNumberRepository>();


            // Config Service
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IFeatureService, FeatureService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IWareHouseService, WareHouseService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductFeatureService, ProductFeatureService>();
            services.AddScoped<IInventoryService, InventoryService>();
        }
    }
}
