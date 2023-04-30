using microController.graphics;
using SFML.Graphics;
using microController.helpers;
using SFML.Window;
using SFML.System;
using microController.system;
using System;

namespace microController.game.entities
{
    public class HC_SR04 : Entity
    {
        private Vector2f DeltaPosition = new Vector2f();

        private Rectangle Rectangle;
        private Texture SensorTexture;
        private Line RayLine;

        private int RayCounter = 100;
        private Vector2f SizeInCm = new Vector2f(5, 2);

        public HC_SR04(Vector2f position, Game game) : base(game)
        {
            this.Position = position;

            this.SensorTexture = ImageHelper.LoadImgNoBackground("HC-SR04.jpg");
            this.Size = this.SizeInCm * Scale.NumPixelPerCm;

            this.Rectangle = new Rectangle(Position, Size, SensorTexture);
            this.RayLine = new Line(this.Position, this.DeltaPosition) { OutlineThickness = 2f, OutlineColor = Color.Red };
        }

        public override void Draw(RenderWindow window)
        {
            this.Rectangle.Draw(window);
            if (RayCounter < 50)
                this.RayLine.Draw(window);
        }

        protected override void OnUpdate(Map map, GameTime gameTime)
        {
            this.Size = this.SizeInCm * Scale.NumPixelPerCm;
            this.Rectangle.Size = this.Size;
            this.Rectangle.Position = this.DrawingPosition;
            this.Rectangle.Origin = new Vector2f(Rectangle.SizeX / 2, Rectangle.SizeY / 2);
            this.Rectangle.Rotation = MathHelper.RadiansToDegrees(this.Rotation) + 90;
            if (RayCounter < 50)
            {
                this.RayLine.Position0 = this.DrawingPosition;
                this.RayLine.Position1 = this.DrawingPosition + this.DeltaPosition;
                RayCounter++;
            }
        }

        public void CastRay(double distance, Map map)
        {
            this.DeltaPosition.X = (float)distance * (float)(Math.Cos(this.Rotation));
            this.DeltaPosition.Y = (float)distance * (float)(Math.Sin(this.Rotation));

            Vector2f DotPosition = new Vector2f(this.Position.X + DeltaPosition.X, this.Position.Y + DeltaPosition.Y);
            map.SetValueToData((int)DotPosition.Y / map.SquareSize, (int)DotPosition.X / map.SquareSize, 1);
            RayCounter = 0;
        }
    }
}
