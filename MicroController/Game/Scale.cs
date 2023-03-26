using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroController.Game.Entities;
using MicroController.Game.Maping;
using System.Runtime.CompilerServices;
using SFML.System;

namespace MicroController.Game
{
    public static class Scale
    {
        public static float NumPixelPerCm { get; private set; }
        public static int SquareSizeInCm { get; private set; }

        public static void Create(int squareSizeInCm, int squareSizeInPixels)
        {
            SquareSizeInCm = squareSizeInCm;
            NumPixelPerCm = squareSizeInPixels / (float)SquareSizeInCm;
        }

        public static void Update(Map map)
        {
            NumPixelPerCm = map.SquareSize / (float)SquareSizeInCm;
        }
    }
}