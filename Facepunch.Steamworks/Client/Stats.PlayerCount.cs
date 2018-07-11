using System;
using System.Collections.Generic;
using System.Text;

namespace Facepunch.Steamworks
{
    public partial class Stats
    {
        public class PlayerCount : IDisposable
        {
            public Action<int> OnUpdated;
            public bool IsFinished { get; private set; }
            public int Count { get; private set; }

            internal Client client;

            public PlayerCount(Client client)
            {
                this.client = client;
            }

            public void Dispose()
            {
                client = null;
            }

            public void Refresh()
            {
                IsFinished = false;
                client.native.userstats.GetNumberOfCurrentPlayers((r, fail) => {
                    IsFinished = true;
                    Count = !fail ? r.CPlayers : -1;
                    if(OnUpdated != null)
                        OnUpdated(Count);
                });
            }
        }
    }
}
