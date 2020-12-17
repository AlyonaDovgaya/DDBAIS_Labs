using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WareHouseApiApp.ViewModel
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Storage { get; set; }
        public string Packaging { get; set; }
        public int ExpirationDate { get; set; }
        public string TypeName { get; set; }
    }
}
