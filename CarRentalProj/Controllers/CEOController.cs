using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CarRentalProj.CSCode;
using CarRentalProj.Models;
using System.IO;
using System.Drawing;

namespace CarRentalProj.Controllers
{
    public partial class CRSSController : Controller // Extention to CRSS
    {
        [Authorize(Users = "Admin")]
        public ActionResult CEO()
        {
            return View();
        }//the 'CEO' View Page has every access to every table in database to manipulate.

        #region Orders
        [Authorize(Users = "Admin")]
        public ActionResult CEO_OrderCMD()
        {
            return View();
        }
        #region EditRent
        [Authorize(Users = "Admin")]
        [HttpGet]
        public ActionResult EditRent(int id)
        {
            var rmodel = CEO_Actions.GetOrder(id);
            return View(rmodel); // do this with the VS build in create View 
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult EditRent(ReservedCar Updated)
        {
            if (ModelState.IsValid)
            {
                if (Updated.StartDate < Updated.EndDate)
                {
                    bool result;
                    CEO_Actions.DynamicUpdateToDB(Updated, out result,null);
                    if (result)
                    {
                        return RedirectToAction("CEO_OrderCMD");
                    }
                }
            }
            return View(Updated);
        }
        #endregion
        [Authorize(Users = "Admin, Emp")]
        public JsonResult OrderDetails(int id)
        {
            var obj = CEO_Actions.GetOrder(id);
            var JsonObj = TurnJsonFrom.That(obj);
            return Json(JsonObj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cars
        [Authorize(Users = "Admin")]
        public ActionResult CEO_CarsCMD()
        {
            return View();
        }
        [Authorize(Users = "Admin")]
        public JsonResult CarDetails(string id)
        {
            var obj = CEO_Actions.GetCar(id);
            var JsonObj = TurnJsonFrom.That(obj);
            return Json(JsonObj, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Users = "Admin")]
        public JsonResult SearchCar(string name, string by)
        {
            if (by == "model")
            {
                var JSONobj = CEO_Actions.SearchCarName(name);
                return Json(JSONobj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var JSONobj = CEO_Actions.SearchCarId(name);
                return Json(JSONobj, JsonRequestBehavior.AllowGet);
            }
        }
        #region EditCar
        [Authorize(Users = "Admin")]
        [HttpGet]
        public ActionResult EditCar(string id)
        {
            var rmodel = CEO_Actions.GetCar(id);
            return View(rmodel); 
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult EditCar(Car Updated)
        {
            if (ModelState.IsValid)
            {
                bool result;
                CEO_Actions.DynamicUpdateToDB(Updated, out result,null);
                if (result)
                {
                    return RedirectToAction("CEO_CarsCMD");
                }
            }
            return View(Updated);
        }
        #endregion
        #endregion

        #region CarType / AKA CarDetails class from EF
        [Authorize(Users = "Admin")]
        public ActionResult CEO_CarTypeCMD()
        {
            return View();
        }
        [Authorize(Users = "Admin")]
        public JsonResult CarTypeDetails(string id)
        {
            var obj = CEO_Actions.GetCarType(id);
            var JsonObj = TurnJsonFrom.That(obj);
            return Json(JsonObj, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Users = "Admin")]
        public JsonResult SearchCarType(string model, string GT , bool only, string company)
        {
            Console.WriteLine();
            var list = CEO_Actions.SearchCarType(model, GT , only , company);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult EditCarType(string id)
        {
            var rmodel = CEO_Actions.GetCarType(id);
            Session["oldfile"] = rmodel.Pic;
            return View(rmodel);
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult EditCarType(CarDetail Updated ,string old)
        {
            if (ModelState.IsValid)
            {
                bool result;
                CEO_Actions.DynamicUpdateToDB(Updated, out result,old);
                if (result)
                {
                    return RedirectToAction("CEO_CarTypeCMD");
                }
                else
                    ViewBag.MsgError = "Car Type already made";
            }
            return View(Updated);
        }
        #endregion

        #region Users
        [Authorize(Users = "Admin")]
        public ActionResult CEO_UsersCMD()
        {
            return View();
        }
        [Authorize(Users = "Admin")]
        public JsonResult SearchUsers(string par, string by)
        {
            if (by == "name")
            {
                var JSONobj = CEO_Actions.SearchUsersByName(par);
                return Json(JSONobj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var JSONobj = CEO_Actions.SearchUsersById(par);
                return Json(JSONobj, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Users = "Admin")]
        public JsonResult UserDetails(string id)
        {
            var obj = CEO_Actions.GetUser(id);
            var JsonObj = TurnJsonFrom.That(obj);
            return Json(JsonObj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult EditUser(string id)
        {
            var rmodel = CEO_Actions.GetUser(id);
            return View(rmodel);
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult EditUser(User Updated)
        {
            if (ModelState.IsValid)
            {
                bool result;
                CEO_Actions.DynamicUpdateToDB(Updated, out result,null);
                if (result)
                {
                    return RedirectToAction("CEO_UsersCMD");
                }
                else
                    ViewBag.MsgError = "Email already Taken";
            }
            return View(Updated);
        }
        #endregion

        #region Workers / Employee
        [Authorize(Users = "Admin")]
        public ActionResult CEO_EmpsCMD()
        {
            return View();
        }
        [Authorize(Users = "Admin")]
        public JsonResult SearchEmps(string par, string by)
        {
            if (by == "name")
            {
                var JSONobj = CEO_Actions.SearchEmpsByName(par);
                return Json(JSONobj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var JSONobj = CEO_Actions.SearchEmpsById(par);
                return Json(JSONobj, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult EditEmp(string id)
        {
            var rmodel = CEO_Actions.GetEmp(id);
            return View(rmodel);
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult EditEmp(Worker Updated)
        {
            if (ModelState.IsValid)
            {
                bool result;
                CEO_Actions.DynamicUpdateToDB(Updated, out result,null);
                if (result)
                {
                    return RedirectToAction("CEO_EmpsCMD");
                }
            }
            return View(Updated);
        } 
        #endregion

        #region Create Options
        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult CreateUser(User NRow)
        {
            if (ModelState.IsValid)
            {
                bool result;
                CEO_Actions.DynamicCreateRowToDB(NRow, out result);
                if (result)
                {
                    return RedirectToAction("CEO_UsersCMD");
                }
                ViewBag.MsgError = "Email/Id already Taken";
            }
            
            return View(NRow);
        }

        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult CreateCar()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult CreateCar(Car NRow)
        {
            if (ModelState.IsValid)
            {
                bool result;
                CEO_Actions.DynamicCreateRowToDB(NRow, out result);
                if (result)
                {
                    return RedirectToAction("CEO_CarsCMD");
                }
                ViewBag.MsgError = "Car ID Number already taken";
            }
            return View(NRow);
        }

        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult CreateCarType()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult CreateCarType(CarDetail NRow)
        {
            if (ModelState.IsValid)
            {
                bool result;
                CEO_Actions.DynamicCreateRowToDB(NRow, out result);
                if (result)
                {
                    return RedirectToAction("CEO_CarTypeCMD");
                }
                ViewBag.MsgError = "This Car Model was already made !";
            }
             return View(NRow);
        }

        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult CreateOrder()
        {
            return View(); 
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult CreateOrder(ReservedCar NRow)
        {
            if (ModelState.IsValid && NRow.StartDate < NRow.EndDate)
            {
                bool result;
                CEO_Actions.DynamicCreateRowToDB(NRow, out result);
                if (result)
                {
                    return RedirectToAction("CEO_OrderCMD");
                }
                ViewBag.MsgError = "Rent Dates are clashing with other Rent Orders with the same Car (Date Over Lap)";
                ViewBag.FK = "Besides that , either UserID/CarID were invalid (don't exist)";
            }
            return View(NRow);
        }

        [HttpGet]
        [Authorize(Users = "Admin")]
        public ActionResult CreateEmployee()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult CreateEmployee(Worker NRow)
        {
            if (ModelState.IsValid)
            {
                bool result;
                CEO_Actions.DynamicCreateRowToDB(NRow, out result);
                if (result)
                {
                    return RedirectToAction("CEO_EmpsCMD");
                }
                ViewBag.MsgError = "ID number already taken !";

            }
            return View(NRow);
        }
        #endregion

        [Authorize(Users ="Admin")]
        public JsonResult RemoveRow(string RowId , string type)
        {
            bool result;
            CEO_Actions.DynamicRemoveRowFromDB(RowId, type, out result);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Users = "Admin")]
        public JsonResult GetCarPic(FormCollection data)
        {
            string filename = Request.Form["Name"];
            filename = filename.Substring(0, 5);
            if (Request.Files["files"] != null && filename.Length == 5)
            {
                CEO_Actions.SaveCarImage(Request.Files["files"] , filename);
            }
                return Json(true, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Users = "Admin")]
        public JsonResult GetCarPic2(FormCollection data)
        {
            string filename = Request.Form["Name"];
            filename = filename.Substring(0, 5);
            Utility.CheckForOldJpg(Session["oldfile"] as string); // this deletes obsolete jpg files that aren't used by ANY of the CarDetails in DB
            if (Request.Files["files"] != null && filename.Length == 5)
            {
                CEO_Actions.SaveCarImage(Request.Files["files"], filename);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}