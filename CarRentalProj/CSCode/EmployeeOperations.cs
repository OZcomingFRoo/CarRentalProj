using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using CarRentalProj.Models;
using CarRentalProj.CSCode;
using System.Web.Mvc;

namespace CarRentalProj.CSCode
{
    public static class EmployeeOperations
    {
        private static CarRentzEntities DB;

        public static bool TryToSignIn(SignInEmp input ,out string Role)
        {
            using (DB = new CarRentzEntities())
            {
                var OldUser = DB.Workers.SingleOrDefault(item => item.Id == input.Identity && item.PSWD == input.Password);
                if (OldUser != null) // Check to see if User Exists 
                {
                    Role = OldUser.profession;
                    FormsAuthentication.SetAuthCookie(Role, false);
                    return true;
                }
            }
            Role = "Error";
            return false;
        }

        public static IEnumerable<OrderItem> SearchByID(string CarIDNumber, bool Only)
        {
            using (DB = new CarRentzEntities())
            {
                var RequestedItem = new List<ReservedCar>();
                if (CarIDNumber != "" && CarIDNumber != null)
                {
                    RequestedItem = (from item in DB.ReservedCars
                                     where item.CarID.Contains(CarIDNumber)
                                     select item).ToList();
                }
                else
                    RequestedItem = DB.ReservedCars.ToList();
                if (Only)
                    RequestedItem = RequestedItem.Where(it => it.ReturnedDate != null).ToList();
                List<OrderItem> converter = CollectionCaster.ToOrderItem(RequestedItem, ref DB);
                return converter;
            }

        }

        public static IEnumerable<OrderItem> SearchByUser(string UserID , bool Only)
        {
            var RequestedItem = new List<ReservedCar>();
            using (DB = new CarRentzEntities())
            {
                if (UserID != null && UserID != "")
                {
                    RequestedItem = (from item in DB.ReservedCars
                                     where item.UserID.Contains(UserID)
                                     select item).ToList();
                }
                else
                    RequestedItem = DB.ReservedCars.ToList();
                if (Only)
                    RequestedItem = RequestedItem.Where(it => it.ReturnedDate == null).ToList();
                List<OrderItem> converter = CollectionCaster.ToOrderItem(RequestedItem, ref DB);
                return converter;
            }
        }

        public static int TryToRetrive(int OrderID ,DateTime? Retrive ,int kilo , out bool status)
        {
            using (DB = new CarRentzEntities())
            {
                try
                {
                    var rw = DB.ReservedCars.SingleOrDefault(One => One.ReservedID == OrderID);
                    DB.UpdateOrder(rw.ReservedID, rw.CarID, rw.UserID, rw.StartDate, rw.EndDate, Retrive);

                    var CarRw = DB.Cars.SingleOrDefault(One => One.CarID == rw.CarID);
                    if (kilo > 0)
                        DB.UpdateCar(CarRw.CarID, CarRw.Details, kilo, CarRw.IsOperational, CarRw.Branch, CarRw.FactoryYear);

                    DB.SaveChanges();
                    status = true;

                    if (kilo > 0 && Retrive != null)
                    {
                        var CarD = DB.CarDetails.SingleOrDefault(One => One.IDDetail == CarRw.Details);
                        rw = DB.ReservedCars.SingleOrDefault(One => One.ReservedID == OrderID);
                        return TotalPrice(CarD, rw, (DateTime)Retrive);
                    }
                    else
                        return 0;
                }


                catch (Exception)
                {
                    status = false;
                    return 0;
                }
            }
        }

        private static int TotalPrice(CarDetail par1 , ReservedCar par2 , DateTime RD)
        {
            DateTime SD = par2.StartDate;
            DateTime ED = par2.EndDate;
            int LRant = (int)par1.LateRent;
            int DRant = (int)par1.DailyRent;
            if (ED >= RD)
            {
                return (RD - SD).Days * DRant;
            }
            else
            {
                return ((ED - SD).Days * DRant + (RD - ED).Days * LRant);
            }
        }
    }
   
}
        




    
