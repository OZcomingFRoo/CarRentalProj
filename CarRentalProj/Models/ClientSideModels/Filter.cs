using System;

namespace CarRentalProj.Models
{
    public class Filter
    {
        /// <summary>
        /// Name of Model
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Name Of company
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// Daily Rent
        /// </summary>
        public int? DRent { get; set; }
        /// <summary>
        /// Factory Year
        /// </summary>
        public string GT { get; set; }
        /// <summary>
        /// True = from current date to the newest ones. False for curruent to Oldest models
        /// </summary>
        public string OrderB { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public short PageNum { get; set; }
        public string FTxt { get; set; }
        public int? FYear { get; set; }
    }
}