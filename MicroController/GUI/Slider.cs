using MicroController.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using MicroController.Mathematics;

namespace MicroController.GUI
{
    public class Slider
    {
        private Vector2f position;

        public float Value;
        /// <summary>
        /// From X to Y
        /// </summary>
        public Vector2f Scale;
        public Vector2f Size;
        /// <summary>
        /// The default value the circle will be on
        /// </summary>
        public float DefaultValue;

        public Rectangle Rectangle;
        public Circle Circle;

        public Color ColorLeft = Color.Black;
        public Color ColorRight = Color.White;

        public Vector2f Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
        public float PositionX
        {
            get { return position.X; }
            set
            {
                position.X = value;
            }
        }
        public float PositionY
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
            }
        }

        public Slider(Vector2f position, Vector2f size, Vector2f scale, float defaultValue, float radius)
        {
            this.position = position;
            this.Scale = scale;
            this.Size = size;
            this.DefaultValue = defaultValue;

            this.Rectangle = new Rectangle(Position, Size) { OutlineThickness = 2f, OutlineColor = Color.Black };

            float x = MathHelper.Map(Scale, new Vector2f(PositionX, PositionX + Size.X), Value);
            float y = Size.Y / 2 + Position.Y;

            this.Circle = new Circle(MathHelper.CenterRectangle(x + PositionX, y, radius*2, radius*2), radius) 
            { OutlineThickness = 2f, OutlineColor = Color.Black };
        }

        public void Draw(RenderWindow window)
        {
            Rectangle.Draw(window);
            Circle.Draw(window);
        }
    }
}
