using ReseauxProtocol.NetworkManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.PacketManagement
{
    public interface IPacket
    {
        bool Initialize(ref IMessage Message);
        void Handle(ref IMessage Message);
    }
}
