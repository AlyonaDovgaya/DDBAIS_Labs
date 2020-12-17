using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WareHouseApiApp
{
    public partial class ProductType
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
