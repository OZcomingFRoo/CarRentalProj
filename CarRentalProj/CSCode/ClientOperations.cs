using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using CarRentalProj.Models;
using System.Data.Entity.Infrastructure;

namespace CarRentalProj.CSCode
{
    public static class ClientOperation
    {
        private static CarRentzEntities DB;
        static ClientOperation()
        {
            DB = new CarRentzEntities();
        }

        #region Simple Tasks
        public static CDetails GetByCarID(string CarNumberID, DateTime SD, DateTime ED)
        {
            using (DB = new CarRentzEntities())
            {
                var r = (from car in DB.Cars
                         where car.CarID == CarNumberID
                         select car).ToArray();
                if (r.Length == 1)
                    return new CDetails(r[0], SD, ED);
                else
                    return null;
            }
        }
        public static bool TryToRent(string UserID, string CarID, DateTime SD, DateTime ED)
        {
            try
            {
                using (DB = new CarRentzEntities())
                {
                    bool OverLap = false;
                    var CarsRentalHistory = DB.ReservedCars.Where(Order => Order.CarID == CarID).ToList();
                    foreach (var Order in CarsRentalHistory)
                    {
                        if (Order.ReturnedDate == null)
                        {
                            OverLap = Utility.DateOverLapped(Order.StartDate, Order.EndDate, SD, ED);
                        }
                    }
                    if (!OverLap)
                    {
                        DB.RentCar(CarID, UserID, SD, ED);
                        DB.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static User GetUserDetails(string UserID)
        {
            try
            {
                using (DB = new CarRentzEntities())
                {
                    var RquUser = DB.Users.SingleOrDefault(TheOne => TheOne.IDNumber == UserID);
                    return RquUser;
                }
            }
            catch (Exception)
            {
                return new User();
            }
        } 
        #endregion

        #region Functions that return collection
        public static IEnumerable<RentedCarItem> MyCars(string id)
        {
            using (DB = new CarRentzEntities())
            {
                var R2 = DB.Users.Where(item => item.IDNumber == id).ToArray()[0].ReservedCars.ToList();
                return CollectionCaster.ToRentedCarItem(R2, ref DB);
            }
        }

        public static IEnumerable<CarLI> SearchCars(Filter Params)
        {
            List<Car> temp;
            using (DB = new CarRentzEntities())
            {
                #region Additional Filters
                temp = CollectionCaster.AdditionalFilter(DB.AdditionalSearch(Params.Model, Params.Company, Params.GT, Params.DRent));

                if (Params.FTxt != null && Params.FTxt != "")
                {
                    temp = temp.Where(i => Params.FTxt.Contains(i.CarDetail.Model) || Params.FTxt.Contains(i.CarDetail.CompanyName) ||
                    i.CarDetail.CompanyName.Contains(Params.FTxt) || i.CarDetail.Model.Contains(Params.FTxt)).ToList();
                }
                if(Params.FYear != null && Params.FYear != 0)
                {
                    temp = temp.Where(i => i.FactoryYear == Params.FYear).ToList();
                }
                #endregion

                temp = temp.Where(EachOne => EachOne.IsOperational == true).ToList();
                
                temp = temp.Where((item) => {
                    var CarsRentalHistory = DB.ReservedCars.Where(Order => Order.CarID == item.CarID).ToList();
                    foreach (var Order in CarsRentalHistory)
                    {
                        if (Order.ReturnedDate == null)
                        {
                            bool OverLap = Utility.DateOverLapped(Order.StartDate, Order.EndDate, Params.StartDate, Params.EndDate);
                            if (OverLap)
                                return false; 
                        }
                    }
                    return true;
                }).ToList();
                switch (Params.OrderB)
                {
                    default:
                        {
                            break;
                        }
                    case "asc":
                        {
                            temp = temp.OrderBy(sd => sd.CarDetail.DailyRent.Value).ToList();
                            break;
                        }
                    case "desc":
                        {
                            temp = temp.OrderByDescending(sd => sd.CarDetail.DailyRent.Value).ToList();
                            break;
                        }
                }
                return CollectionCaster.ToCarList(temp , Params.StartDate , Params.EndDate);
            }
        }
        #endregion
        
        #region Log operation that update Database Regulary
        public static bool TryToLogin(string IDNum, string password)
        {
            using (DB = new CarRentzEntities())
            {
                var OldUser = DB.Users.SingleOrDefault(item => item.IDNumber == IDNum && item.UserPassword == password);
                if (OldUser != null) // Check to see if User Exists 
                {
                    FormsAuthentication.SetAuthCookie("User", false);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool TryToSignUp(User NewComer)
        {
            try
            {
                using (DB = new CarRentzEntities())
                {
                    DB.Users.Add(NewComer);
                    DB.SaveChanges();
                }
                FormsAuthentication.SetAuthCookie("User", false);
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }//User Already Exists/Email Taken
        } 

        public static bool TryToUpdateProfile(User UpUser)
        {
            try
            {
                using (DB = new CarRentzEntities())
                {
                    DB.UpdateUser(UpUser.IDNumber, UpUser.UserName, UpUser.Gender, UpUser.BirthDate, UpUser.Email,UpUser.UserPassword);
                    //DB.UpdateProfile(UpUser.IDNumber, UpUser.UserName, UpUser.Gender, UpUser.BirthDate, UpUser.Email);
                    DB.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }

    
}