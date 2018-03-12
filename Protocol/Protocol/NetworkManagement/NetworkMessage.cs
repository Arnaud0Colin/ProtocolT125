using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReseauxProtocol.NetworkManagement
{
    public class NetworkMessage : Message<MemoryStream>
    {

        public NetworkMessage(MemoryStream PacketStream, NetID ID):
            base(PacketStream, ID)
        {
        }

        public override ushort Header { get { return 0; } }


        public override long Length
        {
            get
            {
                if (Data != null)
                    return Data.Length;
                else
                    return -1;
            }
          /*
            protected set
            {
                Data.SetLength(value);
            }
           * */
        }

    }
}
