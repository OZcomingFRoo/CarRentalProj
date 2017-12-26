using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarRentalProj.Models;
using System.Drawing;
using System.IO;
using System.Web.Hosting;

namespace CarRentalProj.CSCode
{
    public static class CEO_Actions
    {
        private static CarRentzEntities DB;

        #region Get Details Functions
        public static User GetUser(string id)
        {
            using (DB = new CarRentzEntities())
            {
                return DB.Users.Where(TheOne => TheOne.IDNumber == id).ToArray()[0];
            }
        }
        public static Car GetCar(string id)
        {
            using (DB = new CarRentzEntities())
            {
                return DB.Cars.Where(TheOne => TheOne.CarID == id).ToArray()[0];
            }
        }
        public static CarDetail GetCarType(string id)
        {
            using (DB = new CarRentzEntities())
            {

                return DB.CarDetails.Where(TheOne => TheOne.IDDetail == id).ToArray()[0];
            }
        }
        public static ReservedCar GetOrder(int OrderIDNum)
        {
            using (DB = new CarRentzEntities())
            {
                return DB.ReservedCars.Where(TheOne => TheOne.ReservedID == OrderIDNum).ToArray()[0];
            }
        }
        public static Worker GetEmp(string id)
        {
            using (DB = new CarRentzEntities())
            {
                return DB.Workers.Where(TheOne => TheOne.Id== id).ToArray()[0];
            }
        }
        #endregion

        #region Search Functions
        public static IEnumerable<UserItem> SearchUsersById(string par)
        {
            using (DB = new CarRentzEntities())
            {
                if (par != null && par != "all" && par != "")
                {
                    var Getter = DB.Users.Where(EachOne => EachOne.IDNumber.Contains(par)).ToList();
                    var r = CollectionCaster.ToUserItem(Getter);
                    return r;
                }
                else
                {
                    var Getter = DB.Users.ToList();
                    var r = CollectionCaster.ToUserItem(Getter);
                    return r;
                }
            }
        }
        public static IEnumerable<UserItem> SearchUsersByName(string par)
        {
            using (DB = new CarRentzEntities())
            {
                var Getter = DB.Users.Where(EachOne => EachOne.UserName.Contains(par)).ToList();
                var r = CollectionCaster.ToUserItem(Getter);
                return r;
            }
        }
        public static IEnumerable<CarTypeItem> SearchCarType(string par, string GT, bool Only , string Company)
        {
            using (DB = new CarRentzEntities())
            {
                List<CarDetail> Getter = new List<CarDetail>();
                if (par != null && par != "" && par != "all")
                    Getter = DB.CarDetails.Where(Each1 => Each1.Model.Contains(par) == true).ToList();
                else
                    Getter = DB.CarDetails.ToList();

                if(Company != "Any")
                    Getter = Getter.Where(EachOne => (EachOne.CompanyName == Company)).ToList();

                if (GT.ToLower() != "any")
                    Getter = Getter.Where(EachOne => (EachOne.GearType == GT)).ToList();

                if(Only)
                    Getter = Getter.Where(EachOne => (EachOne.IsObsolete == false)).ToList();

                var r = CollectionCaster.ToCarTypeItem(Getter);
                return r;
            }
        }
        public static IEnumerable<CarItem> SearchCarName(string par)
        {
            using (DB = new CarRentzEntities())
            {
                if (par != "" && par != null && par != "all")
                {
                    var RequestedCars = DB.Cars.Where(Kar => Kar.CarDetail.Model.Contains(par)).ToList();
                    return CollectionCaster.ToCarItem(RequestedCars);
                }
                else
                {
                    var RequestedCars = DB.Cars.ToList();
                    return CollectionCaster.ToCarItem(RequestedCars);
                }
            }
        }
        public static IEnumerable<CarItem> SearchCarId(string par)
        {
            using (DB = new CarRentzEntities())
            {
                if (par != "" && par != null && par != "all")
                {
                    var RequestedCars = DB.Cars.Where(Kar => Kar.CarID.Contains(par)).ToList();
                    return CollectionCaster.ToCarItem(RequestedCars);
                }
                else
                {
                    var RequestedCars = DB.Cars.ToList();
                    return CollectionCaster.ToCarItem(RequestedCars);
                }
            }
        }
        public static IEnumerable<Worker> SearchEmpsById(string par)
        {
            using (DB = new CarRentzEntities())
            {
                if (par != null && par != "all" && par != "")
                {
                    var Getter = DB.Workers.Where(EachOne => EachOne.Id.Contains(par)).ToList();
                    return Getter;
                }
                else
                {
                    var Getter = DB.Workers.ToList();
                    return Getter;
                }
            }
        }
        public static IEnumerable<Worker> SearchEmpsByName(string par)
        {
            using (DB = new CarRentzEntities())
            {
                var Getter = DB.Workers.Where(EachOne => EachOne.FullName.Contains(par)).ToList();
                return Getter;
            }
        }
        //Order search is already implamented in the Employee Operations
        #endregion

