using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroController.GUI;
using MicroController.Shapes;
using MicroController.InputOutput;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MicroController.Game
{
    public partial class Settings
    {
        // SquareSize
        private static Text SquareSizeText;
        private static Slider SquareSizeSlider;
        private static TextBox SquareSizeTextBox;
        private static Rectangle SquareSizeRectangle;
        private static int SquareSize;

        // Image0FillColor
        private static Text Image0FillColorText;
        private static Slider Image0FillColorSliderR;
        private static Slider Image0FillColorSliderG;
        private static Slider Image0FillColorSliderB;
        private static TextBox Image0FillColorTextBoxR;
        private static TextBox Image0FillColorTextBoxG;
        private static TextBox Image0FillColorTextBoxB;
        private static Rectangle Image0FillColorRectangle;
        private static Rectangle Image0FillColorSampleRectangle;
        private static Color Image0FillColor;
        // R, G, B Text
        private static Text Image0FillColorRText;
        private static Text Image0FillColorGText;
        private static Text Image0FillColorBText;

        // Image1FillColor
        private static Text Image1FillColorText;
        private static Slider Image1FillColorSliderR;
        private static Slider Image1FillColorSliderG;
        private static Slider Image1FillColorSliderB;
        private static TextBox Image1FillColorTextBoxR;
        private static TextBox Image1FillColorTextBoxG;
        private static TextBox Image1FillColorTextBoxB;
        private static Rectangle Image1FillColorRectangle;
        private static Rectangle Image1FillColorSampleRectangle;
        private static Color Image1FillColor;
        // R, G, B Text
        private static Text Image1FillColorRText;
        private static Text Image1FillColorGText;
        private static Text Image1FillColorBText;

        // OutlineColor
        private static Text OutlineColorText;
        private static Slider OutlineColorSliderR;
        private static Slider OutlineColorSliderG;
        private static Slider OutlineColorSliderB;
        private static TextBox OutlineColorTextBoxR;
        private static TextBox OutlineColorTextBoxG;
        private static TextBox OutlineColorTextBoxB;
        private static Rectangle OutlineColorRectangle;
        private static Rectangle OutlineColorSampleRectangle;
        private static Color OutlineColor;
        // R, G, B Text
        private static Text OutlineColorRText;
        private static Text OutlineColorGText;
        private static Text OutlineColorBText;

        private static bool MapWindowResized = true;
        private static int MapCounter = 0;

        private static void MapSettingsInit(Game game)
        {
            // SquareSize
            SquareSize = game.map.SquareSize;
            SquareSizeText = new Text("SquareSize:" + SquareSize.ToString("0000"), MessegeManager.Courier);
            SquareSizeSlider = new Slider(new Vector2f(), new Vector2f(), new Vector2f(1, 100), SquareSize, 10);
            SquareSizeTextBox = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            SquareSizeRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };

            // Image0FillColor
            Image0FillColor = game.map.Image0FillColor;
            Image0FillColorText = new Text("Empty square Color:", MessegeManager.Courier);
            Image0FillColorSliderR = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), Image0FillColor.R, 10);
            Image0FillColorSliderG = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), Image0FillColor.G, 10);
            Image0FillColorSliderB = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), Image0FillColor.B, 10);
            Image0FillColorTextBoxR = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            Image0FillColorTextBoxG = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            Image0FillColorTextBoxB = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            Image0FillColorSampleRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 1f, OutlineColor = Color.Black, FillColor = Image0FillColor };
            Image0FillColorRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };
            // R, G, B Text
            Image0FillColorRText = new Text("R: 000", MessegeManager.Courier);
            Image0FillColorGText = new Text("G: 000", MessegeManager.Courier);
            Image0FillColorBText = new Text("B: 000", MessegeManager.Courier);

            // Image1FillColor
            Image1FillColor = game.map.Image1FillColor;
            Image1FillColorText = new Text("Full square Color:", MessegeManager.Courier);
            Image1FillColorSliderR = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), Image1FillColor.R, 10);
            Image1FillColorSliderG = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), Image1FillColor.G, 10);
            Image1FillColorSliderB = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), Image1FillColor.B, 10);
            Image1FillColorTextBoxR = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            Image1FillColorTextBoxG = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            Image1FillColorTextBoxB = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            Image1FillColorSampleRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 1f, OutlineColor = Color.Black, FillColor = Image1FillColor };
            Image1FillColorRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };
            // R, G, B Text
            Image1FillColorRText = new Text("R: 000", MessegeManager.Courier);
            Image1FillColorGText = new Text("G: 000", MessegeManager.Courier);
            Image1FillColorBText = new Text("B: 000", MessegeManager.Courier);

            // OutlineColor
            OutlineColor = game.map.OutlineColor;
            OutlineColorText = new Text("Outline Color:", MessegeManager.Courier);
            OutlineColorSliderR = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), OutlineColor.R, 10);
            OutlineColorSliderG = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), OutlineColor.G, 10);
            OutlineColorSliderB = new Slider(new Vector2f(), new Vector2f(), new Vector2f(0, 255), OutlineColor.B, 10);
            OutlineColorTextBoxR = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            OutlineColorTextBoxG = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            OutlineColorTextBoxB = new TextBox(new Vector2f(), new Vector2f(), MessegeManager.Courier, 0);
            OutlineColorSampleRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 1f, OutlineColor = Color.Black, FillColor = Image1FillColor };
            OutlineColorRectangle = new Rectangle(new Vector2f(), new Vector2f()) { OutlineThickness = 2f, OutlineColor = RectangleOutlineColor, FillColor = RectangleFillColor };
            // R, G, B Text
            OutlineColorRText = new Text("R: 000", MessegeManager.Courier);
            OutlineColorGText = new Text("G: 000", MessegeManager.Courier);
            OutlineColorBText = new Text("B: 000", MessegeManager.Courier);

            // Events
            SquareSizeTextBox.TextBoxEnterPressed += SquareSizeTextBox_TextBoxEnterPressed;

            Image0FillColorTextBoxR.TextBoxEnterPressed += Image0FillColorTextBoxR_TextBoxEnterPressed;
            Image0FillColorTextBoxG.TextBoxEnterPressed += Image0FillColorTextBoxG_TextBoxEnterPressed;
            Image0FillColorTextBoxB.TextBoxEnterPressed += Image0FillColorTextBoxB_TextBoxEnterPressed;

            Image1FillColorTextBoxR.TextBoxEnterPressed += Image1FillColorTextBoxR_TextBoxEnterPressed;
            Image1FillColorTextBoxG.TextBoxEnterPressed += Image1FillColorTextBoxG_TextBoxEnterPressed;
            Image1FillColorTextBoxB.TextBoxEnterPressed += Image1FillColorTextBoxB_TextBoxEnterPressed;

            OutlineColorTextBoxR.TextBoxEnterPressed += OutlineColorTextBoxR_TextBoxEnterPressed;
            OutlineColorTextBoxG.TextBoxEnterPressed += OutlineColorTextBoxG_TextBoxEnterPressed;
            OutlineColorTextBoxB.TextBoxEnterPressed += OutlineColorTextBoxB_TextBoxEnterPressed;
        }

        private static void MapSettingsUpdate(Game game)
        {
            // Universals
            uint characterSize = (uint)ButtonSize.Y / 4;
            float smallRectangleSizeX = game.Window.Size.X / 2.07f;
            float smallRectangleSizeY = game.Window.Size.Y / 9;

            float largeRectangleSizeX = smallRectangleSizeX;
            float largeRectangleSizeY = smallRectangleSizeY * 2.2f;

            // SquareSize
            Vector2f SquareSizePosition = new Vector2f(4, ButtonSize.Y + 15);
            SquareSizeText.DisplayedString = "SquareSize:" + SquareSize.ToString("0000");
            SquareSizeText.Position = SquareSizePosition;
            SquareSizeText.CharacterSize = characterSize;

            SquareSizeTextBox.PositionX = SquareSizeText.GetGlobalBounds().Left + MessegeManager.GetTextRect("SquareSize: 0000", MessegeManager.Courier, characterSize).Width + 10;
            SquareSizeTextBox.PositionY = SquareSizePosition.Y;
            SquareSizeTextBox.SizeX = smallRectangleSizeX / 3;
            SquareSizeTextBox.SizeY = smallRectangleSizeY;
            SquareSizeTextBox.Text.CharacterSize = characterSize;
            SquareSizeTextBox.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            SquareSizeSlider.PositionX = SquareSizeTextBox.Position.X + SquareSizeTextBox.Size.X + 10;
            SquareSizeSlider.PositionY = SquareSizePosition.Y + smallRectangleSizeY / 2 - SquareSizeSlider.Size.Y / 2;
            SquareSizeSlider.Size.X = smallRectangleSizeX - SquareSizeSlider.PositionX;
            SquareSizeSlider.Size.Y = 5f;
            if (MapWindowResized)
                SquareSizeSlider.SetCircleToValue(SquareSize);
            SquareSizeSlider.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                SquareSize = (int)Math.Round(SquareSizeSlider.Value);

            SquareSizeRectangle.Position = SquareSizePosition;
            SquareSizeRectangle.SizeX = smallRectangleSizeX;
            SquareSizeRectangle.SizeY = smallRectangleSizeY;


            // Image0FillColor
            Vector2f Image0FillColorPosition = new Vector2f(4, ButtonSize.Y + 15 * 2 + smallRectangleSizeY);
            Image0FillColorText.Position = Image0FillColorPosition;
            Image0FillColorText.CharacterSize = characterSize;

            Image0FillColorSampleRectangle.PositionX = Image0FillColorPosition.X + Image0FillColorText.GetLocalBounds().Width + 10;
            Image0FillColorSampleRectangle.PositionY = Image0FillColorPosition.Y;
            Image0FillColorSampleRectangle.SizeX = largeRectangleSizeX - Image0FillColorText.GetLocalBounds().Width - 20;
            Image0FillColorSampleRectangle.SizeY = Image0FillColorText.GetLocalBounds().Height + 6;
            Image0FillColorSampleRectangle.FillColor = Image0FillColor;

            // Red
            Image0FillColorRText.DisplayedString = "R: " + Image0FillColor.R.ToString("000");
            Image0FillColorRText.Position = new Vector2f(Image0FillColorPosition.X, Image0FillColorPosition.Y + Image0FillColorText.GetLocalBounds().Height + 6);
            Image0FillColorRText.CharacterSize = characterSize;

            Image0FillColorTextBoxR.PositionX = Image0FillColorPosition.X + MessegeManager.GetTextRect("R: 000", MessegeManager.Courier, characterSize).Width + 10;
            Image0FillColorTextBoxR.PositionY = Image0FillColorPosition.Y + Image0FillColorText.GetLocalBounds().Height + Image0FillColorText.GetLocalBounds().Top + 5;
            Image0FillColorTextBoxR.SizeX = smallRectangleSizeX / 3;
            Image0FillColorTextBoxR.SizeY = smallRectangleSizeY / 2;
            Image0FillColorTextBoxR.Text.CharacterSize = characterSize;
            Image0FillColorTextBoxR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            Image0FillColorSliderR.PositionX = Image0FillColorTextBoxR.PositionX + Image0FillColorTextBoxR.SizeX + 10;
            Image0FillColorSliderR.PositionY = Image0FillColorTextBoxR.PositionY + Image0FillColorTextBoxR.SizeY / 2;
            Image0FillColorSliderR.Size.X = largeRectangleSizeX - Image0FillColorSliderB.PositionX + Image0FillColorPosition.X - 10;
            Image0FillColorSliderR.Size.Y = 5f;
            if (MapWindowResized)
                Image0FillColorSliderR.SetCircleToValue(Image0FillColor.R);
            Image0FillColorSliderR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                Image0FillColor.R = (byte)Math.Round(Image0FillColorSliderR.Value);

            // Green
            Image0FillColorGText.DisplayedString = "G: " + Image0FillColor.G.ToString("000");
            Image0FillColorGText.Position = new Vector2f(Image0FillColorPosition.X, Image0FillColorTextBoxR.PositionY + Image0FillColorTextBoxR.SizeY);
            Image0FillColorGText.CharacterSize = characterSize;

            Image0FillColorTextBoxG.PositionX = Image0FillColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            Image0FillColorTextBoxG.PositionY = Image0FillColorTextBoxR.Position.Y + Image0FillColorTextBoxR.Size.Y + 5;
            Image0FillColorTextBoxG.SizeX = smallRectangleSizeX / 3;
            Image0FillColorTextBoxG.SizeY = smallRectangleSizeY / 2;
            Image0FillColorTextBoxG.Text.CharacterSize = characterSize;
            Image0FillColorTextBoxG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            Image0FillColorSliderG.PositionX = Image0FillColorTextBoxG.PositionX + Image0FillColorTextBoxG.SizeX + 10;
            Image0FillColorSliderG.PositionY = Image0FillColorTextBoxG.PositionY + Image0FillColorTextBoxG.SizeY / 2;
            Image0FillColorSliderG.Size.X = largeRectangleSizeX - Image0FillColorSliderB.PositionX + Image0FillColorPosition.X - 10;
            Image0FillColorSliderG.Size.Y = 5f;
            if (MapWindowResized)
                Image0FillColorSliderG.SetCircleToValue(Image0FillColor.G);
            Image0FillColorSliderG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                Image0FillColor.G = (byte)Math.Round(Image0FillColorSliderG.Value);

            // Blue
            Image0FillColorBText.DisplayedString = "B: " + Image0FillColor.B.ToString("000");
            Image0FillColorBText.Position = new Vector2f(Image0FillColorPosition.X, Image0FillColorTextBoxG.PositionY + Image0FillColorTextBoxG.SizeY);
            Image0FillColorBText.CharacterSize = characterSize;

            Image0FillColorTextBoxB.PositionX = Image0FillColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            Image0FillColorTextBoxB.PositionY = Image0FillColorTextBoxG.Position.Y + Image0FillColorTextBoxG.Size.Y + 5;
            Image0FillColorTextBoxB.SizeX = smallRectangleSizeX / 3;
            Image0FillColorTextBoxB.SizeY = smallRectangleSizeY / 2;
            Image0FillColorTextBoxB.Text.CharacterSize = characterSize;
            Image0FillColorTextBoxB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            Image0FillColorSliderB.PositionX = Image0FillColorTextBoxB.PositionX + Image0FillColorTextBoxB.SizeX + 10;
            Image0FillColorSliderB.PositionY = Image0FillColorTextBoxB.PositionY + Image0FillColorTextBoxB.SizeY / 2;
            Image0FillColorSliderB.Size.X = largeRectangleSizeX - Image0FillColorSliderB.PositionX + Image0FillColorPosition.X - 10;
            Image0FillColorSliderB.Size.Y = 5f;
            if (MapWindowResized)
                Image0FillColorSliderB.SetCircleToValue(Image0FillColor.B);
            Image0FillColorSliderB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                Image0FillColor.B = (byte)Math.Round(Image0FillColorSliderB.Value);

            Image0FillColorRectangle.Position = Image0FillColorPosition;
            Image0FillColorRectangle.SizeX = largeRectangleSizeX;
            Image0FillColorRectangle.SizeY = largeRectangleSizeY;


            // Image1FillColor
            Vector2f Image1FillColorPosition = new Vector2f(4, ButtonSize.Y + 15 * 3 + smallRectangleSizeY + largeRectangleSizeY);
            Image1FillColorText.Position = Image1FillColorPosition;
            Image1FillColorText.CharacterSize = characterSize;

            Image1FillColorSampleRectangle.PositionX = Image1FillColorPosition.X + Image1FillColorText.GetLocalBounds().Width + 10;
            Image1FillColorSampleRectangle.PositionY = Image1FillColorPosition.Y;
            Image1FillColorSampleRectangle.SizeX = largeRectangleSizeX - Image1FillColorText.GetLocalBounds().Width - 20;
            Image1FillColorSampleRectangle.SizeY = Image1FillColorText.GetLocalBounds().Height + 6;
            Image1FillColorSampleRectangle.FillColor = Image1FillColor;

            // Red
            Image1FillColorRText.DisplayedString = "R: " + Image1FillColor.R.ToString("000");
            Image1FillColorRText.Position = new Vector2f(Image1FillColorPosition.X, Image1FillColorPosition.Y + Image1FillColorText.GetLocalBounds().Height + 6);
            Image1FillColorRText.CharacterSize = characterSize;

            Image1FillColorTextBoxR.PositionX = Image1FillColorPosition.X + MessegeManager.GetTextRect("R: 000", MessegeManager.Courier, characterSize).Width + 10;
            Image1FillColorTextBoxR.PositionY = Image1FillColorPosition.Y + Image1FillColorText.GetLocalBounds().Height + Image1FillColorText.GetLocalBounds().Top + 5;
            Image1FillColorTextBoxR.SizeX = smallRectangleSizeX / 3;
            Image1FillColorTextBoxR.SizeY = smallRectangleSizeY / 2;
            Image1FillColorTextBoxR.Text.CharacterSize = characterSize;
            Image1FillColorTextBoxR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            Image1FillColorSliderR.PositionX = Image1FillColorTextBoxR.PositionX + Image1FillColorTextBoxR.SizeX + 10;
            Image1FillColorSliderR.PositionY = Image1FillColorTextBoxR.PositionY + Image1FillColorTextBoxR.SizeY / 2;
            Image1FillColorSliderR.Size.X = largeRectangleSizeX - Image1FillColorSliderB.PositionX + Image1FillColorPosition.X - 10;
            Image1FillColorSliderR.Size.Y = 5f;
            if (MapWindowResized)
                Image1FillColorSliderR.SetCircleToValue(Image1FillColor.R);
            Image1FillColorSliderR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                Image1FillColor.R = (byte)Math.Round(Image1FillColorSliderR.Value);

            // Green
            Image1FillColorGText.DisplayedString = "G: " + Image1FillColor.G.ToString("000");
            Image1FillColorGText.Position = new Vector2f(Image1FillColorPosition.X, Image1FillColorTextBoxR.PositionY + Image1FillColorTextBoxR.SizeY);
            Image1FillColorGText.CharacterSize = characterSize;

            Image1FillColorTextBoxG.PositionX = Image1FillColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            Image1FillColorTextBoxG.PositionY = Image1FillColorTextBoxR.Position.Y + Image1FillColorTextBoxR.Size.Y + 5;
            Image1FillColorTextBoxG.SizeX = smallRectangleSizeX / 3;
            Image1FillColorTextBoxG.SizeY = smallRectangleSizeY / 2;
            Image1FillColorTextBoxG.Text.CharacterSize = characterSize;
            Image1FillColorTextBoxG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            Image1FillColorSliderG.PositionX = Image1FillColorTextBoxG.PositionX + Image1FillColorTextBoxG.SizeX + 10;
            Image1FillColorSliderG.PositionY = Image1FillColorTextBoxG.PositionY + Image1FillColorTextBoxG.SizeY / 2;
            Image1FillColorSliderG.Size.X = largeRectangleSizeX - Image1FillColorSliderB.PositionX + Image1FillColorPosition.X - 10;
            Image1FillColorSliderG.Size.Y = 5f;
            if (MapWindowResized)
                Image1FillColorSliderG.SetCircleToValue(Image1FillColor.G);
            Image1FillColorSliderG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                Image1FillColor.G = (byte)Math.Round(Image1FillColorSliderG.Value);

            // Blue
            Image1FillColorBText.DisplayedString = "B: " + Image1FillColor.B.ToString("000");
            Image1FillColorBText.Position = new Vector2f(Image1FillColorPosition.X, Image1FillColorTextBoxG.PositionY + Image1FillColorTextBoxG.SizeY);
            Image1FillColorBText.CharacterSize = characterSize;

            Image1FillColorTextBoxB.PositionX = Image1FillColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            Image1FillColorTextBoxB.PositionY = Image1FillColorTextBoxG.Position.Y + Image1FillColorTextBoxG.Size.Y + 5;
            Image1FillColorTextBoxB.SizeX = smallRectangleSizeX / 3;
            Image1FillColorTextBoxB.SizeY = smallRectangleSizeY / 2;
            Image1FillColorTextBoxB.Text.CharacterSize = characterSize;
            Image1FillColorTextBoxB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            Image1FillColorSliderB.PositionX = Image1FillColorTextBoxB.PositionX + Image1FillColorTextBoxB.SizeX + 10;
            Image1FillColorSliderB.PositionY = Image1FillColorTextBoxB.PositionY + Image1FillColorTextBoxB.SizeY / 2;
            Image1FillColorSliderB.Size.X = largeRectangleSizeX - Image1FillColorSliderB.PositionX + Image1FillColorPosition.X - 10;
            Image1FillColorSliderB.Size.Y = 5f;
            if (MapWindowResized)
                Image1FillColorSliderB.SetCircleToValue(Image1FillColor.B);
            Image1FillColorSliderB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                Image1FillColor.B = (byte)Math.Round(Image1FillColorSliderB.Value);

            Image1FillColorRectangle.Position = Image1FillColorPosition;
            Image1FillColorRectangle.SizeX = largeRectangleSizeX;
            Image1FillColorRectangle.SizeY = largeRectangleSizeY;


            // OutlineColor
            Vector2f OutlineColorPosition = new Vector2f(4 + smallRectangleSizeX + 15, ButtonSize.Y + 15);
            OutlineColorText.Position = OutlineColorPosition;
            OutlineColorText.CharacterSize = characterSize;

            OutlineColorSampleRectangle.PositionX = OutlineColorPosition.X + OutlineColorText.GetLocalBounds().Width + 10;
            OutlineColorSampleRectangle.PositionY = OutlineColorPosition.Y;
            OutlineColorSampleRectangle.SizeX = largeRectangleSizeX - OutlineColorText.GetLocalBounds().Width - 20;
            OutlineColorSampleRectangle.SizeY = OutlineColorText.GetLocalBounds().Height + 6;
            OutlineColorSampleRectangle.FillColor = OutlineColor;

            // Red
            OutlineColorRText.DisplayedString = "R: " + OutlineColor.R.ToString("000");
            OutlineColorRText.Position = new Vector2f(OutlineColorPosition.X, OutlineColorPosition.Y + OutlineColorText.GetLocalBounds().Height + 6);
            OutlineColorRText.CharacterSize = characterSize;

            OutlineColorTextBoxR.PositionX = OutlineColorPosition.X + MessegeManager.GetTextRect("R: 000", MessegeManager.Courier, characterSize).Width + 10;
            OutlineColorTextBoxR.PositionY = OutlineColorPosition.Y + OutlineColorText.GetLocalBounds().Height + OutlineColorText.GetLocalBounds().Top + 5;
            OutlineColorTextBoxR.SizeX = smallRectangleSizeX / 3;
            OutlineColorTextBoxR.SizeY = smallRectangleSizeY / 2;
            OutlineColorTextBoxR.Text.CharacterSize = characterSize;
            OutlineColorTextBoxR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            OutlineColorSliderR.PositionX = OutlineColorTextBoxR.PositionX + OutlineColorTextBoxR.SizeX + 10;
            OutlineColorSliderR.PositionY = OutlineColorTextBoxR.PositionY + OutlineColorTextBoxR.SizeY / 2;
            OutlineColorSliderR.Size.X = largeRectangleSizeX - OutlineColorSliderB.PositionX + OutlineColorPosition.X - 10;
            OutlineColorSliderR.Size.Y = 5f;
            if (MapWindowResized)
                OutlineColorSliderR.SetCircleToValue(OutlineColor.R);
            OutlineColorSliderR.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                OutlineColor.R = (byte)Math.Round(OutlineColorSliderR.Value);

            // Green
            OutlineColorGText.DisplayedString = "G: " + OutlineColor.G.ToString("000");
            OutlineColorGText.Position = new Vector2f(OutlineColorPosition.X, OutlineColorTextBoxR.PositionY + OutlineColorTextBoxR.SizeY);
            OutlineColorGText.CharacterSize = characterSize;

            OutlineColorTextBoxG.PositionX = OutlineColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            OutlineColorTextBoxG.PositionY = OutlineColorTextBoxR.Position.Y + OutlineColorTextBoxR.Size.Y + 5;
            OutlineColorTextBoxG.SizeX = smallRectangleSizeX / 3;
            OutlineColorTextBoxG.SizeY = smallRectangleSizeY / 2;
            OutlineColorTextBoxG.Text.CharacterSize = characterSize;
            OutlineColorTextBoxG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            OutlineColorSliderG.PositionX = OutlineColorTextBoxG.PositionX + OutlineColorTextBoxG.SizeX + 10;
            OutlineColorSliderG.PositionY = OutlineColorTextBoxG.PositionY + OutlineColorTextBoxG.SizeY / 2;
            OutlineColorSliderG.Size.X = largeRectangleSizeX - OutlineColorSliderB.PositionX + OutlineColorPosition.X - 10;
            OutlineColorSliderG.Size.Y = 5f;
            if (MapWindowResized)
                OutlineColorSliderG.SetCircleToValue(OutlineColor.G);
            OutlineColorSliderG.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                OutlineColor.G = (byte)Math.Round(OutlineColorSliderG.Value);

            // Blue
            OutlineColorBText.DisplayedString = "B: " + OutlineColor.B.ToString("000");
            OutlineColorBText.Position = new Vector2f(OutlineColorPosition.X, OutlineColorTextBoxG.PositionY + OutlineColorTextBoxG.SizeY);
            OutlineColorBText.CharacterSize = characterSize;

            OutlineColorTextBoxB.PositionX = OutlineColorPosition.X + MessegeManager.GetTextRect("G: 000", MessegeManager.Courier, characterSize).Width + 10;
            OutlineColorTextBoxB.PositionY = OutlineColorTextBoxG.Position.Y + OutlineColorTextBoxG.Size.Y + 5;
            OutlineColorTextBoxB.SizeX = smallRectangleSizeX / 3;
            OutlineColorTextBoxB.SizeY = smallRectangleSizeY / 2;
            OutlineColorTextBoxB.Text.CharacterSize = characterSize;
            OutlineColorTextBoxB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet);

            OutlineColorSliderB.PositionX = OutlineColorTextBoxB.PositionX + OutlineColorTextBoxB.SizeX + 10;
            OutlineColorSliderB.PositionY = OutlineColorTextBoxB.PositionY + OutlineColorTextBoxB.SizeY / 2;
            OutlineColorSliderB.Size.X = largeRectangleSizeX - OutlineColorSliderB.PositionX + OutlineColorPosition.X - 10;
            OutlineColorSliderB.Size.Y = 5f;
            if (MapWindowResized)
                OutlineColorSliderB.SetCircleToValue(OutlineColor.B);
            OutlineColorSliderB.Update(Mouse.GetPosition() - game.Window.Position - MouseManager.MouseOffSet, Mouse.IsButtonPressed(Mouse.Button.Left));
            if (!MapWindowResized)
                OutlineColor.B = (byte)Math.Round(OutlineColorSliderB.Value);

            OutlineColorRectangle.Position = OutlineColorPosition;
            OutlineColorRectangle.SizeX = largeRectangleSizeX;
            OutlineColorRectangle.SizeY = largeRectangleSizeY;

            if (MapWindowResized)
            {
                MapCounter++;
            }
            if (MapCounter == 2)
            {
                MapWindowResized = false;
                MapCounter = 0;
            }
        }

        private static void MapSettingsDraw(RenderWindow window) 
        {
            SquareSizeRectangle.Draw(window);
            window.Draw(SquareSizeText);
            SquareSizeTextBox.Draw(window);
            SquareSizeSlider.Draw(window);

            Image0FillColorRectangle.Draw(window);
            window.Draw(Image0FillColorText);
            Image0FillColorSampleRectangle.Draw(window);
            window.Draw(Image0FillColorRText);
            Image0FillColorTextBoxR.Draw(window);
            Image0FillColorSliderR.Draw(window);
            window.Draw(Image0FillColorGText);
            Image0FillColorTextBoxG.Draw(window);
            Image0FillColorSliderG.Draw(window);
            window.Draw(Image0FillColorBText);
            Image0FillColorTextBoxB.Draw(window);
            Image0FillColorSliderB.Draw(window);

            Image1FillColorRectangle.Draw(window);
            window.Draw(Image1FillColorText);
            Image1FillColorSampleRectangle.Draw(window);
            window.Draw(Image1FillColorRText);
            Image1FillColorTextBoxR.Draw(window);
            Image1FillColorSliderR.Draw(window);
            window.Draw(Image1FillColorGText);
            Image1FillColorTextBoxG.Draw(window);
            Image1FillColorSliderG.Draw(window);
            window.Draw(Image1FillColorBText);
            Image1FillColorTextBoxB.Draw(window);
            Image1FillColorSliderB.Draw(window);

            OutlineColorRectangle.Draw(window);
            window.Draw(OutlineColorText);
            OutlineColorSampleRectangle.Draw(window);
            window.Draw(OutlineColorRText);
            OutlineColorTextBoxR.Draw(window);
            OutlineColorSliderR.Draw(window);
            window.Draw(OutlineColorGText);
            OutlineColorTextBoxG.Draw(window);
            OutlineColorSliderG.Draw(window);
            window.Draw(OutlineColorBText);
            OutlineColorTextBoxB.Draw(window);
            OutlineColorSliderB.Draw(window);
        }

        private static void SquareSizeTextBox_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                SquareSize = Convert.ToInt32(SquareSizeTextBox.TextString);
                SquareSizeSlider.SetCircleToValue(SquareSize);
            }
            catch { }
        }

        private static void Image0FillColorTextBoxR_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Image0FillColor.R = Convert.ToByte(Image0FillColorTextBoxR.TextString);
                Image0FillColorSliderR.SetCircleToValue(Image0FillColor.R);
            }
            catch { }
        }
        private static void Image0FillColorTextBoxG_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Image0FillColor.G = Convert.ToByte(Image0FillColorTextBoxG.TextString);
                Image0FillColorSliderG.SetCircleToValue(Image0FillColor.G);
            }
            catch { }
        }
        private static void Image0FillColorTextBoxB_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Image0FillColor.B = Convert.ToByte(Image0FillColorTextBoxB.TextString);
                Image0FillColorSliderB.SetCircleToValue(Image0FillColor.B);
            }
            catch { }
        }

        private static void Image1FillColorTextBoxR_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Image1FillColor.R = Convert.ToByte(Image1FillColorTextBoxR.TextString);
                Image1FillColorSliderR.SetCircleToValue(Image1FillColor.R);
            }
            catch { }
        }
        private static void Image1FillColorTextBoxG_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Image1FillColor.G = Convert.ToByte(Image1FillColorTextBoxG.TextString);
                Image1FillColorSliderG.SetCircleToValue(Image1FillColor.G);
            }
            catch { }
        }
        private static void Image1FillColorTextBoxB_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                Image1FillColor.B = Convert.ToByte(Image1FillColorTextBoxB.TextString);
                Image1FillColorSliderB.SetCircleToValue(Image1FillColor.B);
            }
            catch { }
        }

        private static void OutlineColorTextBoxR_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                OutlineColor.R = Convert.ToByte(OutlineColorTextBoxR.TextString);
                OutlineColorSliderR.SetCircleToValue(OutlineColor.R);
            }
            catch { }
        }
        private static void OutlineColorTextBoxG_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                OutlineColor.G = Convert.ToByte(OutlineColorTextBoxG.TextString);
                OutlineColorSliderG.SetCircleToValue(OutlineColor.G);
            }
            catch { }
        }
        private static void OutlineColorTextBoxB_TextBoxEnterPressed(object source, EventArgs args)
        {
            try
            {
                OutlineColor.B = Convert.ToByte(OutlineColorTextBoxB.TextString);
                OutlineColorSliderB.SetCircleToValue(OutlineColor.B);
            }
            catch { }
        }
    }
}
