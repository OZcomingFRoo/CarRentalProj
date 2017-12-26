using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalProj.Models
{
    public class FilteredReuslt
    {
        //Data Members
        public List<CarLI> List { get; set; }
        private short _index;
        //Data Members


        public int Index
        {
            get { return _index; }
            set
            {
                if (value < List.Count && value >= 0)
                    _index = (short)value;
            }
        }

        public FilteredReuslt(List<CarLI> filteredRes)
        {
            _index = 0;
            this.List = filteredRes;
        }

        public FilteredReuslt(List<CarLI> filteredRes , int i)
        {
            _index = (short)i;
            this.List = filteredRes;
        }

        public int Lenght
        {
            get
            {
                float temp = (List.Count / 10f);
                if (    (List.Count / 10) < temp        )
                {
                    return (List.Count / 10) + 1;
                }
                else
                    return  List.Count / 10;
            }
        }

        public List<CarLI> Get()
        {
            var x = List.Skip(Index * 10).Take(10);
            return x.ToList();
        }
        public List<CarLI> Get(int i)
        {
            if (i >= 0 && i * 10 < List.Count)
            {
                var x = List.Skip(i * 10).Take(10);
                return x.ToList();
            }
            else
                return this.Get();

        }
    }
}