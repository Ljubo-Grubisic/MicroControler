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
            map = new Map(Map.GenerateMapWithWall(1000, 1000), 10, new Maping.Window
            {
                Position = new Vector2i(50, 50),
                Size = new Vector2i((int)Window.Size.X - 100, (int)Window.Size.Y - 100)
            });

            rayCaster = new RayCaster(fov: 60, angleSpacingRay: 0.5f, depthOffFeild: 100, windowPosition: new Vector2i(0, 0),
                windowSize: new Vector2i((int)WindowWidth, (int)WindowHeight), rayMapColor: Color.Red, horizontalColor: new Color(150, 0, 0),
                verticalColor: new Color(255, 10, 10), drawMapRays: false);
            camera = new Camera(new Vector2f(100f, 100f));

            serial = new Serial("COM3", 9600);
            serial.StartReading();

            PauseMenu.Init(this);
        }
    }
}
