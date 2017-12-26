using System;

namespace CarRentalProj.Models
{
    public class CDetails
    {
        #region Simple Data Members for the Car's Details
        public string Model { get; set; }
        public string Company { get; set; }
        public string GearType { get; set; }
        public short FactoryYear { get; set; }
        private string Branch { get; set; }
        private string _pic; 
        #endregion

        public string CarID { get; set; }
        public int DailyRent { get; set; }
        public int LateDaily { get; set; }
        public DateTime SD { get; set; }
        public DateTime ED { get; set; }



        public string Pic
        {
            get { return _pic + ".jpg"; }
            set { _pic = value; }
        }

        public CDetails(Car par, DateTime SD , DateTime ED)
        {
            this.CarID = par.CarID;
            this.SD = SD;
            this.ED = ED;
            this.Model = par.CarDetail.Model;
            this.Company = par.CarDetail.CompanyName;
            this.Branch = par.Branch;
            this.FactoryYear = par.FactoryYear;
            this.LateDaily = (int)par.CarDetail.LateRent;
            this.DailyRent = (int)par.CarDetail.DailyRent;
            this.GearType = par.CarDetail.GearType;
            this.Pic = par.CarDetail.IDDetail.Substring(0, par.CarDetail.IDDetail.Length - 1);
        }

        public string Details
        {
            get
            {
                return "<h5> Company " + Company + "</h5> <h5>Model " + Model + "</h5> <h5>GearType " + GearType + "</h5> <h5>Factory Year " + FactoryYear + "</h5>"
                    + GearType + "</h5> <h5>Branch Location " + Branch + "</h5>";
            }
        }
        
    }
}