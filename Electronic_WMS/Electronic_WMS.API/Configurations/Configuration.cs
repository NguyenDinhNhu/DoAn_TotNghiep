using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Service.Service;
using Microsoft.AspNetCore.Authorization;

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
            services.AddScoped<ISerialNumberService, SerialNumberService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Tạo các chính sách từ các vai trò
            var serviceProvider = services.BuildServiceProvider();
            var roleService = serviceProvider.GetRequiredService<IRolesService>();
            var roles = roleService.GetListCombobox();

            foreach (var role in roles)
            {
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(role.RoleName, policy => policy.RequireRole(role.RoleName));
                });
            }

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOrStocker", policy => policy.RequireAnyRole("Administrator", "Stocker"));
                options.AddPolicy("AdminOrStockerOrSupplier", policy => policy.RequireAnyRole("Administrator", "Stocker", "Supplier"));
                options.AddPolicy("AdminOrStockerOrShop", policy => policy.RequireAnyRole("Administrator", "Stocker", "Shop"));
            });
        }
    }

    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireAnyRole(this AuthorizationPolicyBuilder builder, params string[] roles)
        {
            builder.RequireAuthenticatedUser();
            builder.RequireRole(roles);
            return builder;
        }
    }
}
