using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReseauxProtocol.T125Protocol
{
    public class COTP : CProtocole
    {
        public PDU_TYPE PDU;
        public ushort Destination;
        public ushort Source;
        public byte Class;

        public override ushort Size
        {
            get
            {
                if (_IsSize)
                    return _size;
                else
                    return (ushort)(OwnSize);
            }
            set
            {
                _IsSize = true;
                _size = value;
            }
        }

        public override ushort OwnSize 
        { 
            get 
            {
                if (PDU != PDU_TYPE.DATA_INDICATION)
                {
                    return 7;
                }
                else
                {
                    return 3;
                }
            } 
        }
     
        public override ushort Read(byte[] buf, ref ushort Index)
        {
            Size = buf[++Index];
            Size++;
            PDU = (PDU_TYPE)buf[++Index];
            if (Size > 3)
            {
                Destination = (ushort)((buf[++Index] << 8) + buf[++Index]);
                Source = (ushort)((buf[++Index] << 8) + buf[++Index]);
                Class = buf[++Index];
            }
            else
                Class = buf[++Index];

            return Index;

        }

        public override ushort Write(ref byte[] buf, ref UInt16 Index)
        {

            if (PDU != PDU_TYPE.DATA_INDICATION)
            {
                buf[++Index] = (byte)(Size-1);
                buf[++Index] = (byte)PDU;
                buf[++Index] = (byte)(Destination >> 8);
                buf[++Index] = (byte)((ushort)Destination & (ushort)0xff);
                buf[++Index] = (byte)(Source >> 8);
                buf[++Index] = (byte)((ushort)Source & (ushort)0xff);
                buf[++Index] = Class;
            }
            else
            {
                buf[++Index] = (byte)(Size-1);
                buf[++Index] = (byte)PDU;
                buf[++Index] = Class;
            }
            return Index;
        }

        public override ushort Write(ref StreamWriter SW, ref UInt16 Index)
        {

            if (PDU != PDU_TYPE.DATA_INDICATION)
            {
                SW.Write("{ ");
                SW.Write(((byte)(Size - 1)).ToString("X2"));
                SW.Write(", ");
                SW.Write(((byte)PDU).ToString("X2"));
                SW.Write(", ");
                SW.Write(((byte)(Destination >> 8)).ToString("X2"));
                SW.Write(", ");
                SW.Write(((byte)((ushort)Destination & (ushort)0xff)).ToString("X2"));
                SW.Write(", ");
                SW.Write(((byte)(Source >> 8)).ToString("X2"));
                SW.Write(", ");
                SW.Write(((byte)((ushort)Source & (ushort)0xff)).ToString("X2"));
                SW.Write(", ");
                SW.Write((Class).ToString("X2"));
                SW.WriteLine("} ");
            }
            else
            {
                SW.Write("{ ");
                SW.Write(((byte)(Size - 1)).ToString("X2"));
                SW.Write(", ");
                SW.Write(((byte)PDU).ToString("X2"));
                SW.Write(", ");
                SW.Write((Class).ToString("X2"));
                SW.WriteLine("} ");

            }
            return Index;
        }
    }

    public class Request_COTP : COTP
    {
        public TSAP_TYPE SourceParameter;
        public string SourceTSAP;
        public TSAP_TYPE DestinationParameter;
        public string DestinationTSAP;


        public override ushort OwnSize
        {
            get
            {
                  return (ushort)( 7 + ((SourceTSAP != null) ? SourceTSAP.Length + 2 : 0) + ((DestinationTSAP != null) ? DestinationTSAP.Length + 2 : 0));
            }
        }


        public override ushort Write(ref byte[] buf, ref ushort Index)
        {

            base.Write(ref buf, ref Index);

            if (SourceTSAP != null)
            {
                buf[++Index] = (byte)SourceParameter;
                buf[++Index] = (byte)SourceTSAP.Length;
                for (byte i = 0; i < SourceTSAP.Length; i++)
                    buf[++Index] = (byte)SourceTSAP[i];
            }

            if (DestinationTSAP != null)
            {
                buf[++Index] = (byte)DestinationParameter;
                buf[++Index] = (byte)DestinationTSAP.Length;
                for (byte i = 0; i < DestinationTSAP.Length; i++)
                    buf[++Index] = (byte)DestinationTSAP[i];
            }

            return Index;
        }
        public override ushort Read(byte[] buf, ref ushort Index)
        {
            base.Read(buf, ref Index);

           SourceParameter = (TSAP_TYPE) buf[++Index];
           byte length = buf[++Index];
           for (byte i = 0; i < length; i++)
               DestinationTSAP += (char)buf[++Index];

          DestinationParameter = (TSAP_TYPE)buf[++Index];
           length = buf[++Index];
           for (byte i = 0; i < length; i++)
               SourceTSAP += (char)buf[++Index];

         return Index;
        }
    }


}
