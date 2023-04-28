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
        public bool Connected { get => ClientRead.Connected && ClientWrite.Connected; }

        private List<BluetoothDeviceInfo> Devices { get; set; } = new List<BluetoothDeviceInfo>();

        private BluetoothClient ClientRead { get; set; }
        private BluetoothClient ClientWrite { get; set; }

        private BluetoothDeviceInfo TargetedDeviceWrite { get; set; }
        public string TargetedDeviceWriteName { get; set; } = "HC-06-write";
        public string TargetedDeviceWritePin { get; set; } = "1234";

        private BluetoothDeviceInfo TargetedDeviceRead { get; set; }
        public string TargetedDeviceReadName { get; set; } = "HC-06-read";
        public string TargetedDeviceReadPin { get; set; } = "1234";

        private StreamReader StreamReader { get; set; }
        private char[] IncomingDataBuffer { get; set; } = new char[150];
        private int LastByteRead;

        private StreamWriter StreamWriter { get; set; }
        private Queue<string> OutcomingDataBuffer { get; set; } = new Queue<string>();

        private Thread BluetoothThreadConnect { get; set; }
        private Thread BluetoothThreadRead { get; set; }
        private Thread BluetoothThreadWrite { get; set; }

        public delegate void BluetoothEventHandler(Bluetooth sender, BluetoothEventArgs args);
        public event BluetoothEventHandler DataRecived;

        public Bluetooth()
        {
            this.ClientWrite = new BluetoothClient() { };
            this.ClientRead = new BluetoothClient() { };
            this.BluetoothThreadConnect = new Thread(ConnectLoop) { IsBackground = true, Name = "Bluetooth-Connect" };
            this.BluetoothThreadRead = new Thread(ReadLoop) { IsBackground = true, Name = "Bluetooth-Read" };
            this.BluetoothThreadWrite = new Thread(WriteLoop) { IsBackground = true, Name = "Bluetooth-Write" };
            this.TargetedDeviceWrite = new BluetoothDeviceInfo(new BluetoothAddress(new ulong()));
        }

        public void Start()
        {
            if (BluetoothThreadConnect.ThreadState == (ThreadState.Unstarted | ThreadState.Background))
            {
                BluetoothThreadConnect.Start();
                BluetoothThreadRead.Start();
                BluetoothThreadWrite.Start();
            }
            else
            {
                BluetoothThreadConnect.Resume();
                BluetoothThreadRead.Resume();
                BluetoothThreadWrite.Resume();
            }
        }

        public void Stop()
        {
            BluetoothThreadConnect.Suspend();
            BluetoothThreadRead.Suspend();
            BluetoothThreadWrite.Suspend();
        }

        public void Write(string text)
        {
            OutcomingDataBuffer.Enqueue(text);
        }

        private void ConnectLoop()
        {
            while (BluetoothThreadConnect.IsAlive)
            {
                TargetedDeviceWrite.Refresh();

                if (!Devices.Contains(TargetedDeviceWrite) || !TargetedDeviceWrite.Connected || !ClientWrite.Connected || !ClientRead.Connected)
                {
                    Scan();
                }

                Connect();
            }
        }
        private void ReadLoop()
        {
            while (BluetoothThreadRead.IsAlive)
            {
                if (ClientRead.Connected && StreamReader != null)
                {
                    Read();
                }
            }
        }
        private void WriteLoop()
        {
            while (BluetoothThreadWrite.IsAlive)
            {
                if (OutcomingDataBuffer.Count != 0)
                {
                    if (ClientWrite.Connected && StreamWriter != null)
                    {
                        StreamWriter.Write(OutcomingDataBuffer.Dequeue());
                        StreamWriter.Flush();
                    }
                }
            }
        }

        private void Read()
        {
            int index = 0;
            for (int i = 0; i < IncomingDataBuffer.Length; i++)
            {
                this.IncomingDataBuffer[i] = (char)0;
            }
            while (true)
            {
                LastByteRead = StreamReader.BaseStream.ReadByte();

                if (LastByteRead == -1 || LastByteRead == 0)
                {
                    break;
                }
                this.IncomingDataBuffer[index] = (char)LastByteRead;
                if (LastByteRead == 59)
                {
                    OnDataRecived(IncomingDataBuffer, index);
                    break;
                }
                index++;
            }
        }

        private void Connect()
        {
            foreach (BluetoothDeviceInfo device in Devices)
            {
                if (device.DeviceName == TargetedDeviceWriteName)
                {
                    TargetedDeviceWrite = device;
                    if (TargetedDeviceWrite.InstalledServices == null)
                        TargetedDeviceWrite.SetServiceState(BluetoothService.SerialPort, true);
                    try
                    {
                        if (!TargetedDeviceWrite.Connected)
                            BluetoothSecurity.PairRequest(TargetedDeviceWrite.DeviceAddress, TargetedDeviceWritePin);
                        if (!ClientWrite.Connected)
                        {
                            ClientWrite.Connect(TargetedDeviceWrite.DeviceAddress, BluetoothService.SerialPort);
                            if (ClientWrite.Connected)
                                StreamWriter = new StreamWriter(ClientWrite.GetStream());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to connect to write device: {0}, Exception: {1}", device.DeviceName, ex);
                    }
                }
                if (device.DeviceName == TargetedDeviceReadName)
                {
                    TargetedDeviceRead = device;
                    if (TargetedDeviceRead.InstalledServices == null)
                        TargetedDeviceRead.SetServiceState(BluetoothService.SerialPort, true);
                    try
                    {
                        if (!TargetedDeviceRead.Connected)
                            BluetoothSecurity.PairRequest(TargetedDeviceRead.DeviceAddress, TargetedDeviceReadPin);
                        if (!ClientRead.Connected)
                        {
                            ClientRead.Connect(TargetedDeviceRead.DeviceAddress, BluetoothService.SerialPort);
                            if (ClientRead.Connected)
                                StreamReader = new StreamReader(ClientRead.GetStream());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to connect to read device: {0}, Exception: {1}", device.DeviceName, ex);
                    }
                }
            }
        }

        private void Scan()
        {
            BluetoothDeviceInfo[] devices = ClientWrite.DiscoverDevices().ToArray();
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

        protected virtual void OnDataRecived(char[] data, int lastIndex)
        {
            if (DataRecived != null)
                DataRecived(this, new BluetoothEventArgs(data, lastIndex));
        }
    }
}