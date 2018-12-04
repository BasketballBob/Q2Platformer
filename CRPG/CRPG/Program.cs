using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Julien West 12/3/2018
namespace CRPG
{
    class Program
    {

        private static Player _player = new Player();

        static void Main(string[] args)
        {
            GameEngine.Initialize();
            _player.Name = "Fred the Fearless";


            Console.ReadLine();
        }
    }
}
