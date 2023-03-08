using MicroControler.Game.Entities;
using MicroControler.Game.RayCasting;
using MicroControler.GameLooping;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroControler.InputOutput
{
    public static class KeyboardManager
    {
        private static bool[] KeyHandlers =  new bool[32];

        /// <summary>
        /// Returns true only on key press and not if you hold it
        /// </summary>
        /// <param name="key">The key you want to check</param>
        /// <param name="id">Index of the KeyHandler can range from 0 to 32 always give a unique id</param>
        /// <returns></returns>
        public static bool OnKeyPress(Keyboard.Key key, int id)
        {
            if (!Keyboard.IsKeyPressed(key))
            {
                KeyHandlers[id] = true;
                return false;
            }
            else if (Keyboard.IsKeyPressed(key))
            {
                if (KeyHandlers[id])
                {
                    KeyHandlers[id] = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool IsKeyPressed(Keyboard.Key key) 
        { 
            return Keyboard.IsKeyPressed(key);
        }
    }
}
