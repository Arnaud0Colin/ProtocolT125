
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ReseauxProtocol.T125Protocol.Old;

namespace ReseauxProtocol.T125Protocol
{
    public class Trames : List<Trame>
    {
    }


    public class Trame : CProtocole
    {

        byte[] _data = new byte[2] { 0x03, 0x00 };
        IProtocole _ISO;
        byte[] _End = null;


        ReceiveData _Data;
        public ReceiveData Data
        {
            get
            {
                return _Data;
            }
        }


        public override ushort Size
        {
            get
            {
                if (_IsSize)
                    return _size;
                else
                    return (ushort)(OwnSize + ((_ISO != null) ? _ISO.Size : 0) + ((_Data != null) ? _Data.Size : 0));
            }
            set
            {
                _IsSize = true;
                _size = value;
            }
        }
        public override ushort OwnSize { get { return (ushort) (4 + (_End != null ? _End.Count() : 0)); } }
        
        public PDU_TYPE PDU;

        public Trame()
        {
            _ISO = null;
            _Data = new ReceiveData();
        }

        public Trame(IProtocole ISO, ReceiveData Data, byte[] end )
        {
            _ISO = ISO;
            _Data = Data;
            _End = end;
        }

        public Trame(byte[] data)
        {
            ushort Index = 0;
            Index = Read( data, ref Index);
        }

     

        public byte Version
        {
            get
            {
                return _data[0];
            }
            set
            {
                _data[0] = value;
            }
        }

        public byte Reserved
        {
            get
            {
                return _data[1];
            }
            set
            {
                _data[1] = value;
            }
        }

        public void Fichier(string file, bool append = false)
        {
            StreamWriter _fs = null;

            if (_fs == null)
            {
                _fs = new StreamWriter(file, append);
            }
            ushort Index = 0;

            _fs.WriteLine("-------------------------------------------------------------------------------------------------------------------------------");

            Write(ref _fs, ref Index);

            _fs.Flush();

            _fs.Close();
            _fs = null;
        }

        public override ushort Write(ref StreamWriter SW, ref ushort Index)
        {
            if (SW == null)
                return 0;

            ushort count = Size;
            string stp = " Count = " + Size.ToString();
            SW.WriteLine(stp);

            SW.Write("{ ");
            SW.Write(_data[0].ToString("X2"));
            SW.Write(", ");
            SW.Write(_data[1].ToString("X2"));
            SW.Write(", ");
            SW.Write(((byte)(count >> 8)).ToString("X2"));
            SW.Write(", ");
            SW.Write(((byte)((ushort)count & (ushort)0xff)).ToString("X2"));
            SW.WriteLine("} ");

            ushort id = 3;
            if (_ISO != null)
                id = _ISO.Write(ref SW, ref id);

            if(_Data != null)
                id = _Data.Write(ref SW, ref id);


            SW.WriteLine("");
            SW.Write("{ ");

            if (_End != null)
                foreach (byte b in _End)
                {
                    SW.Write(b.ToString("X2"));
                    SW.Write(", ");
                }
            SW.WriteLine("} ");

            bool bok = (count == id);
            return Index;
        }



        public override ushort Write(ref byte[] buf, ref ushort Index)
        {
            ushort count = Size;
            buf = new byte[count];

            buf[0] = _data[0];
            buf[1] = _data[1];
            buf[2] = (byte)(count >> 8);
            buf[3] = (byte)((ushort)count & (ushort)0xff);

            ushort id = 3;
            id = _ISO.Write(ref buf, ref id);
            id = _Data.Write(ref buf, ref id);


            if (_End != null) 
            foreach( byte b in _End)
                buf[++id] = b;

            bool bok = (count == id);
            return Index;
        }

        [EndSize(3)]
        public override ushort Read(byte[] buf, ref ushort Index)
        {

            _data[0] = buf[Index + 0];
            _data[1] = buf[Index + 1];
            Size = (ushort)((buf[Index + 2] << 8) + buf[Index + 3]);

            byte count = buf[Index + 4];
            PDU = (PDU_TYPE)buf[Index + 5];


#if DEBUG
            Debug.rr2("Trame size =" + Size.ToString() + " and count = " + count + "  PDU = " + PDU.ToString());
#endif

            
            Index += 3;
            if( count > 6 )
                _ISO = new Request_COTP();
            else
                _ISO = new COTP();
           _ISO.Read(buf, ref Index);
        
           switch (PDU)
           {
               case PDU_TYPE.CONNECT_CONFIRMATION:
                   _Data = new ReceiveData();                
                   break;
               case PDU_TYPE.DATA_INDICATION:
                   if (_Data == null)
                       _Data = new ReceiveData();
                   break;             
               case PDU_TYPE.DISCONNECT_REQUEST:
                   _Data = null;
                   break;
               default:
                   _Data = null;
                   break;
           }

           int Calcul = Size - _ISO.Size - OwnSize - 1 -3;

           if (_Data != null)
           {
               //_Data.Size = (ushort)(Size - _ISO.Size - OwnSize - 1);
               _Data.Size = (ushort)Calcul;
               _Data.Read(buf, ref Index);
           }

            _End = new byte[3];

           _End[0] = buf[Index++];
           _End[1] = buf[Index++];
           _End[2] = buf[Index++];


           return Index;

        }

    }
}
