using SFML.Window;
using static SFML.Window.Keyboard;

namespace MicroController.InputOutput
{
    public static class KeyboardManagerS
    {
        public static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.BackSpaceReturn";

        private static bool[] KeyHandlersDown = new bool[(int)Key.KeyCount];
        
        private static bool[] KeyHandlers = new bool[(int)Key.KeyCount];

        private static bool[] KeyHandlersTime = new bool[(int)Key.KeyCount];
        private static float[] TimeHandlers = new float[(int)Key.KeyCount];

        private static bool[] KeyHandlersUp = new bool[(int)Key.KeyCount];
        private static bool[] UpHandler = new bool[(int)Key.KeyCount];

        /// <summary>
        /// Returns true only when the key is pressed
        /// </summary>
        /// <param name="key">The key you want to check</param>
        /// <returns></returns>
        public static bool OnKeyDown(Key key)
        {
            if (!IsKeyDown(key))
            {
                KeyHandlersDown[(int)key] = true;
                return false;
            }
            else
            {
                if (KeyHandlersDown[(int)key])
                {
                    KeyHandlersDown[(int)key] = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns true only when the key is relesed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool OnKeyUp(Key key)
        {
            if (!IsKeyDown(key))
            {
                KeyHandlersUp[(int)key] = true;
            }
            else
            {
                if (KeyHandlersUp[(int)key])
                {
                    KeyHandlersUp[(int)key] = false;
                    UpHandler[(int)key] = true;
                }
            }

            if (UpHandler[(int)key] && !IsKeyDown(key))
            { 
                UpHandler[(int)key] = false;
                return true;
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
        /// <param name="timeTillTrue">The time it will take until the function will return true</param>
        /// <returns></returns>
        public static bool OnKeyDownForTime(Key key, float timeTillTrue)
        {
            if (!IsKeyDown(key))
            {
                KeyHandlersTime[(int)key] = true;
            }
            else
            {
                if (KeyHandlersTime[(int)key])
                {
                    TimeHandlers[(int)key] = Program.Game.GameTime.TotalTimeElapsed;
                    KeyHandlersTime[(int)key] = false;
                }
            }

            if (!IsKeyDown(key))
            {
                TimeHandlers[(int)key] = 0;
            }

            if (IsKeyDown(key) && Program.Game.GameTime.TotalTimeElapsed - TimeHandlers[(int)key] > timeTillTrue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsKeyDown(Key key)
        {
            return Keyboard.IsKeyPressed(key);
        }

        public static string ReadInput(string input)
        {
            string output;

            output = Keys(input);

            return output;
        }

        private static string Keys(string input)
        {
            string output = input;
            string buffer;

            // Increment LockKeys for each key
            for (int i = 0; i < (int)Key.KeyCount; i++)
            {
                if (!IsKeyDown((Key)i))
                {
                    KeyHandlers[i] = true;
                }
                if (IsKeyDown((Key)i))
                {
                    if (KeyHandlers[i])
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
                    KeyHandlers[i] = false;
                }
            }
            return output;
        }
    }
}
