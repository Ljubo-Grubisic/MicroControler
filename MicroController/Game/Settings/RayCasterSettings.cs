using MicroController.GUI;
using MicroController.Shapes;
using MicroController.InputOutput;
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
        // Fov
        private static Text FovText;
        private static Slider FovSlider;
        private static TextBox FovTextBox;
        private static Rectangle FovRectangle;
        private static float Fov;

        // AngleSpacing
        private static Text AngleSpacingRayText;
        private static Slider AngleSpacingRaySlider;
        private static TextBox AngleSpacingRayTextBox;
        private static Rectangle AngleSpacingRayRectangle;
        private static float AngleSpacingRay;

        // DepthOffFeild
        private static Text DepthOffFeildText;
        private static Slider DepthOffFeildSlider;
        private static TextBox DepthOffFeildTextBox;
        private static Rectangle DepthOffFeildRectangle;
        private static int DepthOffFeild;

        // RayMapColor
        private static Text RayMapColorText;
        private static Slider RayMapColorSliderR;
        private static Slider RayMapColorSliderG;
        private static Slider RayMapColorSliderB;
        private static TextBox RayMapColorTextBoxR;
        private static TextBox RayMapColorTextBoxG;
        private static TextBox RayMapColorTextBoxB;
        private static Rectangle RayMapColorRectangle;
        private static Color RayMapColor;

        // HorizontalColor
        private static Text HorizontalColorText;
        private static Slider HorizontalColorSliderR;
        private static Slider HorizontalColorSliderG;
        private static Slider HorizontalColorSliderB;
        private static TextBox HorizontalColorTextBoxR;
        private static TextBox HorizontalColorTextBoxG;
        private static TextBox HorizontalColorTextBoxB;
        private static Rectangle HorizontalColorRectangle;
        private static Color HorizontalColor;

        // VerticalColor
        private static Text VerticalColorText;
        private static Slider VerticalColorSliderR;
        private static Slider VerticalColorSliderG;
        private static Slider VerticalColorSliderB;
        private static TextBox VerticalColorTextBoxR;
        private static TextBox VerticalColorTextBoxG;
        private static TextBox VerticalColorTextBoxB;
        private static Rectangle VerticalColorRectangle;
        private static Color VerticalColor;

        // R, G, B Text
        private static Text RColorText;
        private static Text GColorText;
        private static Text BColorText;

        private static Color RectangleFillColor = new Color(140, 191, 187);
        private static Color RectangleOutlineColor = new Color(69, 94, 92);

        private static void RayCasterSettingsInit(Game game)
        {
            // Fov
            Fov = game.rayCaster.Fov;
            FovText = new Text("Fov:" + Fov.ToString("000.000"), MessegeManager.Courier);
            FovSlider = new Slider(new Vector2f(), new Vector2f(), new Vector2f(25, 120), Fov, 10);
            FovTextBox = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            FovRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };

            // AngleSpacingRay
            AngleSpacingRay = game.rayCaster.AngleSpacingRay;
            AngleSpacingRayText = new Text("Angle Spacing:" + AngleSpacingRay.ToString("00.0000"), MessegeManager.Courier);
            AngleSpacingRaySlider = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0.05f, 5), AngleSpacingRay, 10);
            AngleSpacingRayTextBox = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            AngleSpacingRayRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };

            // DepthOffFeild
            DepthOffFeild = game.rayCaster.DepthOffFeild;
            DepthOffFeildText = new Text("Angle Spacing:" + DepthOffFeild.ToString("0000"), MessegeManager.Courier);
            DepthOffFeildSlider = new Slider(new Vector2f(), new Vector2f(), new Vector2f(1, 9999), DepthOffFeild, 10);
            DepthOffFeildTextBox = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            DepthOffFeildRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };

            // Events
            FovTextBox.TextBoxEnterPressed += FovTextBox_TextBoxEnterPressed;
            AngleSpacingRayTextBox.TextBoxEnterPressed += AngleSpacingTextBox_TextBoxEnterPressed;
            DepthOffFeildTextBox.TextBoxEnterPressed += DepthOffFeildTextBox_TextBoxEnterPressed;
        }


        private static void RayCasterSettingsUpdate(Game game)
        {
            // Universals
            uint characterSize = (uint)ButtonSize.Y / 4;
            float smallRectangleSizeX = game.Window.Size.X / 2.5f;
            float smallRectangleSizeY = game.Window.Size.Y / 9;

            // Fov
            Vector2f FovPosition = new Vector2f(4, ButtonSize.Y + 15);
            FovText.DisplayedString = "Fov:" + Fov.ToString("000.000");
            FovText.Position = FovPosition;
            FovText.CharacterSize = characterSize;

            FovTextBox.PositionX = FovText.GetGlobalBounds().Left + MessegeManager.GetTextRect("Fov: 000.000", MessegeManager.Courier, characterSize).Width + 10;
            FovTextBox.PositionY = FovPosition.Y;
            FovTextBox.SizeX = smallRectangleSizeX / 3;
            FovTextBox.SizeY = smallRectangleSizeY;
            FovTextBox.Text.CharacterSize = characterSize;
            FovTextBox.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            FovSlider.PositionX = FovTextBox.Position.X + FovTextBox.Size.X + 10;
            FovSlider.PositionY = FovPosition.Y + smallRectangleSizeY / 2 - FovSlider.Size.Y / 2;
            FovSlider.Size.X = smallRectangleSizeX - FovSlider.PositionX;
            FovSlider.Size.Y = 5f;
            FovSlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            Fov = FovSlider.Value;

            FovRectangle.Position = FovPosition;
            FovRectangle.SizeX = smallRectangleSizeX;
            FovRectangle.SizeY = smallRectangleSizeY;
            
            // AngleSpacing
            Vector2f AngleSpacingPosition = new Vector2f(4, ButtonSize.Y + 2 * 15 + smallRectangleSizeY);
            AngleSpacingRayText.DisplayedString = "Angle Spacing:" + AngleSpacingRay.ToString("00.0000");
            AngleSpacingRayText.Position = AngleSpacingPosition;
            AngleSpacingRayText.CharacterSize = characterSize;

            AngleSpacingRayTextBox.PositionX = AngleSpacingRayText.GetGlobalBounds().Left + MessegeManager.GetTextRect("Angle Spacing: 00.0000", MessegeManager.Courier, characterSize).Width + 10;
            AngleSpacingRayTextBox.PositionY = AngleSpacingPosition.Y;
            AngleSpacingRayTextBox.SizeX = smallRectangleSizeX / 3;
            AngleSpacingRayTextBox.SizeY = smallRectangleSizeY;
            AngleSpacingRayTextBox.Text.CharacterSize = characterSize;
            AngleSpacingRayTextBox.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            AngleSpacingRaySlider.PositionX = AngleSpacingRayTextBox.Position.X + AngleSpacingRayTextBox.Size.X + 10;
            AngleSpacingRaySlider.PositionY = AngleSpacingPosition.Y + smallRectangleSizeY / 2 - AngleSpacingRaySlider.Size.Y / 2;
            AngleSpacingRaySlider.Size.X = smallRectangleSizeX - AngleSpacingRaySlider.PositionX;
            AngleSpacingRaySlider.Size.Y = 5f;
            AngleSpacingRaySlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            AngleSpacingRay = AngleSpacingRaySlider.Value;

            AngleSpacingRayRectangle.Position = AngleSpacingPosition;
            AngleSpacingRayRectangle.SizeX = smallRectangleSizeX;
            AngleSpacingRayRectangle.SizeY = smallRectangleSizeY;

            // DepthOffFeild
            Vector2f DepthOffFeildPosition = new Vector2f(4, ButtonSize.Y * 2 + 3 * 15 + smallRectangleSizeY);
            DepthOffFeildText.DisplayedString = "Depth Off Feild:" + DepthOffFeild.ToString("0000");
            DepthOffFeildText.Position = DepthOffFeildPosition;
            DepthOffFeildText.CharacterSize = characterSize;

            DepthOffFeildTextBox.PositionX = DepthOffFeildText.GetGlobalBounds().Left + MessegeManager.GetTextRect("Depth Off Feild: 0000", MessegeManager.Courier, characterSize).Width + 10;
            DepthOffFeildTextBox.PositionY = DepthOffFeildPosition.Y;
            DepthOffFeildTextBox.SizeX = smallRectangleSizeX / 3;
            DepthOffFeildTextBox.SizeY = smallRectangleSizeY;
            DepthOffFeildTextBox.Text.CharacterSize = characterSize;
            DepthOffFeildTextBox.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            DepthOffFeildSlider.PositionX = DepthOffFeildTextBox.Position.X + DepthOffFeildTextBox.Size.X + 10;
            DepthOffFeildSlider.PositionY = DepthOffFeildPosition.Y + smallRectangleSizeY / 2 - DepthOffFeildSlider.Size.Y / 2;
            DepthOffFeildSlider.Size.X = smallRectangleSizeX - DepthOffFeildSlider.PositionX;
            DepthOffFeildSlider.Size.Y = 5f;
            DepthOffFeildSlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            DepthOffFeild = (int)DepthOffFeildSlider.Value;

            DepthOffFeildRectangle.Position = DepthOffFeildPosition;
            DepthOffFeildRectangle.SizeX = smallRectangleSizeX;
            DepthOffFeildRectangle.SizeY = smallRectangleSizeY;
        }

        private static void RayCasterSettingsDraw(RenderWindow window)
        {
            FovRectangle.Draw(window);
            window.Draw(FovText);
            FovTextBox.Draw(window);
            FovSlider.Draw(window);

            AngleSpacingRayRectangle.Draw(window);
            window.Draw(AngleSpacingRayText);
            AngleSpacingRayTextBox.Draw(window);
            AngleSpacingRaySlider.Draw(window);

            DepthOffFeildRectangle.Draw(window);
            window.Draw(DepthOffFeildText);
            DepthOffFeildTextBox.Draw(window);
            DepthOffFeildSlider.Draw(window);
        }

        private static void FovTextBox_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Fov = Convert.ToSingle(FovTextBox.TextString);
                FovSlider.SetCircleToValue(Fov);
            }
            catch { }
        }
        private static void AngleSpacingTextBox_TextBoxEnterPressed(Object source, EventArgs args)
        {
            try
            {
                AngleSpacingRay = Convert.ToSingle(AngleSpacingRayTextBox.TextString);
                AngleSpacingRaySlider.SetCircleToValue(AngleSpacingRay);
            }
            catch { }
        }

        private static void DepthOffFeildTextBox_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                DepthOffFeild = Convert.ToInt32(DepthOffFeildTextBox.TextString);
                DepthOffFeildSlider.SetCircleToValue(DepthOffFeild);
            }
            catch { }
        }
    }
}
