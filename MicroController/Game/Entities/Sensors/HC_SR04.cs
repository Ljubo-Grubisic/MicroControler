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
        }

        protected override void OnUpdate(Map map, GameTime gameTime)
        {
            this.Size = this.SizeInCm * Scale.NumPixelPerCm;
            this.Rectangle.Size = this.Size;
            this.Rectangle.Position = this.DrawingPosition;
            this.Rectangle.Origin = new Vector2f(Rectangle.SizeX / 2, Rectangle.SizeY / 2);
            this.Rectangle.Rotation = MathHelper.RadiansToDegrees(this.Rotation) + 90;

            this.RayLine.Position0 = this.DrawingPosition;
            this.RayLine.Position1 = this.DrawingPosition + this.DeltaPosition;
        }

        public void CastRay(double distance, Map map)
        {
            this.DeltaPosition.X = (float)distance * Scale.NumPixelPerCm * (float)(Math.Cos(this.Rotation));
            this.DeltaPosition.Y = (float)distance * Scale.NumPixelPerCm * (float)(Math.Sin(this.Rotation));

            Vector2f DotPosition = new Vector2f(this.Position.X * Scale.NumPixelPerCm + DeltaPosition.X, this.Position.Y * Scale.NumPixelPerCm + DeltaPosition.Y);
            if ((int)DotPosition.Y / map.SquareSize > 0 && (int)DotPosition.Y / map.SquareSize < map.DataSize.X &&
                 (int)DotPosition.X / map.SquareSize > 0 && (int)DotPosition.X / map.SquareSize < map.DataSize.Y)
            {
                map.SetValueToData((int)DotPosition.Y / map.SquareSize, (int)DotPosition.X / map.SquareSize, 1);
            }
        }
    }
}
