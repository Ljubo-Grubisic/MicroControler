using MicroController.Game.Entities;
using MicroController.InputOutput;
using MicroController.InputOutput.PortComunication;
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
using MicroController.Game.Entities.Sensors;

namespace MicroController.Game
{
    public class Game : MainLoop
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

        protected override void LoadContent()
        {
            MessegeManager.LoadContent();
            Window.Resized += Window_Resized;
        }

        protected override void Initialize()
        {
            map = new Map(Map.GenerateMapWithWallRandom(1000, 1000), 20, new Window
            {
                Position = new Vector2i(50, 50),
                Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100)
            }, this);

            rayCaster = new RayCaster(fov: 60, angleSpacingRay: 0.5f, depthOffFeild: 100, windowPosition: new Vector2i(0, 0),
                windowSize: new Vector2i((int)WindowWidth, (int)WindowHeight), rayMapColor: Color.Red, horizontalColor: new Color(150, 0, 0),
                verticalColor: new Color(255, 10, 10), drawMapRays: false);
            camera = new Camera(new Vector2f(100f, 100f), this);

            serial = new Serial("COM3", 9600);
            serial.StartReading();

            PauseMenu.Init(this);
            Scale.Create(1, map.SquareSize);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsGamePaused)
            {
                camera.Update(gameTime, map);
                map.Window.Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100);
                map.Update(camera, GameTime);
                OpenCloseMap();
                Scale.Update(map);
            }
            else
            {
                PauseMenu.Update(this);
            }
            PauseMenu.OpenClosePauseMenu(this);
        }

        protected override void Draw(GameTime gameTime)
        {

            if (!PauseMenu.IsSettingsOpen)
            {
                switch (WindowState)
                {
                    case 0:
                        rayCaster.Draw(this.Window, ref this.map, camera);
                        break;
                    case 1:
                        map.DrawMap(this.Window);
                        rayCaster.Draw(this.Window, ref this.map, camera);
                        camera.Draw(this.Window);
                        break;
                }
            }
            if (IsGamePaused)
            {
                PauseMenu.Draw(this.Window);
            }

            MessegeManager.DrawPerformanceData(this, Color.Red);
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
