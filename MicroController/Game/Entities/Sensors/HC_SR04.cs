using microController.graphics;
using SFML.Graphics;
using microController.helpers;
using SFML.Window;
using SFML.System;
using microController.system;

namespace microController.game.entities
{
    public class HC_SR04 : Entity
    {
        private float scale = 0.02f;

        private Rectangle Rectangle;
        private Texture SensorTexture;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public HC_SR04(Vector2f position, Game game) : base(game)
        {
            this.Position = position;

            this.SensorTexture = ImageHelper.LoadImgNoBackground("HC-SR04.jpg");
            this.Size = (Vector2f)SensorTexture.Size * Scale;

            this.Rectangle = new Rectangle(Position, Size, SensorTexture);
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
