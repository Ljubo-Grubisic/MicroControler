using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MicroController.InputOutput.PortComunication.Wireless
{
    public partial class Bluetooth
    {
        public bool Connected { get => TargetedDevice.Connected; }

        public List<BluetoothDeviceInfo> Devices { get; set; } = new List<BluetoothDeviceInfo>();
        private BluetoothClient Client { get; set; }

        public BluetoothDeviceInfo TargetedDevice { get; set; }
        public string TargetedDeviceName { get; set; } = "HC-06";
        public string TargetedDevicePin { get; set; } = "1234";

        private char[] IncomingDataBuffer { get; set; }

        private Thread BluetoothThread { get; set; }

        public delegate void BluetoothEventHandler(Bluetooth sender, BluetoothEventArgs args);
        public event BluetoothEventHandler DataRecived;

        public Bluetooth()
        {
            this.IncomingDataBuffer = new char[50];
            this.Client = new BluetoothClient() { };
            this.BluetoothThread = new Thread(Loop) { IsBackground = true, Name = "Bluetooth" };
            this.TargetedDevice = new BluetoothDeviceInfo(new BluetoothAddress(new ulong()));
        }

        public void Start()
        {
            if (BluetoothThread.ThreadState == (ThreadState.Unstarted | ThreadState.Background))
            {
                BluetoothThread.Start();
            }
            else
            {
                BluetoothThread.Resume();
            }
        }

        public void Stop()
        {
            BluetoothThread.Suspend();
        }

        public void Write(string text)
        {
            if (Client.Connected)
            {
                Stream stream = Client.GetStream();
                using (StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8))
                {
                    streamWriter.Write(text);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            else
            {
                Console.WriteLine("Failed to write to device: client is not connected!");
            }
        }

        private void Loop()
        {
            while (BluetoothThread.IsAlive)
            {
                TargetedDevice.Refresh();

                if (!Devices.Contains(TargetedDevice) || !TargetedDevice.Connected)
                {
                    Scan();
                }

                if (!TargetedDevice.Connected || !Client.Connected)
                {
                    Connect();
                }

                Read();
            }
        }

        private void Read()
        {
            if (Client.Connected)
            {
                Stream stream = Client.GetStream();
                using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    IncomingDataBuffer = streamReader.ReadLine().ToCharArray();
                    OnDataRecived();
                    streamReader.Close();
                }
            }
        }

        private void Connect()
        {
            foreach (BluetoothDeviceInfo device in Devices)
            {
                if (device.DeviceName == TargetedDeviceName)
                {
                    TargetedDevice = device;
                    if (TargetedDevice.InstalledServices == null)
                    {
                        TargetedDevice.SetServiceState(BluetoothService.SerialPort, true);
                    }
                    try
                    {
                        if (!TargetedDevice.Connected)
                            BluetoothSecurity.PairRequest(TargetedDevice.DeviceAddress, TargetedDevicePin);
                        if (!Client.Connected)
                            Client.Connect(TargetedDevice.DeviceAddress, BluetoothService.SerialPort);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to connect to device: {0}, Exception: {1}", device.DeviceName, ex);
                    }
                    Console.WriteLine("Is client connected: {0}", Client.Connected);
                    Console.WriteLine("Is targeted device connected: {0}", TargetedDevice.Connected);
                }
            }
        }

        private void Scan()
        {
            BluetoothDeviceInfo[] devices = Client.DiscoverDevices().ToArray();
            if (devices.Length == 0)
            {
                Console.WriteLine("No device found");
            }
            foreach (BluetoothDeviceInfo d in devices)
            {
                Console.WriteLine("Device name: {0}, Device address: {1}", d.DeviceName, d.DeviceAddress);
                if (!Devices.Contains(d))
                {
                    Devices.Add(d);
                }
            }
        }

        protected virtual void OnDataRecived()
        {
            if (DataRecived != null)
                DataRecived(this, new BluetoothEventArgs(this.IncomingDataBuffer));
        }
    }
}