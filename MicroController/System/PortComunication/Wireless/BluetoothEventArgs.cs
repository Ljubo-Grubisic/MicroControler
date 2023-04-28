using System;
using System.Text;

namespace microController.system
{
    public class BluetoothEventArgs : EventArgs
    {
        public char[] IncomingDataBuffer { get; private set; }
        public string IncomingDataString { get; private set; }

        public BluetoothEventArgs(char[] incomingDataBuffer, int lastIndex)
        {
            this.IncomingDataBuffer = incomingDataBuffer;
            this.IncomingDataBuffer[lastIndex] = (char)0;
            for (int i = 0; i < lastIndex; i++)
            {
                this.IncomingDataString += char.ConvertFromUtf32(IncomingDataBuffer[i]);
            }
        }
    }
}