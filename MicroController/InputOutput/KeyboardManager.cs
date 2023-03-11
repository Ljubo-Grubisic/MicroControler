using MicroController.Game.Entities;
using MicroController.Game.RayCasting;
using MicroController.GameLooping;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MicroController.InputOutput
{
    public static class KeyboardManager
    {
        private static bool[] KeyHandlersPress =  new bool[32];
        private static float[] StartTime = new float[32];
        private static bool[] KeyHandlersTime = new bool[32];

        /// <summary>
        /// Returns true only on key press and not if you hold it
        /// </summary>
        /// <param name="key">The key you want to check</param>
        /// <param name="id">Index of the KeyHandler can range from 0 to 31 always give a unique id</param>
        /// <returns></returns>
        public static bool OnKeyPress(Keyboard.Key key, int id)
        {
            if (!Keyboard.IsKeyPressed(key))
            {
                KeyHandlersPress[id] = true;
                return false;
            }
            else if (Keyboard.IsKeyPressed(key))
            {
                if (KeyHandlersPress[id])
                {
                    KeyHandlersPress[id] = false;
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

        /// <summary>
        /// Returns true after you have pressed the button for 1 s
        /// </summary>
        /// <param name="key">The key you want to check</param>
        /// <param name="id">Index of the KeyHandler can range from 0 to 31 always give a unique id the id only needs to be unique for this function</param>
        /// <returns></returns>
        public static bool OnKeyDownForTime(Keyboard.Key key, GameTime time, int id, float timeTillTrue)
        {
            if (!IsKeyPressed(key))
            {
                StartTime[id] = 0;
                KeyHandlersTime[id] = false;
            }
            if (IsKeyPressed(key) && !KeyHandlersTime[id])
            {
                StartTime[id] = time.TotalTimeElapsed;
                KeyHandlersTime[id] = true;
                return false;
            }
            if (IsKeyPressed(key) && time.TotalTimeElapsed - StartTime[id] > timeTillTrue)
            {
                return true;
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
