using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalProj.Models
{
    public class OrdeReceipt
    {
        public string UserID { get; set; }
        public string CarID { get; set; }
        public short DailyRent { get; set; }
        private DateTime SD;
        private DateTime ED;

        public DateTime Start
        {
            get { return SD; }
        }

        public DateTime End
        {
            get { return ED; }
        }
        public double TotalCost
        {
            get
            {
                return (ED - SD).TotalDays * DailyRent;
            }
        }

        public OrdeReceipt(string user , string car , DateTime S , DateTime E, short DailyRent)
        {
            UserID = user;
            CarID = car;
            SD = S;
            ED = E;
            this.DailyRent = DailyRent;
        }

        public OrdeReceipt(string userId ,CDetails Derive) : this(userId,Derive.CarID, Derive.SD , Derive.ED , (short)Derive.DailyRent)
         {

        }


    }
}