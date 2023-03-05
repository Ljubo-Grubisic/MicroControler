using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace MicroControler.InputOutput.PortComunication
{
    public class Serial
    {
        private Thread ReadThread;
        public SerialPort Port;
        public string Info = "";

        private bool Started = false;

        public Serial(string portName, int baudRate) 
        { 
            Port = new SerialPort(portName, baudRate);
            ReadThread = new Thread(Read);
            ReadThread.IsBackground = true;
        }

        public void StartReading()
        {
            try
            {
                Port.Open();
            }
            catch(System.IO.IOException Exception) 
            {
                Console.WriteLine(Exception);
                Console.WriteLine("Failed to open the serial port.");
                return;
            }
            if (!Started)
            {
                ReadThread.Start();
                Started = true;
            }
            else
            {
                ReadThread.Resume();
            }
        }

        public void StopReading()
        {
            Port.Close();
            ReadThread.Suspend();
        }

        private void Read()
        {
            while(true)
            {
                try
                {
                    Info = Port.ReadLine();
                    Info = Info.Trim();
                }
                catch
                {
                    Console.WriteLine("Failed reading the serial info");
                }
            }
        }
    }
}
