using microController.helpers;
using microController.graphics;
using microController.system;
using SFML.Graphics;
using SFML.System;

namespace microController.game.entities
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

        public override void Draw(RenderWindow window)
        {
            this.Rectangle.Draw(window);
        }

        protected override void OnUpdate(Map map, GameTime gameTime)
        {
            this.Rectangle.Position = this.DrawingPosition;
            this.Rectangle.Origin = new Vector2f(Rectangle.SizeX / 2, Rectangle.SizeY / 2);
            this.Rectangle.Rotation = MathHelper.RadiansToDegrees(this.Rotation) + 180;
        }
    }
}
