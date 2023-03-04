using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using MicroControler.Mathematics;
using MicroControler.Game.Text;
using MicroControler.Game.RayCasting;
using MicroControler.PortComunication;
using MicroControler.Game.Entity;
using MicroControler.GameLooping;
using MicroControler.Game.Maping;

namespace MicroControler.Game
{
    public partial class Game : GameLoop
    {
        
        public uint WindowWidth = 1080;
        public uint WindowHeight = 720;
        public string WindowTitle = "Tutorial Game";

        public RayCaster rayCaster;
        
        public Player player;
        public Serial serial;
        public Map map;

        private int WindowState = 0;
        private bool Keyreset = true;

        public Game(uint windowWidth, uint windowHeight, string title) : base(windowWidth, windowHeight, title, 
            new Color(MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f)))
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            WindowTitle = title;
        }

        public override void Draw(GameTime gameTime)
        {
            MessegeManager.DrawPerformanceData(this, Color.White);

            
            switch (WindowState)
            {
                case 0:
                    rayCaster.Draw(this.Window, ref this.map, player);
                    break;
                case 1:
                    map.DrawMap(this.Window);
                    rayCaster.Draw(this.Window, ref this.map, player);
                    player.Draw(this.Window, map);
                    break;
            }

            MessegeManager.Message(this, serial.Info, Color.Red, 1);

            MessegeManager.DrawPerformanceData(this, Color.Red);
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            MessegeManager.LoadContent();
            map = new Map(Map.GenerateMapWithWall(100, 100), 20, new Maping.Window { Position = new Vector2i(50, 50), 
                Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100) });
            player = new Player(new Vector2f(20f, 20f));

            rayCaster = new RayCaster(fov:60, angleSpacingRay:0.5f, depthOffFeild:1000, windowPosition:new Vector2i(0, 0), windowSize:new Vector2i(1100, 800), 
                rayMapColor:Color.Red, horizontalColor: new Color(MathHelper.FloatToByte(0.5f), MathHelper.FloatToByte(0.5f), MathHelper.FloatToByte(0.1f)), 
                verticalColor: new Color(MathHelper.FloatToByte(0.7f), MathHelper.FloatToByte(0.4f), MathHelper.FloatToByte(0.1f)), drawMapRays: false);

            serial = new Serial("COM3", 9600);
            serial.StartReading();
        }
        

        public override void Update(GameTime gameTime)
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
            CheckButtons();
            map.CheckMapBorder(player, Window);
        }

        private void CheckButtons()
        {
            if (!Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                Keyreset = true;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                if (Keyreset)
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
                    Keyreset = false;
                }
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                player.Rotation -= 0.05f * GameTime.DeltaTime * 100;
                if (player.Rotation < 0)
                {
                    player.Rotation += 2f * (float)Math.PI;
                }
                player.DeltaPositionX = (float)(Math.Cos(player.Rotation));
                player.DeltaPositionY = (float)(Math.Sin(player.Rotation));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                player.Rotation += 0.05f * GameTime.DeltaTime * 75;
                if (player.Rotation > 2f * (float)Math.PI)
                {
                    player.Rotation -= 2f * (float)Math.PI;
                }
                player.DeltaPositionX = (float)(Math.Cos(player.Rotation));
                player.DeltaPositionY = (float)(Math.Sin(player.Rotation));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                player.PositionX += player.DeltaPosition.X * GameTime.DeltaTime * 150;
                player.PositionY += player.DeltaPosition.Y * GameTime.DeltaTime * 150;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                player.PositionX -= player.DeltaPosition.X * GameTime.DeltaTime * 150;
                player.PositionY -= player.DeltaPosition.Y * GameTime.DeltaTime * 150;
            }
        }
    }
}
