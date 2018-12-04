using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRPG
{
    public class Location
    {
        public int ID;
        public string Name;
        public string Description;
        public Location LocationToNorth;
        public Location LocationToSouth;
        public Location LocationToEast;
        public Location LocationToWest;

        //Constructor
        public Location(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}
