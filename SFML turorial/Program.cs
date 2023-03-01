using MicroControler.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroControler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game.Game tutoGame = new Game.Game(1024, 512, "Test");
            tutoGame.Run();
        }
    }
}
