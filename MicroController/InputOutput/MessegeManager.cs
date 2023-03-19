using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using MicroController.MainLooping;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using MicroController.Mathematics;
using MicroController.Shapes;

namespace MicroController.InputOutput
{
    public static class MessegeManager
    {
        private const string Arial_FONT_PATH = "Resources/Fonts/arial.ttf";
        private const string Courier_FONT_PATH = "Resources/Fonts/courier.ttf";
        public static Font Arial;
        public static Font Courier;

        public static void LoadContent()
        {
            Arial = new Font(Arial_FONT_PATH);
            Courier = new Font(Courier_FONT_PATH);
        }

        public static void DrawPerformanceData(MainLoop gameLoop, Color fontColor)
        {
            if (Arial ==  null)
                return;

            string totalTimeElapsedStr = gameLoop.GameTime.TotalTimeElapsed.ToString("0.000");
            string deltaTimeStr = gameLoop.GameTime.DeltaTime.ToString("0.00000");
            float fps = 1f / gameLoop.GameTime.DeltaTime;
            string fpsStr = fps.ToString("0.00");

            SFML.Graphics.Text textTotalTimeElapsed = new SFML.Graphics.Text(totalTimeElapsedStr, Arial, 14);
            textTotalTimeElapsed.Position = new Vector2f(4f, 8f);
            textTotalTimeElapsed.Color = fontColor;

            SFML.Graphics.Text textDeltaTime = new SFML.Graphics.Text(deltaTimeStr, Arial, 14);
            textDeltaTime.Position = new Vector2f(4f, 28f);
            textDeltaTime.Color = fontColor;

            SFML.Graphics.Text textFps = new SFML.Graphics.Text(fpsStr, Arial, 14);
            textFps.Position = new Vector2f(4f, 48f);
            textFps.Color = fontColor;

            gameLoop.Window.Draw(textTotalTimeElapsed);
            gameLoop.Window.Draw(textDeltaTime);
            gameLoop.Window.Draw(textFps);
        }

        public static void Message(RenderWindow window, string textStr, Font font, Vector2f position, Color fontColor, uint fontSize = 14)
        {
            if (Arial == null)
                return;

            Text text = new Text(textStr, font, fontSize);
            text.Position = new Vector2f(position.X, position.Y);
            text.Color = fontColor;

            window.Draw(text);
        }
        public static void Message(RenderWindow window, string textStr, Font font, Vector2f position, uint fontSize = 14)
        {
            if (Arial == null)
                return;

            Text text = new Text(textStr, font, fontSize);
            text.Position = new Vector2f(position.X, position.Y);
            text.Color = Color.Black;

            window.Draw(text);
        }
        public static void Message(RenderWindow window, string textStr, Vector2f position, uint fontSize = 14)
        {
            if (Arial == null)
                return;

            Text text = new Text(textStr, Arial, fontSize);
            text.Position = new Vector2f(position.X, position.Y);
            text.Color = Color.Black;

            window.Draw(text);
        }

        public static FloatRect GetTextRect(string textStr, Font font, uint fontSize)
        {
            return new Text(textStr, font, fontSize).GetLocalBounds();
        }
    }
}
