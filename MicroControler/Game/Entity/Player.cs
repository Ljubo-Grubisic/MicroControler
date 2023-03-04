using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using MicroControler.Shapes;
using MicroControler.Mathematics;
using MicroControler.Game.RayCasting;
using MicroControler.Game.Maping;

namespace MicroControler.Game.Entity
{
    public class Player : RayCastEable
    {
        private Vector2f position;
        private Vector2f deltaPosition = new Vector2f(0, 0);

        public float Rotation { get; set; } = -(float)Math.PI/2;
        public float DeltaScale { get; set; } = 25;

        private Rectangle Rectangle { get; set; }
        private Line FacingLine { get; set; }
        
        public Vector2f Position
        {
            get { return position; }
            set
            {
                position = value;
                PositionChanged();
                DeltaPositionChanged();
            }
        }
        public float PositionX
        {
            get { return position.X; }
            set
            {
                position.X = value;
                PositionChanged();
                DeltaPositionChanged();
            }
        }
        public float PositionY
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                PositionChanged();
                DeltaPositionChanged();
            }
        }

        public Vector2f DeltaPosition
        {
            get { return deltaPosition; }
            set
            {
                deltaPosition = value;
                DeltaPositionChanged();
            }
        }
        public float DeltaPositionX
        {
            get { return deltaPosition.X; }
            set
            {
                deltaPosition.X = value;
                DeltaPositionChanged();
            }
        }
        public float DeltaPositionY
        {
            get { return deltaPosition.Y; }
            set
            {
                deltaPosition.Y = value;
                DeltaPositionChanged();
            }
        }

        static private Vector2f loadSize { get; } = new Vector2f(8f, 8f);

        public Player(Vector2f position)
        {
            this.position = position;
            this.deltaPosition.X = (float)(Math.Cos(this.Rotation));
            this.deltaPosition.Y = (float)(Math.Sin(this.Rotation));
            this.FacingLine = new Line(this.Position, MathHelper.AddDeltaPosition(this.Position, this.DeltaPosition, DeltaScale))
            {
                OutlineColor = Color.Yellow
            };
            this.Rectangle = new Rectangle(MathHelper.CenterRectangle(position, loadSize), loadSize.X, loadSize.Y)
            {
                FillColor = Color.Yellow
            };
        }
        public Player(float positionX, float positionY)
        {
            this.position = new Vector2f(positionX, positionY);
            this.deltaPosition.X = (float)(Math.Cos(this.Rotation));
            this.deltaPosition.Y = (float)(Math.Sin(this.Rotation));
            this.FacingLine = new Line(this.Position, MathHelper.AddDeltaPosition(this.Position, this.DeltaPosition, DeltaScale))
            {
                OutlineColor = Color.Yellow
            };
            this.Rectangle = new Rectangle(MathHelper.CenterRectangle(positionX, positionY, loadSize), loadSize.X, loadSize.Y)
            {
                FillColor = Color.Yellow
            };
        }

        public void Draw(RenderWindow window, Map map)
        {
            Rectangle.Position += (Vector2f)map.MapWindow.Position;
            FacingLine.Position0 += (Vector2f)map.MapWindow.Position;
            FacingLine.Position1 += (Vector2f)map.MapWindow.Position;
            window.Draw(this.Rectangle);
            window.Draw(this.FacingLine);
            FacingLine.Position0 -= (Vector2f)map.MapWindow.Position;
            FacingLine.Position1 -= (Vector2f)map.MapWindow.Position;
            Rectangle.Position -= (Vector2f)map.MapWindow.Position;
        }

        public void PositionChanged()
        {
            this.Rectangle.Position = MathHelper.CenterRectangle(this.Position, loadSize);
        }

        public void DeltaPositionChanged()
        {
            this.FacingLine.Position0 = this.Position;
            this.FacingLine.Position1 = MathHelper.AddDeltaPosition(this.Position, this.DeltaPosition, DeltaScale);
        }
    }
}
