using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalProj.Models
{
    public class RentedCarItem
    {
        public string Id { get; set; }
        public string CarName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public RentedCarItem(ReservedCar convertMe)
        {
            this.Id = convertMe.CarID;
            this.Start = convertMe.StartDate;
            this.End = convertMe.EndDate;
        }

        public RentedCarItem(ReservedCar par1 , string name ): this(par1)
        {
            this.CarName = name;
        }
    }
}