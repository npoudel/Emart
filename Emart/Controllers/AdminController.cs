using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emart.ViewModels;
using Emart.Models;
using System.IO;

namespace Emart.Controllers
{
    public class AdminController : Controller
    {
        Random randoma = new Random();
        //int randomNumber = randoma.Next(0, 1000);

        string dpath;
        TemplateContext db = new TemplateContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Order()
        {


            var order = db.Order.SqlQuery("SELECT * from Orders").FirstOrDefault();

            return View(order);
        }
        public ActionResult Visualise()
        {
            return View();
        }
        public ActionResult AddProduct()
        {
            String id = Session["VendorId"].ToString();
            ProductViewModel myProductViewModel = new ProductViewModel();
            var mycategory = db.VendorCategory.SqlQuery("Select * from VendorCategories where VendorId=@p0", id).ToList();
            myProductViewModel.VendorCategory = mycategory;
            ViewData["mycate"] = mycategory;
            ViewData["id"] = id;

            return View("~/Views/Product/AddProduct.cshtml");

        }

        [HttpPost]
        public ActionResult AddProduct(FormCollection myproduct, HttpPostedFileBase file)
        {

            //image
            if (file.ContentType.Contains("image"))
            {
                if (file.ContentLength > 0)
                {
                    string datenow = DateTime.Now.ToString("yyyyMMddhhmmss");
                    //string datenow = now.ToString(HHmmss);

                    string FileName = Path.GetFileName(file.FileName);
                    if (FileName.Length > 180)
                    {
                        FileName = datenow + Path.GetExtension(FileName);
                    }
                    FileName = datenow + "_" + FileName;
                    string FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), FileName);
                    dpath = "/UploadedFiles/" + FileName;
                    file.SaveAs(FilePath);
                }

            }
            //image
            int result;
            String vid = Session["VendorId"].ToString();
            string ProductName = myproduct["ProductName"];
            string ProductDescription = myproduct["ProductDescription"];
            string Price = myproduct["NewPrice"];
            string Detail = myproduct["comment"];
            string VendorCaategoryId = myproduct["VendorCategoryId"];
            string MainCategoryId = myproduct["VendorCategoryId"];

            string myattribute = myproduct["myattribute"];
            string myvalue = myproduct["myvalue"];

            var attributeArray = myattribute.Split(',');
            var valueArray = myvalue.Split(',');

            List<object> lst = new List<object>();
            lst.Add(vid);
            lst.Add(ProductName);
            lst.Add(ProductDescription);
            lst.Add(Price);
            lst.Add(Detail);
            lst.Add(VendorCaategoryId);
            lst.Add(MainCategoryId);
            lst.Add(dpath);

            object[] myprdt = lst.ToArray();
            result = db.Database.ExecuteSqlCommand("INSERT into Products(VendorId,ProductName,ProductDescription,Price,Detail,VendorCategoryId,MainCategoryId,ImagePath) VALUES (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7)", myprdt);

            var Pid = db.Product.SqlQuery("Select * from Products where VendorId = @p0 ORDER BY ProductId DESC", vid).FirstOrDefault();

            string ProductId = Pid.ProductId.ToString();
            for (int i = 0; i < attributeArray.Length; i++)
            {
                db.Database.ExecuteSqlCommand("INSERT into Attributes(AttributeName,AttributeValue,VendorId,ProductId) VALUES (@p0,@p1,@p2,@p3)", attributeArray[i], valueArray[i], vid, ProductId);

            }

            ProductViewModel myvm = new ProductViewModel();
            myvm.Vendor = db.Vendor.SqlQuery("Select * from Vendors where VendorId = @p0", vid).FirstOrDefault();

            return View("~/Views/Product/AddProduct1.cshtml", myvm);

        }
    }

}