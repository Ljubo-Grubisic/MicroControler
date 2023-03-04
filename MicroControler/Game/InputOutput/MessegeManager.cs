using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using MicroControler.GameLooping;

namespace MicroControler.Game.InputOutput
{
    public static class MessegeManager
    {
        public const string CONSOLE_FONT_PATH = "Fonts/arial.ttf";
        public static Font consoleFont;

        public static void LoadContent()
        {
            consoleFont = new Font(CONSOLE_FONT_PATH);
        }

        public static void DrawPerformanceData(GameLoop gameLoop, Color fontColor)
        {
            if (consoleFont ==  null)
                return;

            string totalTimeElapsedStr = gameLoop.GameTime.TotalTimeElapsed.ToString("0.000");
            string deltaTimeStr = gameLoop.GameTime.DeltaTime.ToString("0.00000");
            float fps = 1f / gameLoop.GameTime.DeltaTime;
            string fpsStr = fps.ToString("0.00");

            SFML.Graphics.Text textTotalTimeElapsed = new SFML.Graphics.Text(totalTimeElapsedStr, consoleFont, 14);
            textTotalTimeElapsed.Position = new Vector2f(4f, 8f);
            textTotalTimeElapsed.Color = fontColor;

            SFML.Graphics.Text textDeltaTime = new SFML.Graphics.Text(deltaTimeStr, consoleFont, 14);
            textDeltaTime.Position = new Vector2f(4f, 28f);
            textDeltaTime.Color = fontColor;

            SFML.Graphics.Text textFps = new SFML.Graphics.Text(fpsStr, consoleFont, 14);
            textFps.Position = new Vector2f(4f, 48f);
            textFps.Color = fontColor;

            gameLoop.Window.Draw(textTotalTimeElapsed);
            gameLoop.Window.Draw(textDeltaTime);
            gameLoop.Window.Draw(textFps);
        }
        public static void Message(GameLoop gameLoop, string textStr, Color fontColor, int messegeNumber)
        {
            if (consoleFont == null)
                return;

            SFML.Graphics.Text text = new SFML.Graphics.Text(textStr, consoleFont, 14);
            text.Position = new Vector2f(4f, 68f + 20 * (messegeNumber - 1));
            text.Color = fontColor;

            gameLoop.Window.Draw(text);
        }
        public static void Message(GameLoop gameLoop, string textStr)
        {
            if (consoleFont == null)
                return;

            SFML.Graphics.Text text = new SFML.Graphics.Text(textStr, consoleFont, 14);
            text.Position = new Vector2f(4f, 68f);
            text.Color = Color.White;

            gameLoop.Window.Draw(text);
        }
        public static void Message(GameLoop gameLoop, string textStr, Vector2f postion)
        {
            if (consoleFont == null)
                return;

            SFML.Graphics.Text text = new SFML.Graphics.Text(textStr, consoleFont, 14);
            text.Position = new Vector2f(postion.X, postion.Y);
            text.Color = Color.White;

            gameLoop.Window.Draw(text);
        }
        public static void Message(RenderWindow window, string textStr, Color fontColor, int messegeNumber)
        {
            if (consoleFont == null)
                return;

            SFML.Graphics.Text text = new SFML.Graphics.Text(textStr, consoleFont, 14);
            text.Position = new Vector2f(4f, 68f + 20 * (messegeNumber - 1));
            text.Color = fontColor;

            window.Draw(text);
        }
        public static void Message(RenderWindow window, string textStr)
        {
            if (consoleFont == null)
                return;

            SFML.Graphics.Text text = new SFML.Graphics.Text(textStr, consoleFont, 14);
            text.Position = new Vector2f(4f, 68f);
            text.Color = Color.White;

            window.Draw(text);
        }
        public static void Message(RenderWindow window, string textStr, Vector2f postion)
        {
            if (consoleFont == null)
                return;

            SFML.Graphics.Text text = new SFML.Graphics.Text(textStr, consoleFont, 14);
            text.Position = new Vector2f(postion.X, postion.Y);
            text.Color = Color.White;

            window.Draw(text);
        }
    }
}
