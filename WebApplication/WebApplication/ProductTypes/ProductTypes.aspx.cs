using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.ProductTypes
{
    public partial class ProductTypes : System.Web.UI.Page
    {
        private readonly WarehouseContext _context = new WarehouseContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetProductTypes();
        }

        private void GetProductTypes()
        {
            IEnumerable<ProductType> productTypes = _context.ProductTypes.ToList();
            ProtuctTypesGridView.DataSource = productTypes;
            ProtuctTypesGridView.DataBind();
        }

        protected void ProtuctTypesGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ProtuctTypesGridView.PageIndex = e.NewPageIndex;
            GetProductTypes();
        }

        protected void AddProtuctTypeButton_Click(object sender, EventArgs e)
        {
            string name = ProtuctTypeNameTextBox.Text;
            string description = ProtuctTypeDescriptionTextBox.Text;
            string features = ProtuctTypeFeaturesTextBox.Text;

            if (CheckValues(name, description, features))
            {
                ProductType productType = new ProductType
                {
                    Name = name,
                    Description = description,
                    Features = features
                };

                _context.ProductTypes.Add(productType);
                _context.SaveChanges();

                ProtuctTypeNameTextBox.Text = string.Empty;
                ProtuctTypeDescriptionTextBox.Text = string.Empty;
                ProtuctTypeFeaturesTextBox.Text = string.Empty;

                AddStatusLabel.Text = "Product type was successfully added.";

                ProtuctTypesGridView.PageIndex = ProtuctTypesGridView.PageCount;
                GetProductTypes();
            }
        }

        protected void ProtuctTypesGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ProtuctTypesGridView.EditIndex = e.NewEditIndex;
            GetProductTypes();
        }

        protected void ProtuctTypesGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ProtuctTypesGridView.EditIndex = -1;
            GetProductTypes();
        }

        protected void ProtuctTypesGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string name = (string)e.NewValues["Name"];
            string description = (string)e.NewValues["Description"];
            string features = (string)e.NewValues["Features"];

            if (CheckValues(name, description, features))
            {
                var row = ProtuctTypesGridView.Rows[e.RowIndex];
                int id = int.Parse(row.Cells[1].Text);

                ProductType productType = _context.ProductTypes.FirstOrDefault(p => p.Id == id);

                productType.Name = name;
                productType.Description = description;
                productType.Features = features;

                _context.SaveChanges();

                AddStatusLabel.Text = "Product type was successfully updated.";

                ProtuctTypesGridView.EditIndex = -1;
                GetProductTypes();
            }
        }

        protected void ProtuctTypesGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var row = ProtuctTypesGridView.Rows[e.RowIndex];
            int id = int.Parse(row.Cells[1].Text);

            ProductType productType = _context.ProductTypes.FirstOrDefault(p => p.Id == id);
            _context.ProductTypes.Remove(productType);
            _context.SaveChanges();

            AddStatusLabel.Text = "Product type was successfully deleted.";
            GetProductTypes();
        }

        private bool CheckValues(string name, string description, string features)
        {
            if (string.IsNullOrEmpty(name))
            {
                AddStatusLabel.Text = "Incorrect 'Name' data.";
                return false;
            }

            if (string.IsNullOrEmpty(description))
            {
                AddStatusLabel.Text = "Incorrect 'Description' data.";
                return false;
            }

            if (string.IsNullOrEmpty(features))
            {
                AddStatusLabel.Text = "Incorrect 'Features' data.";
                return false;
            }

            return true;
        }
    }
}