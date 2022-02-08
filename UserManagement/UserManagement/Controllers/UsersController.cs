using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class UsersController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Users
        public ActionResult Index()
        {

            if (Session["UserId"] != null)
            { return View(db.Users.ToList()); }

            return RedirectToAction("Login", "Users");
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,username,password,confirmPassword,type")] User user)
        {

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,username,password,confirmPassword,type")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(String Username, String Password, String Confirmpassword, String type)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(u => u.username == Username).FirstOrDefault();
                if (Password.Equals(Confirmpassword) && user == null)
                {
                    User userobj = new Models.User();


                    userobj.password = Password;
                    userobj.confirmPassword = Confirmpassword;
                    userobj.type = type;
                    userobj.username = Username;
                    db.Users.Add(userobj);

                    db.SaveChanges();
                    Session["UserID"] = Username;
                    Session["UserType"] = type;
                    ModelState.Clear();
                    ViewBag.Message = "Successfully Registered";
                    if (type == "Pollster")
                    {
                        return RedirectToAction("Login", "Users");
                    }
                    else
                    {
                        return RedirectToAction("AdminIndex", "Home");
                    }
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.Message = "Registeration Failed";
                }
            }

            return View();

        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(String Username, String Password)
        {
            string type = "Pollster";
            var user = db.Users.Where(u => u.username == Username && u.password == Password && u.type == type).FirstOrDefault();
            if (user != null)
            {

                //Session["UserID"] = user.username.ToString();
                //Session["UserType"] = user.type.ToString();
                Session["UserId"] = user.Id=19;
                Session["UserName"] = user.username.ToString();
                Session["UserType"] = user.type.ToString();
                if (type == "Pollster")
                {
                    ViewBag.loginmessage = "Login successfully";
                    return RedirectToAction("PollsterIndex", "Home");
                }
                else
                {
                    ViewBag.loginmessage = "Login successfully";
                    return RedirectToAction("AdminPage", "Home");
                }

            }
            else
            {
                ModelState.AddModelError("", "Incorrect Username or Password");

                ViewBag.loginerrormessage = "Incorrect Username or Password";
                return View();
            }

        }
        public ActionResult Mnopqrabc123()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Mnopqrabc123(String Username, String Password)
        {
            string type = "Admin";
            var user = db.Users.Where(u => u.username == Username && u.password == Password && u.type == type).FirstOrDefault();
            if (user != null)
            {

                //Session["UserID"] = user.username.ToString();
                //Session["UserType"] = user.type.ToString();
                Session["UserId"] = user.Id;
                Session["UserName"] = user.username.ToString();
                Session["UserType"] = user.type.ToString();
                if (type == "Admin")
                {
                    return RedirectToAction("AdminPage", "Home");
                }
                else
                {
                    return RedirectToAction("Mnopqrabc123", "Users");
                }

            }
            else
            {
                ModelState.AddModelError("", "Incorrect Username or Password");
                return View();
            }

        }




        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecoverPassword(String currentPassword, String newPassword1, String newPassword2)
        {

            var Username = Session["Username"].ToString();
            if (ModelState.IsValid && Username != null)
            {
                var user = db.Users.Where(u => u.username == Username && u.password == currentPassword).FirstOrDefault();
                if (user != null)
                {
                    if (newPassword1 != newPassword2)
                    {
                        ViewBag.errorMsg = "Passwords are not match";
                    }
                    if (newPassword1 == "" && newPassword2 == "")
                    {
                        ViewBag.errorMsg = "Passwords are required";
                    }
                    else
                    {
                        User userobj = new Models.User();

                        userobj.password = newPassword1;
                        userobj.confirmPassword = newPassword2;

                        db.Users.Add(userobj);
                        if (ModelState.IsValid)
                        {

                            db.Entry(userobj).State = EntityState.Modified;
                            db.SaveChanges();

                            if (userobj.type == "Pollster")
                            {
                                return RedirectToAction("Login", "Users");
                            }
                            else
                            {
                                return RedirectToAction("AdminIndex", "Home");
                            }
                        }
                        else
                        {
                            ModelState.Clear();
                            ViewBag.Message = "Password is not changed";
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "Current paswword is not match";
                }
            }

            return RedirectToAction("Login", "Users");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login", "Users");
        }

        public ActionResult allPollster()
        {
            try
            {
                if (Session["UserType"].ToString() == "Admin" && Session["UserName"].ToString() != null)
                {
                    var countPollster = from p in db.Users where p.type == "Pollster" select p.username;
                    return View(countPollster.ToList());
                }
                return RedirectToAction("Login", "Users");
            }

            catch (Exception ex)
            {
                return RedirectToAction("Login", "Users");
            }

        }

        public ActionResult GetallPollster()
        {
            try
            {
                if (Session["UserType"].ToString() == "Admin" && Session["UserName"].ToString() != null)
                {
                    var countPollster = from p in db.Users where p.type == "Pollster" select p;
                    return View(countPollster.ToList());
                }
                else
                {
                    return RedirectToAction("Login", "Users");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Users");
            }
        }

        public ActionResult GetallSurveyss()
        {
            try
            {
                if (Session["UserType"].ToString() == "Admin" && Session["UserName"].ToString() != null)
                {
                    var surveycounterr = (from s in db.Surveys select s);
                    return View(surveycounterr.ToList());
                }
                else
                {
                    return RedirectToAction("Login", "Users");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Users");
            }
        }


        public ActionResult countSurvey()
        {
            if (Session["UserType"].ToString() == "Admin" && Session["UserName"].ToString() != null)
            {
                var surveycounterr = (from s in db.Surveys select s).Count();
                ViewBag.surveyCount = surveycounterr;
                return View(ViewBag.surveyCount);
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }
    }
}