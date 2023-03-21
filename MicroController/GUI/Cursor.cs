using MicroController.Shapes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroController.GUI
{
    public class Cursor
    {
        public Vector2f Position;
     
        private Line[] Lines = new Line[3];
        public float SizeCenter;
        public float SizeSides;

        public Cursor(Vector2f position, Color color, float outlineThickness, float sizeCenter, float sizeSides)
        {
            this.Position = position;
            this.SizeCenter = sizeCenter;
            this.SizeSides = sizeSides;

            for (int i = 0; i < 3; i++)
            {
                Lines[i] = new Line();
                Lines[i].OutlineColor = color;
                Lines[i].OutlineThickness = outlineThickness;
            }
            Lines[0].Position0 = new Vector2f(Position.X, Position.Y - (SizeCenter / 2));
            Lines[0].Position1 = new Vector2f(Position.X, Position.Y + (SizeCenter / 2));

            Lines[1].Position0 = new Vector2f(Position.X - (SizeSides / 2), Position.Y + (SizeCenter / 2));
            Lines[1].Position1 = new Vector2f(Position.X + (SizeSides / 2), Position.Y + (SizeCenter / 2));

            Lines[2].Position0 = new Vector2f(Position.X - (SizeSides / 2), Position.Y - (SizeCenter / 2));
            Lines[2].Position1 = new Vector2f(Position.X + (SizeSides / 2), Position.Y - (SizeCenter / 2));
        }

        public void Update(Vector2f positon)
        {
            Position = positon;
            Lines[0].Position0X = Position.X;
            Lines[0].Position0Y = Position.Y - (SizeCenter / 2);
            Lines[0].Position1X = Position.X;
            Lines[0].Position1Y = Position.Y + (SizeCenter / 2);

            Lines[1].Position0X = Position.X - (SizeSides / 2);
            Lines[1].Position0Y = Position.Y + (SizeCenter / 2);
            Lines[1].Position1X = Position.X + (SizeSides / 2);
            Lines[1].Position1Y = Position.Y + (SizeCenter / 2);

            Lines[2].Position0X = Position.X - (SizeSides / 2);
            Lines[2].Position0Y = Position.Y - (SizeCenter / 2);
            Lines[2].Position1X = Position.X + (SizeSides / 2);
            Lines[2].Position1Y = Position.Y - (SizeCenter / 2);
        }

        public void Draw(RenderWindow window)
        {
            for (int i = 0; i < 3; i++)
            {
                Lines[i].Draw(window);
            }
        }
    }
}
