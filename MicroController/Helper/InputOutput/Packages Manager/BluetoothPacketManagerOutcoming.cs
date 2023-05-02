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
        public static void SendVehicleRotationCommand(VehicleRotationMode vehicleRotationMode, byte speed)
        {
            Bluetooth.Write("R" + (uint)vehicleRotationMode + "&" + speed + ";");
        }

        public static void SendVehicleMovementCommand(VehicleMovementMode vehicleMovementMode, byte speed) 
        {
            Bluetooth.Write("M" + (uint)vehicleMovementMode + "&" + speed + ";");
        }
        
        public static void SendPumpStateCommand(PumpState pumpState)
        {
            Bluetooth.Write("P" + (uint)pumpState + ";");
        }

        public enum VehicleRotationMode
        {
            Clockwise,
            CounterClockwise
        }

        public enum VehicleMovementMode
        {
            Forward,
            Backward,
            Break
        }

        public enum PumpState
        {
            Off,
            On
        }
    }
}
