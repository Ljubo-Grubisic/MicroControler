using MicroController.InputOutput;
using MicroController.Shapes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Runtime.InteropServices;
using MicroController.Mathematics;

namespace MicroController.GUI
{
    public class Button
    {
        public string Text;
        public uint FontSize = 14;
        public Rectangle Rectangle;

        public Color TextColor;
        public Color BaseTextColor = new Color(Color.Black);
        public Color TextColorOnHover = new Color(Color.White);
        public Color TextColorOnClick = new Color(Color.Blue);

        public delegate void ButtonEventHandler(object source, EventArgs args);

        // Logic events
        public event ButtonEventHandler ButtonClicked;
        public event ButtonEventHandler ButtonPressed;
        public event ButtonEventHandler ButtonHovering;

        // Animation events
        public event ButtonEventHandler ButtonDefaultAnimation;
        public event ButtonEventHandler ButtonPressedAnimation;
        public event ButtonEventHandler ButtonHoveringAnimation;

        private bool Lock;

        public Button(Vector2f position, Vector2f size, string text)
        {
            this.Rectangle = new Rectangle(position, size) { OutlineColor = Color.Black, OutlineThickness = 2f };
            this.Text = text;
            this.TextColor = BaseTextColor;
        }

        public void Draw(RenderWindow window)
        {
            Rectangle.Draw(window);
            MessegeManager.Message(window, Text, MessegeManager.Courier, MathHelper.CenterTextInRectangle(Rectangle.Position, Rectangle.Size, 
                MessegeManager.GetTextRect(Text, MessegeManager.Courier, FontSize, window), window), TextColor, FontSize);
        }

        public void Update(Vector2i mousePos, bool mouseState)
        {
            OnButtonDefalutAnimation();
            if (IsMouseInBtn(mousePos))
            {
                OnButtonHoveringAnimation();
                OnButtonHovering();
            }
            if (IsMouseInBtn(mousePos) && mouseState)
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
            this.Rectangle.FillColor = Color.White;
            this.Rectangle.OutlineColor = Color.Blue;
            this.TextColor = this.TextColorOnClick;
        }
        protected virtual void HoveringAnimation()
        {
            this.Rectangle.FillColor = Color.Black;
            this.TextColor = this.TextColorOnHover;
        }
        protected virtual void DefaultAnimation()
        {
            this.Rectangle.FillColor = Color.White;
            this.Rectangle.OutlineColor = Color.Black;
            this.TextColor = Color.Black;
        }
        #endregion
        
        private bool IsMouseInBtn(Vector2i mousePosition)
        {
            if (mousePosition.X > Rectangle.PositionX && mousePosition.X < Rectangle.PositionX + Rectangle.SizeX &&
                mousePosition.Y > Rectangle.PositionY && mousePosition.Y < Rectangle.PositionY + Rectangle.SizeY)
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
