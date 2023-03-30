using MicroController.Mathematics;
using System.Threading.Tasks;
using MicroController.Game.Entities;
using MicroController.Shapes;
using SFML.Graphics;
using MicroController.SFMLHelper;
using SFML.Window;
using SFML.System;

namespace MicroController.Game.Entities.Sensors
{
    public class FlameSensor : Entity
    {
        private Rectangle Rectangle;
        private Texture SensorTexture;

        private static Vector2f SizeInCm = new Vector2f(4, 2);

        public FlameSensor(Vector2f position, Game game) : base(game)
        {
            this.Position = position;

            this.SensorTexture = ImageHelper.LoadImgNoBackground("FlameSensor.png");
            this.Size = SizeInCm * Scale.NumPixelPerCm;

            this.Rectangle = new Rectangle(Position, Size, SensorTexture);
        }

        public void Draw(RenderWindow window)
        {
            this.Rectangle.Draw(window);
        }

        public new void Update(Map map)
        {
            base.Update(map);

            this.Size = SizeInCm * Scale.NumPixelPerCm;
            this.Rectangle.Size = this.Size;
            this.Rectangle.Position = this.DrawingPosition;
            this.Rectangle.Origin = new Vector2f(Rectangle.SizeX / 2, Rectangle.SizeY / 2);
            this.Rectangle.Rotation = MathHelper.RadiansToDegrees(this.Rotation) + 180;
        }
    }
}
