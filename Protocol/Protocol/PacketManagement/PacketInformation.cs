using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.PacketManagement
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PacketInformation : System.Attribute
    {
        public PacketInformation(UInt16 Header, bool IsIncoming)
        {
            this.Header = Header;
            this.IsIncoming = IsIncoming;
        }

        public UInt16 Header { get; private set; }
        public bool IsIncoming { get; private set; }
    }
}
