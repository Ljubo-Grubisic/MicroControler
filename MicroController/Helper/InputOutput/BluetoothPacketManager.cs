using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using microController.Helper;
using microController.helpers;
using microController.system;
using SFML.Graphics;
using SFML.System;

namespace microController.helpers
{
    public static class BluetoothPacketManager
    {
        public static Bluetooth Bluetooth { get; private set; }

        public delegate void Dht11EventHandler(object sender, Dht11EventArgs e);
        public delegate void FlameEventHandler(object sender, FlameEventArgs e);
        public delegate void SonarEventHandler(object sender, SonarEventArgs e);
        public delegate void GpsEventHandler(object sender, GpsEventArgs e);

        public static event Dht11EventHandler Dht11DataRecived;
        public static event FlameEventHandler FlameDataRecived;
        public static event SonarEventHandler SonarDataRecived;
        public static event GpsEventHandler GpsDataRecived;

        public static void Init()
        {
            Bluetooth = new Bluetooth();
            Bluetooth.Start();
            Bluetooth.DataRecived += Bluetooth_DataRecived;
        }

        private static void Bluetooth_DataRecived(Bluetooth sender, BluetoothEventArgs args)
        {
            if (args.IncomingDataString[0] == 'G')
            {
                DecodeGpsData(args.IncomingDataString);
            }
            if (args.IncomingDataString[0] == 'D')
            {
                DecodeDht11Data(args.IncomingDataString);
            }
            if (args.IncomingDataString[0] == 'H')
            {
                DecodeSonarData(args.IncomingDataString);
            }
            if (args.IncomingDataString[0] == 'F')
            {
                DecodeFlameData(args.IncomingDataString);
            }
        }

        private static void DecodeGpsData(string data)
        {
            double latitude = Convert.ToDouble(StringHelper.GetDataBetweenChars(data, 'A', 'O'));
            double longitude = Convert.ToDouble(StringHelper.GetDataBetweenChars(data, 'O', 'R'));
            double rotation = Convert.ToDouble(StringHelper.GetDataBetweenChars(data, 'R', 'S'));
            double speed = Convert.ToDouble(StringHelper.GetDataBetweenChars(data, 'S'));

            GpsDataRecived?.Invoke(Bluetooth, new GpsEventArgs(latitude, longitude, rotation, speed));
        }
        private static void DecodeDht11Data(string data)
        {
            int indentifier = Convert.ToInt32(data[1]);
            double temperature = Convert.ToDouble(StringHelper.GetDataBetweenChars(data, 'T', 'H'));
            double humidity = Convert.ToDouble(StringHelper.GetDataBetweenChars(data, 'H'));

            Dht11DataRecived?.Invoke(Bluetooth, new Dht11EventArgs(temperature, humidity, indentifier));
        }
        private static void DecodeSonarData(string data)
        {
            int indentifier = Convert.ToInt32(data[1]);
            double distance = Convert.ToDouble(StringHelper.GetDataBetweenChars(data, 'D', 'R'));
            double rotation = Convert.ToDouble(StringHelper.GetDataBetweenChars(data, 'R'));

            Dht11DataRecived?.Invoke(Bluetooth, new Dht11EventArgs(distance, rotation, indentifier));
        }
        private static void DecodeFlameData(string data)
        {
            int indentifier = Convert.ToInt32(data[1]);
            double flame = Convert.ToDouble(StringHelper.GetDataAfterChar(data, '&'));

            FlameDataRecived?.Invoke(Bluetooth, new FlameEventArgs(flame, indentifier));
        }
        

        public class Dht11EventArgs : EventArgs
        {
            /// <summary>
            /// In Celsius
            /// </summary>
            public double Temperature { get; private set; }
            /// <summary>
            /// In a percentage
            /// </summary>
            public double Humidity { get; private set; }
            /// <summary>
            /// Indetifies which sensor is sending the data 0-top, 1-back, 2-right, 3-left
            /// </summary>
            public int Indentifier { get; private set; }

            public Dht11EventArgs(double temperature, double humidity, int indentifier)
            {
                this.Temperature = temperature;
                this.Humidity = humidity;
                this.Indentifier = indentifier;
            }
        }

        public class FlameEventArgs : EventArgs
        {
            public double Flame { get; private set; }
            /// <summary>
            /// Indetifies which sensor is sending the data 0-top, 1-back, 2-right, 3-left
            /// </summary>
            public int Indentifier { get; private set; }

            public FlameEventArgs(double flame, int indentifier)
            {
                this.Flame = flame;
                this.Indentifier = indentifier;
            }
        }

        public class SonarEventArgs : EventArgs
        {
            /// <summary>
            /// In cm
            /// </summary>
            public double Distance { get; private set; }
            /// <summary>
            /// In degrees
            /// </summary>
            public double Rotation { get; private set; }
            /// <summary>
            /// Indetifies which sensor is sending the data 0-top, 1-back
            /// </summary>
            public int Indentifier { get; private set; }

            public SonarEventArgs(double distance, double rotation, int indentifier)
            {
                this.Distance = distance;
                this.Rotation = rotation;
                this.Indentifier = indentifier;
            }
        }

        public class GpsEventArgs : EventArgs
        {
            /// <summary>
            /// In degrees
            /// </summary>
            public double Latitude { get; private set; }
            /// <summary>
            /// In degrees
            /// </summary>
            public double Longtitude { get; private set; }
            /// <summary>
            /// In degrees
            /// </summary>
            public double Rotation { get; private set; }
            /// <summary>
            /// In km/h
            /// </summary>
            public double Speed { get; private set; }
            /// <summary>
            /// In meters
            /// </summary>
            public double LatitudeInMeters { get => Latitude * 111 * 1000; }
            /// <summary>
            /// In meters
            /// </summary>
            public double LongtitudeInMeters { get => Longtitude * 111 * 1000; }

            public GpsEventArgs(double latitude, double longtitude, double rotation, double speed)
            {
                this.Latitude = latitude;
                this.Longtitude = longtitude;
                this.Rotation = rotation;
                this.Speed = speed;
            }
        }

        public class CameraEventArgs : EventArgs
        {
            /// <summary>
            /// R, G, B 
            /// </summary>
            public byte[] Data { get; private set; }
            /// <summary>
            /// R, G, B
            /// </summary>
            public byte[,] DataIn2d { get; private set; }
            /// <summary>
            /// The image
            /// </summary>
            public Image Image { get; private set; }

            public static Vector2i VideoSize = new Vector2i(784, 510);

            public CameraEventArgs(byte[] data)
            {
                this.Data = data;
                this.DataIn2d = ImageHelper.UnflattenByteArray(this.Data, VideoSize.Y, VideoSize.X);
                this.Image = new Image((uint)VideoSize.X, (uint)VideoSize.Y, this.Data);
            }
        }
    }
}
