using MicroController.Graphics;
using MicroController.Graphics;
using MicroController.Helpers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SFML.Window;

namespace MicroController.Game
{
    public static partial class Settings
    {
        // Scale
        private static Text ScaleText;
        private static Slider ScaleSlider;
        private static TextBox ScaleTextBox;
        private static Rectangle ScaleRectangle;
        private static float Scale;

        // Speed
        private static Text SpeedText;
        private static Slider SpeedSlider;
        private static TextBox SpeedTextBox;
        private static Rectangle SpeedRectangle;
        private static float Speed;

        private static bool CameraWindowResized = true;
        private static int CameraCounter = 0;

        private static void CameraSettingsInit(Game game)
        {
            // Scale
            Scale = game.camera.Scale;
            ScaleText = new Text("Scale:" + Scale.ToString("000.0000"), MessegeManager.Courier);
            ScaleSlider = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0.01f, 2), Scale, 10);
            ScaleTextBox = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            ScaleRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };

            // Speed
            Speed = game.camera.Speed;
            SpeedText = new Text("Speed:" + Speed.ToString("000.0000"), MessegeManager.Courier);
            SpeedSlider = new Slider(new Vector2f(), new Vector2f(), new Vector2f(1, 1000), Speed, 10);
            SpeedTextBox = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            SpeedRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };

            // Events
            ScaleTextBox.TextBoxEnterPressed += ScaleTextBox_TextBoxEnterPressed;
            SpeedTextBox.TextBoxEnterPressed += SpeedTextBox_TextBoxEnterPressed;
        }

        private static void CameraSettingsUpdate(Game game)
        {
            // Universals
            uint characterSize = (uint)ButtonSize.Y / 4;
            float smallRectangleSizeX = game.Window.Size.X / 2.07f;
            float smallRectangleSizeY = game.Window.Size.Y / 9;

            // Scale
            Vector2f ScalePosition = new Vector2f(4, ButtonSize.Y + 15);
            ScaleText.DisplayedString = "Scale:" + Scale.ToString("000.0000");
            ScaleText.Position = ScalePosition;
            ScaleText.CharacterSize = characterSize;

            ScaleTextBox.PositionX = ScaleText.GetGlobalBounds().Left + MessegeManager.GetTextRect("Scale: 000.0000", MessegeManager.Courier, characterSize).Width + 10;
            ScaleTextBox.PositionY = ScalePosition.Y;
            ScaleTextBox.SizeX = smallRectangleSizeX / 3;
            ScaleTextBox.SizeY = smallRectangleSizeY;
            ScaleTextBox.Text.CharacterSize = characterSize;
            ScaleTextBox.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            ScaleSlider.PositionX = ScaleTextBox.Position.X + ScaleTextBox.Size.X + 10;
            ScaleSlider.PositionY = ScalePosition.Y + smallRectangleSizeY / 2 - ScaleSlider.Size.Y / 2;
            ScaleSlider.Size.X = smallRectangleSizeX - ScaleSlider.PositionX;
            ScaleSlider.Size.Y = 5f;
            if (CameraWindowResized)
                ScaleSlider.SetCircleToValue(Scale);
            ScaleSlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!CameraWindowResized)
                Scale = ScaleSlider.Value;

            ScaleRectangle.Position = ScalePosition;
            ScaleRectangle.SizeX = smallRectangleSizeX;
            ScaleRectangle.SizeY = smallRectangleSizeY;

            // Speed
            Vector2f SpeedPosition = new Vector2f(4, ButtonSize.Y + 15 * 2 + smallRectangleSizeY );
            SpeedText.DisplayedString = "Speed:" + Speed.ToString("000.0000");
            SpeedText.Position = SpeedPosition;
            SpeedText.CharacterSize = characterSize;

            SpeedTextBox.PositionX = SpeedText.GetGlobalBounds().Left + MessegeManager.GetTextRect("Speed: 000.0000", MessegeManager.Courier, characterSize).Width + 10;
            SpeedTextBox.PositionY = SpeedPosition.Y;
            SpeedTextBox.SizeX = smallRectangleSizeX / 3;
            SpeedTextBox.SizeY = smallRectangleSizeY;
            SpeedTextBox.Text.CharacterSize = characterSize;
            SpeedTextBox.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            SpeedSlider.PositionX = SpeedTextBox.Position.X + SpeedTextBox.Size.X + 10;
            SpeedSlider.PositionY = SpeedPosition.Y + smallRectangleSizeY / 2 - SpeedSlider.Size.Y / 2;
            SpeedSlider.Size.X = smallRectangleSizeX - SpeedSlider.PositionX;
            SpeedSlider.Size.Y = 5f;
            if (CameraWindowResized)
                SpeedSlider.SetCircleToValue(Speed);
            SpeedSlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!CameraWindowResized)
                Speed = SpeedSlider.Value;

            SpeedRectangle.Position = SpeedPosition;
            SpeedRectangle.SizeX = smallRectangleSizeX;
            SpeedRectangle.SizeY = smallRectangleSizeY;

            if (CameraWindowResized)
            {
                CameraCounter++;
            }
            if (CameraCounter == 1)
            {
                CameraWindowResized = false;
                CameraCounter = 0;
            }
        }
        private static void CameraSettingsDraw(RenderWindow window) 
        {
            ScaleRectangle.Draw(window);
            window.Draw(ScaleText);
            ScaleTextBox.Draw(window);
            ScaleSlider.Draw(window);

            SpeedRectangle.Draw(window);
            window.Draw(SpeedText);
            SpeedTextBox.Draw(window);
            SpeedSlider.Draw(window);
        }

        private static void ScaleTextBox_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Scale = Convert.ToSingle(ScaleTextBox.DisplayedString);
                ScaleSlider.SetCircleToValue(Scale);
            }
            catch { }
        }
        private static void SpeedTextBox_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Speed = Convert.ToSingle(SpeedTextBox.DisplayedString);
                SpeedSlider.SetCircleToValue(Speed);
            }
            catch { }
        }
    }
}
