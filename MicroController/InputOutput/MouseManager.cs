using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace MicroController.InputOutput
{
    public static class MouseManager
    {
        private static bool[] KeyHandlersPress = new bool[100];

        public static bool OnMouseDown(Mouse.Button button, uint id)
        {
            if (!Mouse.IsButtonPressed(button))
            {
                KeyHandlersPress[id] = true;
                return false;
            }
            else if (Mouse.IsButtonPressed(button))
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

        public static bool IsMouseButtonPressed(Mouse.Button button)
        {
            return Mouse.IsButtonPressed(button);
        }
    }
}
