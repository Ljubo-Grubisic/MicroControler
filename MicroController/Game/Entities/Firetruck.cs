using MicroController.Mathematics;
using MicroController.SFMLHelper;
using MicroController.Shapes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroController.Game.Entities
{
    public class Firetruck : Entity
    {
        private float scale = 0.02f;
        private Vector2f SizeInCm = new Vector2f(15, 26);

        private Rectangle Rectangle;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Firetruck(Vector2f position, Game game) : base(game)
        {
            this.Position = position;

            this.Size = SizeInCm * Scale;

            this.Rectangle = new Rectangle(Position, Size) { FillColor = Color.Red };
        }

        public void Draw(RenderWindow window)
        {
            this.Rectangle.Draw(window);
        }

        public new void Update(Map map)
        {
            base.Update(map);

            this.Rectangle.Position = this.DrawingPosition;
            this.Rectangle.Origin = new Vector2f(Rectangle.SizeX / 2, Rectangle.SizeY / 2);
            this.Rectangle.Rotation = MathHelper.RadiansToDegrees(this.Rotation) + 180;
        }
    }
}
