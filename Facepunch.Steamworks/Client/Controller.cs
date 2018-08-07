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

        public void ActivateActionLayer(string actionLayer)
        {
            var actionLayerHandle = client.native.controller.GetActionSetHandle(actionLayer);
            if(actionLayerHandle == 0)
                throw new Exception("Invalid action layer!");

            client.native.controller.ActivateActionSetLayer(handle, actionLayerHandle);
        }

        public void DeactivateActionLayer(string actionLayer)
        {
            var actionLayerHandle = client.native.controller.GetActionSetHandle(actionLayer);
            if(actionLayerHandle == 0)
                throw new Exception("Invalid action layer!");

            client.native.controller.ActivateActionSetLayer(handle, actionLayerHandle);
        }

        public void DeactivateAllActionLayers()
        {
            client.native.controller.DeactivateAllActionSetLayers(handle);
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

        public string GetGlyphForDigitalAction(string actionSet, string digitalAction)
        {
            var digitalActionHandle = client.native.controller.GetDigitalActionHandle(digitalAction);
            if(digitalActionHandle == 0)
                throw new Exception("Invalid digital action!");

            var actionSetHandle = client.native.controller.GetActionSetHandle(actionSet);
            if(actionSetHandle == 0)
                throw new Exception("Invalid action set!");

            SteamNative.ControllerActionOrigin origin;
            int result = client.native.controller.GetDigitalActionOrigins(handle, actionSetHandle, digitalActionHandle, out origin);

            //Workaround for missing Xbox glyphs
            switch(origin) {
                case SteamNative.ControllerActionOrigin.XBox360_LeftStick_Click:
                case SteamNative.ControllerActionOrigin.XBoxOne_LeftStick_Click:
                    origin = SteamNative.ControllerActionOrigin.PS4_LeftStick_Click;
                    break;

                case SteamNative.ControllerActionOrigin.XBox360_RightStick_Click:
                case SteamNative.ControllerActionOrigin.XBoxOne_RightStick_Click:
                    origin = SteamNative.ControllerActionOrigin.PS4_RightStick_Click;
                    break;
            }

            return client.native.controller.GetGlyphForActionOrigin(origin);
        }

        public string GetGlyphForAnalogAction(string actionSet, string analogAction)
        {
            var analogActionHandle = client.native.controller.GetAnalogActionHandle(analogAction);
            if(analogActionHandle == 0)
                throw new Exception("Invalid analog action!");

            var actionSetHandle = client.native.controller.GetActionSetHandle(actionSet);
            if(actionSetHandle == 0)
                throw new Exception("Invalid action set!");

            SteamNative.ControllerActionOrigin origin;
            int result = client.native.controller.GetAnalogActionOrigins(handle, actionSetHandle, analogActionHandle, out origin);

            //Workaround for missing Xbox glyphs
            switch(origin) {
                case SteamNative.ControllerActionOrigin.XBox360_LeftStick_Move:
                case SteamNative.ControllerActionOrigin.XBoxOne_LeftStick_Move:
                    origin = SteamNative.ControllerActionOrigin.PS4_LeftStick_Move;
                    break;

                case SteamNative.ControllerActionOrigin.XBox360_RightStick_Move:
                case SteamNative.ControllerActionOrigin.XBoxOne_RightStick_Move:
                    origin = SteamNative.ControllerActionOrigin.PS4_RightStick_Move;
                    break;
            }

            return client.native.controller.GetGlyphForActionOrigin(origin);
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
