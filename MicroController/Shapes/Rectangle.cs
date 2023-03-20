using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace MicroController.Shapes
{
    public class Rectangle : RectangleShape
    {
        private Vector2f Buffer = new Vector2f(0, 0);
        public float PositionX
        {
            get { return Position.X; }
            set 
            {
                Buffer.X = value;
                Buffer.Y = PositionY;
                Position = Buffer; 
            }
        }
        public float PositionY
        {
            get { return Position.Y; }
            set 
            {
                Buffer.X = PositionX;
                Buffer.Y = value;
                Position = Buffer; 
            }
        }

        public float SizeX
        {
            get { return Size.X; }
            set 
            { 
                Buffer.X = value;
                Buffer.Y = SizeY;
                Size = Buffer; 
            }
        }
        public float SizeY
        {
            get { return Size.Y; }
            set 
            { 
                Buffer.X = SizeX;
                Buffer.Y = value;
                Size = Buffer; 
            }
        }

        public Rectangle(Vector2f position, Vector2f size) : base()
        { 
            this.Position = position;
            this.Size = size;
        }
        public Rectangle(Vector2f position, Vector2f size, Texture texture) : base()
        {
            this.Position = position;
            this.Size = size;
            this.Texture = texture;
        }
        public Rectangle(Vector2f position, float sizeX, float sizeY) : base()
        {
            this.Position = position;
            this.Size = new Vector2f(sizeX, sizeY);
        }
        public Rectangle(float positionX, float positionY, Vector2f size) : base()
        {
            this.Position = new Vector2f(positionX, positionY);
            this.Size = size;
        }
        public Rectangle(float positionX, float positionY, float sizeX, float sizeY) : base()
        {
            this.Position = new Vector2f(positionX, positionY);
            this.Size = new Vector2f(sizeX, sizeY);
        }

        public void Draw(RenderWindow window)
        {
            this.Draw(window, RenderStates.Default);
        }
    }
}