        #region Dynamic Functions that Manipulate Data Base
        public static void DynamicUpdateToDB(object row, out bool status, string CDIDOLD)
        {
            using (CarRentzEntities DB = new CarRentzEntities())
            {
                try
                {
                    string type = row.GetType().Name;
                    switch (type)
                    {
                        default:
                            {
                                status = false;
                                return;
                            }
                        case "User":
                            {
                                var n = row as User;
                                status = true;
                                DB.UpdateUser(n.IDNumber, n.UserName, n.Gender, n.BirthDate, n.Email, n.UserPassword);
                                break;
                            }
                        case "Car":
                            {
                                var n = row as Car;
                                status = true;
                                DB.UpdateCar(n.CarID, n.Details, n.KiloMetraz, n.IsOperational, n.Branch, n.FactoryYear);
                                break;
                            }
                        case "CarDetail":
                            {
                                var n = row as CarDetail;
                                status = true;
                                DB.UpdateCarType(n.IDDetail, n.Model, n.CompanyName, (short)n.DailyRent, (short)n.LateRent, n.GearType, n.IsObsolete, CDIDOLD);
                                break;
                            }
                        case "ReservedCar":
                            {
                                var n = row as ReservedCar;
                                status = true;
                                DB.UpdateOrder(n.ReservedID, n.CarID, n.UserID, n.StartDate, n.EndDate, n.ReturnedDate);
                                break;
                            }
                        case "Worker":
                            {
                                var n = row as Worker;
                                DB.UpdateEmp(n.Id, n.FullName, n.PSWD, n.profession);
                                status = true;
                                break;
                            }
                    }
                    DB.SaveChanges();
                }
                catch (Exception)
                {
                    status = false;
                }
            }
        }

        public static void DynamicCreateRowToDB(object row, out bool status)
        {
            using (CarRentzEntities DB = new CarRentzEntities())
            {
                string type = row.GetType().Name;
                try
                {
                    switch (type)
                    {
                        default:
                            {
                                status = false;
                                return;
                            }
                        case "User":
                            {
                                var n = row as User;
                                DB.Users.Add(n);
                                status = true;

                                break;
                            }
                        case "Car":
                            {
                                var n = row as Car;
                                DB.Cars.Add(n);
                                status = true;
                                break;
                            }
                        case "CarDetail":
                            {
                                var n = row as CarDetail;
                                n.IsObsolete = false;
                                DB.CarDetails.Add(n);
                                status = true;
                                break;
                            }
                        case "ReservedCar":
                            {
                                var n = row as ReservedCar;
                                bool OverLap = false;
                                var CarsRentalHistory = DB.ReservedCars.Where(Order => Order.CarID == n.CarID).ToList();
                                foreach (var Order in CarsRentalHistory)
                                {
                                    if (Order.ReturnedDate == null)
                                    {
                                        OverLap = Utility.DateOverLapped(Order.StartDate, Order.EndDate, n.StartDate, n.EndDate);
                                    }
                                }
                                if (!OverLap)
                                {
                                    DB.ReservedCars.Add(n);
                                    status = true;
                                    break;
                                }
                                else
                                {
                                    status = false;
                                    break;
                                }
                            }
                        case "Worker":
                            {
                                var n = row as Worker;
                                DB.Workers.Add(n);
                                status = true;
                                break;
                            }
                    }
                    DB.SaveChanges();
                }
                catch (Exception)
                {
                    status = false;
                }
            }
        }

        public static void DynamicRemoveRowFromDB(string RowId, string type, out bool status)
        {
            using (DB = new CarRentzEntities())
            {
                try
                {
                    switch (type)
                    {
                        default:
                            {
                                status = false;
                                return;
                            }
                        case "Order":
                            {
                                int index = int.Parse(RowId);
                                var row = DB.ReservedCars.Where(order => order.ReservedID == index).ToArray()[0];
                                DB.ReservedCars.Remove(row);
                                status = true;
                                break;
                            }
                        case "Emp":
                            {
                                DB.Workers.Remove(DB.Workers.SingleOrDefault(one => one.Id == RowId));
                                status = true;
                                break;
                            }
                        case "Car":
                            {
                                DB.Cars.Remove(DB.Cars.SingleOrDefault(one => one.CarID == RowId));
                                status = true;
                                break;
                            }
                        case "CarD":
                            {
                                DB.CarDetailsChangeObs(RowId);
                                status = true;
                                break;
                            }
                        case "User":
                            {
                                DB.Users.Remove(DB.Users.SingleOrDefault(one => one.IDNumber == RowId));
                                status = true;
                                break;
                            }

                    }
                    DB.SaveChanges();
                }
                catch (Exception)
                {
                    status = false;
                }
            }
        } 

        public static void SaveCarImage(HttpPostedFileBase data , string fileName)
        {
            using (BinaryReader br = new BinaryReader(data.InputStream))
            {
                var Imagebytes = br.ReadBytes(data.ContentLength);
                Image img = Utility.byteArrayToImage(Imagebytes);
                string path = HostingEnvironment.ApplicationPhysicalPath;
                string folder = path + @"\CompanyIcons\"+fileName+ ".jpg";
                img.Save(folder, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            
        }
        #endregion
    }
}