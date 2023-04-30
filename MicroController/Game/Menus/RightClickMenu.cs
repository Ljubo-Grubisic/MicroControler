using microController.graphics;
using microController.helpers;
using microController.game;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace microController.game
{
    public static class RightClickMenu
    {
        private static Rectangle Rectangle;
        private static Button CameraPositionTrackFiretruck;
        private static Button CameraRotationTrackFiretruck;

        private static bool Clicked = false;
        private static Vector2i MousePos;

        public static bool IsCameraPositionTracking = false;
        public static bool IsCameraRotationTracking = false;

        public static void Init(Game game)
        {
            Rectangle = new Rectangle(new Vector2f(), new Vector2f());
            CameraPositionTrackFiretruck = new Button(new Vector2f(), new Vector2f(), "Camera pos track firetruck");
            CameraRotationTrackFiretruck = new Button(new Vector2f(), new Vector2f(), "Camera rot track firetruck");

            CameraPositionTrackFiretruck.ButtonClicked += CameraPositionTrackFiretruck_ButtonClicked;
            CameraRotationTrackFiretruck.ButtonClicked += CameraRotationTrackFiretruck_ButtonClicked;
            game.Window.MouseButtonPressed += Window_MouseButtonPressed;
        }


        public static void Update(Game game)
        {
            float buttonSizeX = game.Window.Size.X / 8;
            float buttonSizeY = game.Window.Size.Y / 10;

            int numberButtons = 2;

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
        }
        
        private static void CameraRotationTrackFiretruck_ButtonClicked(Button source, EventArgs args)
        {
            IsCameraRotationTracking = !IsCameraRotationTracking;
        }

        private static void CameraPositionTrackFiretruck_ButtonClicked(Button source, EventArgs args)
        {
            IsCameraPositionTracking = !IsCameraPositionTracking;
        }
    }
}
