using ReseauxProtocol.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.NetworkManagement
{
    public class SocketMessage : Message<ByteStream>
    {

        protected long _size;
        protected bool _IsSize = false;


        public SocketMessage(ByteStream PacketSocket, NetID ID) :
            base(PacketSocket, ID)
        {
        }

        public override ushort Header { get { return 0; } }

        public override long Length
        {
            get
            {
                if (_IsSize)
                    return _size;
                else
                    return -1;
            }
            /*
            protected set
            {
                _IsSize = true;
               _size = value;
            }
             * */
        }

    }
}
