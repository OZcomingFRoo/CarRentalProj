using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalProj.Models
{
    public class CarItem
    {
        public string CarID { get; set; }
        public bool Operational { get; set; }
        public string CarName { get; set; }

        public CarItem()
        {
            CarID = "";
            Operational = false;
            CarName = "";
        }

        public CarItem(Car par)
        {
            CarID = par.CarID;
            CarName = par.CarDetail.Model + " " + par.CarDetail.CompanyName;
            Operational = par.IsOperational;
        }
    }
}