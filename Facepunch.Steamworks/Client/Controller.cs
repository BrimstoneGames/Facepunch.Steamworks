using System;
using System.Collections.Generic;
using System.Text;

namespace Facepunch.Steamworks
{
    public class AnalogData
    {
        public float x;
        public float y;
    }

    public class Controller : IDisposable
    {
        internal Client client;
        internal SteamNative.ControllerHandle_t handle;

        public UInt64 Handle => handle.Value;

        internal Controller(Client c, SteamNative.ControllerHandle_t h)
        {
            client = c;
            if(h == 0)
                throw new Exception("Invalid controller handle!");

            handle = h;
        }

        public void ActivateActionSet(string actionSet)
        {
            var actionSetHandle = client.native.controller.GetActionSetHandle(actionSet);
            if(actionSetHandle == 0)
                throw new Exception("Invalid action set!");

            client.native.controller.ActivateActionSet(handle, actionSetHandle);
        }
        
        public AnalogData GetAnalogAction(string analogAction)
        {
            if(handle == UInt64.MaxValue)
                return new AnalogData();

            var analogActionHandle = client.native.controller.GetAnalogActionHandle(analogAction);
            if(analogActionHandle == 0)
                throw new Exception("Invalid analog action!");

            var data = client.native.controller.GetAnalogActionData(handle, analogActionHandle);
            return new AnalogData() { x = data.X, y = data.Y };
        }

        public bool GetDigitalAction(string digitalAction)
        {
            if(handle == UInt64.MaxValue)
                return false;

            var digitalActionHandle = client.native.controller.GetDigitalActionHandle(digitalAction);
            if(digitalActionHandle == 0)
                throw new Exception("Invalid digital action!");

            var data = client.native.controller.GetDigitalActionData(handle, digitalActionHandle);
            return data.BState;
        }

        public void TriggerVibration(float leftSpeed, float rightSpeed)
        {
            if(handle == UInt64.MaxValue)
                return;

            client.native.controller.TriggerVibration(handle, (ushort)(leftSpeed * 1000000), (ushort)(rightSpeed * 1000000));
        }

        public void Dispose()
        {
            client = null;
        }
    }
}
