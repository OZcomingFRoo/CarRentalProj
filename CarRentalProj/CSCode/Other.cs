using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CarRentalProj.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;

namespace CarRentalProj.CSCode
{
    #region object Validators Attributes
    public class FactoryYearValidation : ValidationAttribute
    {
        public FactoryYearValidation()
        {
            ErrorMessage = "Invalid Year";
        }
        public override bool IsValid(object value)
        {
            if (value is int || value is short)
            {
                return ((short)value > 1900 && (short)value <= (short)DateTime.Now.Year);
            }
            else return false;

        }
    }
    public class IdentityValidation : ValidationAttribute
    {
        public IdentityValidation()
        {
            ErrorMessage = "Invalid Identity Number";
        }
        public override bool IsValid(object value)
        {
            string ID = (value as string);

            int sum = 0;

            if (ID != null && ID.Length == 9)
            {
                for (int i = 0; i < ID.Length - 1; i++)
                {
                    if (i % 2 == 0)
                        sum += int.Parse(ID[i].ToString());
                    else
                    {
                        int x = int.Parse(ID[i].ToString()) * 2;
                        if (x > 9)
                            x = x % 10 + x / 10;
                        sum += x;
                    }
                }
                return ((sum + int.Parse(ID[8].ToString())) % 10 == 0);
            }
            return false;
        }
    }
    public class PasswordValidation : ValidationAttribute
    {
        public PasswordValidation()
        {
            ErrorMessage = "Must contain atleast :\n 1)one Uppercase\n 2)one Lowercase\n 3)one number char 4) lenght between 8 and 20";
        }
        public override bool IsValid(object value)
        {
            if (value is string)
            {
                return (Regex.IsMatch((value as string), "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$") && (value as string).Length >= 8 && (value as string).Length <= 20);
            }
            else
                return false;
        }
    }
    public class BirthDateValidation : ValidationAttribute
    {
        public BirthDateValidation()
        {
            ErrorMessage = "Must be 18 or above";
        }
        public override bool IsValid(object value)
        {
            if (value != null && value is DateTime)
            {
                var temp = DateTime.Now.AddYears(-18);
                return ((DateTime)value <= temp);
            }
            else
                return false;
        }
    }
    public class GenderValidation : ValidationAttribute
    {
        public GenderValidation()
        {
            ErrorMessage = "Invalid Gender";
        }
        public override bool IsValid(object value)
        {
            if (value is string)
            {
                return ((value as string).ToLower() == "male" || (value as string).ToLower() == "female");
            }
            else return false;
        }
    }
    public class GearValidation : ValidationAttribute
    {
        public GearValidation()
        {
            ErrorMessage = "Invalid Gear";
        }
        public override bool IsValid(object value)
        {
            if (value is string)
            {
                return ((value as string) == "Menual" || (value as string) == "Auto");
            }
            else return false;
        }
    }
    public class ProfessionValidation : ValidationAttribute
    {
        public ProfessionValidation()
        {
            ErrorMessage = "Invalid profession";
        }
        public override bool IsValid(object value)
        {
            if (value is string)
            {
                return ((value as string).ToLower() == "admin" || (value as string).ToLower() == "emp");
            }
            else return false;
        }
    }
    public class CarIDValidation : ValidationAttribute
    {
        public CarIDValidation()
        {
            ErrorMessage = "MUST BE 7/8 DIGITS";
        }
        public override bool IsValid(object value)
        {
            if (value is string)
            {
                return ((value as string).Length == 7 || (value as string).Length == 8);
            }
            else return false;
        }
    }
    public class PosNumValidation : ValidationAttribute
    {
        public PosNumValidation()
        {
            ErrorMessage = "Must be positive";
        }
        public override bool IsValid(object value)
        {
            if ((int)value > 0)
                return true;
            else
                return false;
        }
    }
    #endregion

    public static class Utility
    {
        public static void CheckForOldJpg(string fileName)
        {
            if (fileName != null)
            {
                string path = HostingEnvironment.ApplicationPhysicalPath;
                string folder = path + @"\CompanyIcons\" + fileName;
                if(File.Exists(folder))
                {
                    //Delete file
                    File.Delete(folder);
                }
            }
        }

        public static readonly List<string> GenderType = new List<string>() {"Male" , "Female" };

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static List<string> CompanyTags()
        {  
            using (CarRentzEntities temp = new CarRentzEntities())
            {
               return temp.CompanyTags().ToList();
            }
                
        }

        public static List<string> ModelTags(string companyname)
        {
            using (CarRentzEntities temp = new CarRentzEntities())
            {
                return temp.ModelTags(companyname).ToList();
            }
        }

        public static bool DateOverLapped(DateTime Astart , DateTime Aend , DateTime Bstart, DateTime Bend)
        {
            return (Astart < Bend && Bstart < Aend);
        }

        public static double TotalRentCost (int DRant , DateTime SD , DateTime ED)

        {
            return (ED - SD).TotalDays * DRant;
        }

