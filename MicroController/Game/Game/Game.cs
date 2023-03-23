using MicroController.Game.Entities;
using MicroController.InputOutput;
using MicroController.InputOutput.PortComunication;
using MicroController.Game.Maping;
using MicroController.Game.RayCasting;
using MicroController.MainLooping;
using MicroController.Mathematics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using MicroController.GUI;
using MicroController.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MicroController.Game
{
    public partial class Game : MainLoop
    {
        private uint WindowWidth = 1080;
        private uint WindowHeight = 720;
        private string WindowTitle;
        public static Color WindowFillColor = new Color(MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f));

        public RayCaster rayCaster;

        private Serial serial;
        public Map map;
        public Camera camera;

        private int WindowState = 0;
        public bool IsGamePaused = false;

        public Game(uint windowWidth, uint windowHeight, string title) : base(windowWidth, windowHeight, title, WindowFillColor)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            WindowTitle = title;
        }

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            if(Window.Size.X < 765 || Window.Size.Y < 308)
            {
                Window.Size = new Vector2u(765, 308);
            }
            else
            {
                WindowHeight = Window.Size.Y;
                WindowWidth = Window.Size.X;
                View view = new View(new FloatRect(0, 0, WindowWidth, WindowHeight));
                rayCaster.WindowSize = new Vector2i((int)WindowWidth, (int)WindowHeight);
                Window.SetView(view);
            }
        }

        private void OpenCloseMap()
        {
            if (KeyboardManager.OnKeyPress(Keyboard.Key.M, 0))
            {
                if (WindowState == 0)
                {
                    WindowState++;
                    rayCaster.DrawMapRays = true;
                    rayCaster.Draw3D = false;
                }
                else if (WindowState == 1)
                {
                    WindowState--;
                    rayCaster.DrawMapRays = false;
                    rayCaster.Draw3D = true;
                }
            }
        }
    }
}
