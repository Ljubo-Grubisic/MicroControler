using microController.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microController
{
    internal class Program
    {
        public static Game Game;

        static void Main(string[] args)
        {
            Game = new Game(1024, 512, "MicroControler");
            Game.Run();
        }
    }
}
