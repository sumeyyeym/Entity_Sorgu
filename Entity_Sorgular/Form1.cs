using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entity_Sorgular
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        NorthwindEntities db = new NorthwindEntities();
        private void BtnOrn1_Click(object sender, EventArgs e)
        {
            #region Link To Entity;
            //dataGridView1.DataSource = db.Products.Where(x => x.UnitPrice >= 20 && x.UnitPrice <= 50)
            //                                          .Select(x => new
            //                                          {
            //                                              UrunNo = x.ProductID,
            //                                              UrunAdi = x.ProductName,
            //                                              StokAdet = x.UnitsInStock,
            //                                              Fiyat = x.UnitPrice
            //                                          })
            //                                          .OrderBy(x => x.Fiyat)
            //                                          .ToList(); 
            #endregion

            #region Link to SQL
            var result = from p in db.Products
                         where p.UnitPrice >= 20 && p.UnitPrice <= 50
                         orderby p.UnitPrice ascending
                         select new
                         {
                             UrunNo = p.ProductID,
                             UrunAdi = p.ProductName,
                             StokAdet = p.UnitsInStock,
                             Fiyat = p.UnitPrice
                         };
            dataGridView1.DataSource = result.ToList(); 
            #endregion
        }

        private void Btnorn2_Click(object sender, EventArgs e)
        {
            var result = from x in db.Orders
                         select new
                         {
                             x.Customer.CompanyName,
                             Personel = x.Employee.FirstName + " " + x.Employee.LastName,
                             x.OrderID,
                             x.OrderDate,
                             KargoAdi = x.Shipper.CompanyName
                         };
            dataGridView1.DataSource = result.ToList();

            //dataGridView1.DataSource = db.Orders.Select(x => new
            //{
            //    x.Customer.CompanyName,
            //    Personel = x.Employee.FirstName + " " + x.Employee.LastName, //eğer personel yazıp tanımlamazsak hata alırız. çünkü iki sütunu birleştirip yeni sütun oluşturduk ama veri tabanı üzerinde bu yeni sütunu tutacak bir nesne olmadığı için hata verir
            //    x.OrderID,
            //    x.OrderDate,
            //    KargoAdi = x.Shipper.CompanyName //başında x.shipper yazmasına rağmen ram'e çıktığında bunu okumaz. bu sebeple yukarıda customer kısmındaki companyname'in de olmasından dolayı içeride iki tane companyname algılar ve hata verir. bunu başka bir nesneye atamak gerek.
            //}).ToList(); //lazy load yaptık
        }

        private void BtnOrn3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.Customers.Where(x => x.CompanyName.Contains("restaurant")).ToList();

            //var result = from c in db.Customers
            //             where c.CompanyName.Contains("restaurant")
            //             select c;
            //dataGridView1.DataSource = result.ToList();
        }

        private void BtnOrn4_Click(object sender, EventArgs e)
        {
            #region Yöntem 1
            //Category cat = db.Categories.FirstOrDefault(x => x.CategoryName == "Beverages");
            //Product prod = new Product();
            //prod.ProductName = "Kola 1";
            //prod.UnitPrice = 5.00m;
            //prod.UnitsInStock = 500;
            //prod.CategoryID = cat.CategoryID;

            //db.Products.Add(prod);
            //db.SaveChanges(); 
            #endregion

            db.Categories.FirstOrDefault(x => x.CategoryName == "Beverages").Products.Add(new Product
            {
                ProductName = "Kola 2",
                UnitPrice = 5.00m,
                UnitsInStock = 500
            });
            db.SaveChanges();

            dataGridView1.DataSource = db.Products.Where(x => x.ProductName.StartsWith("Kola"))
                .Select(x => new
                {
                    x.ProductName,
                    x.UnitPrice,
                    x.UnitsInStock,
                    x.CategoryID
                }).ToList();
        }
    }
}
