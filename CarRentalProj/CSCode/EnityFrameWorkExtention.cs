using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CarRentalProj.CSCode;
using CarRentalProj.Models;
using System.Web.Hosting;

namespace CarRentalProj
{
    #region Car Extention Model
    [MetadataType(typeof(CarValidtion))]
    public partial class Car
    {
        public CarLI ToCarLIModel(DateTime SD , DateTime ED)
        {
            return new CarLI(this , SD , ED);
        }
    }
    public class CarValidtion
    {
        [Required]
        [CarIDValidation]
        public string CarID { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        [PosNumValidation]
        public Nullable<int> KiloMetraz { get; set; }
        [Required]
        public bool IsOperational { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        [FactoryYearValidation]
        public short FactoryYear { get; set; }
    }
    #endregion

    #region User Extention Model

    [MetadataType(typeof(UserValidation))]
    public partial class User
    { }
    public class UserValidation
    {
        [Required]
        [IdentityValidation]
        public string IDNumber { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "format for email is wrong")]
        public string Email { get; set; }
        [Required]
        [PasswordValidation]
        public string UserPassword { get; set; }
        [BirthDateValidation]
        public Nullable<System.DateTime> BirthDate { get; set; }
        [GenderValidation]
        public string Gender { get; set; }
    }
    #endregion

    #region ReservedCar Extention Model
    [MetadataType(typeof(OrderValidation))]
    public partial class ReservedCar
    {

    }
    public class OrderValidation
    {
        public int ReservedID { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        [Required]
        public string CarID { get; set; }
        [Required]
        public string UserID { get; set; }
    }
    #endregion

    #region CarType Extention Model
    [MetadataType(typeof(CarDetailValidation))]
    public partial class CarDetail
    {
        public string Pic
        { get
            {
                return this.IDDetail.Substring(0, this.IDDetail.Length - 1) + ".jpg";
            }
        }
    }
    public class CarDetailValidation
    {
        [Required]
        public string IDDetail { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        [PosNumValidation]
        public int? DailyRent { get; set; }
        [Required]
        [PosNumValidation]
        public int? LateRent { get; set; }
        [Required]
        [GearValidation]
        public string GearType { get; set; }
        [Required]
        public bool IsObsolete { get; set; }
    }
    #endregion

    #region Worker/Emp Extention Model
    [MetadataType(typeof(EmpValidation))]
    public partial class Worker
    {
    }
    public class EmpValidation
    {
        [Required]
        [IdentityValidation]
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [PasswordValidation]
        public string PSWD { get; set; }
        [Required]
        [ProfessionValidation]
        public string profession { get; set; }
    } 
    #endregion

}