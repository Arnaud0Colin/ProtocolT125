using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
//using CatchException;

namespace ReseauxProtocol.T125Protocol
{
    public abstract class Connection
    {
        protected string _Address;
        protected short _port;
        protected EndPoint _Server;

        public Connection(string Address, short port)
        {
            _Address = Address;
            _port = port;
            _Server = (EndPoint)new IPEndPoint(System.Net.IPAddress.Parse(Address), 0);
        }

        public Socket Open()
        {
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                socket.Connect(_Address, _port);
            }
            catch (SocketException ex)
            {
                //CatchMe.WriteException(ex).Where().Write();                
            }
            return socket;
        }

 
        /*
        public bool TimeOut(ref Socket server, int time)
        {
            if (server != null)
            {
                int sockopt = -1;

                if ((sockopt = (int)server.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout)) == time)
                    return true;

                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, time);
                return ( time == (int)server.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout));
            }

            return false;
        }
        */
    

        protected  int ReceiveFrom(ref Socket server, ref byte[] buf, SocketFlags flag )
        {
            try
            {
               // IPEndPoint sender = new IPEndPoint(System.Net.IPAddress.Any, 0);
                //EndPoint tmpRemote = (EndPoint)sender;

                int count = server.ReceiveFrom(buf, ref _Server);
#if DEBUG
              // Debug.rr( ByteToString.GetString(buf, count)); 
#endif

               return count;

            }
            catch (SocketException ex)
            {
                //CatchMe.WriteException(ex).Where().Write();
                return 0;
            }
        }

       protected  int Receive(ref Socket server, ref byte[] buf, SocketFlags flag )
        {
            try
            {
                    return server.Receive(buf, flag);
            }
            catch (SocketException ex)
            {
                //CatchMe.WriteException(ex).Where().Write();
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
                //CatchMe.WriteException(ex).Where().Write();
                return false;
            }
            return true;
        }

        /*
       protected T ReceiveField<T>(ref byte[] buf) where T : CProtocole, new()
        {
            T tRecu = new T();
            ushort Index = 0;
           ushort read = tRecu.Read(buf, ref Index);

            return tRecu;
        }
         * */

       public static List<T> ReceiveField<T>(ref byte[] buf, int size) where T : CProtocole, new()
       {
           List<T> tRecu = null;
           //return tRecu;
           ushort Index = 0;
           ushort Cur = 0;
           ushort Stop = 0;
           while (Index < size)
           {
               Stop = Index;
               var r = new T();
               Cur = r.Read(buf, ref Index);
               if (Cur != 0)
               {
                   if (tRecu == null)
                       tRecu = new List<T>();

                   tRecu.Add(r);
               }

               if (Stop == Index)
                   break;
           }
//           ushort read = tRecu.Read(buf, ref Index);
           return tRecu;
       }

        /*
       public static List<Trame> Trames(byte[] data, int size)
       {
           List<Trame> Result = null;
           ushort Index = 0;
           ushort Cur = 0;
           ushort Stop = 0;
           while (Index < size)
           {
               Stop = Index;
               var r = new Trame(null, null, null);
               Cur = r.Read(data, ref Index);
               if (Cur != 0)
               {
                   if (Result == null)
                       Result = new List<Trame>();

                   Result.Add(r);
               }

               if (Stop == Index)
                   break;
           }

           return Result;
       }
        */

    }   
}
