using Emart.Models;
using Emart.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emart.Controllers
{
    public class ShopController : Controller
    {
        List<Product> mylst = new List<Product>();
        TemplateContext db = new TemplateContext();
        //Template TemplateData { get; set; }
        // GET: Shop/{username}
        public Template Tname(Vendor VendorData)
        {
            try
            {
                var TemplateData = db.Template.SqlQuery("Select * from Templates where TemplateId = @p0", VendorData.TemplateId).FirstOrDefault();
                return TemplateData;
            }
            catch (Exception)
            {

                return null;
            }
            ;
        }
        public ActionResult Index(string id)
        {
            string VendorUsername = id;
            var VendorData = db.Vendor.SqlQuery("Select * from Vendors where VendorUserName = @p0", VendorUsername).SingleOrDefault();

            // var TemplateData = db.Template.SqlQuery("Select * from Templates where TemplateId = @p0", VendorData.TemplateId).FirstOrDefault();

            var TemplateData = Tname(VendorData);
            Session["myname"] = VendorData.VendorId;

            var output = db.Database.SqlQuery<Eshopper>(" Select * from " + TemplateData.TemplateName + " where VendorId = @p0 ", VendorData.VendorId).SingleOrDefault();
            var Pro = db.Product.SqlQuery("Select * from Products where VendorId=@p0 ORDER BY ProductId DESC", VendorData.VendorId).FirstOrDefault();

            EshopperViewModel mytheme = new EshopperViewModel();
            mytheme.Output = output;
            mytheme.Template = TemplateData;
            mytheme.Product = Pro;
            var path = Path.Combine(@"~/Views/shop", TemplateData.TemplateName, "index.cshtml");
           // TempData["tname"] = TemplateData;
            return View(path, mytheme);
           // Create(TemplateData);


        }

        public ActionResult Create(string id)
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(Vendor detail)
        {
            //try
            //{
            //    List<object> lst = new List<object>();
            //    lst.Add(detail.UserName);
            //    lst.Add(detail.FullName);
            //    lst.Add("1");
            //    object[] Vendor = lst.ToArray();
            //    int result = db.Database.ExecuteSqlCommand("INSERT into vendors(UserName,FullName,TemplateId) VALUES (@p0,@p1,@p2)", Vendor);
            //    if (result > 0)
            //    {
            //        ViewBag.msg = "added";
            //    }

            //    Session["UserName"] = detail.UserName.ToString();


            //    return RedirectToAction("choosetemplate");
            //}
            //catch 
            //{
            //    return View();

            //}
            try
            {
                if (ModelState.IsValid)
                {
                    db.Vendor.Add(detail);
                    db.SaveChanges();

                }

             //   ModelState.Clear();
                Session["created"] = detail.VendorUserName.ToString();

                return RedirectToAction("choosetemplate");
            }
            catch (Exception)
            {

                return View();
            }
            
        }
        public ActionResult VendorLogin()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult VendorLogin(Vendor user)
        {
            var usr = db.Vendor.Single(u => u.VendorUserName == user.VendorUserName && u.Password == user.Password);
            if (usr != null)
            {
                Session["VendorUserName"] = user.VendorUserName.ToString();
                var VenId = db.Vendor.SqlQuery("Select * from Vendors where VendorUserName = @p0", user.VendorUserName).SingleOrDefault();
                Session["VendorId"] = VenId.VendorId.ToString();
                
               // Customize(VenId.VendorId);
                return RedirectToAction("Customize", new { id = VenId.VendorId });


            }
            else
            {
                ModelState.AddModelError("", "Username or Password is wrong");
            }
            return View();
        }



        public ActionResult choosetemplate()
        {
            if (Session["created"] != null)
            {
                
                var TemplateList = db.Template.SqlQuery("SELECT * from Templates").ToList();
                return View(TemplateList);
            }
            return RedirectToAction("VendorLogin");
            
        }

        [HttpGet]
        public ActionResult selecttemplate(string id)
        {
            if (Session["created"] != null)
            {
                string User_Name = Session["created"].ToString();           
                List<object> parameters = new List<object>();
                parameters.Add(id);
                parameters.Add(User_Name);
                object[] objectarray = parameters.ToArray();
                int result = db.Database.ExecuteSqlCommand("UPDATE vendors set TemplateId=@p0 where VendorUserName=@p1", objectarray);


                //var User = db.Vendor.SqlQuery("SELECT * from Vendors where UserName = @p0", User_Name).SingleOrDefault();

                // Session["UserId"] = User.VendorId.ToString();


                return RedirectToAction("VendorLogin");                
            }
            return HttpNotFound();

        }

        public ActionResult Customize(int Id)
        {
            var User = db.Vendor.SqlQuery("SELECT * from Vendors where VendorId = @p0", Id).SingleOrDefault();
            var TemplateData = Tname(User);
            var TemplateDetail = new object();
            
            if (TemplateData.TemplateName == "Eshoppers"){
                TemplateDetail = db.Database.SqlQuery<Eshopper>("Select * from " + TemplateData.TemplateName + " where VendorId = @p0 ", Id).SingleOrDefault();
                if (TemplateDetail == null)
                {
                    TemplateDetail = db.Database.SqlQuery<Eshopper>("Select * from " + TemplateData.TemplateName + " where Id = 3").SingleOrDefault();
                    //  TemplateDetailBool = true;

                }
               

            }


            var path = Path.Combine(@"~/Views/shop", TemplateData.TemplateName, "customize.cshtml");


            return View(path,TemplateDetail);
        }

        public ActionResult CustomizeEshopper(Eshopper shop, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            //aakash
            string dpath1;
            string dpath2;
            string dpath3;
            try
            {
                if (file1.ContentType.Contains("image"))
                {
                    if (file1.ContentLength > 0)
                    {
                        string datenow = DateTime.Now.ToString("yyyyMMddhhmmss");
                        //string datenow = now.ToString(HHmmss);

                        string FileName = Path.GetFileName(file1.FileName);
                        if (FileName.Length > 180)
                        {
                            FileName = datenow + Path.GetExtension(FileName);
                        }
                        FileName = datenow + "_" + FileName;
                        string FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), FileName);
                        dpath1 = "/UploadedFiles/" + FileName;
                        shop.dpath1 = dpath1;
                        file1.SaveAs(FilePath);
                    }

                }
            }
            catch (Exception)
            {

                ViewBag.Message = "File upload failed!!";
                return View();
            }
            try
            {
                if (file2.ContentType.Contains("image"))
                {
                    if (file2.ContentLength > 0)
                    {
                        string datenow = DateTime.Now.ToString("yyyyMMddhhmmss");
                        //string datenow = now.ToString(HHmmss);

                        string FileName = Path.GetFileName(file2.FileName);
                        if (FileName.Length > 180)
                        {
                            FileName = datenow + Path.GetExtension(FileName);
                        }
                        FileName = datenow + "_" + FileName;
                        string FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), FileName);
                        dpath2 = "/UploadedFiles/" + FileName;
                        shop.dpath2 = dpath2;
                        file2.SaveAs(FilePath);
                    }

                }
            }
            catch (Exception)
            {

                ViewBag.Message = "File upload failed!!";
                return View();
            }
            try
            {
                if (file3.ContentType.Contains("image"))
                {
                    if (file3.ContentLength > 0)
                    {
                        string datenow = DateTime.Now.ToString("yyyyMMddhhmmss");
                        //string datenow = now.ToString(HHmmss);

                        string FileName = Path.GetFileName(file3.FileName);
                        if (FileName.Length > 180)
                        {
                            FileName = datenow + Path.GetExtension(FileName);
                        }
                        FileName = datenow + "_" + FileName;
                        string FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), FileName);
                        dpath3 = "/UploadedFiles/" + FileName;
                        shop.dpath3 = dpath3;
                        file3.SaveAs(FilePath);
                    }

                }
            }
            catch (Exception)
            {

                ViewBag.Message = "File upload failed!!";
                return View();
            }

            //aakash
            int Result;
            List<object> lst = new List<object>();
            lst.Add(shop.slider1_Text1);
            lst.Add(shop.slider1_Text2);
            lst.Add(shop.slider1_Text3);
            lst.Add(shop.slider2_Text1);
            lst.Add(shop.slider2_Text2);
            lst.Add(shop.slider2_Text3);
            lst.Add(shop.slider3_Text1);
            lst.Add(shop.slider3_Text2);
            lst.Add(shop.slider3_Text3);
            lst.Add(shop.slider1_Text1_color);
            lst.Add(shop.slider1_Text2_color);
            lst.Add(shop.slider1_Text3_color);
            lst.Add(shop.slider2_Text1_color);
            lst.Add(shop.slider2_Text2_color);
            lst.Add(shop.slider2_Text3_color);
            lst.Add(shop.slider3_Text1_color);
            lst.Add(shop.slider3_Text2_color);
            lst.Add(shop.slider3_Text3_color);
            lst.Add(shop.Text1);
            lst.Add(shop.Text1_color);
            lst.Add(shop.Text2);
            lst.Add(shop.Text2_color);
            lst.Add(shop.Text3);
            lst.Add(shop.Text3_color);
            lst.Add(shop.dpath1);
            lst.Add(shop.dpath2);
            lst.Add(shop.dpath3);
            lst.Add(shop.VendorId);
            object[] CustomizedShop = lst.ToArray();
            var NewVendor = db.Eshopper.SqlQuery("SELECT * from Eshoppers where VendorId=@p0", shop.VendorId).SingleOrDefault();
            if (NewVendor != null)
            {
                Result = db.Database.ExecuteSqlCommand("UPDATE Eshoppers set Slider1_Text1=@p0,slider1_Text2=@p1,slider1_Text3=@p2,slider2_Text1=@p3,slider2_Text2=@p4,slider2_Text3=@p5,slider3_Text1=@p6,slider3_Text2=@p7,slider3_Text3=@p8,slider1_Text1_color=@p9,slider1_Text2_color=@p10,slider1_Text3_color=@p11,slider2_Text1_color=@p12,slider2_Text2_color=@p13,slider2_Text3_color=@p14,slider3_Text1_color=@p15,slider3_Text2_color=@p16,slider3_Text3_color=@p17,Text1=@p18,Text1_color=@p19,Text2=@p20,Text2_color=@p21,Text3=@p22,Text3_color=@p23,dpath1=@p24,dpath2=@p25,dpath3=@p26 where VendorId=@p27", CustomizedShop);

            }
            else
            {
                Result = db.Database.ExecuteSqlCommand("INSERT into Eshoppers(slider1_Text1,slider1_Text2,slider1_Text3,slider2_Text1,slider2_Text2,slider2_Text3,slider3_Text1,slider3_Text2,slider3_Text3,slider1_Text1_color,slider1_Text2_color,slider1_Text3_color,slider2_Text1_color,slider2_Text2_color,slider2_Text3_color,slider3_Text1_color,slider3_Text2_color,slider3_Text3_color,Text1,Text1_color,Text2,Text2_color,Text3,Text3_color,dpath1,dpath2,dpath3) VALUES (@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21,@p22,@p23,@p24,@p25,@p26) where VendorId=@p27", CustomizedShop);
            }
            if (Result > 0)
            {
                if (shop.VendorId == 0)
                {
                    shop.VendorId = 1;
                }
                return RedirectToAction("Customize", new { id = shop.VendorId });
            }

            return HttpNotFound();
        }

        public ActionResult Cart(string id)
        {
            var p = db.Cart.SqlQuery("Select * from Carts where userId = @p0 AND status = 'pending'", id).ToList();
            //object[] myproduct = p.ToArray();
            //Product[] pr;
            //for (int i = 0; i < myproduct.Length; i++)
            //{
            //    //List<Product> mylst = new List<Product>();
            //    Product pr = db.Product.SqlQuery("Select * from Products where ProudctId = @p0", myproduct[i]).FirstOrDefault();
            //    //mylst.Add(pr);
            //}
            
            return View("~/Views/shop/Eshoppers/cart.cshtml",p);
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Product()
        {
            return View();
        }
        public ActionResult ProductDetail()
        {
            return View();
        }
        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult BlogSingle()
        {
            return View();
        }
        public ActionResult WishList()
        {
            return View();
        }
        public ActionResult ImageUpload()
        {
            return View();
        }
        
        public ActionResult VendorCart()
        {
            return View();
        }
        public ActionResult VendorCheckout()
        {
            return View();
        }
        
        public ActionResult VendorContact()
        {
            return View();
        }
        public ActionResult VendorError()
        {
            return View();
        }
        public ActionResult VendorProduct()
        {
            return View();
        }
        public ActionResult VendorProductDetail()
        {
            return View();
        }
        public ActionResult VendorBlog()
        {
            return View();
        }
        public ActionResult VendorBlogSingle()
        {
            return View();
        }
        public ActionResult VendorWishList()
        {
            return View();
        }
        public ActionResult CategoryRetrieve()
        {
            return PartialView("CategoryRetrieve",db.Category.ToList());
        }
        public ActionResult BrandRetrieve()
        {
            return PartialView("BrandRetrieve", db.Brand.ToList());
        }
        public ActionResult ViewProduct()
        {
            IEnumerable<EshopperViewModel> model = null;
            model = (from i in db.ImageStore
                     join p in db.Product on i.ProductId equals p.ProductId
                     select new EshopperViewModel
                     {
                         ProductId = i.ProductId,
                         ProductName = p.ProductName,
                         ImagePath=i.ImagePath
                     });

            return View(model);
        }
    }
}