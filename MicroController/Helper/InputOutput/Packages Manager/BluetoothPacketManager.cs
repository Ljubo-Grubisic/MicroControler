using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using microController.helpers;
using microController.system;
using SFML.Graphics;
using SFML.System;

namespace microController.helpers
{
    public static partial class BluetoothPacketManager
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
    }
}
