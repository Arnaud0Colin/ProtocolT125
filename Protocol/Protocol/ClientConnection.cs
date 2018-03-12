using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ReseauxProtocol
{

    public abstract class ClientConnection
    {
        protected string _Address;
        protected short _port;

        public ClientConnection(string Address, short port)
        {
            _Address = Address;
            _port = port;
        }

        public Socket Open()
        {
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(_Address, _port);
            }
            catch (SocketException ex)
            {
                //CatchException.CatchMe.WriteException(ex).Where().Write();
            }
            return socket;
        }

        protected int Receive(ref Socket server, ref byte[] buf, SocketFlags flag)
        {
            try
            {
                return server.Receive(buf, flag);
            }
            catch (SocketException ex)
            {
                //CatchException.CatchMe.WriteException(ex).Where().Write();
                return 0;
            }
        }

        protected bool Send(ref Socket server, ref byte[] buf, SocketFlags flag)
        {
            try
            {
                server.Send(buf, buf.Length, flag);
            }
            catch (SocketException ex)
            {
                //CatchException.CatchMe.WriteException(ex).Where().Write();
                return false;
            }
            return true;
        }

        /*
       protected T ReceiveField<T>(ref byte[] buf) where T : CProtocole, new()
        {
            T tRecu = new T();
            ushort Index = 0;
            tRecu.Read(buf, ref Index);

            return tRecu;
        }
        */
    }
}
