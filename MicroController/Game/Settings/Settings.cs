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
        private static string WindowSelected = "RayCaster";

        private static Rectangle Rectangle;
        private static Rectangle OutlineRectangle;

        private static Button RayCasterButton;
        private static Button MapButton;
        private static Button CameraButton;
        private static Button CloseSettingsButton;

        private static Button ApplySettingsButton;

        private static int buttonSpacing = 3;
        private static Vector2f ButtonSize = new Vector2f();

        private static Color RectangleFillColor = new Color(140, 191, 187);
        private static Color RectangleOutlineColor = new Color(69, 94, 92);

        public static void Init(Game game)
        {
            Game = game;

            Rectangle = new Rectangle(new Vector2f(), new Vector2f());
            OutlineRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = Color.Black };

            RayCasterButton = new Button(new Vector2f(), new Vector2f(), "RayCaster", MessegeManager.Courier, 0);
            MapButton = new Button(new Vector2f(), new Vector2f(), "Map", MessegeManager.Courier, 0);
            CameraButton = new Button(new Vector2f(), new Vector2f(), "Camera", MessegeManager.Courier, 0);
            CloseSettingsButton = new Button(new Vector2f(), new Vector2f(), "X", MessegeManager.Courier, 0);
            ApplySettingsButton = new Button(new Vector2f(), new Vector2f(), "Apply", MessegeManager.Courier, 0);

            RayCasterButton.ButtonClicked += RayCasterButton_ButtonClicked;
            MapButton.ButtonClicked += MapButton_ButtonClicked;
            CameraButton.ButtonClicked += CameraButton_ButtonClicked;
            CloseSettingsButton.ButtonClicked += CloseSettingsButton_ButtonClicked;
            ApplySettingsButton.ButtonClicked += ApplySettingsButton_ButtonClicked;
            game.Window.Resized += Window_Resized;

            RayCasterSettingsInit(game);
            MapSettingsInit(game);
            CameraSettingsInit(game);
        }


        private static void SearchBarSettingsUpdate(Game game) 
        {
            ButtonSize.X = game.Window.Size.X / 7;
            ButtonSize.Y = game.Window.Size.Y / 9;

            Rectangle.SizeX = game.Window.Size.X; Rectangle.SizeY = game.Window.Size.Y;
            OutlineRectangle.PositionX = 2f; OutlineRectangle.PositionY = 2f;
            OutlineRectangle.SizeX = game.Window.Size.X + 2; OutlineRectangle.SizeY = ButtonSize.Y;

            RayCasterButton.PositionX = 2;
            RayCasterButton.PositionY = 2;
            RayCasterButton.Size = ButtonSize;
            RayCasterButton.Text.CharacterSize = (uint)ButtonSize.Y / 3;
            RayCasterButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));

            MapButton.PositionX = ButtonSize.X + buttonSpacing * 2;
            MapButton.PositionY = 2;
            MapButton.Size = ButtonSize;
            MapButton.Text.CharacterSize = (uint)ButtonSize.Y / 3;
            MapButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));

            CameraButton.PositionX = ButtonSize.X * 2 + buttonSpacing * 3 + 1;
            CameraButton.PositionY = 2;
            CameraButton.Size = ButtonSize;
            CameraButton.Text.CharacterSize = (uint)ButtonSize.Y / 3;
            CameraButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));

            
            CloseSettingsButton.PositionX = game.Window.Size.X - ButtonSize.Y - 2;
            CloseSettingsButton.PositionY = 2;
            CloseSettingsButton.SizeX = ButtonSize.Y;
            CloseSettingsButton.SizeY = ButtonSize.Y;
            CloseSettingsButton.Text.CharacterSize = (uint)ButtonSize.Y / 2;
            CloseSettingsButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));

            ApplySettingsButton.Position = (Vector2f)game.Window.Size - ButtonSize - new Vector2f(2, 2);
            ApplySettingsButton.Size = ButtonSize;
            ApplySettingsButton.Text.CharacterSize = (uint)ButtonSize.Y / 2;
            ApplySettingsButton.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
        }
        
        private static void SearchBarSettingsDraw(RenderWindow window)
        {
            Rectangle.Draw(window);
            OutlineRectangle.Draw(window);
            RayCasterButton.Draw(window);
            MapButton.Draw(window);
            CameraButton.Draw(window);
            CloseSettingsButton.Draw(window);
            ApplySettingsButton.Draw(window);
        }

        public static void Update(Game game)
        {
            SearchBarSettingsUpdate(game);

            if (WindowSelected == Windows[0])
            {
                RayCasterSettingsUpdate(game);
            }
            if (WindowSelected == Windows[1])
            {
                MapSettingsUpdate(game);
            }
            if (WindowSelected == Windows[2])
            {
                CameraSettingsUpdate(game);
            }
        }

        public static void Draw(RenderWindow window)
        {
            SearchBarSettingsDraw(window);

            if (WindowSelected == Windows[0])
            {
                RayCasterSettingsDraw(window);
            }
            if (WindowSelected == Windows[1])
            {
                MapSettingsDraw(window);
            }
            if (WindowSelected == Windows[2])
            {
                CameraSettingsDraw(window);
            }
        }

        private static void RayCasterButton_ButtonClicked(object source, EventArgs args)
        {
            WindowSelected = Windows[0];
        }

        private static void MapButton_ButtonClicked(object source, EventArgs args)
        {
            WindowSelected = Windows[1];
        }

        private static void CameraButton_ButtonClicked(object source, EventArgs args)
        {
            WindowSelected = Windows[2];
        }

        private static void CloseSettingsButton_ButtonClicked(object source, EventArgs args)
        {
            PauseMenu.IsSettingsOpen = false;
        }
        private static void Window_Resized(object sender, SizeEventArgs e)
        {
            RayCasterWindowResized = true;
            MapWindowResized = true;
            CameraWindowResized= true;
        }

        private static void ApplySettingsButton_ButtonClicked(object source, EventArgs args)
        {
            // RayCaster
            Game.rayCaster.Fov = Fov;
            Game.rayCaster.AngleSpacingRay = AngleSpacingRay;
            Game.rayCaster.DepthOffFeild = DepthOffFeild;
            Game.rayCaster.RayMapColor = RayMapColor;
            Game.rayCaster.HorizontalColor = HorizontalColor;
            Game.rayCaster.VerticalColor = VerticalColor;

            // Map
            Game.map.SquareSize = SquareSize;
            Game.map.Image0FillColor = Image0FillColor;
            Game.map.Image1FillColor = Image1FillColor;
            Game.map.OutlineColor = OutlineColor;
            Game.map.UpdateSquareImagesForce();
            Game.map.UpdateMapTextureForce();

            // Camera
            Game.camera.Scale = Scale;
            Game.camera.Speed = Speed;
        }
    }
}
