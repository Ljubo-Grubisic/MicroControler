using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroController.Shapes;
using System.Data.SqlTypes;
using MicroController.Game.RayCasting;

namespace MicroController.Game.Entities
{
    public class Entity : RayCastEable
    {
        private Vector2f position;
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
    }
}
