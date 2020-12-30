using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StorageConditions { get; set; }
        public string Packaging { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Price { get; set; }
        public int ProductTypeId { get; set; }
        public int ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ProductType ProductType { get; set; }

    }
}
