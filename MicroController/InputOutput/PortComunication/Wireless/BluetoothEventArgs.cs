using System;
using System.Text;

namespace MicroController.InputOutput.PortComunication.Wireless
{
    public class BluetoothEventArgs : EventArgs
    {
        public char[] IncomingDataBuffer { get; private set; }
        public string IncomingDataString { get; private set; }

        public BluetoothEventArgs(char[] incomingDataBuffer)
        {
            this.IncomingDataBuffer = incomingDataBuffer;
            for (int i = 0; i < IncomingDataBuffer.Length; i++)
            {
                this.IncomingDataString += char.ConvertFromUtf32(IncomingDataBuffer[i]);
            }
        }
    }
}