using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReseauxProtocol.NetworkManagement
{
    public interface IMessage
    {

        ushort Header { get; }

    }

    public abstract class Message<T> : IMessage
    {
       protected Mutex mut = new Mutex();
        private T _packet;
        public T Data { get { return _packet; } }
        public NetID Client { get; private set; }      

        public Message(T packet, NetID ID)
        {
            _packet = packet;
            Client = ID;
        }

        public abstract ushort Header { get; }

        public abstract long Length
        {
            get;
          //  protected set;
        }

    }
}
