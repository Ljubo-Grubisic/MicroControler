using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace microController.graphics
{
    public class Line : ConvexShape
    {
        private Vector2f[] position = new Vector2f[2];

        public new Vector2f[] Position
        {
            get { return position; }
            set 
            { 
                position = value;
                DataChanged();
            }
        }
        public Vector2f Position0
        {
            get { return position[0]; }
            set
            {
                position[0] = value;
                DataChanged();
            }
        }
        public float Position0X
        {
            get { return position[0].X; }
            set
            {
                position[0].X = value;
                DataChanged();
            }
        }
        public float Position0Y
        {
            get { return position[0].Y; }
            set
            {
                position[0].Y = value;
                DataChanged();
            }
        }

        public Vector2f Position1
        {
            get { return position[1]; }
            set
            {
                position[1] = value;
                DataChanged();
            }
        }
        public float Position1X
        {
            get { return position[1].X; }
            set
            {
                position[1].X = value;
                DataChanged();
            }
        }
        public float Position1Y
        {
            get { return position[1].Y; }
            set
            {
                position[1].Y = value;
                DataChanged();
            }
        }

        public Line(Vector2f positionDot1, Vector2f positionDot2)  : base(4)
        {
            this.Position[0] = positionDot1;
            this.Position[1] = positionDot2;
            this.OutlineThickness = 1f;
            DataChanged();
        }
        public Line() : base(4)
        {
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(this);
        }

        private void DataChanged()
        {
            this.SetPoint(0, position[0]);
            this.SetPoint(1, position[0]);
            this.SetPoint(2, position[1]);
            this.SetPoint(3, position[1]);
        }
    }
}
