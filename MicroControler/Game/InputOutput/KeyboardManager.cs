using MicroControler.Game.Entity;
using MicroControler.Game.RayCasting;
using MicroControler.GameLooping;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroControler.Game.InputOutput
{
    public static class KeyboardManager
    {
        private static bool[] KeyHandlers =  new bool[32];

        internal static void OpenCloseMap(ref RayCaster rayCaster, ref int windowState)
        {
            if (!Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                KeyHandlers[0] = true;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                if (KeyHandlers[0])
                {
                    if (windowState == 0)
                    {
                        windowState++;
                        rayCaster.DrawMapRays = true;
                        rayCaster.Draw3D = false;
                    }
                    else if (windowState == 1)
                    {
                        windowState--;
                        rayCaster.DrawMapRays = false;
                        rayCaster.Draw3D = true;
                    }
                    KeyHandlers[0] = false;
                }
            }
        }

        internal static void PlayerMovment(ref Player player, ref GameTime gameTime)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                player.Rotation -= 0.05f * gameTime.DeltaTime * 100;
                if (player.Rotation < 0)
                {
                    player.Rotation += 2f * (float)Math.PI;
                }
                player.DeltaPositionX = (float)(Math.Cos(player.Rotation));
                player.DeltaPositionY = (float)(Math.Sin(player.Rotation));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                player.Rotation += 0.05f * gameTime.DeltaTime * 75;
                if (player.Rotation > 2f * (float)Math.PI)
                {
                    player.Rotation -= 2f * (float)Math.PI;
                }
                player.DeltaPositionX = (float)(Math.Cos(player.Rotation));
                player.DeltaPositionY = (float)(Math.Sin(player.Rotation));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                player.PositionX += player.DeltaPosition.X * gameTime.DeltaTime * 150;
                player.PositionY += player.DeltaPosition.Y * gameTime.DeltaTime * 150;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                player.PositionX -= player.DeltaPosition.X * gameTime.DeltaTime * 150;
                player.PositionY -= player.DeltaPosition.Y * gameTime.DeltaTime * 150;
            }
        }
    }
}
