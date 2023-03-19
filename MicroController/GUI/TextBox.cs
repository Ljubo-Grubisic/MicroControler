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
        public string TextString
        {
            get { return Text.DisplayedString; }
            set { Text.DisplayedString = value; }
        }
        public Text Text;

        private int Index = 0;
        private Cursor Cursor;

        private bool Clicked = false;
        private uint id;
        private static uint counter = 0;

        public TextBox(Vector2f position, Vector2f size, uint fontSize, Font font) : base(position, size)
        {
            this.OutlineThickness = 2f;
            this.OutlineColor = Color.Black;
            this.Text = new Text("", font, fontSize);
            this.Text.Color = Color.Black;
            this.Text.Position = new Vector2f(PositionX, PositionY + ((SizeY - MessegeManager.GetTextRect("A", font, fontSize).Height) / 2));
            this.Cursor = new Cursor(position, Color.Black, 0.5f, fontSize + 2f, 12f);
            this.Cursor.Update(new Vector2f(MessegeManager.GetTextRect("", Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));

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
                Cursor.Draw(window);
            }
        }

        public void Update(Vector2i mousePos )
        {
            this.Text.Position = new Vector2f(PositionX, PositionY + ((SizeY - Text.GetLocalBounds().Height) / 2) - Text.GetLocalBounds().Top);

            UpdateIndexBounds();

            if (Index == TextString.Length && TextString.Length != 0)
            {
                this.Cursor.Update(new Vector2f(MessegeManager.GetTextRect(TextString, Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));
            }
            else if (TextString != "")
            {
                this.Cursor.Update(new Vector2f(MessegeManager.GetTextRect(TextString.Remove(Index), Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));
            }
            else if(Index == 0)
            {
                this.Cursor.Update(new Vector2f(MessegeManager.GetTextRect("", Text.Font, Text.CharacterSize).Width + PositionX + 3, SizeY / 2 + PositionY));
            }

            bool mouseState = MouseManager.OnMouseDown(Mouse.Button.Left, id);

            if (mouseState && Clicked)
            {
                this.Clicked = false;
            }
            if (IsMouseInBtn(mousePos) && mouseState)
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
                            this.TextString = this.TextString.Remove(Index - 1, 1);
                            Index--;
                        }
                        catch { }
                    }
                    if (input.Contains("Space"))
                    {
                        input = input = input.Replace("Space", " ");
                    }

                    this.TextString = this.TextString.Insert(Index, input);
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
            if (Index > TextString.Length && TextString.Length != 0)
            {
                Index = TextString.Length;
            }
        }

        private bool IsMouseInBtn(Vector2i mousePosition)
        {
            if (mousePosition.X > this.PositionX && mousePosition.X < this.PositionX + this.SizeX &&
                mousePosition.Y > this.PositionY && mousePosition.Y < this.PositionY + this.SizeY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
