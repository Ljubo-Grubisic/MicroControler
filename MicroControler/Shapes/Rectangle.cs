using SFML.Graphics;
using SFML.System;

namespace MicroControler.Shapes
{
    public class Rectangle : ConvexShape
    {
        private Vector2f position;
        private float width;
        private float height;

        public new Vector2f Position
        {
            get { return position; }
            set
            {
                position = value;
                DataChanged();
            }
        }
        public float PositionX
        {
            get { return position.X; }
            set
            {
                position.X = value;
                DataChanged();
            }
        }
        public float PositionY
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                DataChanged();
            }
        }
        public float Width
        {
            get { return width; }
            set
            {
                width = value;
                DataChanged();
            }
        }
        public float Height
        {
            get { return height; }
            set
            {
                height = value;
                DataChanged();
            }
        }

        public Rectangle(Vector2f position, float width, float height) : base(4)
        {
            Position = position;
            Width = width;
            Height = height;
            DataChanged();
        }
        public Rectangle(Vector2f position, Vector2f size) : base(4)
        {
            Position = position;
            Width = size.X;
            Height = size.Y;
            DataChanged();
        }
        public Rectangle(float positionX, float positionY, Vector2f size) : base(4)
        {
            Position = new Vector2f(positionX, positionY);
            Width = size.X;
            Height = size.Y;
            DataChanged();
        }
        public Rectangle(float positionX, float positionY, float width, float height) : base(4)
        {
            Position = new Vector2f(positionX, positionY);
            Width = width;
            Height = height;
            DataChanged();
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(this);
        }

        private void DataChanged()
        {
            this.SetPoint(0, new Vector2f(PositionX, PositionY));
            this.SetPoint(1, new Vector2f(PositionX + Width, PositionY));
            this.SetPoint(3, new Vector2f(PositionX, PositionY + Height));
            this.SetPoint(2, new Vector2f(PositionX + Width, PositionY + Height));
        }
    }
}
