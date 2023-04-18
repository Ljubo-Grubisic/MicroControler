using MicroController.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroController
{
    internal class Program
    {
        public static Game.Game Game;

        static void Main(string[] args)
        {
            Game = new Game.Game(1024, 512, "MicroControler");
            Game.Run();
        }
    }
}
