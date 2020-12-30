using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
    }
}
