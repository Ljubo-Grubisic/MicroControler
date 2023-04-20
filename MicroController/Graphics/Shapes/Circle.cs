using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MicroController.Graphics
{
    public class Circle : CircleShape
    {
        Vector2f Buffer = new Vector2f();
        public float PositionX
        {
            get { return Position.X; }
            set
            {
                Buffer.X = value;
                Buffer.Y = Position.Y;
                Position = Buffer;
            }
        }
        public float PositionY
        {
            get { return Position.Y; }
            set
            {
                Buffer.X = Position.X;
                Buffer.Y = value;
                Position = Buffer;
            }
        }

        public Circle(Vector2f position, float radius, uint pointCount = 100) : base(radius, pointCount)
        { 
            this.Position = position;
        }
        public Circle(float positionX, float positionY, float radius, uint pointCount = 100) : base(radius, pointCount)
        {
            this.Position = new Vector2f(positionX, positionY);
        }

        public void Draw(RenderWindow window)
        {
            base.Draw(window, RenderStates.Default);
        }
    }
}
