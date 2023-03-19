using MicroController.GameLooping;
using SFML.Graphics;
using SFML.Window;
using System.IO;
using static SFML.Window.Keyboard;

namespace MicroController.InputOutput
{
    public static class KeyboardManager
    {
        public static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.BackSpace";

        private static bool[] KeyHandlersPress = new bool[100];
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

        private static bool[] KeyHandlersPressTextBox = new bool[300];
        public static bool OnKeyPressTextBoxOnly(Keyboard.Key key, int id)
        {
            if (!Keyboard.IsKeyPressed(key))
            {
                KeyHandlersPressTextBox[id] = true;
                return false;
            }
            else if (Keyboard.IsKeyPressed(key))
            {
                if (KeyHandlersPressTextBox[id])
                {
                    KeyHandlersPressTextBox[id] = false;
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

        public static string ReadInput(string input)
        {
            string output;

            output = Keys(input);

            return output;
        }

        private static bool[] LockKeys = new bool[256];
        private static string Keys(string input)
        {
            string output = input;
            string buffer;

            // Increment LockKeys for each key
            for (int i = 0; i < 256; i++)
            {
                if (!IsKeyPressed((Key)i))
                {
                    LockKeys[i] = true;
                }
                if (IsKeyPressed((Key)i))
                {
                    if (LockKeys[i])
                    {
                        buffer = ((Key)i).ToString();
                        if (buffer.Contains("Num"))
                        {
                            buffer = buffer.Remove(0, 3);
                        }
                        if (buffer == "Period")
                        {
                            buffer = buffer.Replace("Period", ".");
                        }
                        if (Alphabet.Contains(buffer))
                        {
                            output += buffer;
                        }
                    }
                    LockKeys[i] = false;
                }
            }
            return output;
        }
    }
}
