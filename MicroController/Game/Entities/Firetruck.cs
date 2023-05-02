using microController.helpers;
using microController.graphics;
using microController.system;
using SFML.Graphics;
using SFML.System;
using System;
using System.Net.Sockets;
using System.Management.Instrumentation;
using SFML.Window;
using System.Threading;

namespace microController.game.entities
{
    public class Firetruck : Entity
    {
        /// <summary>
        /// 0-top, 1-back
        /// </summary>
        public HC_SR04[] Sonars { get; private set; } = new HC_SR04[2];
        /// <summary>
        /// 0-top, 1-back, 2-right, 3-left
        /// </summary>
        public FlameSensor[] FlameSensors { get; private set; } = new FlameSensor[4];
        /// <summary>
        /// 0-top, 1-back, 2-right, 3-left
        /// </summary>
        public DHT11[] DHT11s { get; private set; } = new DHT11[4];
        /// <summary>
        /// In km/h
        /// </summary>
        public double Speed { get; private set; }
        /// <summary>
        /// The strenght the dc motors will use to move the vehicle
        /// 255 - max, 0 - none
        /// </summary>
        public byte MotorSpeed { get; set; } = 100;

        public DrivingMode VehicleDrivingMode { get; set; } = DrivingMode.None;
        private BluetoothPacketManager.PumpState PumpState { get; set; } = BluetoothPacketManager.PumpState.Off;

        private Vector2f SizeInCm = new Vector2f(15, 26);

        private Vector2f GpsOrigin = new Vector2f();
        private Vector2f MapOrigin = new Vector2f();

        private Rectangle Rectangle;

        private Map Map;
        public PathFinder PathFinder;

        private Map AutoMap;
        private PathFinder AutoPathFinder;

        private bool OriginSet = false;

