using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication.Data;
using WebApplication.Models;
using System.Data.Entity;

namespace WebApplication.Products
{
    public partial class Products : System.Web.UI.Page
    {
        private readonly WarehouseContext _context = new WarehouseContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetProducts();
        }

        private void GetProducts()
        {
            IEnumerable<Product> products = _context.Products.Include(p => p.ProductType).Include(p => p.Manufacturer).ToList();
            ProductsGridView.DataSource = products;
            ProductsGridView.DataBind();
        }

        protected void ProductsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ProductsGridView.PageIndex = e.NewPageIndex;
            GetProducts();
        }

        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            string name = ProductNameTextBox.Text;
            string storageConditions = ProductStorageConditionsTextBox.Text;
            string packaging = ProductPackagingTextBox.Text;
            DateTime expiryDate = ProductExpiryDateTextBox.SelectedDate;
            decimal price = decimal.TryParse(ProductPriceTextBox.Text, out price) == true ? price : default;
            int productTypeId = int.Parse(ProductTypesDropDownList.SelectedValue);
            int manufacturerId = int.Parse(ManufacturersDropDownList.SelectedValue);

            if (CheckValues(name, storageConditions, packaging, expiryDate, price))
            {
                Product product = new Product
                {
                    Name = name,
                    StorageConditions = storageConditions,
                    Packaging = packaging,
                    ExpiryDate = expiryDate,
                    Price = price,
                    ProductTypeId = productTypeId,
                    ManufacturerId = manufacturerId
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                ProductNameTextBox.Text = string.Empty;
                ProductStorageConditionsTextBox.Text = string.Empty;
                ProductPackagingTextBox.Text = string.Empty;
                ProductExpiryDateTextBox.SelectedDate = DateTime.Today.Date;
                ProductPriceTextBox.Text = string.Empty;
                ProductTypesDropDownList.SelectedIndex = 0;
                ManufacturersDropDownList.SelectedIndex = 0;

                AddStatusLabel.Text = "Product was successfully added.";

                ProductsGridView.PageIndex = ProductsGridView.PageCount;
                GetProducts();
            }
        }

        protected void ProductsGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ProductsGridView.EditIndex = e.NewEditIndex;
            GetProducts();
        }

        protected void ProductsGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ProductsGridView.EditIndex = -1;
            GetProducts();
        }

        protected void ProductsGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string name = (string)e.NewValues["Name"];
            string storageConditions = (string)e.NewValues["StorageConditions"];
            string packaging = (string)e.NewValues["Packaging"];
            DateTime expiryDate = (DateTime)e.NewValues["ExpiryDate"];
            decimal price = decimal.TryParse((string)e.NewValues["Price"], out price) == true ? price : default;
            int productTypeId = int.Parse((string)e.NewValues["ProductTypeId"]);
            int manufacturerId = int.Parse((string)e.NewValues["ManufacturerId"]);

            if (CheckValues(name, storageConditions, packaging, expiryDate, price))
            {
                var row = ProductsGridView.Rows[e.RowIndex];
                int id = int.Parse(row.Cells[1].Text);

                Product product = _context.Products.FirstOrDefault(p => p.Id == id);

                product.Name = name;
                product.StorageConditions = storageConditions;
                product.Packaging = packaging;
                product.ExpiryDate = expiryDate;
                product.Price = price;
                product.ProductTypeId = productTypeId;
                product.ManufacturerId = manufacturerId;

                _context.SaveChanges();

                AddStatusLabel.Text = "Product was successfully updated.";

                ProductsGridView.EditIndex = -1;
                GetProducts();
            }
        }

        protected void ProductsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var row = ProductsGridView.Rows[e.RowIndex];
            int id = int.Parse(row.Cells[1].Text);

            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
            _context.Products.Remove(product);
            _context.SaveChanges();

            AddStatusLabel.Text = "Product was successfully deleted.";
            GetProducts();
        }

        private bool CheckValues(string name, string storageConditions, string packaging, DateTime expiryDate, decimal price)
        {
            if (string.IsNullOrEmpty(name))
            {
                AddStatusLabel.Text = "Incorrect 'Name' data.";
                return false;
            }

            if (string.IsNullOrEmpty(storageConditions))
            {
                AddStatusLabel.Text = "Incorrect 'Storage conditions' data.";
                return false;
            }

            if (string.IsNullOrEmpty(packaging))
            {
                AddStatusLabel.Text = "Incorrect 'Packaging' data.";
                return false;
            }

            if (expiryDate == default)
            {
                AddStatusLabel.Text = "Incorrect 'Expiry date' data.";
                return false;
            }

            if (price == default)
            {
                AddStatusLabel.Text = "Incorrect 'Price' data.";
                return false;
            }

            return true;
        }
    }
}