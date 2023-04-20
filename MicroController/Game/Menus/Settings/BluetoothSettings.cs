using MicroController.Graphics;
using MicroController.Graphics;
using MicroController.Helpers;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Instrumentation;
using Network;

namespace MicroController.Game
{
    public static partial class Settings
    {
        // ConnectingMode
        private static Text ConnectingModeText;
        private static DropBox ConnectingModeDropBox;
        private static Rectangle ConnectingModeRectangle;


        private static void BluetoothSettingsInit(Game game)
        {
            ConnectingModeText = new Text("Connecting Mode: ", MessegeManager.Courier) { Color = Color.Black };
            ConnectingModeDropBox = new DropBox(new Vector2f(), new Vector2f(), new List<string>() { "Manual", "Automatic" });
            ConnectingModeRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };


        }
        

        private static void BluetoothSettingsUpdate(Game game) 
        {
            // Universals
            uint characterSize = (uint)ButtonSize.Y / 4;
            float smallRectangleSizeX = game.Window.Size.X / 2.07f;
            float smallRectangleSizeY = game.Window.Size.Y / 9;
            Vector2i mousePos = Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet;


            Vector2f ConnectingModePosition = new Vector2f(4, ButtonSize.Y + 15);
            ConnectingModeText.Position = ConnectingModePosition;
            ConnectingModeText.CharacterSize = characterSize;

            ConnectingModeDropBox.Position = ConnectingModePosition + new Vector2f(ConnectingModeText.GetGlobalBounds().Width, 0);
            ConnectingModeDropBox.Size = new Vector2f(smallRectangleSizeX / 3, smallRectangleSizeY / 2);
            ConnectingModeDropBox.CharacterSize = characterSize;
            ConnectingModeDropBox.Update(mousePos);

        }

        private static void BluetoothSettingsDraw(RenderWindow window) 
        {
            ConnectingModeRectangle.Draw(window);
            window.Draw(ConnectingModeText);
            ConnectingModeDropBox.Draw(window);

        }

    }
}