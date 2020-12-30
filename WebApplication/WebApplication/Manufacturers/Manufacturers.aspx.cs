using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Manufacturers
{
    public partial class Manufacturers : System.Web.UI.Page
    {
        private readonly WarehouseContext _context = new WarehouseContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetManufacturers();
        }

        private void GetManufacturers()
        {
            IEnumerable<Manufacturer> manufacturers = _context.Manufacturers.ToList();
            ManufacturersGridView.DataSource = manufacturers;
            ManufacturersGridView.DataBind();
        }

        protected void ManufacturersGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ManufacturersGridView.PageIndex = e.NewPageIndex;
            GetManufacturers();
        }

        protected void AddManufacturerButton_Click(object sender, EventArgs e)
        {
            string name = ManufacturerNameTextBox.Text;

            if (CheckValues(name))
            {
                Manufacturer manufacturer = new Manufacturer
                {
                    Name = name
                };

                _context.Manufacturers.Add(manufacturer);
                _context.SaveChanges();

                ManufacturerNameTextBox.Text = string.Empty;

                AddStatusLabel.Text = "Manufacturer was successfully added.";

                ManufacturersGridView.PageIndex = ManufacturersGridView.PageCount;
                GetManufacturers();
            }
        }

        protected void ManufacturersGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ManufacturersGridView.EditIndex = e.NewEditIndex;
            GetManufacturers();
        }

        protected void ManufacturersGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ManufacturersGridView.EditIndex = -1;
            GetManufacturers();
        }

        protected void ManufacturersGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string name = (string)e.NewValues["Name"];

            if (CheckValues(name))
            {
                var row = ManufacturersGridView.Rows[e.RowIndex];
                int id = int.Parse(row.Cells[1].Text);

                Manufacturer manufacturer = _context.Manufacturers.FirstOrDefault(m => m.Id == id);

                manufacturer.Name = name;

                _context.SaveChanges();

                AddStatusLabel.Text = "Manufacturer was successfully updated.";

                ManufacturersGridView.EditIndex = -1;
                GetManufacturers();
            }
        }

        protected void ManufacturersGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var row = ManufacturersGridView.Rows[e.RowIndex];
            int id = int.Parse(row.Cells[1].Text);

            Manufacturer manufacturer = _context.Manufacturers.FirstOrDefault(m => m.Id == id);
            _context.Manufacturers.Remove(manufacturer);
            _context.SaveChanges();

            AddStatusLabel.Text = "Manufacturer was successfully deleted.";
            GetManufacturers();
        }

        private bool CheckValues(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                AddStatusLabel.Text = "Incorrect 'Name' data.";
                return false;
            }

            return true;
        }
    }
}