using System;
using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerFormViewModel
    {
        public Walker Walker { get; set; }
        public List<Walk> Walks { get; set; }
        public int TotalWalkTIme 
        { 
            get
            {
                foreach (var w in Walks)
                    {
                    w.Duration++;
                    }
                return TotalWalkTIme;
            }
        }
    }
}
