using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Entities
{
    public class WMSDbContext : DbContext
    {
        public WMSDbContext(DbContextOptions<WMSDbContext> options)
               : base(options)
        {

        }

        public DbSet<BrandEntity> BrandEntities { get; set; }
        public DbSet<CategoryEntity> CategoryEntities { get; set; }
        public DbSet<FeatureEntity> FeatureEntities { get; set; }
        public DbSet<InventoryEntity> InventoryEntities { get; set; }
        public DbSet<InventoryLineEntity> InventoryLineEntities { get; set; }
        public DbSet<ProductEntity> ProductEntities { get; set; }
        public DbSet<ProductFeatureEntity> ProductFeatureEntities { get; set; }
        public DbSet<RolesEntity> RolesEntities { get; set; }
        public DbSet<UsersEntity> UsersEntities { get; set; }
        public DbSet<WareHouseEntity> WareHouseEntities { get; set;}
        public DbSet<SerialNumberEntity> SerialNumberEntities { get; set;}
    }
}
