using MicroController.Game.Entities;
using MicroController.Game.Maping;
using MicroController.Game.RayCasting;
using MicroController.GUI;
using MicroController.InputOutput;
using MicroController.InputOutput.PortComunication;
using SFML.Graphics;
using SFML.System;
using System;

namespace MicroController.Game
{
    public partial class Game
    {
        protected override void LoadContent()
        {
            MessegeManager.LoadContent();
        }

        protected override void Initialize()
        {
            map = new Map(Map.GenerateMapWithWallRandom(1000, 1000), 10, new Maping.Window
            {
                Position = new Vector2i(50, 50),
                Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100)
            });

            rayCaster = new RayCaster(fov: 60, angleSpacingRay: 0.5f, depthOffFeild: 100, windowPosition: new Vector2i(0, 0),
                windowSize: new Vector2i((int)WindowWidth, (int)WindowHeight), rayMapColor: Color.Red, horizontalColor: new Color(150, 0, 0),
                verticalColor: new Color(255, 10, 10), drawMapRays: false);
            camera = new Camera(new Vector2f(100f, 100f));

            button = new Button(new Vector2f(500, 410), new Vector2f(150, 50), "TextLargeButton", MessegeManager.Arial, 16);

            button2 = new Button(new Vector2f(100, 100), new Vector2f(450, 300), "BUtton");
            slider = new Slider(new Vector2f(100, 100), new Vector2f(100, 7.5f), new Vector2f(1, 180), 90, 10);
            slider2 = new Slider(new Vector2f(300, 100), new Vector2f(350, 7.5f), new Vector2f(1, 100), 10, 10);

            textBox = new TextBox(new Vector2f(100, 450), new Vector2f(100, 50), 16, MessegeManager.Arial);
            textBox2 = new TextBox(new Vector2f(450, 100), new Vector2f(300, 70), 16, MessegeManager.Arial);

            serial = new Serial("COM3", 9600);
            serial.StartReading();
        }
    }
}
