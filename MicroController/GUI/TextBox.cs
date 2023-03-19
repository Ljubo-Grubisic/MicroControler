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
        private int OldTextLenght = 0;

        public TextBox(Vector2f position, Vector2f size, uint fontSize, Font font) : base(position, size)
        {
            this.OutlineThickness = 2f;
            this.OutlineColor = Color.Black;
            this.Text = new Text("Te", font, fontSize);
            this.Text.Color = Color.Black;
            this.Text.Position = new Vector2f(PositionX, PositionY + ((SizeY - Text.GetLocalBounds().Height) / 2));
            this.Cursor = new Cursor(position, Color.Black, 1f, fontSize + 2f, 12f);

            this.id = counter += 3;
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
            UpdateIndex();
            this.Text.Position = new Vector2f(PositionX, PositionY + ((SizeY - Text.GetLocalBounds().Height) / 2) - Text.GetLocalBounds().Top);
            
            if (Index == TextString.Length - 1)
            {
                this.Cursor.Update(new Vector2f(MessegeManager.GetTextRect(TextString, Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));
            }
            else if(TextString != "")
            {
                this.Cursor.Update(new Vector2f(MessegeManager.GetTextRect(TextString.Remove(Index + 1), Text.Font, Text.CharacterSize).Width + PositionX + 1, SizeY / 2 + PositionY));
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

            if(Clicked)
            {
                UpdateIndex();
                this.TextString = KeyboardManager.ReadInput(TextString);
                
                if (this.TextString.Contains("BackSpace"))
                {
                    this.TextString = this.TextString.Replace("BackSpace", "");
                    try
                    {
                        this.TextString = this.TextString.Remove(Index, 1);
                        OldTextLenght = TextString.Length;
                    }
                    catch{}
                }
                if (TextString.Length > OldTextLenght)
                {
                    Index = TextString.Length;
                    OldTextLenght = TextString.Length;
                }
            }
        }

        private void UpdateIndex()
        {
            if (KeyboardManager.OnKeyPressTextBoxOnly(Keyboard.Key.Left, (int)id + 1))
            {
                Index--;
            }
            if (KeyboardManager.OnKeyPressTextBoxOnly(Keyboard.Key.Right, (int)id + 2))
            {
                Index++;
            }
            if (Index < 0)
            {
                Index = 0;
            }
            if (Index > TextString.Length - 1 && TextString.Length != 0)
            {
                Index = TextString.Length - 1;
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
