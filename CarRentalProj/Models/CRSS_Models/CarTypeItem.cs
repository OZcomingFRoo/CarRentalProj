using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalProj.Models
{
    public class CarTypeItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Gear { get; set; }

        public CarTypeItem()
        {

        }

        public CarTypeItem(CarDetail RawData)
        {
            this.ID = RawData.IDDetail;
            this.Name = RawData.Model + " " + RawData.CompanyName;
            this.Gear = RawData.GearType;
        }
    }
}