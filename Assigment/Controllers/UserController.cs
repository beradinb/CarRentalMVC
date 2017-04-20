using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assigment.Models;
using Assigment.ViewModels;
using System.Web.Helpers;
using System.Net;


namespace Assigment.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplayUsers()
        {
            var entities = new MyDBEntities();
            return View(entities.users.ToList());
        }


        [HttpGet]
        public ActionResult CheckUserExists()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckUserExists(user model)
        {
            if (ModelState.IsValid)
            {
                var db = new MyDBEntities();

                var v = db.users.Where(u => u.Username.Equals(model.Username)).FirstOrDefault();
                if (v != null)
                {
                    ViewData["Message"] = "Record exists";
                }
                else
                {
                    ViewData["Message"] = "Fail";
                }
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(user model)
        {
            //var hashedPassword = Crypto.HashPassword(model.Password);
            var hashed = Assigment.HashClass.Encode(model.Password);
            if (ModelState.IsValid)
            {
                var db = new MyDBEntities();
                var admin = 0;
                if ((int)Session["isAdmin"] == 1)
                {
                    admin = 1;
                }
                db.users.Add(new user
                {
                    Firstname = model.Firstname,
                    Surname = model.Surname,
                    Username = model.Username,
                    EmailAddress = model.EmailAddress,
                    Password = hashed,
                    isAdmin = admin
                });
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLoginVM model) //notice we’re using the ViewModel
        {
            if (ModelState.IsValid)
            {
                //var hashedPassword = Crypto.HashPassword(model.Password);
                var hashed = HashClass.Encode(model.Password);
                var db = new MyDBEntities();
                var v = db.users.Where(u => u.Username.Equals(model.Username) && u.Password.Equals(hashed)).FirstOrDefault();

                if (v != null)
                {
                    ViewData["Message"] = "Login Successful";
                    Session["loggedIn"] = true;
                    Session["user"] = v.Username;
                    Session["id"] = v.Id;
                    Session["isAdmin"] = v.isAdmin;
                    if ((int)Session["isAdmin"] == 1)
                    {
                        return RedirectToAction("DisplayCars", "Car");
                    }
                    else
                    {
                        return RedirectToAction("UserArea", "Car");
                    } 
                    

                }
                else
                {
                    ViewData["Message"] = "Login Unsuccessful";
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRegistrationVM model)
        {
            if (ModelState.IsValid)
            {
                //var hashedPassword = Crypto.HashPassword(model.Password);
                var hashed = Assigment.HashClass.Encode(model.Password);
                var db = new MyDBEntities();
                var admin = 0;
                if (Session["isAdmin"] != null)
                { 
                    if ((int)Session["isAdmin"] == 1)
                    {
                        admin = 1;
                    }
                }
                db.users.Add(new user
                {
                    Firstname = model.Firstname,
                    Surname = model.Surname,
                    Username = model.Username,
                    EmailAddress = model.EmailAddress,
                    Password = hashed,
                    isAdmin = admin
                });
                db.SaveChanges();
                return RedirectToAction("Login", "User");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult doesUserNameExist(string Username)
        {
            var db = new MyDBEntities(); //Where Entities is replaced by the name of YOUR entities
            return Json(!db.users.Any(u => u.Username == Username), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update()
        {
            var db = new MyDBEntities();
            UserRegistrationVM vmodel = new UserRegistrationVM();



            if (Session["LoggedIn"] != null)
            {

                if (Session["id"] == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var userToUpdate = db.users.Find((int)Session["id"]);

                vmodel.Id = ((int)Session["id"]);
                vmodel.Firstname = userToUpdate.Firstname;
                vmodel.Surname = userToUpdate.Surname;
                vmodel.Username = userToUpdate.Username;
                vmodel.Password = userToUpdate.Password;
                vmodel.EmailAddress = userToUpdate.EmailAddress;

            }
            else
            {
                Response.Redirect("../User/Login"); //making sure you substitute “Login” for whatever your login method is actually called if different
            }
            
            return View(vmodel);
        }

        [HttpPost]
        public ActionResult Update(UserRegistrationVM model)
        {
            if (Session["LoggedIn"] != null)
            {
                
                var db = new MyDBEntities();
                var userToUpdate = db.users.Find((int)Session["id"]);
                var hashed = Assigment.HashClass.Encode(model.Password);

                if (ModelState.IsValid)
                {
                    if (userToUpdate != null)
                    {
                        userToUpdate.Id = (int)Session["id"];
                        userToUpdate.Firstname = model.Firstname;
                        userToUpdate.Surname = model.Surname;
                        userToUpdate.Username = model.Username;
                        userToUpdate.Password = hashed;
                        userToUpdate.EmailAddress = model.EmailAddress;
                        db.SaveChanges();
                    }
                }
                ViewData["Message"] = "Record updated";

            }
            else
            {
                Response.Redirect("../User/Login"); //making sure you substitute “Login” for whatever your login method is actually called if different
            }
           
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            var db = new MyDBEntities();
            var userToDelete = db.users.Find(id);
            if (userToDelete != null)
            {
                user user = userToDelete;
                db.users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("DisplayUsers", "User");
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return Redirect("../Home");
        }





    }
}