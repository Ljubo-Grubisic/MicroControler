using MicroController.InputOutput;
using MicroController.MainLooping;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroController.Game
{
    public partial class Game
    {
        protected override void Update(GameTime gameTime)
        {
            ResizeWindow();
            camera.Update(gameTime, map);
            map.Update(camera, GameTime);
            OpenCloseMap();
            button.Update(Mouse.GetPosition() - Window.Position - new Vector2i(8, 30), Mouse.IsButtonPressed(Mouse.Button.Left));
            button2.Update(Mouse.GetPosition() - Window.Position - new Vector2i(8, 30), Mouse.IsButtonPressed(Mouse.Button.Left));
            slider.Update(Mouse.GetPosition() - Window.Position - new Vector2i(8, 30), Mouse.IsButtonPressed(Mouse.Button.Left));
            slider2.Update(Mouse.GetPosition() - Window.Position - new Vector2i(8, 30), Mouse.IsButtonPressed(Mouse.Button.Left));

            textBox.Update(Mouse.GetPosition() - Window.Position - new Vector2i(8, 30));
            textBox2.Update(Mouse.GetPosition() - Window.Position - new Vector2i(8, 30));
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

            rayCaster.Fov = slider.Value;
            slider.Draw(Window);
            map.SquareSize = (int)slider2.Value;
            slider2.Draw(Window);

            button.Draw(Window);

            textBox.Draw(Window);
            textBox2.Draw(Window);

            //MessegeManager.Message(this, serial.Info, Color.Red, 1);

            MessegeManager.DrawPerformanceData(this, Color.Red);
        }
    }
}
