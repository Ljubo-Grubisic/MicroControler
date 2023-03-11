using MicroController.Mathematics;
using MicroController.Shapes;
using SFML.System;
using SFML.Graphics;
using System;
using MicroController.GameLooping;
using MicroController.InputOutput;
using SFML.Window;
using MicroController.Game.Maping;
using MicroController.SFMLHelper;
using System.Data;

namespace MicroController.Game.Entities
{
    public class Camera : Entity
    {
        public float Speed = 150;
        public Vector2f Size { get; set; }

        private Vector2f DeltaPosition;
        private Rectangle Rectangle { get; set; }

        private readonly float Scale = 0.20f;

        public Camera(Vector2f position)
        {
            this.Position = position;
            
            this.DeltaPosition.X = (float)(Math.Cos(this.Rotation));
            this.DeltaPosition.Y = (float)(Math.Sin(this.Rotation));

            Texture texture = ImageHelper.LoadPngNoBackground("Camera.png");
            this.Size = (Vector2f)texture.Size * Scale;

            this.Rectangle = new Rectangle(Position, Size, texture);
        }

        public void Draw(RenderWindow window)
        {
            this.Rectangle.Draw(window);
        }

        public void Update(GameTime gameTime, Map map)
        {
            base.Update(map);

            this.Rectangle.Position = this.DrawingPosition;
            this.Rectangle.Origin = new Vector2f(Rectangle.SizeX / 2, Rectangle.SizeY / 2);
            this.Rectangle.Rotation = MathHelper.RadiansToDegrees(this.Rotation) + 180;

            if (KeyboardManager.IsKeyPressed(Keyboard.Key.A))
            {
                this.Rotation -= 0.05f * gameTime.DeltaTime * 75;
                if (this.Rotation < 0)
                {
                    this.Rotation += 2f * (float)Math.PI;
                }
                this.DeltaPosition.X = (float)(Math.Cos(this.Rotation));
                this.DeltaPosition.Y = (float)(Math.Sin(this.Rotation));
            }
            if (KeyboardManager.IsKeyPressed(Keyboard.Key.D))
            {
                this.Rotation += 0.05f * gameTime.DeltaTime * 75;
                if (this.Rotation > 2f * (float)Math.PI)
                {
                    this.Rotation -= 2f * (float)Math.PI;
                }
                this.DeltaPosition.X = (float)(Math.Cos(this.Rotation));
                this.DeltaPosition.Y = (float)(Math.Sin(this.Rotation));
            }
            if (KeyboardManager.IsKeyPressed(Keyboard.Key.W))
            {
                this.PositionX += this.DeltaPosition.X * gameTime.DeltaTime * this.Speed;
                this.PositionY += this.DeltaPosition.Y * gameTime.DeltaTime * this.Speed;
            }
            if (KeyboardManager.IsKeyPressed(Keyboard.Key.S))
            {
                this.PositionX -= this.DeltaPosition.X * gameTime.DeltaTime * this.Speed;
                this.PositionY -= this.DeltaPosition.Y * gameTime.DeltaTime * this.Speed;
            }
        }
    }
}
