using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ReseauxProtocol.NetworkManagement;

namespace ReseauxProtocol
{
    class ServerManager
    {
 
        IPAddress hostAddress;
        Int32 hostPort;

        TcpListener sListener;
        ClientManager sClients;
        NetIDManager idManager;
        bool sRuninng;

        static ServerManager instance;

        public event NetworkEventHandler ClientConnected;
        public event NetworkEventHandler ClientDisconnected;



        public bool Running { get { return sRuninng; } private set { sRuninng = value; } }
        public bool Pending { get { if (Running) { return sListener.Pending(); } else { return false; } } }
        public ClientManager Clients { get { return sClients; } }


        public ServerManager(String ServerAddress, Int32 ServerPort)
        {
            if (!IPAddress.TryParse(ServerAddress, out hostAddress))
                throw new ArgumentException("Cannot parse IP address, " + ServerAddress);

            if (ServerPort == 0)
            {
                hostPort = 6112;
            }
            else
            {
                hostPort = ServerPort;
            }

            idManager = new NetIDManager();
            sClients = new ClientManager();

            Start();
        }

        public void Start()
        {
            sListener = new TcpListener(hostAddress, hostPort);
            sListener.Start();
            sRuninng = true;
        }

        public void Stop()
        {
            sListener.Stop();
            sListener = null;
            sRuninng = false;
        }

        public static ServerManager Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public void ProcessActions()
        {
            if (Pending)
            {
                var client = new Client(sListener.AcceptTcpClient().Client, idManager.GenerateID());
                client.Connected += OnConnection;
                client.Disconnected += OnConnectionLost;
                sClients.AddClient(client);
            }

            foreach (Client client in sClients.ToArray())
            {
                client.CheckForIncoming();
                client.ProcessPackets();
                client.ClearQueue();
            }
        }

        private void OnConnection(NetID ID)
        {
            ClientConnected(ID);
        }

        private void OnConnectionLost(NetID ID)
        {
            ClientDisconnected(ID);
        }

    }
}
