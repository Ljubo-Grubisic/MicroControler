using MicroController.Game.Entities;
using MicroController.InputOutput;
using MicroController.InputOutput.PortComunication;
using MicroController.Game.Maping;
using MicroController.Game.RayCasting;
using MicroController.GameLooping;
using MicroController.Mathematics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using MicroController.GUI;
using MicroController.Shapes;
using System.Diagnostics;

namespace MicroController.Game
{
    public partial class Game : GameLoop
    {
        private uint WindowWidth = 1080;
        private uint WindowHeight = 720;
        private string WindowTitle;
        public static Color WindowFillColor = new Color(MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f));

        private RayCaster rayCaster;

        private Serial serial;
        private Map map;
        private Camera camera;
        private Button button;
        private Button button2;

        private int WindowState = 0;

        public Game(uint windowWidth, uint windowHeight, string title) : base(windowWidth, windowHeight, title, WindowFillColor)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            WindowTitle = title;
        }

        #region Setup
        protected override void LoadContent()
        {
            MessegeManager.LoadContent();
            
        }

        protected override void Initialize()
        {
            map = new Map(Map.GenerateMapWithWall(1000, 1000), 10, new Maping.Window
            {
                Position = new Vector2i(50, 50),
                Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100)
            });

            rayCaster = new RayCaster(fov: 60, angleSpacingRay: 0.5f, depthOffFeild: 100, windowPosition: new Vector2i(0, 0), 
                windowSize: new Vector2i((int)WindowWidth, (int)WindowHeight), rayMapColor: Color.Red, horizontalColor: new Color(150, 0, 0),
                verticalColor: new Color(255, 10, 10), drawMapRays: false);
            camera = new Camera(new Vector2f(100f, 100f));

            button = new Button(new Vector2f(500, 410), new Vector2f(150, 50), "TextLargeButton");
            button.ButtonClicked += Button_ButtonClicked;

            button2 = new Button(new Vector2f(100, 100), new Vector2f(450, 300), "BUtton");

            serial = new Serial("COM3", 9600);
            serial.StartReading();

        }

        private void Button_ButtonClicked(object source, EventArgs args)
        {
            Console.WriteLine(1);
        }
        #endregion

        #region Loop
        protected override void Update(GameTime gameTime)
        {
            ResizeWindow();
            camera.Update(gameTime, map);
            map.Update(camera, GameTime);
            OpenCloseMap();
            button.Update(Mouse.GetPosition() - Window.Position - new Vector2i(8, 30), Mouse.IsButtonPressed(Mouse.Button.Left));
            button2.Update(Mouse.GetPosition() - Window.Position - new Vector2i(8, 30), Mouse.IsButtonPressed(Mouse.Button.Left));
        }

        protected override void Draw(GameTime gameTime)
        {
            WindowState = 8;
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
            button2.FontSize = 100;

            button.Draw(Window);
            button2.Draw(Window);
            //MessegeManager.Message(this, serial.Info, Color.Red, 1);

            //MessegeManager.DrawPerformanceData(this, Color.Red);
        }

        private void ResizeWindow()
        {
            if (WindowHeight != Window.Size.Y || WindowWidth != Window.Size.X)
            {
                WindowHeight = Window.Size.Y;
                WindowWidth = Window.Size.X;
                View view = new View(new FloatRect(0, 0, WindowWidth, WindowHeight));
                rayCaster.WindowSize = new Vector2i((int)WindowWidth, (int)WindowHeight);
                map.MapWindow.Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100);
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
        #endregion
    }
}
