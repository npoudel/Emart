using Emart.DALS;
using Emart.Models;
using Emart.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.Entity;
using System.Web.Mvc.Filters;
using Emart.ViewModels;

namespace Emart.Controllersu
{
    public class AccountController : Controller
    {
        EmartDBContexts db = new EmartDBContexts();
        SocialViewModel db1 = new ViewModels.SocialViewModel();

        /// <summary>
        /// INDEX
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var name = Session["Username"];
            E_Users result = db.E_Userss.SqlQuery("Select * from E_Users where Username=@p0",name).FirstOrDefault();
            SocialViewModel mysocialviewmodel = new SocialViewModel();
            mysocialviewmodel.E_users = result;
            return View(mysocialviewmodel);




        }
        [HttpPost]
        public ActionResult Index(S_Post spost)
        {
            string msg = "";

            if (ModelState.IsValid)

            {


                if (Session["ID"] != null)
                {
                    var id = Session["ID"];
                    try
                    {
                        List<object> obj = new List<Object>();
                        obj.Add(spost.PostTime);
                        obj.Add(spost.PostContent);
                        
                        obj.Add(spost.Post_like);
                        obj.Add(spost.S_userID);
                        obj.Add(spost.Postimgid);

                        object[] dataarray = obj.ToArray();
                        int output = db.Database.ExecuteSqlCommand("insert into S_Post(PostTime,PostContent,Post_like,S_userID,Postimgid)values(@p0,@p1,@p2,@p3,@p4)", dataarray);
                        if (output > 0)
                        {
                            ViewBag.Message = "Added";
                        }
                        //return RedirectToAction("EmartInWall");
                        //return Convert.ToString(output);
                        msg = "Successfully Saved";
                    }
                    catch (Exception e)
                    {
                        msg = e.Message;

                    }
                }

                else
                {
                    msg = "Please Provide the content";
                }
            }
            if (Request.IsAjaxRequest())
            {
                return new JsonResult { Data = msg, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                ViewBag.Message = msg;
                return View(spost);
            }

        }
        public ActionResult ListPossts()
        {
            if (Session["ID"] != null)
            {

                S_Post post = new S_Post();
                SocialViewModel a = new SocialViewModel();
                var id = Session["ID"];
                var Posttlist = db.S_Posts.SqlQuery("Select * from S_Post").ToList();
                //c = new SocialViewModel();




                return View(Posttlist);


                //    }

            }

            return View();

        }



        //REGISTER ForACCOUNT
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Register(E_Users user)
        {

            if (ModelState.IsValid)
            {
                using (EmartDBContexts db = new EmartDBContexts())
                {

                    db.E_Userss.Add(user);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " successfully created.";
            }
            return View();


        }
        public ActionResult Recommend()
        {
            return View();
        }



        public JsonResult Recommends()
        {

            //List<string> products = new List<string>(new string[] { "Jacket", "Shoes", "Pc", "Key", "Cooker", "Mobile" });
            //List<string> users = new List<string>(new string[] { "hira", "anita", "bibek" });
            //List<E_Users> users = db.E_Userss.SqlQuery("Select DISTINCT ID From  E_Users").ToList();
            List<int> users = db.Recommends.Select(x => x.ID).Distinct().ToList();
            List<String> products = db.Recommends.Select(x => x.ProductName).Distinct().ToList();
            //List<Recommends> recom_prods = db.Recommendss.SqlQuery("Select DISTINCT ProductName From Recommends").ToList();
            //List<String> products = new List<String>();
            //foreach (Recommends rc in recom_prods)
            //    products.Add(rc.ProductName);
            ////}

            //List<string> ProductsRecommendTable = new List<string>();
            //List<string> listToAdd = new List<string>();
            //ProductsRecommendTable.AddRange(listToAdd);




            Dictionary<string, double?[]> ProductsRecommendTable =
                        new Dictionary<string, double?[]>();

            for (int i = 0; i < users.Count; i++)
            {
                String user = Convert.ToString(users[i]);
                var recommendtablelist = db.Recommends.SqlQuery("Select * from Recommends where userid=@p0", user).ToList();
                double?[] productBoughtCount = Enumerable.Repeat((double?)0, products.Count).ToArray();
                for (int j = 0; j < recommendtablelist.Count; j++)
                {
                    string ProductName = recommendtablelist[j].ProductName;
                    int index = products.IndexOf(ProductName);
                    productBoughtCount[index] += Math.Abs(recommendtablelist[j].quantity) * recommendtablelist[j].unitprice;
                }
                ProductsRecommendTable.Add(user, productBoughtCount);

            }

            //aba kunai euta user ko lagi
            string curuserid = Session["ID"].ToString();
            string msg = "";
            if (ProductsRecommendTable.Count() <= 0)
            {
                msg = "No recommendations, Not enough user interaction to recommend";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            double?[] curuserShoppingList = ProductsRecommendTable[curuserid];
            double? maxSimilartiy = -100000;
            int similarUserIdx = -1;
            for (int i = 0; i < users.Count; i++)
            {
                if (Convert.ToString(users[i]) != curuserid)
                {
                    double?[] otheruserShoppingList = ProductsRecommendTable[Convert.ToString(users[i])];
                    double? similarity = ComputeCoeff(curuserShoppingList, otheruserShoppingList);
                    if (similarity > maxSimilartiy)
                    {
                        maxSimilartiy = similarity;
                        similarUserIdx = i;

                    }
                }

            }

            msg = "";
            if (similarUserIdx < 0)
            {
                msg = "No recommendations, Not enough user interaction to recommend";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

            double?[] recommendedList = ProductsRecommendTable[Convert.ToString(users[similarUserIdx])];
            //Array.Sort(recommendedList);
            List<string> recommendStringList = new List<string>();
            for (int i = 0; i < recommendedList.Length; i++)
            {
                if (recommendedList[i] != 0)
                {
                    recommendStringList.Add(products[i]);
                }
            }



            return Json(recommendStringList, JsonRequestBehavior.AllowGet);
        }


        public double? ComputeCoeff(double?[] values1, double?[] values2)
        {
            if (values1.Length != values2.Length)
                throw new ArgumentException("values must be the same length");
            var avg1 = values1.Average();
            var avg2 = values2.Average();
            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();
            var sumSqr1 = values1.Sum(x => Math.Pow((double)(x - avg1), 2.0));
            var sumSqr2 = values2.Sum(y => Math.Pow((double)(y - avg2), 2.0));
            var result = sum1 / Math.Sqrt(sumSqr1 * sumSqr2);
            return result;
        }

        /// <summary>
        /// LOGIN
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(E_Users user)
        {


            var usr = db.E_Userss.Single(u => u.Username == user.Username && u.Password == user.Password);
            if (usr != null)
            {

                Session["ID"] = usr.ID.ToString();
                Session["Username"] = user.Username.ToString();

                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Login");
            }


        }
        //public JsonResult AddComment(S_Comments coment)
        //{
        //    List<object> obj = new List<Object>();
        //    obj.Add(coment.Post_id);
        //    obj.Add(coment.userID);

        //    obj.Add(coment.Coment);
        //    obj.Add(coment.Com_time);

        //    object[] dataarray = obj.ToArray();
        //    int output = db.Database.ExecuteSqlCommand("insert into S_Comments(Post_id,userID,Coment,Com_Time)values(@p0,@p1,@p2,@p3)", dataarray);
        //    if (output > 0)
        //    {
        //        ViewBag.Message = "Added";
        //    }
        //    return(dataarray,JsonRequestBehavior.Allowget());

        //}
        //public ActionResult Addfrn(string id)

        //{
        //    string msg = " ";
        //    var ids = id;
        //    try
        //    {
        //        List<object> obj = new List<Object>();



        //        int output = db.Database.ExecuteSqlCommand("insert into S_frns(S_userID,S_frnID,Status)values(@p0,@p1,@p2)",Session["ID"],id,0);
        //        if (output > 0)
        //        {
        //            ViewBag.Message = "Added";
        //        }
        //        //return RedirectToAction("EmartInWall");
        //        //return Convert.ToString(output);
        //        msg = "Successfully Saved";
        //    }
        //    catch (Exception e)
        //    {
        //        msg = e.Message;

        //    }
        //    return Json(msg, JsonRequestBehavior.AllowGet);



        //}

        public JsonResult Addfrn(S_frns frn)
        {

            String msg = "";
            try
            {
                List<object> obj = new List<Object>();
                obj.Add(frn.S_userID);
                obj.Add(frn.s_frnID);

                obj.Add(frn.Status);


                object[] dataarray = obj.ToArray();
                int output = db.Database.ExecuteSqlCommand("insert into S_frns(S_userID,S_frnID,Status)values(@p0,@p1,@p2)", dataarray);
                if (output > 0)
                {
                    ViewBag.Message = "Added";
                }
                //return RedirectToAction("EmartInWall");
                //return Convert.ToString(output);
                msg = "Successfully Saved";
            }
            catch (Exception e)
            {
                msg = e.Message;

            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetFrnInfo(String id)
        {

            E_Users user = db.E_Userss.SqlQuery("Select * from E_Users where ID=@p0 ", id).SingleOrDefault();

            SocialViewModel mysocialviewmodel = new SocialViewModel();
            mysocialviewmodel.E_users = user;
            return PartialView("Profile", mysocialviewmodel);





        }

        public JsonResult GetPost()
        {

            List<S_Post> Posttlist = db.S_Posts.SqlQuery("select *from S_Post").ToList();

            //string test = '{"0":[{'PostContent':'katiyo katiyo katiyo'},{'S_userID':'12'},{'Likes':'0'},{'Comment':[]}],"1":[{'PostContent':'katiyo katiyo katiyo'},{'S_userID':'12'},{'Likes':'0'},{'Comment':[]}]}';

            string[] data = new string[Posttlist.Count];
            //string data = "{";
            for (int i = 0; i < Posttlist.Count; i++)
            {
                //data += "{"
                int idx = Posttlist.Count - i - 1;
                List<S_Comments> Commentlist = db.S_Commentss.SqlQuery("select *from S_Comments where Post_id=@p0", Posttlist[i].postID).ToList();
                data[idx] = "[{\"PostContent\":\"" + Posttlist[i].PostContent + "\"}," +
                           "{\"S_userID\":\"" + Posttlist[i].S_userID + "\"}," +
                           "{\"Likes\":\"" + Posttlist[i].Post_like + "\"}," +
                           "{\"Postimgid\":\"" + Posttlist[i].Postimgid + "\"}," +
                           "{\"Post_id\":\"" + Posttlist[i].postID + "\"},";

                data[idx] += "{\"Comment\":[";

                var commenttxt = "";

                for (int j = 0; j < Commentlist.Count; j++)
                {
                    data[idx] += "\"" + Commentlist[j].Coment + "\"," +
                                 "\"" + Commentlist[j].userID + "\",";
                    commenttxt = "something inserted";
                }

                if (commenttxt.Length > 0)
                {
                    data[idx] = data[idx].Substring(0, data[idx].Length - 1);
                }


                data[idx] += "]}]";

            }

            //Posttlist.
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NotificationWatcher()
        {
            var userid = Session["ID"];
            List<Notifications> frnsList = db.Notifications.SqlQuery("select * from Notifications where userid=@p0 and status=@p1", userid, 1).ToList();
            return Json(frnsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateNotificationStatus()
        {
            var userid = Session["ID"];
            int err = db.Database.ExecuteSqlCommand("UPDATE Notifications SET status=@p0 where userid=@p1",0,userid);
            string message = "Notification status updated";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadPostImage(S_Post post)

        {
            int ImageID = 0;
            var file = post.PostImage;
            byte[] imagbyte = null;

            if (file != null)
            {
                file.SaveAs(Server.MapPath("/uploadpostimage/" + file.FileName));

                BinaryReader reader = new BinaryReader(file.InputStream);
                imagbyte = reader.ReadBytes(file.ContentLength);
                UserPostImages img = new UserPostImages();
                img.ImageName = file.FileName;
                img.ImageByte = imagbyte;
                img.ImagePath = "/uploadpostimage/" + file.FileName;
                img.ImageisDeleted = false;
                db.UserPostImages.Add(img);
                db.SaveChanges();

                List<S_frns> frnsList = db.S_frnss.SqlQuery("select * from S_frns where s_frnID=@p0", post.S_userID).ToList();
                for(var i = 0; i < frnsList.Count; i++)
                {
                    int err = db.Database.ExecuteSqlCommand("insert into Notifications (userid,postid,status) values(@p0,@p1,@p2)",frnsList[i].S_userID,post.postID,1);
                }


                ImageID = img.ImageID;
                post.Postimgid = ImageID;
                S_Post p = new S_Post();
                List<object> obj = new List<Object>();
                obj.Add(post.PostTime);
                obj.Add(post.PostContent);

                obj.Add(post.Post_like);
                obj.Add(post.S_userID);
                obj.Add(post.Postimgid);

                object[] dataarray = obj.ToArray();
                int output = db.Database.ExecuteSqlCommand("insert into S_Post(PostTime,PostContent,Post_like,S_userID,Postimgid)values(@p0,@p1,@p2,@p3,@p4)", dataarray);
                if (output > 0)
                {
                    ViewBag.Message = "Added";
                }
            }

            return Json(ImageID, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostImageRetrive(int imgID)
        {
            EmartDBContexts db = new EmartDBContexts();
            var img = db.UserPostImages.SingleOrDefault(x => x.ImageID == imgID);
            return File(img.ImageByte, "image/jpg");
        }

        public JsonResult CheckFrn()
        {
            var suserid = Session["ID"];
            List<S_frns> c = db.S_frnss.SqlQuery("select * from S_frns where S_userID=@p0 and Status=@p1", suserid, false).ToList();
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddComments(S_Comments data)
        {
            var time = DateTime.Now;

            List<object> obj = new List<Object>();
            obj.Add(data.Post_id);
            obj.Add(data.userID);

            obj.Add(data.Coment);
            obj.Add(time);

            object[] dataarray = obj.ToArray();

            string msg = "";
            int output = db.Database.ExecuteSqlCommand("insert into S_Comments(Post_id,userID,Coment,Com_time) values(@p0,@p1,@p2,@p3)", dataarray);
            if (output > 0)
            {
                ViewBag.Message = "Added";
                msg = "Saved";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UploadImage(E_Users user)

        {
            int ImageID = 0;

            var file = user.UserImage;
            byte[] imagbyte = null;

            if (file != null)
            {

                file.SaveAs(Server.MapPath("/uploadimage/" + file.FileName));
                BinaryReader reader = new BinaryReader(file.InputStream);
                imagbyte = reader.ReadBytes(file.ContentLength);
                UserImageStore img = new UserImageStore();
                img.ImageName = file.FileName;
                img.ImageByte = imagbyte;
                img.ImagePath = "/uploadimage/" + file.FileName;
                img.ImageisDeleted = false;
                db.UserImageStore.Add(img);
                db.SaveChanges();



                E_Users b = new E_Users();


                E_Users usr = new E_Users();

                usr.ID = 5;
                usr.PhoneNumber = 100;
                usr.imgid = 10;
              db.E_Userss.SqlQuery("UPDATE E_Users SET imgid=@p0 where ID=@p1", img.ImageID, user.ID);
                db.SaveChanges();

                int output = db.Database.ExecuteSqlCommand("update E_Users set imgid=@p0 where ID=@p1 ", img.ImageID, user.ID);
                ImageID = img.ImageID;
                ImageID = user.ID;
                if (output > 0)
                {
                    ViewBag.Messge = "Added";
                }













            }

            return Json(ImageID, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImageRetrive(int imgID)
        {
            EmartDBContexts db = new EmartDBContexts();
            var img = db.UserImageStore.SingleOrDefault(x => x.ImageID == imgID);
            return File(img.ImageByte, "image/jpg");
        }
        public ActionResult ImagePostRetrive(int imgID)
        {
            EmartDBContexts db = new EmartDBContexts();
            var img = db.UserPostImages.SingleOrDefault(x => x.ImageID == imgID);
            return File(img.ImageByte, "image/jpg");
        }


        public ActionResult Member()
        {
            var usrid = Session["ID"];
            var usr = db.E_Userss.SqlQuery("Select * from E_Users where ID!= @p0",usrid).ToList();

       
          
            return View(usr);


        }
        public JsonResult Members()
        {
            var d = Session["ID"];
            var usrSS = db.E_Userss.SqlQuery("Select * from E_Users where ID!= @p0", d).ToList();
            return Json(usrSS, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult SendRequest(E_Users obj)
        {
            if (Session["UserID"] != null)
            {

                string name = Session["ID"].ToString();
                int nam = Int32.Parse(name);
                List<object> user = new List<object>();



                user.Add(nam);

                user.Add(obj.ID);
                user.Add(0);

                object[] userarray = user.ToArray();
                db.Database.ExecuteSqlCommand("Insert into E_Friends values(@p0,@p1,@p2)", userarray);

            }
            return View();
        }


         
            
        
        //public JsonResult Members()
        //{ 

           
        //        SocialViewModel a = new SocialViewModel();
        //        var idu = Session["ID"];
        //        var membertlist = db.E_Userss.SqlQuery("select * from E_Users where ID!=@p0", idu).ToList();
        //        //List<SocialViewModel> c = new Lis<SocialViewModel>();




        //        //    }

        //        return Json(membertlist, JsonRequestBehavior.AllowGet);

            
            
       

        //}

        public ActionResult ImageRetriveFromLogedUSer(int ID)
        {
           

            var usrimg = db.E_Userss.SqlQuery("Select imgid from E_Users where ID=@p0", ID).FirstOrDefault();
            Convert.ToInt32(usrimg);

            UserImageStore usr = db.UserImageStore.SqlQuery("Select * from UserImageStores where imgid=@p0", usrimg).FirstOrDefault();

        
           

            return File(usr.ImageByte,"image/jpg");


        }


        [HttpGet]
        public ActionResult ProfileView(String name)
        {
            if (Session["ID"] != null)
            {
                
                E_Users user = new E_Users();
                E_Users result = db.E_Userss.SqlQuery("Select * from E_Users where ID=@p0", name).FirstOrDefault();
                SocialViewModel mysocialviewmodel = new SocialViewModel();
                mysocialviewmodel.E_users = result;
                return View(mysocialviewmodel);

            }
            else
            {
                return RedirectToAction("Login");
            }

            //SocialViewModel socialuser = new SocialViewModel();
            //var users = db.E_Userss.SqlQuery("Select * from E_Users where ID = @p0",id).SingleOrDefault();
            //socialuser.E_users = users;
            //return View(socialuser);
        }
        public ActionResult Group()
        {
            var g = db.Groups.SqlQuery("Select * From Groups").ToList();
            return View(g);
        }
        public PartialViewResult NavBar()
        {
            var name = Session["Username"];
            E_Users usr = db.E_Userss.SqlQuery("Select * from E_Users where Username=@p0", name).FirstOrDefault();
            SocialViewModel views = new SocialViewModel();
            views.E_users = usr;
            return PartialView("_NavBar", views);
        }

        //public ActionResult ListPost()
        //{

        //    if (Session["ID"] != null)
        //    {
        //        SocialViewModel a = new SocialViewModel();
        //        var id = Session["ID"];
        //        var Posttlist = db.S_Posts.SqlQuery("select *from S_Post").ToList();
        //        //List<SocialViewModel> c = new Lis<SocialViewModel>();

        //        return View(Posttlist);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login");
        //    }




        //    //          List<SocialViewModel> r = new List<SocialViewModel>();

        //    //           var postlist = (from Cust in db.E_Userss
        //    //                               join Ord in db.S_Posts on Cust.ID equals Ord.S_userID 
        //    //select new{ Cust.Username, Cust.Email, Ord.Post_like, Ord.PostTime, Ord.PostContent}).ToList();
        //    //           //query getting data from database from joining two tables and storing data in customerlist



        //    //           foreach (var item in postlist)
        //    //           {
        //    //               SocialViewModel objcvm = new SocialViewModel(); // ViewModel
        //    //               objcvm.Username = item.Username.ToString();
        //    //               objcvm.Email = item.Email;
        //    //               objcvm.PostLike= item.Post_like;
        //    //               objcvm.PostTime = item.PostTime;
        //    //               objcvm.PostContent = item.PostContent;
        //    //               r.Add(objcvm);
        //    //           }
        //    //           //Using foreach loop fill data from custmerlist to List<CustomerVM>.
        //    //           return View(r); //List of CustomerVM (ViewModel)


        //}





      public JsonResult AddComment()
        {
            var a = 1;
            return Json(a);
        }

        public ActionResult ImageRetriveFromUSer(int UserID)
        {
            int result = db.Database.ExecuteSqlCommand("Select imgid from E_Users where ID=@p0", UserID);
            return RedirectToAction("ImageRetrive" + result);
        }

        public ActionResult ProfileViewUser(String id)
        {
            Convert.ToInt32(id);
            E_Users user = new E_Users();
            E_Users result = db.E_Userss.SqlQuery("Select * from E_Users where ID=@p0", id).FirstOrDefault();
            SocialViewModel mysocialviewmodel = new SocialViewModel();
            mysocialviewmodel.E_users = result;
            return View(mysocialviewmodel);




        }


        /// <summary>
        ///  PROFILES 
        /// </summary>
        /// <returns></returns>

        public new ActionResult Profile()
        {
            if (Session["ID"] != null)
            {
                var id = Session["ID"];
                E_Users user = new E_Users();
                E_Users result = db.E_Userss.SqlQuery("Select * from E_Users where ID=@p0", id).FirstOrDefault();
                SocialViewModel mysocialviewmodel = new SocialViewModel();
                mysocialviewmodel.E_users = result;
                return View(mysocialviewmodel);

            }
            else
            {
                return RedirectToAction("Login");
            }
        }




        public ActionResult Photos()
        {
            if (Session["ID"] != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }


        //POSTLIKE
        public JsonResult SubmitPostLike(String Data)
        {
            var user_id = Session["ID"];

            var date = DateTime.Now;
            var result = db.S_Likess.SqlQuery("Insert Into");
            if (result != null)
            {
                S_likes post = new S_likes();
                string output = "Update Successfully";
                return Json(output, JsonRequestBehavior.AllowGet);

            }
            else
            {
                string output = "try again";
                return Json(output, JsonRequestBehavior.AllowGet);
            }

        }


        ////            public JsonResult SubmitPostDislike(S_likes like)
        ////        {

        ////            var user_id = Session["ID"];
        ////            var post_id = like["likeID"];
        ////            var result= db.S_Likess.SqlQuery("DELETE from S_likess where user_id = user_id and post_id = post_id");)

        ////                           if (result != null)

        ////            {
        ////               var result= db.S_Likess.SqlQuery(" UPDATE S_likess set total_like = total_like - 1 where post_id = post_id");
        ////                S_likes post = new S_likes();
        ////                string output = "Update Successfully";
        ////                return Json(output, JsonRequestBehavior.AllowGet);

        ////            }
        ////            else
        ////            {
        ////                string output = "try again";
        ////                return Json(output, JsonRequestBehavior.AllowGet);
        ////            }
        ////        }
        ////        public JsonResult SubmitPostComment(S_Comments comtns)
        ////        {
        ////            var user_id = Session["ID"];
        ////            var post_id = comtns["post_id"];
        ////            var comment = comtns["comment"];
        ////            var date = DateTime.Now;
        ////            var result = db.S_Commentss.SqlQuery("Insert into S_Commentss(comID,Post_id,userID,Coment,Com_time)Values( ,post_id,user_id,comment,date");
        ////        }

        ////        public JsonResult getPostComment(S_Comments comts)
        ////        {
        ////            var comment_query = db.S_Commentss.SqlQuery("Select * from S_Commentss");
        ////            Array commentInfo = array();
        ////            foreach(S_Comments as commmment)
        ////            {
        ////                commentInfo[]= commentInfo;
        ////            }
        ////            return commentInfo;

        ////        }



        ////        //public JsonResult Upload(E_Users model)

        ////        //{
        ////        //    var file = model.ImageFile;
        ////        //    if (file != null)
        ////        //    {

        ////        //        var fileName = Path.GetFileName(file.FileName);
        ////        //        var exention = Path.GetExtension(file.FileName);
        ////        //        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

        ////        //        file.SaveAs(Server.MapPath("/UploadImages/" + file.FileName));


        ////        //    }
        ////        //    return Json(file.FileName, JsonRequestBehavior.AllowGet);

        ////        //}
        ////    }



        ////}
    }
}