using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace microController.system
{
    public partial class Bluetooth
    {
        public bool Connected { get => Client.Connected; }

        public List<BluetoothDeviceInfo> Devices { get; set; } = new List<BluetoothDeviceInfo>();
        private BluetoothClient Client { get; set; }

        public BluetoothDeviceInfo TargetedDevice { get; set; }
        public string TargetedDeviceName { get; set; } = "HC-06";
        public string TargetedDevicePin { get; set; } = "1234";

        private char[] IncomingDataBuffer { get; set; }

        private Queue<string> OutgoingDataQueue { get; set; }
        private bool IsStreamOpen = true;

        private Thread BluetoothThread { get; set; }

        public delegate void BluetoothEventHandler(Bluetooth sender, BluetoothEventArgs args);
        public event BluetoothEventHandler DataRecived;

        public Bluetooth()
        {
            this.IncomingDataBuffer = new char[50];
            this.Client = new BluetoothClient() { };
            this.BluetoothThread = new Thread(Loop) { IsBackground = true, Name = "Bluetooth" };
            this.TargetedDevice = new BluetoothDeviceInfo(new BluetoothAddress(new ulong()));
            this.OutgoingDataQueue = new Queue<string>();
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
            OutgoingDataQueue.Enqueue(text);
        }

        private void Loop()
        {
            while (BluetoothThread.IsAlive)
            {
                TargetedDevice.Refresh();

                if (!Devices.Contains(TargetedDevice) || !TargetedDevice.Connected || !Client.Connected)
                {
                    Scan();
                }

                if (!Client.Connected || !TargetedDevice.Connected)
                {
                    Connect();
                }

                InitStreams();

                Read();
            }
        }

        private void Read()
        {
            if (Client.Connected) 
            { 
                OnDataRecived();
                while(this.OutgoingDataQueue.Count > 0)
                {
                }
            }
        }

        private void InitStreams()
        {
            if (Client.Connected)
            {
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
            Devices.Clear();
            if (devices.Length == 0)
            {
                Console.WriteLine("No device found");
            }
            foreach (BluetoothDeviceInfo d in devices)
            {
                Console.WriteLine("Device name: {0}, Device address: {1}", d.DeviceName, d.DeviceAddress);
                Devices.Add(d);
            }
        }

        protected virtual void OnDataRecived()
        {
            if (DataRecived != null)
                DataRecived(this, new BluetoothEventArgs(this.IncomingDataBuffer));
        }
    }
}