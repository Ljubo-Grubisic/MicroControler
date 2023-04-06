using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using MicroController.Shapes;
using MicroController.InputOutput;
using System.Threading;
using MicroController.Mathematics;
using SFML.Window;
using System.Runtime.InteropServices;
using System.Net;

namespace MicroController.GUI
{
    public class TextBox : Rectangle
    {
        public string DisplayedString
        {
            get { return Text.DisplayedString; }
            set { Text.DisplayedString = value; }
        }
        public Text Text;

        #region Getters and Setters
        /// <summary>
        /// Character size of the text
        /// </summary>
        public uint CharacterSize { get => Text.CharacterSize; set => Text.CharacterSize = value; }
        /// <summary>
        /// Font of the text
        /// </summary>
        public Font TextFont { get => Text.Font; set => Text.Font = value; }
        #endregion

        private int Index = 0;
        private Cursor EditTextCursor;

        public delegate void TextBoxEventHandler(object source, EventArgs args);

        public event TextBoxEventHandler TextBoxEnterPressed;

        private bool Clicked = false;
        private uint id;
        private static uint counter = 0;

        public TextBox(Vector2f position, Vector2f size) : base(position, size)
        {
            this.OutlineThickness = 2f;
            this.OutlineColor = Color.Black;
            this.Text = new Text("", MessegeManager.Courier, 14);

            this.Text.Color = Color.Black;
            this.Text.Position = new Vector2f(PositionX, PositionY + ((SizeY - MessegeManager.GetTextRect("A", MessegeManager.Courier, 14).Height) / 2));
            this.EditTextCursor = new Cursor(position, Color.Black, 0.5f, 14 + 2f, 12f);

            this.id = counter;
            counter += 3;
            if (counter == 99)
                counter = 0;
        }
        public TextBox(Vector2f position, Vector2f size, Font font, uint fontSize) : base(position, size)
        {
            this.OutlineThickness = 2f;
            this.OutlineColor = Color.Black;
            this.Text = new Text("", font, fontSize);
            this.Text.Color = Color.Black;
            this.Text.Position = new Vector2f(PositionX, PositionY + ((SizeY - MessegeManager.GetTextRect("A", font, fontSize).Height) / 2));
            this.EditTextCursor = new Cursor(position, Color.Black, 0.5f, fontSize + 2f, 12f);

            this.id = counter;
            counter += 3;
            if (counter == 99)
                counter = 0;
        }

        public new void Draw(RenderWindow window)
        {
            base.Draw(window);
            window.Draw(Text);
            if (Clicked)
            {
                EditTextCursor.Draw(window);
            }
        }

