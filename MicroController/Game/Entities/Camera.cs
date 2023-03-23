using MicroController.Mathematics;
using MicroController.Shapes;
using SFML.System;
using SFML.Graphics;
using System;
using MicroController.MainLooping;
using MicroController.InputOutput;
using SFML.Window;
using MicroController.Game.Maping;
using MicroController.SFMLHelper;
using System.Data;
using System.Runtime.CompilerServices;

namespace MicroController.Game.Entities
{
    public class Camera : Entity
    {
        public float Speed { get; set; } = 150;
        private float scale = 0.20f;

        private Vector2f DeltaPosition;
        private Rectangle Rectangle { get; set; }
        private Texture CameraTexture { get; set; }

        public float Scale
        {
            get { return scale; }
            set { scale = value; OnScaleChange(); }
        }
        
        public Camera(Vector2f position)
        {
            this.Position = position;
            
            this.DeltaPosition.X = (float)(Math.Cos(this.Rotation));
            this.DeltaPosition.Y = (float)(Math.Sin(this.Rotation));

            CameraTexture = ImageHelper.LoadPngNoBackground("Camera.png");
            this.Size = (Vector2f)CameraTexture.Size * Scale;

            this.Rectangle = new Rectangle(Position, Size, CameraTexture);
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

        private void OnScaleChange()
        {
            this.Rectangle.Size = (Vector2f)this.CameraTexture.Size * Scale;
        }
    }
}
