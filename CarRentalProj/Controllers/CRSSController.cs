using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using CarRentalProj.CSCode;
using CarRentalProj.Models;
using System.Globalization;

namespace CarRentalProj.Controllers
{
    //Server side controller web.
    public partial class CRSSController : Controller
    {
        // GET: CRSS
        [HttpGet]
        public ActionResult Main()
        {
            if (User.Identity.IsAuthenticated)
            {
                switch (User.Identity.Name)
                {
                    default:
                        {
                            return View();
                        }
                    case "Admin":
                        {
                            return View("CEO");
                        }
                    case "Emp":
                        {
                            return View("EmployeePage");
                        }
                }

            }
            return View();
        }// the 'Main' View Page is to sign in only.

        [HttpPost]
        public ActionResult Main(SignInEmp input)
        {
            if (ModelState.IsValid)
            {
                string role;
                if (EmployeeOperations.TryToSignIn(input, out role))
                {
                    switch (role)
                    {
                        default:{ return View("Main"); }
                        case "Admin":
                            {
                                return RedirectToAction("CEO");
                            }
                        case "Emp":
                            {
                                return RedirectToAction("EmployeePage");
                            }
                    }
                }
            }
            return View(input);
        }

        public ActionResult Exit()
        {
            Session.Abandon();
            Response.Cookies[FormsAuthentication.FormsCookieName].Expire‌​s = DateTime.Now.AddDays(-1);
            FormsAuthentication.SignOut();
            return RedirectToAction("Main");
        }

        #region Employee Main Ctrl Functions
        [Authorize(Users ="Emp, Admin")]
        public ActionResult EmployeePage()
        {
            return View();
        }// Employee only have the option to return Cars and Delete Users
        [Authorize(Users = "Emp, Admin")]
        public JsonResult Retrive(int? id, string R, int K)
        {
            try
            {
                if (K > 0)
                {
                    DateTime Returned = DateTime.ParseExact(R, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    bool result;
                    var All = EmployeeOperations.TryToRetrive((int)id, Returned, K, out result);
                    return Json(All, JsonRequestBehavior.AllowGet);
                }
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                bool result;
                var All = EmployeeOperations.TryToRetrive((int)id, null, 0,  out result);
                return Json(All, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Employee Search (Also Used By CEO)
        [Authorize(Users = "Emp, admin")]
        public JsonResult ByUser(string UserID, bool? Inc)
        {
            var r = EmployeeOperations.SearchByUser(UserID,(bool)Inc);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Users = "Emp, admin")]
        public JsonResult ByCar(string CarID, bool? Inc)
        {
            var r = EmployeeOperations.SearchByID(CarID, (bool)Inc);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        
    }

    
} 