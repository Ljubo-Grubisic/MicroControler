using microController.graphics;
using microController.helpers;
using microController.game;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.CodeDom;

namespace microController.game
{
    public static class RightClickMenu
    {
        private static Rectangle Rectangle;
        private static Button CameraPositionTrackFiretruck;
        private static Button CameraRotationTrackFiretruck;
        private static Button PlacePathFinderEndPoint;

        private static bool Clicked = false;
        private static Vector2i MousePos;

        public static bool IsCameraPositionTracking = false;
        public static bool IsCameraRotationTracking = false;

        private static Game Game;

        public static void Init(Game game)
        {
            Rectangle = new Rectangle(new Vector2f(), new Vector2f());
            CameraPositionTrackFiretruck = new Button(new Vector2f(), new Vector2f(), "Camera pos track firetruck");
            CameraRotationTrackFiretruck = new Button(new Vector2f(), new Vector2f(), "Camera rot track firetruck");
            PlacePathFinderEndPoint = new Button(new Vector2f(), new Vector2f(), "Place pathfinder end point");

            game.Window.MouseButtonPressed += Window_MouseButtonPressed;
            CameraPositionTrackFiretruck.ButtonClicked += CameraPositionTrackFiretruck_ButtonClicked;
            CameraRotationTrackFiretruck.ButtonClicked += CameraRotationTrackFiretruck_ButtonClicked;
            PlacePathFinderEndPoint.ButtonClicked += PlacePathFinderEndPoint_ButtonClicked;
        }

        public static void Update(Game game)
        {
            Game = game;
            float buttonSizeX = game.Window.Size.X / 6;
            float buttonSizeY = game.Window.Size.Y / 10;

            if (buttonSizeY > 50)
            {
                buttonSizeY = 50;
            }
            if (buttonSizeX > 150)
            {
                buttonSizeX = 150;
            }

            int numberButtons = 3;

            if (Clicked)
            {
                CameraPositionTrackFiretruck.Position = (Vector2f)MousePos;
                CameraPositionTrackFiretruck.SizeX = buttonSizeX;
                CameraPositionTrackFiretruck.SizeY = buttonSizeY;
                CameraPositionTrackFiretruck.CharacterSize = (uint)buttonSizeX / 14;
                CameraPositionTrackFiretruck.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

                CameraRotationTrackFiretruck.Position = (Vector2f)MousePos + new Vector2f(0, buttonSizeY);
                CameraRotationTrackFiretruck.SizeX = buttonSizeX;
                CameraRotationTrackFiretruck.SizeY = buttonSizeY;
                CameraRotationTrackFiretruck.CharacterSize = (uint)buttonSizeX / 14;
                CameraRotationTrackFiretruck.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

                PlacePathFinderEndPoint.Position = (Vector2f)MousePos + new Vector2f(0, buttonSizeY * 2);
                PlacePathFinderEndPoint.SizeX = buttonSizeX;
                PlacePathFinderEndPoint.SizeY = buttonSizeY;
                PlacePathFinderEndPoint.CharacterSize = (uint)buttonSizeX / 14;
                PlacePathFinderEndPoint.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

                Rectangle.Position = (Vector2f)MousePos;
                Rectangle.SizeX = buttonSizeX;
                Rectangle.SizeY = buttonSizeY * numberButtons;
            }
            else
            {
                Rectangle.Position = new Vector2f(-500, -500);
                Rectangle.SizeX = 0;
                Rectangle.SizeY = 0;
            }
        }

        public static void Draw(RenderWindow window)
        {
            if (Clicked)
            {
                CameraPositionTrackFiretruck.Draw(window);
                CameraRotationTrackFiretruck.Draw(window);
                PlacePathFinderEndPoint.Draw(window);
            }
        }

        private static void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Right)
            {
                MousePos = new Vector2i(e.X, e.Y);
                if (MathHelper.IsMouseInRectangle(Rectangle, MousePos))
                {
                    Clicked = false;
                }
                else
                {
                    Clicked = true;
                }
            }
            if (e.Button == Mouse.Button.Left)
            {
                if (!MathHelper.IsMouseInRectangle(Rectangle, new Vector2i(e.X, e.Y)))
                {
                    Clicked = false;
                }
            }
        }
        
        private static void CameraRotationTrackFiretruck_ButtonClicked(Button source, EventArgs args)
        {
            IsCameraRotationTracking = !IsCameraRotationTracking;
        }

        private static void CameraPositionTrackFiretruck_ButtonClicked(Button source, EventArgs args)
        {
            IsCameraPositionTracking = !IsCameraPositionTracking;
        }

        private static void PlacePathFinderEndPoint_ButtonClicked(Button source, EventArgs args)
        {
            Vector2i Starting = new Vector2i((int)Game.firetruck.Position.Y / Game.map.SquareSize, (int)Game.firetruck.Position.X / Game.map.SquareSize);
            Vector2i Ending = new Vector2i((MousePos.Y - Game.map.Window.Position.Y) / Game.map.SquareSize + Game.map.SquareStarting.X, (MousePos.X - Game.map.Window.Position.X) / Game.map.SquareSize + Game.map.SquareStarting.Y);
            if (Game.map.Data[Starting.X, Starting.Y] != 1 && Game.map.Data[Ending.X, Ending.Y] != 1)
            {
                Game.firetruck.PathFinder.FindPath(new Node { Id = { I = Starting.X, J = Starting.Y } }, new Node { Id = { I = Ending.X, J = Ending.Y } }, Game.map);
            }
        }
    }
}  
