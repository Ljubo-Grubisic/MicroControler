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
        private static Rectangle RayMapColorSampleRectangle;
        private static Color RayMapColor;
        // R, G, B Text
        private static Text RayMapColorRText;
        private static Text RayMapColorGText;
        private static Text RayMapColorBText;

        // HorizontalColor
        private static Text HorizontalColorText;
        private static Slider HorizontalColorSliderR;
        private static Slider HorizontalColorSliderG;
        private static Slider HorizontalColorSliderB;
        private static TextBox HorizontalColorTextBoxR;
        private static TextBox HorizontalColorTextBoxG;
        private static TextBox HorizontalColorTextBoxB;
        private static Rectangle HorizontalColorRectangle;
        private static Rectangle HorizontalColorSampleRectangle;
        private static Color HorizontalColor;
        // R, G, B Text
        private static Text HorizontalColorRText;
        private static Text HorizontalColorGText;
        private static Text HorizontalColorBText;

        // VerticalColor
        private static Text VerticalColorText;
        private static Slider VerticalColorSliderR;
        private static Slider VerticalColorSliderG;
        private static Slider VerticalColorSliderB;
        private static TextBox VerticalColorTextBoxR;
        private static TextBox VerticalColorTextBoxG;
        private static TextBox VerticalColorTextBoxB;
        private static Rectangle VerticalColorSampleRectangle;
        private static Rectangle VerticalColorRectangle;
        private static Color VerticalColor;
        // R, G, B Text
        private static Text VerticalColorRText;
        private static Text VerticalColorGText;
        private static Text VerticalColorBText;

        private static bool RayCasterWindowResized = true;
        private static int RayCasterCounter = 0;

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
            AngleSpacingRayText = new Text("Angle Spacing:" + AngleSpacingRay.ToString("0.0000"), MessegeManager.Courier);
            AngleSpacingRaySlider = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0.05f, 5), AngleSpacingRay, 10);
            AngleSpacingRayTextBox = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            AngleSpacingRayRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };

            // DepthOffFeild
            DepthOffFeild = game.rayCaster.DepthOffFeild;
            DepthOffFeildText = new Text("Depth Off Feild:" + DepthOffFeild.ToString("0000"), MessegeManager.Courier);
            DepthOffFeildSlider = new Slider(new Vector2f(), new Vector2f(), new Vector2f(10, 9999), DepthOffFeild, 10);
            DepthOffFeildTextBox = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            DepthOffFeildRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };

            // RayMapColor
            RayMapColor = game.rayCaster.RayMapColor;
            RayMapColorText = new Text("Map rays Color:", MessegeManager.Courier);
            RayMapColorSliderR = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), RayMapColor.R, 10);
            RayMapColorSliderG = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), RayMapColor.G, 10);
            RayMapColorSliderB = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), RayMapColor.B, 10);
            RayMapColorTextBoxR = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            RayMapColorTextBoxG = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            RayMapColorTextBoxB = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            RayMapColorSampleRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 1f, OutlineColor = Color.Black, FillColor = RayMapColor };
            RayMapColorRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };
            // R, G, B Text
            RayMapColorRText = new Text("R: 000", MessegeManager.Courier);
            RayMapColorGText = new Text("G: 000", MessegeManager.Courier);
            RayMapColorBText = new Text("B: 000", MessegeManager.Courier);

            // HorizontalColor
            HorizontalColor = game.rayCaster.HorizontalColor;
            HorizontalColorText = new Text("Horiontal walls Color:", MessegeManager.Courier);
            HorizontalColorSliderR = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), HorizontalColor.R, 10);
            HorizontalColorSliderG = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), HorizontalColor.G, 10);
            HorizontalColorSliderB = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), HorizontalColor.B, 10);
            HorizontalColorTextBoxR = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            HorizontalColorTextBoxG = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            HorizontalColorTextBoxB = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            HorizontalColorSampleRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 1f, OutlineColor = Color.Black, FillColor = HorizontalColor };
            HorizontalColorRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };
            // R, G, B Text
            HorizontalColorRText = new Text("R: 000", MessegeManager.Courier);
            HorizontalColorGText = new Text("G: 000", MessegeManager.Courier);
            HorizontalColorBText = new Text("B: 000", MessegeManager.Courier);

            // VerticalColor
            VerticalColor = game.rayCaster.VerticalColor;
            VerticalColorText = new Text("Vertical walls Color:", MessegeManager.Courier);
            VerticalColorSliderR = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), VerticalColor.R, 10);
            VerticalColorSliderG = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), VerticalColor.G, 10);
            VerticalColorSliderB = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), VerticalColor.B, 10);
            VerticalColorTextBoxR = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            VerticalColorTextBoxG = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            VerticalColorTextBoxB = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            VerticalColorSampleRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 1f, OutlineColor = Color.Black, FillColor = VerticalColor };
            VerticalColorRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };
            // R, G, B Text
            VerticalColorRText = new Text("R: 000", MessegeManager.Courier);
            VerticalColorGText = new Text("G: 000", MessegeManager.Courier);
            VerticalColorBText = new Text("B: 000", MessegeManager.Courier);

            // Events
            FovTextBox.TextBoxEnterPressed += FovTextBox_TextBoxEnterPressed;
            AngleSpacingRayTextBox.TextBoxEnterPressed += AngleSpacingTextBox_TextBoxEnterPressed;
            DepthOffFeildTextBox.TextBoxEnterPressed += DepthOffFeildTextBox_TextBoxEnterPressed;

            RayMapColorTextBoxR.TextBoxEnterPressed += RayMapColorTextBoxR_TextBoxEnterPressed;
            RayMapColorTextBoxG.TextBoxEnterPressed += RayMapColorTextBoxG_TextBoxEnterPressed;
            RayMapColorTextBoxB.TextBoxEnterPressed += RayMapColorTextBoxB_TextBoxEnterPressed;

            HorizontalColorTextBoxR.TextBoxEnterPressed += HorizontalColorTextBoxR_TextBoxEnterPressed;
            HorizontalColorTextBoxG.TextBoxEnterPressed += HorizontalColorTextBoxG_TextBoxEnterPressed;
            HorizontalColorTextBoxB.TextBoxEnterPressed += HorizontalColorTextBoxB_TextBoxEnterPressed;

            VerticalColorTextBoxR.TextBoxEnterPressed += VerticalColorTextBoxR_TextBoxEnterPressed;
            VerticalColorTextBoxG.TextBoxEnterPressed += VerticalColorTextBoxG_TextBoxEnterPressed;
            VerticalColorTextBoxB.TextBoxEnterPressed += VerticalColorTextBoxB_TextBoxEnterPressed;
        }

        private static void RayCasterSettingsUpdate(Game game)
        {
            // Universals
            uint characterSize = (uint)ButtonSize.Y / 4;
            float smallRectangleSizeX = game.Window.Size.X / 2.07f;
            float smallRectangleSizeY = game.Window.Size.Y / 9;

            float largeRectangleSizeX = smallRectangleSizeX;
            float largeRectangleSizeY = smallRectangleSizeY * 2.2f;

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
            if(RayCasterWindowResized)
                FovSlider.SetCircleToValue(Fov);
            FovSlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                Fov = (float)Math.Round(FovSlider.Value);

            FovRectangle.Position = FovPosition;
            FovRectangle.SizeX = smallRectangleSizeX;
            FovRectangle.SizeY = smallRectangleSizeY;
            
            // AngleSpacing
            Vector2f AngleSpacingPosition = new Vector2f(4, ButtonSize.Y + 2 * 15 + smallRectangleSizeY);
            AngleSpacingRayText.DisplayedString = "Angle Spacing:" + AngleSpacingRay.ToString("0.0000");
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
            if (RayCasterWindowResized)
                AngleSpacingRaySlider.SetCircleToValue(AngleSpacingRay);
            AngleSpacingRaySlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
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
            if (RayCasterWindowResized)
                DepthOffFeildSlider.SetCircleToValue(DepthOffFeild);
            DepthOffFeildSlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                DepthOffFeild = (int)Math.Round(DepthOffFeildSlider.Value);

            DepthOffFeildRectangle.Position = DepthOffFeildPosition;
            DepthOffFeildRectangle.SizeX = smallRectangleSizeX;
            DepthOffFeildRectangle.SizeY = smallRectangleSizeY;

            // RayMapColor
            Vector2f RayMapColorPosition = new Vector2f(4, ButtonSize.Y * 3 + 15 * 4 + smallRectangleSizeY);
            RayMapColorText.Position = RayMapColorPosition;
            RayMapColorText.CharacterSize = characterSize;

            RayMapColorSampleRectangle.PositionX = RayMapColorPosition.X + RayMapColorText.GetLocalBounds().Width + 10;
            RayMapColorSampleRectangle.PositionY = RayMapColorPosition.Y;
            RayMapColorSampleRectangle.SizeX = largeRectangleSizeX - RayMapColorText.GetLocalBounds().Width - 20;
            RayMapColorSampleRectangle.SizeY = RayMapColorText.GetLocalBounds().Height + 6;
            RayMapColorSampleRectangle.FillColor = RayMapColor;

            // Red
            RayMapColorRText.DisplayedString = "R: " + RayMapColor.R.ToString("000");
            RayMapColorRText.Position = new Vector2f(RayMapColorPosition.X, RayMapColorPosition.Y + RayMapColorText.GetLocalBounds().Height + 6);
            RayMapColorRText.CharacterSize = characterSize;

            RayMapColorTextBoxR.PositionX = RayMapColorPosition.X + MessegeManager.GetTextRect("R: 000", MessegeManager.Courier, characterSize).Width + 10;
            RayMapColorTextBoxR.PositionY = RayMapColorPosition.Y + RayMapColorText.GetLocalBounds().Height + RayMapColorText.GetLocalBounds().Top + 5;
            RayMapColorTextBoxR.SizeX = smallRectangleSizeX / 3;
            RayMapColorTextBoxR.SizeY = smallRectangleSizeY / 2;
            RayMapColorTextBoxR.Text.CharacterSize = characterSize;
            RayMapColorTextBoxR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            RayMapColorSliderR.PositionX = RayMapColorTextBoxR.PositionX + RayMapColorTextBoxR.SizeX + 10;
            RayMapColorSliderR.PositionY = RayMapColorTextBoxR.PositionY + RayMapColorTextBoxR.SizeY / 2;
            RayMapColorSliderR.Size.X = largeRectangleSizeX - RayMapColorSliderB.PositionX + RayMapColorPosition.X - 10;
            RayMapColorSliderR.Size.Y = 5f;
            if (RayCasterWindowResized)
                RayMapColorSliderR.SetCircleToValue(RayMapColor.R);
            RayMapColorSliderR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                RayMapColor.R = (byte)Math.Round(RayMapColorSliderR.Value);

            // Green
            RayMapColorGText.DisplayedString = "G: " + RayMapColor.G.ToString("000");
            RayMapColorGText.Position = new Vector2f(RayMapColorPosition.X, RayMapColorTextBoxR.PositionY + RayMapColorTextBoxR.SizeY);
            RayMapColorGText.CharacterSize = characterSize;

            RayMapColorTextBoxG.PositionX = RayMapColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            RayMapColorTextBoxG.PositionY = RayMapColorTextBoxR.Position.Y + RayMapColorTextBoxR.Size.Y + 5;
            RayMapColorTextBoxG.SizeX = smallRectangleSizeX / 3;
            RayMapColorTextBoxG.SizeY = smallRectangleSizeY / 2;
            RayMapColorTextBoxG.Text.CharacterSize = characterSize;
            RayMapColorTextBoxG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            RayMapColorSliderG.PositionX = RayMapColorTextBoxG.PositionX + RayMapColorTextBoxG.SizeX + 10;
            RayMapColorSliderG.PositionY = RayMapColorTextBoxG.PositionY + RayMapColorTextBoxG.SizeY / 2;
            RayMapColorSliderG.Size.X = largeRectangleSizeX - RayMapColorSliderB.PositionX + RayMapColorPosition.X - 10;
            RayMapColorSliderG.Size.Y = 5f;
            if (RayCasterWindowResized)
                RayMapColorSliderG.SetCircleToValue(RayMapColor.G);
            RayMapColorSliderG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                RayMapColor.G = (byte)Math.Round(RayMapColorSliderG.Value);

            // Blue
            RayMapColorBText.DisplayedString = "B: " + RayMapColor.B.ToString("000");
            RayMapColorBText.Position = new Vector2f(RayMapColorPosition.X, RayMapColorTextBoxG.PositionY + RayMapColorTextBoxG.SizeY);
            RayMapColorBText.CharacterSize = characterSize;

            RayMapColorTextBoxB.PositionX = RayMapColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            RayMapColorTextBoxB.PositionY = RayMapColorTextBoxG.Position.Y + RayMapColorTextBoxG.Size.Y + 5;
            RayMapColorTextBoxB.SizeX = smallRectangleSizeX / 3;
            RayMapColorTextBoxB.SizeY = smallRectangleSizeY / 2;
            RayMapColorTextBoxB.Text.CharacterSize = characterSize;
            RayMapColorTextBoxB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            RayMapColorSliderB.PositionX = RayMapColorTextBoxB.PositionX + RayMapColorTextBoxB.SizeX + 10;
            RayMapColorSliderB.PositionY = RayMapColorTextBoxB.PositionY + RayMapColorTextBoxB.SizeY / 2;
            RayMapColorSliderB.Size.X = largeRectangleSizeX - RayMapColorSliderB.PositionX + RayMapColorPosition.X - 10;
            RayMapColorSliderB.Size.Y = 5f;
            if (RayCasterWindowResized)
                RayMapColorSliderB.SetCircleToValue(RayMapColor.B);
            RayMapColorSliderB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                RayMapColor.B = (byte)Math.Round(RayMapColorSliderB.Value);

            RayMapColorRectangle.Position = RayMapColorPosition;
            RayMapColorRectangle.SizeX = largeRectangleSizeX;
            RayMapColorRectangle.SizeY = largeRectangleSizeY;


            // HorizontalColor
            Vector2f HorizontalColorPosition = new Vector2f(20 + smallRectangleSizeX, ButtonSize.Y + 15);
            HorizontalColorText.Position = HorizontalColorPosition;
            HorizontalColorText.CharacterSize = characterSize;

            HorizontalColorSampleRectangle.PositionX = HorizontalColorPosition.X + HorizontalColorText.GetLocalBounds().Width + 10;
            HorizontalColorSampleRectangle.PositionY = HorizontalColorPosition.Y;
            HorizontalColorSampleRectangle.SizeX = largeRectangleSizeX - HorizontalColorText.GetLocalBounds().Width - 20;
            HorizontalColorSampleRectangle.SizeY = HorizontalColorText.GetLocalBounds().Height + 6;
            HorizontalColorSampleRectangle.FillColor = HorizontalColor;

            // Red
            HorizontalColorRText.DisplayedString = "R: " + HorizontalColor.R.ToString("000");
            HorizontalColorRText.Position = new Vector2f(HorizontalColorPosition.X, HorizontalColorPosition.Y + HorizontalColorText.GetLocalBounds().Height + 6);
            HorizontalColorRText.CharacterSize = characterSize;

            HorizontalColorTextBoxR.PositionX = HorizontalColorPosition.X + MessegeManager.GetTextRect("R: 000", MessegeManager.Courier, characterSize).Width + 10;
            HorizontalColorTextBoxR.PositionY = HorizontalColorPosition.Y + HorizontalColorText.GetLocalBounds().Height + HorizontalColorText.GetLocalBounds().Top + 5;
            HorizontalColorTextBoxR.SizeX = smallRectangleSizeX / 3;
            HorizontalColorTextBoxR.SizeY = smallRectangleSizeY / 2;
            HorizontalColorTextBoxR.Text.CharacterSize = characterSize;
            HorizontalColorTextBoxR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            HorizontalColorSliderR.PositionX = HorizontalColorTextBoxR.PositionX + HorizontalColorTextBoxR.SizeX + 10;
            HorizontalColorSliderR.PositionY = HorizontalColorTextBoxR.PositionY + HorizontalColorTextBoxR.SizeY / 2;
            HorizontalColorSliderR.Size.X = largeRectangleSizeX - HorizontalColorSliderB.PositionX + HorizontalColorPosition.X - 10;
            HorizontalColorSliderR.Size.Y = 5f;
            if (RayCasterWindowResized)
                HorizontalColorSliderR.SetCircleToValue(HorizontalColor.R);
            HorizontalColorSliderR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                HorizontalColor.R = (byte)Math.Round(HorizontalColorSliderR.Value);

            // Green
            HorizontalColorGText.DisplayedString = "G: " + HorizontalColor.G.ToString("000");
            HorizontalColorGText.Position = new Vector2f(HorizontalColorPosition.X, HorizontalColorTextBoxR.PositionY + HorizontalColorTextBoxR.SizeY);
            HorizontalColorGText.CharacterSize = characterSize;

            HorizontalColorTextBoxG.PositionX = HorizontalColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            HorizontalColorTextBoxG.PositionY = HorizontalColorTextBoxR.Position.Y + HorizontalColorTextBoxR.Size.Y + 5;
            HorizontalColorTextBoxG.SizeX = smallRectangleSizeX / 3;
            HorizontalColorTextBoxG.SizeY = smallRectangleSizeY / 2;
            HorizontalColorTextBoxG.Text.CharacterSize = characterSize;
            HorizontalColorTextBoxG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            HorizontalColorSliderG.PositionX = HorizontalColorTextBoxG.PositionX + HorizontalColorTextBoxG.SizeX + 10;
            HorizontalColorSliderG.PositionY = HorizontalColorTextBoxG.PositionY + HorizontalColorTextBoxG.SizeY / 2;
            HorizontalColorSliderG.Size.X = largeRectangleSizeX - HorizontalColorSliderB.PositionX + HorizontalColorPosition.X - 10;
            HorizontalColorSliderG.Size.Y = 5f;
            if (RayCasterWindowResized)
                HorizontalColorSliderG.SetCircleToValue(HorizontalColor.G);
            HorizontalColorSliderG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                HorizontalColor.G = (byte)Math.Round(HorizontalColorSliderG.Value);

            // Blue
            HorizontalColorBText.DisplayedString = "B: " + HorizontalColor.B.ToString("000");
            HorizontalColorBText.Position = new Vector2f(HorizontalColorPosition.X, HorizontalColorTextBoxG.PositionY + HorizontalColorTextBoxG.SizeY);
            HorizontalColorBText.CharacterSize = characterSize;

            HorizontalColorTextBoxB.PositionX = HorizontalColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            HorizontalColorTextBoxB.PositionY = HorizontalColorTextBoxG.Position.Y + HorizontalColorTextBoxG.Size.Y + 5;
            HorizontalColorTextBoxB.SizeX = smallRectangleSizeX / 3;
            HorizontalColorTextBoxB.SizeY = smallRectangleSizeY / 2;
            HorizontalColorTextBoxB.Text.CharacterSize = characterSize;
            HorizontalColorTextBoxB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            HorizontalColorSliderB.PositionX = HorizontalColorTextBoxB.PositionX + HorizontalColorTextBoxB.SizeX + 10;
            HorizontalColorSliderB.PositionY = HorizontalColorTextBoxB.PositionY + HorizontalColorTextBoxB.SizeY / 2;
            HorizontalColorSliderB.Size.X = largeRectangleSizeX - HorizontalColorSliderB.PositionX + HorizontalColorPosition.X - 10;
            HorizontalColorSliderB.Size.Y = 5f;
            if (RayCasterWindowResized)
                HorizontalColorSliderB.SetCircleToValue(HorizontalColor.B);
            HorizontalColorSliderB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                HorizontalColor.B = (byte)Math.Round(HorizontalColorSliderB.Value);

            HorizontalColorRectangle.Position = HorizontalColorPosition;
            HorizontalColorRectangle.SizeX = largeRectangleSizeX;
            HorizontalColorRectangle.SizeY = largeRectangleSizeY;


            // VerticalColor
            Vector2f VerticalColorPosition = new Vector2f(20 + smallRectangleSizeX, ButtonSize.Y + 30 + largeRectangleSizeY);
            VerticalColorText.Position = VerticalColorPosition;
            VerticalColorText.CharacterSize = characterSize;

            VerticalColorSampleRectangle.PositionX = VerticalColorPosition.X + VerticalColorText.GetLocalBounds().Width + 10;
            VerticalColorSampleRectangle.PositionY = VerticalColorPosition.Y;
            VerticalColorSampleRectangle.SizeX = largeRectangleSizeX - VerticalColorText.GetLocalBounds().Width - 20;
            VerticalColorSampleRectangle.SizeY = VerticalColorText.GetLocalBounds().Height + 6;
            VerticalColorSampleRectangle.FillColor = VerticalColor;

            // Red
            VerticalColorRText.DisplayedString = "R: " + VerticalColor.R.ToString("000");
            VerticalColorRText.Position = new Vector2f(VerticalColorPosition.X, VerticalColorPosition.Y + VerticalColorText.GetLocalBounds().Height + 6);
            VerticalColorRText.CharacterSize = characterSize;

            VerticalColorTextBoxR.PositionX = VerticalColorPosition.X + MessegeManager.GetTextRect("R: 000", MessegeManager.Courier, characterSize).Width + 10;
            VerticalColorTextBoxR.PositionY = VerticalColorPosition.Y + VerticalColorText.GetLocalBounds().Height + VerticalColorText.GetLocalBounds().Top + 5;
            VerticalColorTextBoxR.SizeX = smallRectangleSizeX / 3;
            VerticalColorTextBoxR.SizeY = smallRectangleSizeY / 2;
            VerticalColorTextBoxR.Text.CharacterSize = characterSize;
            VerticalColorTextBoxR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            VerticalColorSliderR.PositionX = VerticalColorTextBoxR.PositionX + VerticalColorTextBoxR.SizeX + 10;
            VerticalColorSliderR.PositionY = VerticalColorTextBoxR.PositionY + VerticalColorTextBoxR.SizeY / 2;
            VerticalColorSliderR.Size.X = largeRectangleSizeX - VerticalColorSliderB.PositionX + VerticalColorPosition.X - 10;
            VerticalColorSliderR.Size.Y = 5f;
            if (RayCasterWindowResized)
                VerticalColorSliderR.SetCircleToValue(VerticalColor.R);
            VerticalColorSliderR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                VerticalColor.R = (byte)Math.Round(VerticalColorSliderR.Value);

            // Green
            VerticalColorGText.DisplayedString = "G: " + VerticalColor.G.ToString("000");
            VerticalColorGText.Position = new Vector2f(VerticalColorPosition.X, VerticalColorTextBoxR.PositionY + VerticalColorTextBoxR.SizeY);
            VerticalColorGText.CharacterSize = characterSize;

            VerticalColorTextBoxG.PositionX = VerticalColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            VerticalColorTextBoxG.PositionY = VerticalColorTextBoxR.Position.Y + VerticalColorTextBoxR.Size.Y + 5;
            VerticalColorTextBoxG.SizeX = smallRectangleSizeX / 3;
            VerticalColorTextBoxG.SizeY = smallRectangleSizeY / 2;
            VerticalColorTextBoxG.Text.CharacterSize = characterSize;
            VerticalColorTextBoxG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            VerticalColorSliderG.PositionX = VerticalColorTextBoxG.PositionX + VerticalColorTextBoxG.SizeX + 10;
            VerticalColorSliderG.PositionY = VerticalColorTextBoxG.PositionY + VerticalColorTextBoxG.SizeY / 2;
            VerticalColorSliderG.Size.X = largeRectangleSizeX - VerticalColorSliderB.PositionX + VerticalColorPosition.X - 10;
            VerticalColorSliderG.Size.Y = 5f;
            if (RayCasterWindowResized)
                VerticalColorSliderG.SetCircleToValue(VerticalColor.G);
            VerticalColorSliderG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                VerticalColor.G = (byte)Math.Round(VerticalColorSliderG.Value);

            // Blue
            VerticalColorBText.DisplayedString = "B: " + VerticalColor.B.ToString("000");
            VerticalColorBText.Position = new Vector2f(VerticalColorPosition.X, VerticalColorTextBoxG.PositionY + VerticalColorTextBoxG.SizeY);
            VerticalColorBText.CharacterSize = characterSize;

            VerticalColorTextBoxB.PositionX = VerticalColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            VerticalColorTextBoxB.PositionY = VerticalColorTextBoxG.Position.Y + VerticalColorTextBoxG.Size.Y + 5;
            VerticalColorTextBoxB.SizeX = smallRectangleSizeX / 3;
            VerticalColorTextBoxB.SizeY = smallRectangleSizeY / 2;
            VerticalColorTextBoxB.Text.CharacterSize = characterSize;
            VerticalColorTextBoxB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            VerticalColorSliderB.PositionX = VerticalColorTextBoxB.PositionX + VerticalColorTextBoxB.SizeX + 10;
            VerticalColorSliderB.PositionY = VerticalColorTextBoxB.PositionY + VerticalColorTextBoxB.SizeY / 2;
            VerticalColorSliderB.Size.X = largeRectangleSizeX - VerticalColorSliderB.PositionX + VerticalColorPosition.X - 10;
            VerticalColorSliderB.Size.Y = 5f;
            if (RayCasterWindowResized)
                VerticalColorSliderB.SetCircleToValue(VerticalColor.B);
            VerticalColorSliderB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!RayCasterWindowResized)
                VerticalColor.B = (byte)Math.Round(VerticalColorSliderB.Value);

            VerticalColorRectangle.Position = VerticalColorPosition;
            VerticalColorRectangle.SizeX = largeRectangleSizeX;
            VerticalColorRectangle.SizeY = largeRectangleSizeY;

            if (RayCasterWindowResized)
            {
                RayCasterCounter++;
            }
            if (RayCasterCounter == 2)
            {
                RayCasterWindowResized = false;
                RayCasterCounter = 0;
            }
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

            RayMapColorRectangle.Draw(window);
            window.Draw(RayMapColorText);
            RayMapColorSampleRectangle.Draw(window);
            window.Draw(RayMapColorRText);
            RayMapColorTextBoxR.Draw(window);
            RayMapColorSliderR.Draw(window);
            window.Draw(RayMapColorGText);
            RayMapColorTextBoxG.Draw(window);
            RayMapColorSliderG.Draw(window);
            window.Draw(RayMapColorBText);
            RayMapColorTextBoxB.Draw(window);
            RayMapColorSliderB.Draw(window);

            HorizontalColorRectangle.Draw(window);
            window.Draw(HorizontalColorText);
            HorizontalColorSampleRectangle.Draw(window);
            window.Draw(HorizontalColorRText);
            HorizontalColorTextBoxR.Draw(window);
            HorizontalColorSliderR.Draw(window);
            window.Draw(HorizontalColorGText);
            HorizontalColorTextBoxG.Draw(window);
            HorizontalColorSliderG.Draw(window);
            window.Draw(HorizontalColorBText);
            HorizontalColorTextBoxB.Draw(window);
            HorizontalColorSliderB.Draw(window);

            VerticalColorRectangle.Draw(window);
            window.Draw(VerticalColorText);
            VerticalColorSampleRectangle.Draw(window);
            window.Draw(VerticalColorRText);
            VerticalColorTextBoxR.Draw(window);
            VerticalColorSliderR.Draw(window);
            window.Draw(VerticalColorGText);
            VerticalColorTextBoxG.Draw(window);
            VerticalColorSliderG.Draw(window);
            window.Draw(VerticalColorBText);
            VerticalColorTextBoxB.Draw(window);
            VerticalColorSliderB.Draw(window);
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
        private static void AngleSpacingTextBox_TextBoxEnterPressed(object source, EventArgs args)
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
        
        private static void RayMapColorTextBoxR_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                RayMapColor.R = Convert.ToByte(RayMapColorTextBoxR.TextString);
                RayMapColorSliderR.SetCircleToValue(RayMapColor.R);
            }
            catch { }
        }
        private static void RayMapColorTextBoxG_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                RayMapColor.G = Convert.ToByte(RayMapColorTextBoxG.TextString);
                RayMapColorSliderG.SetCircleToValue(RayMapColor.G);
            }
            catch { }
        }
        private static void RayMapColorTextBoxB_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                RayMapColor.B = Convert.ToByte(RayMapColorTextBoxB.TextString);
                RayMapColorSliderB.SetCircleToValue(RayMapColor.B);
            }
            catch { }
        }

        private static void HorizontalColorTextBoxR_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                HorizontalColor.R = Convert.ToByte(HorizontalColorTextBoxR.TextString);
                HorizontalColorSliderR.SetCircleToValue(HorizontalColor.R);
            }
            catch { }
        }
        private static void HorizontalColorTextBoxG_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                HorizontalColor.G = Convert.ToByte(HorizontalColorTextBoxG.TextString);
                HorizontalColorSliderG.SetCircleToValue(HorizontalColor.G);
            }
            catch { }
        }
        private static void HorizontalColorTextBoxB_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                HorizontalColor.B = Convert.ToByte(HorizontalColorTextBoxB.TextString);
                HorizontalColorSliderB.SetCircleToValue(HorizontalColor.B);
            }
            catch { }
        }

        private static void VerticalColorTextBoxR_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                VerticalColor.R = Convert.ToByte(VerticalColorTextBoxR.TextString);
                VerticalColorSliderR.SetCircleToValue(VerticalColor.R);
            }
            catch { }
        }
        private static void VerticalColorTextBoxG_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                VerticalColor.G = Convert.ToByte(VerticalColorTextBoxG.TextString);
                VerticalColorSliderG.SetCircleToValue(VerticalColor.G);
            }
            catch { }
        }
        private static void VerticalColorTextBoxB_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                VerticalColor.B = Convert.ToByte(VerticalColorTextBoxB.TextString);
                VerticalColorSliderB.SetCircleToValue(VerticalColor.B);
            }
            catch { }
        }
    }
}
