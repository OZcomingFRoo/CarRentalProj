//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarRentalProj
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReservedCar
    {
        public int ReservedID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<System.DateTime> ReturnedDate { get; set; }
        public string UserID { get; set; }
        public string CarID { get; set; }
    
        public virtual Car Car { get; set; }
        public virtual User User { get; set; }
    }
}