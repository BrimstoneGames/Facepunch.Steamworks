using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Facepunch.Steamworks
{
    public class Input : IDisposable
    {
        internal Client client;
        public Controller AllControllers { get; private set; }

        public Input(Client c)
        {
            client = c;
            client.native.controller.Init();
            AllControllers = new Controller(c, UInt64.MaxValue);
        }

        public unsafe List<Controller> GetConnectedControllers()
        {
            SteamNative.ControllerHandle_t[] handles = new SteamNative.ControllerHandle_t[16];
            var controllers = new List<Controller>();

            fixed (SteamNative.ControllerHandle_t* p = handles) {
                int count = 0;

                count = client.native.controller.GetConnectedControllers((IntPtr)p);
                for(int i = 0; i < count; i++) {
                    controllers.Add(new Controller(client, handles[i]));
                }
            }

            return controllers;
        }

        public void RunFrame()
        {
            client.native.controller.RunFrame();
        }

        public void Dispose()
        {
            client.native.controller.Shutdown();
            client = null;
        }
    }
}