        private double AverageTemperature
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < 4; i++)
                {
                    sum += DHT11s[i].Temperature;
                }
                return sum / 4;
            }
        }
        private double AverageHumidity
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < 4; i++)
                {
                    sum += DHT11s[i].Humidity;
                }
                return sum / 4;
            }
        }
        private double AverageFlameStrenght
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < 4; i++)
                {
                    sum += FlameSensors[i].FlameStrenght;
                }
                return sum / 4;
            }
        }

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
            this.Map = game.map;
            PathFinder = new PathFinder(game.map);

            BluetoothPacketManager.GpsDataRecived += BluetoothManager_GpsDataRecived;
            BluetoothPacketManager.Dht11DataRecived += BluetoothManager_Dht11DataRecived;
            BluetoothPacketManager.SonarDataRecived += BluetoothManager_SonarDataRecived;
            BluetoothPacketManager.FlameDataRecived += BluetoothManager_FlameDataRecived;
        }
        public Firetruck(Game game) : base(game)
        {
            this.Position = new Vector2f(game.map.DataSize.Y * Scale.SquareSizeInCm, game.map.DataSize.X * Scale.SquareSizeInCm);
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
            this.Map = game.map;
            PathFinder = new PathFinder(game.map);

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
            this.PathFinder.DisplayPath(window, Map, PathFinder.LastNode);
        }

        protected override void OnUpdate(Map map, GameTime gameTime)
        {
            this.Map = map;
            this.Size = this.SizeInCm * Scale.NumPixelPerCm;
            this.Rectangle.Size = this.Size;
            this.Rectangle.Position = this.DrawingPosition;
            this.Rectangle.Origin = new Vector2f(Rectangle.SizeX / 2, Rectangle.SizeY / 2);
            this.Rectangle.Rotation = MathHelper.RadiansToDegrees(this.Rotation) + 90;

            Vector2f Buffer = new Vector2f();
            Buffer.X = (float)Math.Cos(this.Rotation) * ((SizeInCm.Y / 2) * Scale.NumPixelPerCm);
            Buffer.Y = (float)Math.Sin(this.Rotation) * ((SizeInCm.Y / 2) * Scale.NumPixelPerCm);

            this.Sonars[0].Position = this.Position - Buffer;
            this.Sonars[1].Position = this.Position + Buffer;
            this.Sonars[0].Update(map, gameTime);
            this.Sonars[1].Update(map, gameTime);

            if (this.VehicleDrivingMode == DrivingMode.Keyboard)
            {
                if (KeyboardManager.IsKeyDown(Keyboard.Key.W))
                {
                    BluetoothPacketManager.SendVehicleMovementCommand(BluetoothPacketManager.VehicleMovementMode.Forward, MotorSpeed);
                }
                if (KeyboardManager.IsKeyDown(Keyboard.Key.S))
                {
                    BluetoothPacketManager.SendVehicleMovementCommand(BluetoothPacketManager.VehicleMovementMode.Backward, MotorSpeed);
                }
                if (KeyboardManager.IsKeyDown(Keyboard.Key.Space))
                {
                    BluetoothPacketManager.SendVehicleMovementCommand(BluetoothPacketManager.VehicleMovementMode.Break, MotorSpeed);
                }
                if (KeyboardManager.IsKeyDown(Keyboard.Key.A))
                {
                    BluetoothPacketManager.SendVehicleRotationCommand(BluetoothPacketManager.VehicleRotationMode.CounterClockwise, MotorSpeed);
                }
                if (KeyboardManager.IsKeyDown(Keyboard.Key.D))
                {
                    BluetoothPacketManager.SendVehicleRotationCommand(BluetoothPacketManager.VehicleRotationMode.Clockwise, MotorSpeed);
                }
                if (KeyboardManager.OnKeyPressed(Keyboard.Key.P))
                {
                    if (this.PumpState == BluetoothPacketManager.PumpState.On)
                    {
                        BluetoothPacketManager.SendPumpStateCommand(BluetoothPacketManager.PumpState.Off);
                        this.PumpState = BluetoothPacketManager.PumpState.Off;
                    }
                    else if (this.PumpState == BluetoothPacketManager.PumpState.Off)
                    {
                        BluetoothPacketManager.SendPumpStateCommand(BluetoothPacketManager.PumpState.On);
                        this.PumpState = BluetoothPacketManager.PumpState.On;
                    }
                }
            }
            if (this.VehicleDrivingMode == DrivingMode.FireFinding)
            {
                if (DHT11s[0].Temperature > AverageTemperature + 5 || DHT11s[0].Humidity < AverageHumidity - 20 || FlameSensors[0].FlameStrenght > AverageFlameStrenght + 300)
                {
                    if (DHT11s[0].Temperature < 45 && DHT11s[0].Humidity > 10 && FlameSensors[0].FlameStrenght < 900)
                    {
                        BluetoothPacketManager.SendVehicleMovementCommand(BluetoothPacketManager.VehicleMovementMode.Forward, MotorSpeed);
                    }
                    else
                    {
                        BluetoothPacketManager.SendVehicleMovementCommand(BluetoothPacketManager.VehicleMovementMode.Break, MotorSpeed);
                    }
                }
                if (DHT11s[1].Temperature > AverageTemperature + 5 || DHT11s[1].Humidity < AverageHumidity - 20 || FlameSensors[1].FlameStrenght > AverageFlameStrenght + 300)
                {
                    if (DHT11s[1].Temperature < 45 && DHT11s[1].Humidity > 10 && FlameSensors[1].FlameStrenght < 900)
                    {
                        RotateLeft(180);
                    }
                }
                if (DHT11s[2].Temperature > AverageTemperature + 5 || DHT11s[2].Humidity < AverageHumidity - 20 || FlameSensors[2].FlameStrenght > AverageFlameStrenght + 300)
                {
                    if (DHT11s[2].Temperature < 45 && DHT11s[2].Humidity > 10 && FlameSensors[2].FlameStrenght < 900)
                    {
                        RotateLeft(90);
                    }
                }
                if (DHT11s[3].Temperature > AverageTemperature + 5 || DHT11s[3].Humidity < AverageHumidity - 20 || FlameSensors[3].FlameStrenght > AverageFlameStrenght + 300)
                {
                    if (DHT11s[3].Temperature < 45 && DHT11s[3].Humidity > 10 && FlameSensors[3].FlameStrenght < 900)
                    {
                        RotateRight(90);
                    }
                }
            }
            if (this.VehicleDrivingMode == DrivingMode.Auto)
            {

            }
        }

        private float StartingRotation;
        private bool CommandInExecution = false;
        private void RotateLeft(float degrees)
        {
            if (!this.CommandInExecution)
            {
                this.CommandInExecution = true;
                StartingRotation = degrees;
                ThreadPool.QueueUserWorkItem((WaitCallback) =>
                {
                    while (true)
                    {
                        BluetoothPacketManager.SendVehicleRotationCommand(BluetoothPacketManager.VehicleRotationMode.CounterClockwise, MotorSpeed);
                        if (MathHelper.RadiansToDegrees(this.Rotation) < this.StartingRotation - degrees)
                        {
                            break;
                        }
                    }
                });
            }
        }
        private void RotateRight(float degrees)
        {
            if (!this.CommandInExecution)
            {
                this.CommandInExecution = true;
                StartingRotation = degrees;
                ThreadPool.QueueUserWorkItem((WaitCallback) =>
                {
                    while (true)
                    {
                        BluetoothPacketManager.SendVehicleRotationCommand(BluetoothPacketManager.VehicleRotationMode.Clockwise, MotorSpeed);
                        if (MathHelper.RadiansToDegrees(this.Rotation) > this.StartingRotation + degrees)
                        {
                            break;
                        }
                    }
                });
            }
        }

        private void BluetoothManager_GpsDataRecived(object sender, BluetoothPacketManager.GpsEventArgs e)
        {
            if (OriginSet)
            {
                this.GpsOrigin = new Vector2f((float)e.LatitudeInMeters, (float)e.LongtitudeInMeters);
            }
            this.Position = this.GpsOrigin - new Vector2f((float)e.LatitudeInMeters, (float)e.LongtitudeInMeters) + this.MapOrigin;
            this.Speed = e.Speed;
            this.Rotation = (float)e.Rotation;
        }
        private void BluetoothManager_Dht11DataRecived(object sender, BluetoothPacketManager.Dht11EventArgs e)
        {
            this.DHT11s[e.Indentifier].Temperature = e.Temperature;
            this.DHT11s[e.Indentifier].Humidity = e.Humidity;
        }
        private void BluetoothManager_SonarDataRecived(object sender, BluetoothPacketManager.SonarEventArgs e)
        {
            this.Sonars[e.Indentifier].Rotation = (float)e.Rotation + this.Rotation;
            this.Sonars[e.Indentifier].CastRay(e.Distance, this.Map);
        }
        private void BluetoothManager_FlameDataRecived(object sender, BluetoothPacketManager.FlameEventArgs e)
        {
            this.FlameSensors[e.Indentifier].FlameStrenght = e.Flame;
        }

        public enum DrivingMode
        {
            None,
            Keyboard,
            Auto,
            FireFinding
        }
    }
}
