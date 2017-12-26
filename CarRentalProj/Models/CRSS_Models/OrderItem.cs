using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalProj.Models
{
    public class OrderItem
    {
        public int OrderID { get; set; }
        public string CarIdNum { get; set; }
        public string CarName { get; set; }
        public string Owner{ get; set; }

        public OrderItem(ReservedCar Order , string name)
        {
            this.OrderID = Order.ReservedID;
            CarIdNum = Order.CarID;
            CarName = name;
            Owner = Order.User.UserName + " , " + Order.User.IDNumber;
        }

        public OrderItem(string id , string name , string car)
        {
            CarIdNum = id;
            Owner = name;
            CarName = car;
        }
    }
}