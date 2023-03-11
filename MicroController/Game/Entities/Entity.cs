using SFML.Graphics;
using SFML.System;
using System;
using MicroController.Game.Maping;

namespace MicroController.Game.Entities
{
    public class Entity
    {
        private Vector2f position;
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

        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; OnRotationChange(); }
        }

        protected virtual void OnPositionChange() { }
        protected virtual void OnRotationChange() { }

        protected void Update(Map map)
        {
            UpdateDrawingPosition(map);
        }

        private void UpdateDrawingPosition(Map map)
        {
            this.DrawingPosition.X = this.Position.X + map.MapWindow.Position.X - map.SquareStarting.Y * map.SquareSize;
            this.DrawingPosition.Y = this.Position.Y + map.MapWindow.Position.Y - map.SquareStarting.X * map.SquareSize;
        }
    }
}
