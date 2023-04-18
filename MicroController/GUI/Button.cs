using MicroController.InputOutput;
using MicroController.Shapes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Runtime.InteropServices;
using MicroController.Mathematics;
using SFML.Window;

namespace MicroController.GUI
{
    public class Button : Rectangle
    {
        public string DisplayedString { get => Text.DisplayedString; set => Text.DisplayedString = value; }
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

        public Color TextColor;
        public Color BaseTextColor = new Color(Color.Black);
        public Color TextColorOnHover = new Color(Color.White);
        public Color TextColorOnClick = new Color(Color.Blue);

        public delegate void ButtonEventHandler(Button source, EventArgs args);

        // Logic events
        public event ButtonEventHandler ButtonClicked;
        public event ButtonEventHandler ButtonPressed;
        public event ButtonEventHandler ButtonHovering;

        // Animation events
        public event ButtonEventHandler ButtonDefaultAnimation;
        public event ButtonEventHandler ButtonPressedAnimation;
        public event ButtonEventHandler ButtonHoveringAnimation;

        private bool Lock;

        public Button(Vector2f position, Vector2f size, string text) : base(position, size)
        {
            this.OutlineColor = Color.Black;
            this.OutlineThickness = 2f;
            this.TextColor = BaseTextColor;
            this.Text = new Text(text, MessegeManager.Courier, 14);
        }
        public Button(Vector2f position, Vector2f size, string text, Font font, uint fontSize) : base(position, size)
        {
            this.OutlineColor = Color.Black;
            this.OutlineThickness = 2f;
            this.TextColor = BaseTextColor;
            this.Text = new Text(text, font, fontSize);
        }

        public new void Draw(RenderWindow window)
        {
            base.Draw(window);
            window.Draw(Text);
        }

        public void Update(Vector2i mousePos)
        {
            bool mouseState = MouseManager.IsButtonDown(Mouse.Button.Left);
            Text.Position = MathHelper.CenterTextInRectangle(Position, Size, MessegeManager.GetTextRect(DisplayedString, MessegeManager.Courier, Text.CharacterSize));
            Text.Color = TextColor;
            OnButtonDefalutAnimation();
            if (MathHelper.IsMouseInRectangle(this, mousePos))
            {
                OnButtonHoveringAnimation();
                OnButtonHovering();
            }
            if (MathHelper.IsMouseInRectangle(this, mousePos) && mouseState)
            {
                OnButtonPressedAnimation();
                OnButtonPressed();
                if (Lock)
                {
                    Lock = false;
                    OnButtonClicked();
                }
            }
            else
            {
                Lock = true;
            }
        }
        public void Update(Vector2i mousePos, bool mouseState)
        {
            Text.Position = MathHelper.CenterTextInRectangle(Position, Size, MessegeManager.GetTextRect(DisplayedString, MessegeManager.Courier, Text.CharacterSize));
            Text.Color = TextColor;
            OnButtonDefalutAnimation();
            if (MathHelper.IsMouseInRectangle(this, mousePos))
            {
                OnButtonHoveringAnimation();
                OnButtonHovering();
            }
            if (MathHelper.IsMouseInRectangle(this, mousePos) && mouseState)
            {
                OnButtonPressedAnimation();
                OnButtonPressed();
                if (Lock)
                {
                    Lock = false;
                    OnButtonClicked();
                }
            }
            else
            {
                Lock = true;
            }
        }

        #region Logic Functions
        protected virtual void OnButtonClicked()
        {
            if (ButtonClicked != null)
                ButtonClicked(this, EventArgs.Empty);
        }
        protected virtual void OnButtonPressed()
        {
            if (ButtonPressed != null)
                ButtonPressed(this, EventArgs.Empty);
        }
        protected virtual void OnButtonHovering()
        {
            if (ButtonHovering != null)
                ButtonHovering(this, EventArgs.Empty);
        }
        #endregion

        #region Animation Functions
        protected virtual void OnButtonDefalutAnimation()
        {
            if (ButtonDefaultAnimation != null)
                ButtonDefaultAnimation(this, EventArgs.Empty);
            else
                DefaultAnimation();
        }
        protected virtual void OnButtonPressedAnimation()
        {
            if(ButtonPressedAnimation != null)
                ButtonPressedAnimation(this, EventArgs.Empty);
            else
                PressedAnimation();
        }
        protected virtual void OnButtonHoveringAnimation()
        {
            if(ButtonHoveringAnimation != null)
                ButtonHoveringAnimation(this, EventArgs.Empty);
            else
                HoveringAnimation();
        }
        #endregion

        #region Default animations
        protected virtual void PressedAnimation()
        {
            this.FillColor = Color.White;
            this.OutlineColor = Color.Blue;
            this.TextColor = this.TextColorOnClick;
        }
        protected virtual void HoveringAnimation()
        {
            this.FillColor = Color.Black;
            this.TextColor = this.TextColorOnHover;
        }
        protected virtual void DefaultAnimation()
        {
            this.FillColor = Color.White;
            this.OutlineColor = Color.Black;
            this.TextColor = Color.Black;
        }
        #endregion
    }
}
