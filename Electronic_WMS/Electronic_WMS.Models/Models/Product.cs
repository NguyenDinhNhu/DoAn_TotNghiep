using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Models.Models
{
    public class GetListProduct
    {
        public IEnumerable<ProductVM> ListProduct { get; set; }
        public int Total { get; set; }
    }

    public class GetListProductStock
    {
        public IEnumerable<ProductStock> ListProduct { get; set; }
        public int Total { get; set; }
    }

    public class ProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set;}
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CateId { get; set; }
        public int BrandId { get; set; }
        public int Status {  get; set; }

    }
    public class ProductDetailVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public int CateId { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set;}
        public string CateName { get; set;}
        public List<ProductFeatureVM> ListProductFeature { get; set; }
    }

    public class ProductCombobox
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class InsertOrUpdateProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public int CateId { get; set; }
        public int BrandId { get; set; }
        public IFormFile? FileImage { get; set; }
        public string? ListProductFeature { get; set; }  
    }

    public class CheckQuantity
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductStock
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantityStock { get; set; }
        public int QuantityExported { get; set; }
        public int Incoming { get; set; }
        public int Outgoing { get; set; }
    }

    public class ProductSearch : SearchVM
    { 
        public int CateId { get; set; }
        public int BrandId { get; set; }
    }
}
