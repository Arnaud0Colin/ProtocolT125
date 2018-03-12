using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.PacketManagement
{
    public class PacketRegistrator
    {

        public Dictionary<UInt16, IPacket> PacketIn { get; private set; }
        public Dictionary<UInt16, IPacket> PacketOut { get; private set; }

        private static readonly PacketRegistrator instance = new PacketRegistrator();

        public static PacketRegistrator Instance
        {
            get
            {
                return instance;
            }
        }

        public PacketRegistrator()
        {
            PacketIn = new Dictionary<UInt16, IPacket>();
            PacketOut = new Dictionary<UInt16, IPacket>();
        }

        public void RegisterPacket(Type PacketType)
        {
            var isFromClient = false;
            var header = -1;
            var attributes = PacketType.GetCustomAttributes(typeof(PacketInformation), false);
            if (attributes.Length == 1)
            {
                isFromClient = ((PacketInformation)attributes[0]).IsIncoming;
                header = ((PacketInformation)attributes[0]).Header;
            }

            if (!typeof(IPacket).IsAssignableFrom(PacketType))
            {
                return;
            }

            var packet = (IPacket)Activator.CreateInstance(PacketType);

            if (typeof(IPacket).IsAssignableFrom(packet.GetType()))
            {
                if (header >= 0 && header <= 65535)
                {
                    if (isFromClient)
                        PacketIn.Add(Convert.ToUInt16(header), packet);
                    else
                        PacketOut.Add(Convert.ToUInt16(header), packet);
                }
                else
                {
                    Console.WriteLine("Invalid packet header in class " + packet.GetType().Name);
                }
            }
        }

        public void RegisterPackets(Type[] PacketTypes)
        {
            foreach (Type pType in PacketTypes)
            {
                RegisterPacket(pType);
            }
        }
    }
}
