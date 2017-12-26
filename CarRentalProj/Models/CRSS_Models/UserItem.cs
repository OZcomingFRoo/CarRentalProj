using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalProj.Models
{
    public class UserItem
    {
        public string Identity { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }

        public UserItem()
        { }
        public UserItem(User par)
        {
            Identity = par.IDNumber;
            Name = par.UserName;
            Mail = par.Email;
        }
    }
}