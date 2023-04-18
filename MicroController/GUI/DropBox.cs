using MicroController.Mathematics;
using MicroController.Shapes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using MicroController.InputOutput;
using System.Runtime.Remoting.Messaging;

namespace MicroController.GUI
{
    public class DropBox
    {
        /// <summary>
        /// The displayed strings for each option
        /// </summary>
        private List<string> displayedStrings;
        /// <summary>
        /// The index of the option that is selected
        /// </summary>
        public uint Index;
        /// <summary>
        /// The position of the top right corner of the DropBox
        /// </summary>
        private Vector2f position;
        /// <summary>
        /// The size of the DropBox and the items size
        /// </summary>
        private Vector2f size;
        /// <summary>
        /// Text class object that will draw the text on the screen
        /// </summary>
        public Text Text;

        private Arrow DropBoxArrow;
        private Button DropBoxButton;
        private List<Button> DropBoxItemsButton = new List<Button>();
        private List<string> DisplayedStringsBuffer = new List<string>();

        #region Getters and Setters
        /// <summary>
        /// Character size of the text
        /// </summary>
        public uint CharacterSize { get => Text.CharacterSize; set => Text.CharacterSize = value; }
        /// <summary>
        /// Font of the text
        /// </summary>
        public Font TextFont { get => Text.Font; set => Text.Font = value; }
        /// <summary>
        /// The position of the top right corner of the DropBox
        /// </summary>
        public Vector2f Position
        {
            get => position;
            set
            {
                position = value;
                DropBoxButton.Position = position;
                UpdateDropBoxItemsButtonsPosition();
            }
        }
        /// <summary>
        /// The x component of the position of the top right corner of the DropBox
        /// </summary>
        public float PositionX
        {
            get => position.X;
            set
            {
                position.X = value;
                DropBoxButton.Position = position;
                UpdateDropBoxItemsButtonsPosition();
            }
        }
        /// <summary>
        /// The y component of the position of the top right corner of the DropBox
        /// </summary>
        public float PositionY
        {
            get => position.Y;
            set
            {
                position.Y = value;
                DropBoxButton.Position = position;
                UpdateDropBoxItemsButtonsPosition();
            }
        }
        /// <summary>
        /// The size of the DropBox and the items size
        /// </summary>
        public Vector2f Size
        {
            get => size;
            set
            {
                size = value;
                DropBoxButton.Size = size;
                UpdateDropBoxItemsButtonsPosition();
            }
        }
        /// <summary>
        /// The x component of the size of the DropBox and the items size
        /// </summary>
        public float SizeX
        {
            get => size.X;
            set
            {
                size.X = value;
                DropBoxButton.Size = size;
                UpdateDropBoxItemsButtonsPosition();
            }
        }
        /// <summary>
        /// The y component of the size of the DropBox and the items size
        /// </summary>
        public float SizeY
        {
            get => size.Y;
            set
            {
                size.Y = value;
                DropBoxButton.Size = size;
                UpdateDropBoxItemsButtonsPosition();
            }
        }
        /// <summary>
        /// The displayed strings for each option
        /// </summary>
        public List<string> DisplayedStrings
        {
            get => displayedStrings;
            set
            {
                displayedStrings = value;
                DropBoxItemsButton.Clear();
                AddDropBoxItems();
                DisplayedStringsBuffer = value;
            }
        }
              
        #endregion

        private bool Lock = false;

        /// <summary>
        /// DropBox constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="displayedStrings"></param>
        public DropBox(Vector2f position, Vector2f size, List<string> displayedStrings)
        {
            this.position = position;
            this.size = size;
            this.displayedStrings = displayedStrings;
            foreach (string displayedString in this.displayedStrings)
            {
                DisplayedStringsBuffer.Add(displayedString);
            }

            this.Text = new Text("", MessegeManager.Courier, 14);

            DropBoxButton = new Button(this.Position, this.Size, "", TextFont, CharacterSize);

            DropBoxArrow = new Arrow(new Vector2f(), 20, Arrow.ArrowState.Down) { OutlineThickness = 3f };
            DropBoxArrow.PositionX = this.Position.X + this.Size.X - DropBoxArrow.DeltaSize.X - 2;
            DropBoxArrow.PositionY = this.Position.Y + (this.Size.Y / 2) + (DropBoxArrow.DeltaSize.Y / 2);

            for (int i = 0; i < DisplayedStrings.Count; i++)
            {
                this.DropBoxItemsButton.Add(new Button(this.Position + new Vector2f(0, this.Size.Y * (i + 1)), this.Size, DisplayedStrings[i], TextFont, CharacterSize));
            }
        }