        public void Update(Vector2i mousePos)
        {
            this.Text.Position = new Vector2f(PositionX, PositionY + ((SizeY - Text.GetLocalBounds().Height) / 2) - Text.GetLocalBounds().Top);
            this.EditTextCursor.SizeCenter = Text.CharacterSize + 2f;
            this.EditTextCursor.Update(new Vector2f(MessegeManager.GetTextRect("", Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));

            UpdateIndexBounds();

            if (Index == DisplayedString.Length && DisplayedString.Length != 0)
            {
                this.EditTextCursor.Update(new Vector2f(MessegeManager.GetTextRect(DisplayedString, Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));
            }
            else if (DisplayedString != "")
            {
                this.EditTextCursor.Update(new Vector2f(MessegeManager.GetTextRect(DisplayedString.Remove(Index), Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));
            }
            else if(Index == 0)
            {
                this.EditTextCursor.Update(new Vector2f(MessegeManager.GetTextRect("", Text.Font, Text.CharacterSize).Width + PositionX + 3, SizeY / 2 + PositionY));
            }

            bool mouseState = MouseManager.OnMouseDown(Mouse.Button.Left, id);

            if (mouseState && Clicked)
            {
                this.Clicked = false;
            }
            if (MathHelper.IsMouseInRectangle(this, mousePos) && mouseState)
            {
                this.Clicked = true;
            }

            if (Clicked)
            {
                UpdateIndexKeys();
                UpdateIndexBounds();

                string input = KeyboardManager.ReadInput("");

                if (input != "")
                {
                    if (input.Contains("BackSpace"))
                    {
                        input = input.Replace("BackSpace", "");
                        try
                        {
                            this.DisplayedString = this.DisplayedString.Remove(Index - 1, 1);
                            Index--;
                        }
                        catch { }
                    }
                    if (input.Contains("Space"))
                    {
                        input = input.Replace("Space", " ");
                    }
                    if (input.Contains("Return"))
                    {
                        input = input.Replace("Return", "");
                        OnTextBoxEnterPressed();
                    }

                    this.DisplayedString = this.DisplayedString.Insert(Index, input);
                    Index += input.Length;
                }
            }
        }

        private void UpdateIndexKeys()
        {
            if (KeyboardManager.OnKeyPressTextBoxOnly(Keyboard.Key.Left, (int)id + 1))
            {
                Index--;
            }
            if (KeyboardManager.OnKeyPressTextBoxOnly(Keyboard.Key.Right, (int)id + 2))
            {
                Index++;
            }
        }
        private void UpdateIndexBounds()
        {
            if (Index < 0)
            {
                Index = 0;
            }
            if (Index > DisplayedString.Length && DisplayedString.Length != 0)
            {
                Index = DisplayedString.Length;
            }
        }

        protected virtual void OnTextBoxEnterPressed()
        {
            if (TextBoxEnterPressed != null)
                TextBoxEnterPressed(this, EventArgs.Empty);
        }

        private class Cursor
        {
            public Vector2f Position;

            private Line[] Lines = new Line[3];
            public float SizeCenter;
            public float SizeSides;

            public Cursor(Vector2f position, Color color, float outlineThickness, float sizeCenter, float sizeSides)
            {
                this.Position = position;
                this.SizeCenter = sizeCenter;
                this.SizeSides = sizeSides;

                for (int i = 0; i < 3; i++)
                {
                    Lines[i] = new Line();
                    Lines[i].OutlineColor = color;
                    Lines[i].OutlineThickness = outlineThickness;
                }
                Lines[0].Position0 = new Vector2f(Position.X, Position.Y - (SizeCenter / 2));
                Lines[0].Position1 = new Vector2f(Position.X, Position.Y + (SizeCenter / 2));

                Lines[1].Position0 = new Vector2f(Position.X - (SizeSides / 2), Position.Y + (SizeCenter / 2));
                Lines[1].Position1 = new Vector2f(Position.X + (SizeSides / 2), Position.Y + (SizeCenter / 2));

                Lines[2].Position0 = new Vector2f(Position.X - (SizeSides / 2), Position.Y - (SizeCenter / 2));
                Lines[2].Position1 = new Vector2f(Position.X + (SizeSides / 2), Position.Y - (SizeCenter / 2));
            }

            public void Update(Vector2f positon)
            {
                Position = positon;
                Lines[0].Position0X = Position.X;
                Lines[0].Position0Y = Position.Y - (SizeCenter / 2);
                Lines[0].Position1X = Position.X;
                Lines[0].Position1Y = Position.Y + (SizeCenter / 2);

                Lines[1].Position0X = Position.X - (SizeSides / 2);
                Lines[1].Position0Y = Position.Y + (SizeCenter / 2);
                Lines[1].Position1X = Position.X + (SizeSides / 2);
                Lines[1].Position1Y = Position.Y + (SizeCenter / 2);

                Lines[2].Position0X = Position.X - (SizeSides / 2);
                Lines[2].Position0Y = Position.Y - (SizeCenter / 2);
                Lines[2].Position1X = Position.X + (SizeSides / 2);
                Lines[2].Position1Y = Position.Y - (SizeCenter / 2);
            }

            public void Draw(RenderWindow window)
            {
                for (int i = 0; i < 3; i++)
                {
                    Lines[i].Draw(window);
                }
            }
        }
    }
}
