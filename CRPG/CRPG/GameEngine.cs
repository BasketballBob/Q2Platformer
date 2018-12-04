using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRPG
{
    public static class GameEngine
    {
        public static string Version = "0.0.1";

        public static void Initialize()
        {
            Console.WriteLine("Initializing GameEngine Version {0}",Version);
            Console.WriteLine("\n\nWelcome to the world of {0}!", World.WorldName);
            World.ListLocations();

            Console.WriteLine();

        }
    }
}
