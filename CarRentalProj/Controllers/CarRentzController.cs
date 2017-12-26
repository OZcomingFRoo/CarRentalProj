using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Web.Security;
using CarRentalProj.CSCode;
using CarRentalProj.Models;

namespace CarRentalProj.Controllers
{
    public class CarRentzController : Controller
    {
        // GET: CarRentz
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ListOfCars(Models.Filter input)
        {
            if (input.StartDate < input.EndDate)
            {
                var Coll = ClientOperation.SearchCars(input).ToList();
                var FR = new FilteredReuslt(Coll);
                ViewData.Add("Filter", input); // the Filter input dragged along with the results
                ViewBag.Len = FR.Lenght; // Lenght of the whole list
                ViewBag.Int = input.PageNum;
                return View(FR.Get(input.PageNum));
            }
            else
                return RedirectToAction("Home");
        }
         
        public ActionResult CarDetails(string CID , DateTime? SD , DateTime? ED , string Err)
        {
            ViewBag.MsgError = "";
            if (!string.IsNullOrEmpty(Err))
                ViewBag.MsgError = Err;
            if (SD is DateTime && ED is DateTime)
            {
                var TheCar = ClientOperation.GetByCarID(CID, (DateTime)SD, (DateTime)ED);
                return View(TheCar); 
            }//from List of Cars
            else
            {
                var TheCar = ClientOperation.GetByCarID(CID, DateTime.Now, DateTime.Now.AddDays(1));
                return View(TheCar);
            }//From Cookie
        }

        #region Simple Pages to present !!!
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Policy()
        {
            return View();
        }
        #endregion//Check

        #region SignUpCode Actions
        [HttpGet]
        public ActionResult SignUp()
        {
            if (Session["Id"] == null && Session["PSWD"] == null)
            {
                return View();
            }
            else
                return RedirectToAction("Home");
        }
        [HttpPost]
        public ActionResult SignUp(CarRentalProj.User NewComer)
        {
           if (ModelState.IsValid)
            {
                if (ClientOperation.TryToSignUp(NewComer))
                {
                    Session.Add("Id", NewComer.IDNumber);
                    Session.Add("PSWD", NewComer.UserPassword);
                    FormsAuthentication.SetAuthCookie("User", false);
                    Response.Cookies.Add(new HttpCookie("Id", NewComer.IDNumber) { Expires = DateTime.Now.AddDays(30) });
                    Response.Cookies.Add(new HttpCookie("PSWD", Encryptor.Encrypt(NewComer.UserPassword)) { Expires = DateTime.Now.AddDays(30) });
                    if (string.IsNullOrEmpty(Session["url"] as string)) 
                    {
                        return RedirectToAction("Home");
                    }
                    else
                    {
                        string URL = Session["url"] as string;
                        Session.Remove("url");
                        return Redirect(URL);
                    }
                }
                else
                    ViewBag.MsgError = "Email/Id already Taken";
            }
          return View();
        }
        #endregion
        
        #region Edit Profile
        [HttpGet]
        [Authorize(Users = "User")]
        public ActionResult EditProfile()
        {
            var rmodel = ClientOperation.GetUserDetails(Session["Id"] as string);
            return View(rmodel);
        }
        [HttpPost]
        [Authorize(Users = "User")]
        public ActionResult EditProfile(User Updated)
        {
            if (ModelState.IsValid)
            {
                if (ClientOperation.TryToUpdateProfile(Updated))
                    return View("ManageAccount", Updated);
            }
            return View(Updated);
        } 
        #endregion

        #region Sign In and Out Code Actions
        [HttpGet]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ErrorMsg = "";
            if (returnUrl != null)
            {
                if (returnUrl.Contains("CRSS") == false)
                {
                    Session["url"] = returnUrl;
                    if (Session["Id"] == null && Session["PSWD"] == null)
                    {
                        return View();
                    }
                    else
                        return RedirectToAction("Home");
                }
                else
                    return RedirectToAction("Main", "CRSS");
            }
            else
                return View();
        }
        [HttpPost]
        public ActionResult SignIn(SignInModel input)
        {
            if (ClientOperation.TryToLogin(input.Id, input.Password))
            {
                Session.Add("Id", input.Id);
                Session.Add("PSWD", input.Password);
                if(input.Remember)
                {
                    Response.SetCookie(new HttpCookie("Id", input.Id) { Expires = DateTime.Now.AddDays(30) });
                    Response.SetCookie(new HttpCookie("PSWD", Encryptor.Encrypt(input.Password)) { Expires = DateTime.Now.AddDays(30) });
                }
                if (string.IsNullOrEmpty(Session["url"] as string))
                {
                    return RedirectToAction("Home"); 
                }
                else
                {
                    string URL = Session["url"] as string;
                    Session.Remove("url");
                    return Redirect(URL);
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Password or Id number don't match";
                return View();
            }
                
        }
        public ActionResult LogOut()
        {
            Response.Cookies.Add(new HttpCookie("Id", "") {Expires = DateTime.Now.AddDays(-1) });
            Response.Cookies.Add(new HttpCookie("PSWD", "") { Expires = DateTime.Now.AddDays(-1) });
            Response.Cookies[FormsAuthentication.FormsCookieName].Expire‌​s = DateTime.Now.AddDays(-1);// This code does what Session.Abandon(); doesn't DO FOR SOME REASON
            Session.Abandon(); // This Code right is SUPPOSE TO Remove the Auth.. Cookie , but it seems like it doesn't feel like it.
            Session.Remove("Id");
            Session.Remove("PSWD");
            return RedirectToAction("Home");
        }
        #endregion

        #region Authorized Pages

        [Authorize(Users = "User")]
        public ActionResult ManageCars()
        {
            if (Session["Id"] != null && Session["PSWD"] != null)
            {
                var MyCars = ClientOperation.MyCars(Session["Id"] as string);
                return View(MyCars.ToList());
            }
            else
                return View("Home");

        }//For logged Users ONLY !!!!!

        [Authorize(Users = "User")]
        public ActionResult ManageAccount(string userid)
        {
            var rmodel = ClientOperation.GetUserDetails(userid);
            return View(rmodel);
        }//For logged Users ONLY !!!!!

        [Authorize(Users = "User")]
        public ActionResult RentPayment(string CID, string SD, string ED, int? DCost) // NewRent = NR
        {
            DateTime Start = DateTime.ParseExact(SD, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime End = DateTime.ParseExact(ED, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (Start < End)
            {
                OrdeReceipt NR = new OrdeReceipt(Session["Id"] as string, CID, Start, End, (short)DCost);
                bool result = ClientOperation.TryToRent(NR.UserID, NR.CarID, NR.Start, NR.End);
                if (result)
                {
                    return RedirectToAction("ManageCars");
                }
            }
                string Error = "A date OverLap occured, suggestion :search car in watned dates, make sure (ErrType: Car already Rented for these dates)";
                return RedirectToAction("CarDetails", new { CID = CID, SD = Start, ED = End , Err = Error});
            }

        #endregion

        #region Web API
        public JsonResult NameOfModels(string str)
        {
            var r = Json(CSCode.Utility.ModelTags(str), JsonRequestBehavior.AllowGet);
            return r;
        }

        public JsonResult CHKSess()
        {
            if (Session["Id"] != null && Session["PSWD"] != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}