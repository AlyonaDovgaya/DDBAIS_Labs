using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WareHouseApiApp.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace WareHouseApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private WarehouseContext _context;
        public ProductsController(WarehouseContext wareHouseContext)
        {
            _context = wareHouseContext;
        }
        [HttpGet]
        public List<ProductViewModel> Get()
        {
            var product = _context.Products
                .Where(p => p.Packaging != "")
                .Where(p => p.Storage != "")
                .Include(p => p.Type)
                .OrderByDescending(p => p.ProductId)
                .Select(p =>
                new ProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Storage = p.Storage,
                    Packaging = p.Packaging,
                    TypeName = p.Type.TypeName,
                    ExpirationDate = p.ExpirationDate
                }).Take(20);
            return product.ToList();
        }
        [HttpGet("productTypes")]
        public IEnumerable<ProductType> GetProductTypes()
        {
            return _context.ProductTypes.ToList();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Product product = _context.Products.FirstOrDefault(product => product.ProductId == id);
            if (product == null)
                return NotFound();
            return new ObjectResult(product);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }
        [HttpPut]
        public IActionResult Put([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();
            if (!_context.Products.Any(product => product.ProductId == product.ProductId))
                return NotFound();
            _context.Update(product);
            _context.SaveChanges();
            return Ok(product);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
                return NotFound();
            _context.Products.RemoveRange(product);
            _context.SaveChanges();
            return Ok(product);
        }
    }
}