        public void Update(Vector2i mousePos)
        {
            bool mouseState = MouseManager.IsButtonDown(Mouse.Button.Left);

            DropBoxButton.Update(mousePos);
            if (MathHelper.IsMouseInRectangle(this.DropBoxButton, mousePos) && mouseState)
            {
                if (Lock)
                {
                    if (DropBoxArrow.State == Arrow.ArrowState.Up)
                    {
                        DropBoxArrow.State = Arrow.ArrowState.Down;
                    }
                    else
                    {
                        DropBoxArrow.State = Arrow.ArrowState.Up;
                    }
                    Lock = false;
                }
            }
            else
            {
                Lock = true;
            }

            if (DropBoxArrow.State == Arrow.ArrowState.Up)
            {
                DropBoxArrow.PositionX = this.Position.X + this.Size.X - DropBoxArrow.DeltaSize.X - 2;
                DropBoxArrow.PositionY = this.Position.Y + (this.Size.Y / 2) - (DropBoxArrow.DeltaSize.Y / 2);

                if (MathHelper.IsMouseInRectangle(this.Position + new Vector2f(0, this.Size.Y), this.Size + new Vector2f(0, this.Size.Y * (DisplayedStrings.Count - 1)), mousePos) && mouseState)
                {
                    if ((mousePos.Y - (this.Position.Y + this.Size.Y)) / this.Size.Y % 1 != 0)
                    {
                        Index = (uint)((mousePos.Y - (this.Position.Y + this.Size.Y)) / this.Size.Y) + 1;
                        DropBoxArrow.State = Arrow.ArrowState.Down;
                    }
                }

                foreach (Button button in DropBoxItemsButton)
                {
                    button.Update(mousePos);
                }
            }
            if (DropBoxArrow.State == Arrow.ArrowState.Down)
            {
                DropBoxArrow.PositionX = this.Position.X + this.Size.X - DropBoxArrow.DeltaSize.X - 2;
                DropBoxArrow.PositionY = this.Position.Y + (this.Size.Y / 2) + (DropBoxArrow.DeltaSize.Y / 2);
            }


            for (int i = 0; i < DisplayedStrings.Count; i++)
            {
                if (DisplayedStrings[i] != DisplayedStringsBuffer[i])
                {
                    DisplayedStringsChanged();
                }
            }
            if (DisplayedStrings.Count != DisplayedStringsBuffer.Count)
            {
                DisplayedStringsChanged();
            }

            if (Index != 0)
            {
                this.DropBoxButton.DisplayedString = DisplayedStrings[(int)Index - 1];
            }
        }

        public void Draw(RenderWindow window)
        {
            DropBoxButton.Draw(window);
            DropBoxArrow.Draw(window);
            if (DropBoxArrow.State == Arrow.ArrowState.Up)
            {
                foreach (Button button in DropBoxItemsButton)
                {
                    button.Draw(window);
                }
            }
        }

        private void DisplayedStringsChanged()
        {
            AddDropBoxItems();
            DisplayedStringsBuffer.Clear();
            foreach (string displayedString in this.DisplayedStrings)
            {
                DisplayedStringsBuffer.Add(displayedString);
            }
        }

        private void AddDropBoxItems()
        {
            for (int i = 0; i < DisplayedStrings.Count; i++)
            {
                this.DropBoxItemsButton.Add(new Button(this.Position + new Vector2f(0, this.Size.Y * (i + 1)), this.Size, DisplayedStrings[i], TextFont, CharacterSize));
            }
        }

        private void UpdateDropBoxItemsButtonsPosition()
        {
            for (int i = 0; i < DropBoxItemsButton.Count; i++)
            {
                DropBoxItemsButton[i].Position = this.Position + new Vector2f(0, this.Size.Y * (i + 1));
                DropBoxItemsButton[i].Size = this.Size;
            }
        }

        private class Arrow
        {
            private Vector2f position;
            private float size;
            private ArrowState state;

            private Color color = Color.Black;
            private float outlineThickness = 2.0f;

            public Vector2f DeltaSize { get; private set; }

            public Vector2f Position
            {
                get => position;
                set
                {
                    position = value;
                    OnSizePositionStateChange();
                }
            }
            public float PositionX
            {
                get => position.X;
                set
                {
                    position.X = value;
                    OnSizePositionStateChange();
                }
            }
            public float PositionY
            {
                get => position.Y;
                set
                {
                    position.Y = value;
                    OnSizePositionStateChange();
                }
            }
            public float Size
            {
                get => size;
                set
                {
                    size = value;
                    OnSizePositionStateChange();
                }
            }
            public ArrowState State
            {
                get => state;
                set
                {
                    state = value;
                    OnSizePositionStateChange();
                }
            }
            public Color Color
            {
                get => color;
                set
                {
                    color = value;
                    lines[0].OutlineColor = color;
                    lines[1].OutlineColor = color;
                }
            }
            public float OutlineThickness
            {
                get => outlineThickness;
                set
                {
                    outlineThickness = value;
                    lines[0].OutlineThickness = outlineThickness;
                    lines[1].OutlineThickness = outlineThickness;
                }
            }

            private Line[] lines = new Line[2];

            public Arrow(Vector2f position, float size, ArrowState state)
            {
                this.position = position;
                this.size = size;
                this.state = state;

                lines[0] = new Line(this.Position, this.Position) { OutlineThickness = outlineThickness, OutlineColor = Color };
                lines[1] = new Line(this.Position, this.Position) { OutlineThickness = outlineThickness, OutlineColor = Color };
                this.DeltaSize = new Vector2f(size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                if (this.State == ArrowState.Up)
                {
                    lines[0].Position1 += new Vector2f(-size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                    lines[1].Position1 += new Vector2f(size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                }
                else
                {
                    lines[0].Position1 += new Vector2f(-size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), -size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                    lines[1].Position1 += new Vector2f(size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), -size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                }
            }

            public void Draw(RenderWindow window)
            {
                lines[0].Draw(window);
                lines[1].Draw(window);
            }

            private void OnSizePositionStateChange()
            {
                lines[0].Position0 = this.Position;
                lines[1].Position0 = this.Position;
                this.DeltaSize = new Vector2f(size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));

                if (this.State == ArrowState.Up)
                {
                    lines[0].Position1 = this.Position + new Vector2f(-size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                    lines[1].Position1 = this.Position + new Vector2f(size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                }
                else
                {
                    lines[0].Position1 = this.Position + new Vector2f(-size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), -size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                    lines[1].Position1 = this.Position + new Vector2f(size * (float)Math.Cos(MathHelper.DegreesToRadians(45)), -size * (float)Math.Sin(MathHelper.DegreesToRadians(45)));
                }
            }

            public enum ArrowState
            {
                Up, 
                Down
            }
        }
    }
}
