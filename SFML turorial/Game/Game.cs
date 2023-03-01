using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
using MicroControler.Shapes;
using System.Data;
using MicroControler.Mathematics;
using MicroControler.Game.Debug;
using MicroControler.Game.RayCasting;
using System.Diagnostics;
using System.IO.Ports;
using System.Net.Http.Headers;
using MicroControler.PortComunication;
using MicroControler.Game.Entity;
using MicroControler.GameLooping;
using System.Threading;
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

        public Game(uint windowWidth, uint windowHeight, string title) : base(windowWidth, windowHeight, title, 
            new Color(MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f), MathHelper.FloatToByte(0.3f)))
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            WindowTitle = title;
        }

        public override void Draw(GameTime gameTime)
        {
            DebugUtility.DrawPerformanceData(this, Color.White);

            map.DrawMap(this.Window);
            //rayCaster.Draw(this.Window, ref this.map, player);
            player.Draw(this.Window, map);

            map.CheckMapBorder(player, Window);
            map.DrawChunks(Window);

            DebugUtility.Message(this, serial.Info, Color.Red, 1);

            DebugUtility.DrawPerformanceData(this, Color.Red);
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            DebugUtility.LoadContent();
            map = new Map(Map.GenerateMapWithWallRandom(100, 100), 30, new Maping.Window { Position = new Vector2i(50, 50), Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100) });
            player = new Player(new Vector2f(20f, 20f));

            rayCaster = new RayCaster(fov:60, angleSpacingRay:0.5f, depthOffFeild:60, windowPosition:new Vector2i(650, 0), windowSize:new Vector2i(1100, 800), 
                rayMapColor:Color.Red, horizontalColor: new Color(MathHelper.FloatToByte(0.5f), MathHelper.FloatToByte(0.5f), MathHelper.FloatToByte(0.1f)), 
                verticalColor: new Color(MathHelper.FloatToByte(0.7f), MathHelper.FloatToByte(0.4f), MathHelper.FloatToByte(0.1f)), drawMapRays: true);

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
                //rayCaster.WindowSize = new Vector2i((int)WindowWidth, (int)WindowHeight);
                map.MapWindow.Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100);
                //map.SquareFillWindow();
                Window.SetView(view);
            }
            CheckButtons();
        }

        private void CheckButtons()
        {
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