        public static string htmlDate (this DateTime date)
        {
            string m = date.Month.ToString(), d = date.Day.ToString();
            if(date.Month < 10)
            {
                m = "0" + m;
            }
            if (date.Day < 10)
            {
                d = "0" + d;
            }
            return date.Year + "-" + m + "-" + d;
        }
    }
    public static class Encryptor
    {
        const string hash = "%4h&bn9873*7^><?:'";

        public static string Encrypt(string Password)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(Password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }

        public static string Decrypt(string Cookie)
        {
            byte[] data = Convert.FromBase64String(Cookie);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(result);
                }
            }
        }
    }
    public static class CollectionCaster
    {
        public static List<CarLI> ToCarList(IEnumerable<Car> par, DateTime SD, DateTime ED)
        {
            List<CarLI> r = new List<CarLI>();
            foreach (var item in par)
            {
                r.Add(item.ToCarLIModel(SD, ED));
            }
            return r;
        }

        public static List<UserItem> ToUserItem(IEnumerable<User> List)
        {
            var r = new List<UserItem>();
            foreach (var item in List)
            {
                r.Add(new UserItem(item));
            }
            return r;
        }

        public static List<CarTypeItem> ToCarTypeItem(IEnumerable<CarDetail> List)
        {
            var r = new List<CarTypeItem>();
            foreach (var item in List)
            {
                r.Add(new CarTypeItem(item));
            }
            return r;
        }

        public static List<CarItem> ToCarItem(IEnumerable<Car> List)
        {
            var r = new List<CarItem>();
            foreach (var item in List)
            {
                r.Add(new CarItem(item));
            }
            return r;
        }

        public static List<RentedCarItem> ToRentedCarItem(IEnumerable<ReservedCar> List , ref CarRentzEntities DB)
        {
            List<RentedCarItem> r = new List<RentedCarItem>();
            foreach(var item in List)
            {
                string name = DB.GetNameOfRentedCar(item.CarID).ToArray()[0];
                r.Add(new RentedCarItem(item,name));
            }
            return r;
        }

        public static List<OrderItem> ToOrderItem(IEnumerable<ReservedCar> List , ref CarRentzEntities DB)
        {
            List<OrderItem> r = new List<OrderItem>();
            foreach (var item in List)
            {
                string name = DB.GetNameOfRentedCar(item.CarID).ToArray()[0];
                r.Add(new OrderItem(item , name));
            }
            return r;
        }

        public static List<CarDetailOp> CarDSelect()
        {
            List<CarDetailOp> r = new List<CarDetailOp>();
            using (CarRentzEntities temp = new CarRentzEntities())
            {
                foreach (var item in temp.CarDetails)
                {
                    if (item.IsObsolete == false)
                    {
                        r.Add(new CarDetailOp() { IDDetails = item.IDDetail, Name = item.CompanyName + " " + item.Model + " " + item.GearType }); 
                    }
                }
            }
            return r;
        }

        public static List<Car> AdditionalFilter(IEnumerable<AdditionalSearch_Result> turd)
        {
            List<Car> r = new List<Car>();
            using (CarRentzEntities DB = new CarRentzEntities())
            {
                foreach (var item in turd)
                {
                    var why = DB.CarDetails.SingleOrDefault(one => one.IDDetail == item.Details);
                    r.Add(new Car()
                    {
                        CarID = item.CarID,
                        Details = item.Details,
                        FactoryYear = item.FactoryYear,
                        KiloMetraz = item.KiloMetraz,
                        Branch = item.Branch,
                        IsOperational = item.IsOperational,
                        CarDetail = why
                    });
                }
            }
            return r;
        }
    }
    public static class TurnJsonFrom
    {
        public static object That(ReservedCar n)
        {
            if(n.ReturnedDate != null)
            {
                return new { OrderNum = n.ReservedID, SD = n.StartDate.ToShortDateString(), ED = n.EndDate.ToShortDateString(), User = n.UserID, Car = n.CarID, RDate = ((DateTime)n.ReturnedDate).ToShortDateString() };
            }
            else
            {
                return new { OrderNum = n.ReservedID, SD = n.StartDate.ToShortDateString(), ED = n.EndDate.ToShortDateString(), User = n.UserID, Car = n.CarID, RDate = "" };
            }
        }

        public static object That(User n)
        {
            try
            {
                return new { Idnum = n.IDNumber, Name = n.UserName, Sex = n.Gender, Mail = n.Email, Password = n.UserPassword, Bday = n.BirthDate.Value.ToShortDateString() };
            }
            catch (Exception)
            {
                return new { Idnum = n.IDNumber, Name = n.UserName, Sex = n.Gender, Mail = n.Email, Password = n.UserPassword, Bday = "" };
            }
        }

        public static object That(Car n)
        {
            return new { CarID = n.CarID, Details = n.Details, Branch = n.Branch, Kilo = n.KiloMetraz, Year = n.FactoryYear, Isop = n.IsOperational };
        }
        public static object That(CarDetail n)
        {
            return new { Model = n.Model, Company = n.CompanyName, DRent = n.DailyRent, LRent = n.LateRent, Id = n.IDDetail, GT = n.GearType, IsObsolete =n.IsObsolete };
        }
    }
}