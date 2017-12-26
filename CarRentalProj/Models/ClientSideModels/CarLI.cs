using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CarRentalProj.CSCode.Utility;

namespace CarRentalProj.Models
{
    public class CarLI
    {
        private string _carid;
        private string _pic;
        public string Model { get; set; }
        public string Company { get; set; }
        public int? DailyRent { get; set; }
        public double? TotalRent { get; set; }
        public short? FYear { get; set; }
        public string Pic {
            get { return _pic + ".jpg"; }
            set { _pic = value; }
        }
        public string CarID
        {
            get { return _carid; }
        }

        public CarLI (Car item, DateTime SD , DateTime ED)
        {
            this._carid = item.CarID;
            this.FYear = item.FactoryYear;
            this.Company = item.CarDetail.CompanyName;
            this.Model = item.CarDetail.Model;
            this.DailyRent = item.CarDetail.DailyRent;
            this.TotalRent = TotalRentCost((int)DailyRent , SD, ED);
            this.Pic = item.CarDetail.IDDetail.Substring(0, item.CarDetail.IDDetail.Length - 1);
        }
    }
}