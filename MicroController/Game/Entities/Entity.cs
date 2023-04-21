using SFML.Graphics;
using SFML.System;
using System;
using SFML.Window;
using microController.system;

namespace microController.game.entities
{
    public abstract class Entity
    {
        private Vector2f position;
        private Vector2f size;
        public Vector2f DrawingPosition;
        private float rotation = -(float)Math.PI / 2;

        public virtual Vector2f Position
        {
            get { return position; }
            set { position = value; OnPositionChange(); }
        }
        public virtual float PositionX
        {
            get { return position.X; }
            set { position.X = value; OnPositionChange(); }
        }
        public virtual float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; OnPositionChange(); }
        }

        public virtual Vector2f Size
        {
            get { return size; }
            set { size = value; OnSizeChange(); }
        }
        public virtual float SizeX
        {
            get { return size.X; }
            set { size.X = value; OnSizeChange(); }
        }
        public virtual float SizeY
        {
            get { return size.Y; }
            set { size.Y = value; OnSizeChange(); }
        }

        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; OnRotationChange(); }
        }

        private bool MouseWheelScrolled;

        protected Entity(Game game)
        {
            game.Window.MouseWheelMoved += OnMouseWheelScroll;
        }

        protected virtual void OnPositionChange() { }
        protected virtual void OnRotationChange() { }
        protected virtual void OnSizeChange() { }

        public void Update(Map map, GameTime gameTime)
        {
            UpdateDrawingPosition(map);

            OnUpdate(map, gameTime);
        }

        public abstract void Draw(RenderWindow window);

        protected abstract void OnUpdate(Map map, GameTime gameTime);

        private void UpdateDrawingPosition(Map map)
        {
            if (MouseWheelScrolled)
            {
                this.position = (this.position / map.OldSquareSize) * map.SquareSize;
                MouseWheelScrolled = false;
            }
            this.DrawingPosition.X = this.position.X + map.Window.Position.X - map.SquareStarting.Y * map.SquareSize;
            this.DrawingPosition.Y = this.position.Y + map.Window.Position.Y - map.SquareStarting.X * map.SquareSize;
        }

        private void OnMouseWheelScroll(object sender, MouseWheelEventArgs e)
        {
            this.MouseWheelScrolled = true;
        }
    }
}
