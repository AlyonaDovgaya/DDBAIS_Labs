using System;
using System.Collections.Generic;

#nullable disable

namespace WareHouseApiApp
{
    public partial class Product
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Storage { get; set; }
        public string Packaging { get; set; }
        public int ExpirationDate { get; set; }
        public int TypeId { get; set; }

        public virtual ProductType Type { get; set; }
    }
}
