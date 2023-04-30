using microController.helpers;
using microController.graphics;
using microController.system;
using SFML.Graphics;
using SFML.System;
using System;
using System.Net.Sockets;
using System.Management.Instrumentation;

namespace microController.game.entities
{
    public class Firetruck : Entity
    {
        /// <summary>
        /// 0-top, 1-back
        /// </summary>
        private HC_SR04[] Sonars = new HC_SR04[2];
        /// <summary>
        /// 0-top, 1-back, 2-right, 3-left
        /// </summary>
        private FlameSensor[] FlameSensors = new FlameSensor[4];
        /// <summary>
        /// 0-top, 1-back, 2-right, 3-left
        /// </summary>
        private DHT11[] DHT11s = new DHT11[4];
        /// <summary>
        /// In km/h
        /// </summary>
        private double Speed;

        private Vector2f SizeInCm = new Vector2f(15, 26);

        private Vector2f GpsOrigin = new Vector2f();
        private Vector2f MapOrigin = new Vector2f();

        private Rectangle Rectangle;

        private bool OriginSet = false;

        public Firetruck(Vector2f position, Game game) : base(game)
        {
            this.Position = position;

            this.Size = SizeInCm * Scale.NumPixelPerCm;

            this.Rectangle = new Rectangle(Position, Size) { FillColor = Color.Red };
            this.Sonars[0] = new HC_SR04(this.Position + new Vector2f(SizeInCm.X / 2 * Scale.NumPixelPerCm * (float)Math.Cos(this.Rotation), SizeInCm.Y / 2 * Scale.NumPixelPerCm * (float)Math.Sin(this.Rotation)), game);
            this.Sonars[1] = new HC_SR04(this.Position - new Vector2f(SizeInCm.X / 2 * Scale.NumPixelPerCm * (float)Math.Cos(this.Rotation), SizeInCm.Y / 2 * Scale.NumPixelPerCm * (float)Math.Sin(this.Rotation)), game);
            for (int i = 0; i < 4; i++)
            {
                this.DHT11s[i] = new DHT11(new Vector2f(), game);
            }
            for (int i = 0; i < 4; i++)
            {
                this.FlameSensors[i] = new FlameSensor(new Vector2f(), game);
            }
            this.MapOrigin = new Vector2f(game.map.DataSize.Y * Scale.SquareSizeInCm, game.map.DataSize.X * Scale.SquareSizeInCm);

            BluetoothPacketManager.GpsDataRecived += BluetoothManager_GpsDataRecived;
            BluetoothPacketManager.Dht11DataRecived += BluetoothManager_Dht11DataRecived;
            BluetoothPacketManager.SonarDataRecived += BluetoothManager_SonarDataRecived;
            BluetoothPacketManager.FlameDataRecived += BluetoothManager_FlameDataRecived;
        }


        public override void Draw(RenderWindow window)
        {
            this.Rectangle.Draw(window);
            this.Sonars[0].Draw(window);
            this.Sonars[1].Draw(window);
        }

        protected override void OnUpdate(Map map, GameTime gameTime)
        {
            this.Size = this.SizeInCm * Scale.NumPixelPerCm;
            this.Rectangle.Size = this.Size;
            this.Rectangle.Position = this.DrawingPosition;
            this.Rectangle.Origin = new Vector2f(Rectangle.SizeX / 2, Rectangle.SizeY / 2);
            this.Rectangle.Rotation = MathHelper.RadiansToDegrees(this.Rotation) + 90;

            this.Sonars[0].Position = this.Position + new Vector2f(SizeInCm.X / 2 * Scale.NumPixelPerCm * (float)Math.Cos(this.Rotation), SizeInCm.Y / 2  * Scale.NumPixelPerCm * (float)Math.Sin(this.Rotation));
            this.Sonars[1].Position = this.Position - new Vector2f(SizeInCm.X / 2 * Scale.NumPixelPerCm * (float)Math.Cos(this.Rotation), SizeInCm.Y / 2 * Scale.NumPixelPerCm * (float)Math.Sin(this.Rotation));
            this.Sonars[0].Update(map, gameTime);
            this.Sonars[1].Update(map, gameTime);
        }


        private void BluetoothManager_GpsDataRecived(object sender, BluetoothPacketManager.GpsEventArgs e)
        {
            if (OriginSet)
            {
                this.GpsOrigin = new Vector2f((float)e.LatitudeInMeters, (float)e.LongtitudeInMeters);
            }
        }
        private void BluetoothManager_Dht11DataRecived(object sender, BluetoothPacketManager.Dht11EventArgs e)
        {
        }
        private void BluetoothManager_SonarDataRecived(object sender, BluetoothPacketManager.SonarEventArgs e)
        {
        }
        private void BluetoothManager_FlameDataRecived(object sender, BluetoothPacketManager.FlameEventArgs e)
        {
        }
    }
}
