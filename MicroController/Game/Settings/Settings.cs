using MicroController.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using MicroController.Game.Entities;
using MicroController.Game.Maping;
using MicroController.Game.RayCasting;
using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;
using MicroController.Shapes;
using MicroController.InputOutput;

namespace MicroController.Game
{
    public static partial class Settings
    {
        private static Game Game;

        private static string[] Windows = new string[] { "RayCaster", "Map", "Camera" };

        private static Rectangle Rectangle;
        private static Line Line;

        private static Button RayCasterButton;
        private static Button MapButton;
        private static Button CameraButton;

        private static int buttonSpacing = 3;

        public static void Init(Game game)
        {
            Game = game;

            Rectangle = new Rectangle(new Vector2f(), new Vector2f());
            Line = new Line() { OutlineColor = Color.Black, OutlineThickness = 1 };

            RayCasterButton = new Button(new Vector2f(), new Vector2f(), "RayCaster", MessegeManager.Courier, 0);
            MapButton = new Button(new Vector2f(), new Vector2f(), "Map", MessegeManager.Courier, 0);
            CameraButton = new Button(new Vector2f(), new Vector2f(), "Camera", MessegeManager.Courier, 0);

            RayCasterButton.ButtonClicked += RayCasterButton_ButtonClicked;
            MapButton.ButtonClicked += MapButton_ButtonClicked;
            CameraButton.ButtonClicked += CameraButton_ButtonClicked;
        }

        public static void Update(Game game) 
        {
            float buttonSizeX = game.Window.Size.X / 7;
            float buttonSizeY = game.Window.Size.Y / 9;

            Rectangle.PositionX = 0; Rectangle.PositionY = 0;
            Rectangle.SizeX = game.Window.Size.X; Rectangle.SizeY = game.Window.Size.Y;
            Line.Position0X = 0; Line.Position0Y = buttonSizeY + 3;
            Line.Position1X = game.Window.Size.X + 0; Line.Position1Y = buttonSizeY + 3;

            RayCasterButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            RayCasterButton.PositionX = 2;
            RayCasterButton.PositionY = 2;
            RayCasterButton.SizeX = buttonSizeX;
            RayCasterButton.SizeY = buttonSizeY;
            RayCasterButton.Text.CharacterSize = (uint)buttonSizeY / 4;

            MapButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            MapButton.PositionX = buttonSizeX + buttonSpacing * 2;
            MapButton.PositionY = 2;
            MapButton.SizeX = buttonSizeX;
            MapButton.SizeY = buttonSizeY;
            MapButton.Text.CharacterSize = (uint)buttonSizeY / 4;

            CameraButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            CameraButton.PositionX = buttonSizeX * 2 + buttonSpacing * 3 + 1;
            CameraButton.PositionY = 2;
            CameraButton.SizeX = buttonSizeX;
            CameraButton.SizeY = buttonSizeY;
            CameraButton.Text.CharacterSize = (uint)buttonSizeY / 4;
        }

        public static void Draw(RenderWindow window)
        {
            Rectangle.Draw(window);
            Line.Draw(window);
            RayCasterButton.Draw(window);
            MapButton.Draw(window);
            CameraButton.Draw(window);
        }

        private static void RayCasterButton_ButtonClicked(object source, EventArgs args)
        {

        }

        private static void MapButton_ButtonClicked(object source, EventArgs args)
        {

        }

        private static void CameraButton_ButtonClicked(object source, EventArgs args)
        {

        }
    }
}
