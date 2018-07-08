using System;
using System.Collections.Generic;
using System.Text;

namespace Facepunch.Steamworks
{
    public class Utils : IDisposable
    {
        internal Client client;

        public Utils(Client c) {
            client = c;
        }

        public uint GetServerRealTime() {
            return client.native.utils.GetServerRealTime();
        }

        public string GetIPCountry() {
            return client.native.utils.GetIPCountry();
        }

        public void Dispose() {
            client = null;
        }
    }
}
