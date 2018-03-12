using ReseauxProtocol.NetworkManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.PacketManagement
{
   public class PacketManager
    {
        public Client Client { get; private set; }

        internal PacketManager(Client Client)
        {
            this.Client = Client;
        }

        public void ProcessPackets(IMessage[] NetworkMessages)
        {
            foreach (IMessage message in NetworkMessages)
            {
                ProcessPacket(message);
            }

            Client.ClearQueue();
        }

        public void ProcessPacket(IMessage NetworkMessage)
        {
            IPacket packetTemplate;
            if (!PacketRegistrator.Instance.PacketIn.TryGetValue(NetworkMessage.Header, out packetTemplate))
            {
                Console.WriteLine("Unhandled packet [" + NetworkMessage.Header.ToString() + "]. The packet is not supported by this server build");
                return;
            }

            if (packetTemplate.Initialize(ref NetworkMessage))
            {
               // Console.WriteLine();
               // Console.WriteLine("---> " + BitConverter.ToString(NetworkMessage.PacketData.ToArray()).Replace("-", " "));
                packetTemplate.Handle(ref NetworkMessage);
            }
        }
    }
}
