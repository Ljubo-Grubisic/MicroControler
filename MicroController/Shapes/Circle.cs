using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MicroController.Shapes
{
    public class Circle : CircleShape
    {
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
