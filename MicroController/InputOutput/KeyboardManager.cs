using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;

namespace MicroController.InputOutput
{
    public static class KeyboardManager
    {
        private static bool[] KeysPressed = new bool[(int)Key.KeyCount];
        private static bool[] KeysReleased = new bool[(int)Key.KeyCount];

        private static bool[] KeysHandler = new bool[(int)Key.KeyCount];
        private static bool[] KeysHandlerDown = new bool[(int)Key.KeyCount];
        private static bool[] KeysHandlerUp = new bool[(int)Key.KeyCount];

        private static bool[] UpHandler = new bool[(int)Key.KeyCount];
        private static float[] TimeHandler = new float[(int)Key.KeyCount];

        private static void UpdateKeyPressed()
        {
            for (int i = 0; i < (int)Key.KeyCount; i++)
            {
                if (!IsKeyPressed((Key)i))
                {
                    KeysHandlerDown[i] = true;
                    KeysPressed[i] = false;
                }
                else
                {
                    if (KeysHandlerDown[i])
                    {
                        KeysHandlerDown[i] = false;
                        KeysPressed[i] = true;
                    }
                    else
                    {
                        KeysPressed[i] = false;
                    }
                }
            }
        }
        private static void UpdateKeyReleased()
        {
            for (int i = 0; i < (int)Key.KeyCount; i++)
            {
                // Key Relased
                if (!IsKeyDown((Key)i))
                {
                    KeysHandlerUp[i] = true;
                }
                else
                {
                    if (KeysHandlerUp[i])
                    {
                        KeysHandlerUp[i] = false;
                        UpHandler[i] = true;
                    }
                }

                if (UpHandler[i] && !IsKeyDown((Key)i))
                {
                    UpHandler[i] = false;
                    KeysReleased[i] = true;
                }
                else
                {
                    KeysReleased[i] = false;
                }
            }
        }

        /// <summary>
        /// Returns true only when the key is pressed
        /// </summary>
        /// <param name="key">The key you want to check</param>
        /// <returns></returns>
        public static bool OnKeyPressed(Key key)
        {
            UpdateKeyPressed();
            return KeysPressed[(int)key];
        }

        /// <summary>
        /// Returns true only when the key is released
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool OnKeyReleased(Key key)
        {
            UpdateKeyReleased();
            return KeysReleased[(int)key];
        }

        /// <summary>
        /// Returns true if the key is down
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyDown(Key key)
        {
            return Keyboard.IsKeyPressed(key);
        }

        /// <summary>
        /// Returns true after you have pressed the button for 1 s
        /// </summary>
        /// <param name="key">The key you want to check</param>
        /// <param name="timeTillTrue">The time it will take until the function will return true</param>
        /// <returns></returns>
        public static bool OnKeyDownForTime(Key key, float timeTillTrue)
        {
            UpdateKeyPressed();
            if (OnKeyPressed(key))
            {
                TimeHandler[(int)key] = Program.Game.GameTime.TotalTimeElapsed;
            }
            if (!IsKeyDown(key))
            {
                TimeHandler[(int)key] = 0;
            }
            if (IsKeyDown(key) && Program.Game.GameTime.TotalTimeElapsed - TimeHandler[(int)key] > timeTillTrue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ReadInput(string input, string acceptableCharacters)
        {
            string output;

            output = Keys(acceptableCharacters);

            return output;
        }


        private static string Keys(string acceptableCharacters)
        {
            string output = "";
            string buffer;

            // Increment LockKeys for each key
            for (int i = 0; i < (int)Key.KeyCount; i++)
            {
                if (!IsKeyDown((Key)i))
                {
                    KeysHandler[i] = true;
                }
                if (IsKeyDown((Key)i))
                {
                    if (KeysHandler[i])
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
                        if (acceptableCharacters.Contains(buffer))
                        {
                            output += buffer;
                        }
                    }
                    KeysHandler[i] = false;
                }
            }
            return output;
        }
    }
}
