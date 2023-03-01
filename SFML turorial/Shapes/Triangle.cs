using SFML.Graphics;
using SFML.System;
using MicroControler.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroControler.Mathematics;

namespace MicroControler.Shapes
{
    public class Triangle : ConvexShape
    {
        private Vector2f position;
        private Vector2f[] vertices = new Vector2f[3];

        public new Vector2f Position
        {
            get { return position; }
            set 
            { 
                position = value;
                UpdatePoints();
            }
        }
        public float PositionX
        {
            get { return position.X; }
            set
            {
                position.X = value;
                UpdatePoints();
            }
        }
        public float PositionY
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                UpdatePoints();
            }
        }
        public Vector2f[] Vertices
        {
            get { return vertices; }
            set
            {
                vertices = value;
                UpdatePoints();
            }
        }

        public Triangle(Vector2f[] vertices) : base(3)
        {
            if (vertices.Length != 3)
            {
                throw new Exception("Wrong Vertex data");
            }
            Position = vertices[0];
            Vertices = vertices; 
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X = Vertices[i].X - Position.X;
                Vertices[i].Y = Vertices[i].Y - Position.Y;
            }
        }
        public Triangle(Vector2f firstVertex, Vector2f secondVertex, Vector2f thirdVertex) : base(3)
        {
            Position = firstVertex;
            Vertices = new Vector2f[] { firstVertex, secondVertex, thirdVertex };
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X = Vertices[i].X - Position.X;
                Vertices[i].Y = Vertices[i].Y - Position.Y;
            }
        }
        public Triangle(float x, float y, float distance) : base(3)
        {
            this.Position = new Vector2f(x, y);
            this.Vertices = MathHelper.CalculateVerticiesEquilateral(distance);
        }
        public Triangle(Vector2f position, float distance) : base(3)
        {
            this.Position = new Vector2f(position.X, position.Y);
            this.Vertices = MathHelper.CalculateVerticiesEquilateral(distance);
        }
        public Triangle(float x, float y, float distance, float distanceIsceles)
        {
            this.Position = new Vector2f(x, y);
            this.Vertices = MathHelper.CalculateVerticiesIsosceles(distance, distanceIsceles);
        }
        public Triangle(Vector2f position, float distance, float distanceIsceles)
        {
            this.Position = new Vector2f(position.X, position.Y);
            this.Vertices = MathHelper.CalculateVerticiesIsosceles(distance, distanceIsceles);
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(this);
        }

        private void UpdatePoints()
        {
            this.SetPoint(0, Vertices[0] + Position);
            this.SetPoint(1, Vertices[1] + Position);
            this.SetPoint(2, Vertices[2] + Position);
        }
    }
}
