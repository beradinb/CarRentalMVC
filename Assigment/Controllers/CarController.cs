using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assigment.Models;
using Assigment.ViewModels;
using System.Net;

namespace Assigment.Controllers
{
    public class CarController : Controller
    {
        public ActionResult UserArea()
        {
            if (Session["LoggedIn"] != null)
            {
                ViewData["Message"] = "Welcome " + Session["username"];

            }
            else
            {
                Response.Redirect("../User/Login"); //making sure you substitute “Login” for whatever your login method is actually called if different
            }
            var carEntities = new MyDBEntities();
            return View(carEntities.Cars.ToList());
        }

        [HttpGet]
        public ActionResult Rent (int? id)
        {

            
            if (Session["LoggedIn"] != null)
            {
                var db = new MyDBEntities();
                var carQ = db.Cars.Find(id);
                return View(carQ);
            }
            else
            {
                Response.Redirect("../User/Login"); //making sure you substitute “Login” for whatever your login method is actually called if different
            }
            return View();
        }

        [HttpPost]
        public ActionResult Rent (int? id, Car model, Booking carB, string sday, string eday)
        {
            var db = new MyDBEntities();
            db.Bookings.Add(new Booking
            {
                carID = model.Id,
                userID = (int)Session["id"],
                startDay = sday,
                endDay = eday,
                //CostPD = model.CostPD
            });
            db.SaveChanges();

            return Redirect("../DisplayUserBookings");
        }

        [HttpGet]
        public ActionResult DisplayUserBookings()
        {
            if (Session["LoggedIn"] != null)
            {
                var db = new MyDBEntities();
                var uid = (int)Session["id"];
                var carBooking = db.Bookings.Where(i => i.userID.Equals(uid));
                return View(carBooking.ToList());
            }
            else
            {
                return Redirect("../User/Login"); //making sure you substitute “Login” for whatever your login method is actually called if different
            }
            
        }

        public ActionResult DisplayCars()
        {
            var entities = new MyDBEntities();
            return View(entities.Cars.ToList());
        }

        [HttpGet]
        public ActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCar(CarVM models)
        {
            if (ModelState.IsValid)
            {
                var db = new MyDBEntities();

                db.Cars.Add(new Car
                {
                    Make = models.Make,
                    Model = models.Model,
                    CostPD = models.CostPD,
                    Quantity = models.Quantity
                });
                db.SaveChanges();
                return RedirectToAction("DisplayCars", "Car");
            }
            return View(models);
        }

        [HttpGet]
        public ActionResult UpdateCar(int? id)
        {
            var db = new MyDBEntities();
            CarVM vmodel = new CarVM();



            if (Session["LoggedIn"] != null)
            {

                if (Session["id"] == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var carToUpdate = db.Cars.Find((int)id);

                vmodel.Id = (int) id;
                vmodel.Make = carToUpdate.Make;
                vmodel.Model = carToUpdate.Model;
                vmodel.CostPD = carToUpdate.CostPD;
                vmodel.Quantity = carToUpdate.Quantity;

            }
            else
            {
                Response.Redirect("../User/Login"); //making sure you substitute “Login” for whatever your login method is actually called if different
            }

            return View(vmodel);
        }

        [HttpPost]
        public ActionResult UpdateCar(int? id, CarVM vmodel)
        {
            if (Session["LoggedIn"] != null)
            {

                var db = new MyDBEntities();
                var carToUpdate = db.Cars.Find((int)id);

                if (ModelState.IsValid)
                {
                    if (carToUpdate != null)
                    {
                        carToUpdate.Id = (int)id;
                        carToUpdate.Make = vmodel.Make;
                        carToUpdate.Model = vmodel.Model;
                        carToUpdate.CostPD = vmodel.CostPD;
                        carToUpdate.Quantity = vmodel.Quantity;
                        db.SaveChanges();
                    }
                }
                ViewData["Message"] = "Record updated";
                return RedirectToAction("DisplayCars", "Car");

            }
            else
            {
                Response.Redirect("../User/Login"); //making sure you substitute “Login” for whatever your login method is actually called if different
            }

            return View(vmodel);
        }





    }
}