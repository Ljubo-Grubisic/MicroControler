using MicroController.GUI;
using MicroController.Shapes;
using MicroController.InputOutput;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Instrumentation;

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
            ConnectingModeText = new Text("Connecting Mode: ", MessegeManager.Courier);
            ConnectingModeDropBox = new DropBox(new Vector2f(), new Vector2f(), new List<string>() { "Manual", "Semi Automatic", "Automatic" });
            ConnectingModeRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };


        }

        private static void BluetoothSettingsUpdate(Game game) 
        {
            // Universals
            uint characterSize = (uint)ButtonSize.Y / 4;
            float smallRectangleSizeX = game.Window.Size.X / 2.07f;
            float smallRectangleSizeY = game.Window.Size.Y / 9;

            float largeRectangleSizeX = smallRectangleSizeX;
            float largeRectangleSizeY = smallRectangleSizeY * 2.2f;

        }

        private static void BluetoothSettingsDraw(RenderWindow window) 
        {
        
        }

    }
}