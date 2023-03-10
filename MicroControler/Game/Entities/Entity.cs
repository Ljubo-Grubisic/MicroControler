using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroControler.Shapes;
using System.Data.SqlTypes;
using MicroControler.Game.RayCasting;

namespace MicroControler.Game.Entities
{
    public class Entity : RayCastEable
    {
        private Vector2f position;
        public float Rotation { get; set; } = -(float)Math.PI / 2;

        public virtual Vector2f Position
        {
            get { return position; }
            set { position = value; }
        }
        public virtual float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public virtual float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        protected virtual
    }
}
