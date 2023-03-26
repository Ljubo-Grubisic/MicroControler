using MicroController.InputOutput;
using MicroController.MainLooping;
using SFML.Graphics;
using SFML.System;

namespace MicroController.Game
{
    public partial class Game
    {
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
    }
}
