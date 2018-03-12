//using CatchException;
//using Convertion;
using ProtocolT125.Data;
using ReseauxProtocol;
using ReseauxProtocol.T125Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WinTest
{
    public class ConnectionT125 : Connection
    {

        public ConnectionT125(string Address, short port) : base(Address, port)
        {
        }
        public void rr(string truc)
        {
            StreamWriter _fs = null;

            if (_fs == null)
            {
                _fs = new StreamWriter(@"c:\Trame.txt", true);
            }

            _fs.WriteLine(truc);

            _fs.Flush();

            _fs.Close();
            _fs = null;
        }


        public ReceiveData Connect3(string pid, string Article, short depot, short ste, string client, char key)
        {

            Socket ss = null;
            byte[] RcpBuf = new byte[2048];
            ReceiveData Result = null;

            if ((ss = Open()) == null)
                return Result;

            ss.ReceiveTimeout = 3000;

            int iRecu = 0;
            byte[] buf = null;
            ushort index = 0;
            qTermForm.GetTIPInit(pid).Write(ref buf, ref index);

            if (!Send(ref ss, ref buf, SocketFlags.None))
                return Result;

            while (true)
            {
                try
                {
                    iRecu = ss.ReceiveFrom(RcpBuf, ref _Server);
                }
                catch (SocketException)
                {
                    rr("Time Out");
                    goto Exit;
                }
                if (iRecu != 0)
                {
                    var tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu);
                    if (SendResponse(tRecu, ref Result) == null)
                        goto Exit;

                    rr(ByteToString.GetString(RcpBuf, iRecu));
                }
            }

            Exit:
            ss.Disconnect(true);
            ss = null;

            return Result;

        }

        private object SendResponse(List<Trame> t, ref ReceiveData Result)
        {
            throw new NotImplementedException();
        }

        //public delegate TResult Func<in T, out TResult>(T arg);

        const string  LogPath = @"C:\SourceCode\Projet\TFS\Bibliothèque\Protocol\Protocol.Dev\TestLog";
       static string GetFilePath(string file) => Path.Combine(LogPath, file);


        public ReceiveData Connect2(string pid, string Article, short depot, short ste, string client, char key)
        {

#if DEBUG
             bool erased = false;
#endif

            Socket ss = null;
            byte[] RcpBuf = new byte[1024*10];
            ReceiveData Result = null;

            if ((ss = Open()) == null)
                return Result;

            ss.ReceiveTimeout = 3000;

            List<Trame> tRecu;
            int iRecu = 0;
            byte[] buf = null;
            ushort index = 0;
            qTermForm.GetTIPInit(pid).Write(ref buf, ref index);

            if (!Send(ref ss, ref buf, SocketFlags.None))
                return Result;

            if ((iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.None)) == 0)
                goto Exit;

            if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu)) == null)
                goto Exit;
#if DEBUG
             foreach (Trame xx in tRecu)
                 xx.Fichier(GetFilePath("File1.txt"), erased);
#endif
            var gg = tRecu.Where(p => p.PDU == PDU_TYPE.CONNECT_CONFIRMATION).Count();

            if (tRecu.First().PDU == PDU_TYPE.CONNECT_CONFIRMATION)
            {

                if (tRecu.Count == 1)
                {
                    /* if (ss.Available > 0)
                     {*/
                    if ((iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.Partial)) == 0)
                        goto Exit;

                    if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu)) == null)
                        goto Exit;

#if DEBUG
                 foreach (Trame xx in tRecu)
                     xx.Fichier(GetFilePath("File2.txt"), erased);
#endif

                    // tRecu = new Trame(RcpBuf);
                }

                if (ss.Available > 0)
                {
                    iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.None);
                    if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu)) == null)
                        goto Exit;
#if DEBUG
                     foreach (Trame xx in tRecu)
                         xx.Fichier(GetFilePath("File3.txt"), erased);
#endif

                }
            }
            else
                goto Exit;

            index = 0;
            qTermForm.GetAppel("INTECM ").Write(ref buf, ref index);

            if (!Send(ref ss, ref buf, SocketFlags.None))
                goto Exit;

            if ((iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.None)) == 0)
                goto Exit;
            if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu)) == null)
                goto Exit;
#if DEBUG
             foreach (Trame xx in tRecu)
                 xx.Fichier(GetFilePath("File4.txt"), erased);
#endif

            if (ss.Available > 0)
            {
                if ((iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.None)) == 0)
                    goto Exit;

                if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu)) == null)
                    goto Exit;
#if DEBUG
                 foreach (Trame xx in tRecu)
                     xx.Fichier(GetFilePath("File5.txt"), erased);
#endif
            }

            var gssg = tRecu.First();

            if (gssg.Data.Count == 2 && iRecu < 200)
            {

                index = 0;
                qTermForm.GetClean(gssg.Data._Entete).Write(ref buf, ref index);
                if (!Send(ref ss, ref buf, SocketFlags.None))
                    goto Exit;


                index = 0;
                qTermForm.GetAppel("INTECM ").Write(ref buf, ref index);
                if (!Send(ref ss, ref buf, SocketFlags.None))
                    goto Exit;

                if ((iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.None)) == 0)
                    goto Exit;
                if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu)) == null)
                    goto Exit;
#if DEBUG
                 foreach (Trame xx in tRecu)
                     xx.Fichier(GetFilePath("File6.txt"), erased);
#endif
            }

            //Thread.Sleep(50);
            index = 0;
            INTECM.GetAction("38223 ", depot, ste,  key).Write(ref buf, ref index);
            if (!Send(ref ss, ref buf, SocketFlags.None))
                goto Exit;

            if ((iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.None)) == 0)
                goto Exit;

            if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu)) == null)
                goto Exit;
#if DEBUG
             foreach (Trame xx in tRecu)
                 xx.Fichier(GetFilePath("File7.txt"), erased);
#endif



            Result = tRecu.First().Data;

            if (Result.Count == 2)
            {
                //CatchMe.WriteMessage(Result.chant[0].Text).Where().Write();
            }

            if (ss.Available > 0)
            {
                if ((iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.None)) == 0)
                    goto Exit;

                if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu)) == null)
                    goto Exit;
#if DEBUG
                 foreach (Trame xx in tRecu)
                     xx.Fichier(GetFilePath("File8.txt"), erased);
#endif
            }


            //iRecu = Receive(ref ss, ref RcpBuf, SocketFlags.None);
            //tRecu = ReceiveField(ref RcpBuf);

            Exit:
            ss.Disconnect(true);
            ss = null;

            return Result;
        }
    }
}
