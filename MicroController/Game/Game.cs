using microController.game.entities;
using microController.helpers;
using microController.system;
using microController.graphics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Security.Permissions;

namespace microController.game
{
    public class Game : GameLoop
    {
        private uint WindowWidth = 1080;
        private uint WindowHeight = 720;
        private string WindowTitle;
        public static Color WindowFillColor = new Color(MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f));

        public RayCaster rayCaster;
        
        public Map map;
        public Camera camera;
        public Firetruck firetruck;

        public int WindowState = 0;
        public bool IsGamePaused = false;

        public Game(uint windowWidth, uint windowHeight, string title) : base(windowWidth, windowHeight, title, WindowFillColor)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            WindowTitle = title;
        }

        protected override void LoadContent()
        {
            Window.SetKeyRepeatEnabled(false);
            MessegeManager.LoadContent();
            KeyboardManager.Init();
            BluetoothPacketManager.Init();
            Window.Resized += Window_Resized;
        }

        protected override void Initialize()
        {
            map = new Map(Map.GenerateMapWithWallRandom(100, 100), 20, new Window
            {
                Position = new Vector2i(50, 50),
                Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100)
            }, this);
            Scale.Create(3, map.SquareSize);

            rayCaster = new RayCaster(fov: 60, angleSpacingRay: 0.5f, depthOffFeild: 100, windowPosition: new Vector2i(0, 0),
                windowSize: new Vector2i((int)WindowWidth, (int)WindowHeight), rayMapColor: Color.Red, horizontalColor: new Color(150, 0, 0),
                verticalColor: new Color(255, 10, 10), drawMapRays: false);
            camera = new Camera(new Vector2f(100f, 100f), this);

            firetruck = new Firetruck(new Vector2f(100f, 100f), this);
            firetruck.VehicleDrivingMode = Firetruck.DrivingMode.Keyboard;

            PauseMenu.Init(this);
            RightClickMenu.Init(this);
        } 

        protected override void Update(GameTime gameTime)
        {
            if (!IsGamePaused)
            {
                camera.Update(map, gameTime);
                firetruck.Update(map, gameTime);
                if (WindowState == 0 || WindowState == 1)
                {
                    map.Window.Position = new Vector2i(50, 50);
                    rayCaster.WindowPosition = new Vector2i(0, 0);
                    rayCaster.WindowSize = new Vector2i((int)WindowWidth, (int)WindowHeight);
                    map.Window.Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100);
                }
                else if (WindowState == 2)
                {
                    map.Window.Position = new Vector2i(0, 0);
                    rayCaster.WindowPosition.X = (int)WindowWidth / 2;
                    rayCaster.WindowSize = new Vector2i((int)WindowWidth / 2, (int)WindowHeight);
                    map.Window.Size = new Vector2i((int)WindowWidth / 2, (int)WindowHeight);
                }
                map.Update(camera, GameTime);
                OpenCloseMap();
                RightClickMenu.Update(this);
                Scale.Update(map);
            }
            else
            {
                PauseMenu.Update(this);
            }
            PauseMenu.OpenClosePauseMenu(this);

            if (RightClickMenu.IsCameraPositionTracking)
                camera.Position = firetruck.Position;
            if (RightClickMenu.IsCameraRotationTracking)
                camera.Rotation = firetruck.Rotation;

            KeyboardManager.Update();
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
                        firetruck.Draw(this.Window);
                        rayCaster.Draw(this.Window, ref this.map, camera);
                        camera.Draw(this.Window);
                        break;
                    case 2:
                        rayCaster.Draw(this.Window, ref this.map, camera);
                        map.DrawMap(this.Window);
                        firetruck.Draw(this.Window);
                        rayCaster.Draw(this.Window, ref this.map, camera);
                        camera.Draw(this.Window);
                        break;
                }
            }
            if (IsGamePaused)
            {
                PauseMenu.Draw(this.Window);
            }
            RightClickMenu.Draw(this.Window);

            MessegeManager.DrawPerformanceData(this, Color.Red);
        }

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            if(Window.Size.X < 765)
            {
                Window.Size = new Vector2u(765, Window.Size.Y);
            }
            else if(Window.Size.Y < 308)
            {
                Window.Size = new Vector2u(Window.Size.X, 308);
            }
            else
            {
                WindowHeight = Window.Size.Y;
                WindowWidth = Window.Size.X;
                View view = new View(new FloatRect(0, 0, WindowWidth, WindowHeight));
                Window.SetView(view);
            }
        }

        private void OpenCloseMap()
        {
            if (KeyboardManager.OnKeyPressed(Keyboard.Key.M))
            {
                if (WindowState == 0)
                {
                    WindowState++;
                    rayCaster.DrawMapRays = false;
                    rayCaster.Draw3D = false;
                }
                else if (WindowState == 1)
                {
                    WindowState++;
                    rayCaster.DrawMapRays = false;
                    rayCaster.Draw3D = true;
                }
                else if( WindowState == 2)
                {
                    WindowState = 0;
                }
            }
        }
    }
}
