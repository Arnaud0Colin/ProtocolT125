using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReseauxProtocol.T125Protocol
{

    public enum PDU_TYPE : ushort
    {
        CONNECT_REQUEST = 0xe0,
        //CONNECT_INDICATION = 0x00,
        //CONNECT_RESPONSE  = 0x00,
        CONNECT_CONFIRMATION = 0xd0,
        //DATA_REQUEST  = 0x00,
        DATA_INDICATION = 0xf0,
        //DISCONNECT_INDICATION = 0x00,
        DISCONNECT_REQUEST = 0x80,
    };

    public enum TSAP_TYPE
    {
        SRC_TSAP = 0xc1,
        DST_TSAP = 0xc2
    };


    public enum DataEntryCode
    {
        Any = 0,
        Alpha = 1,
        Numeric = 2,
        Protected = 3
    }
    public enum IntensityCode
    {
        Normal = 0,
        Video_Off = 1,
        Low = 2,
        Reverce = 2,
        Blinking = 3
    }

    public enum Color
    {
        Black = 0,
        Red = 1,
        Green = 2,
        Yellow = 3,
        Blue = 4,
        Magentra = 5,
        Cyan = 6,
        White = 7
    }

    



    public enum CODE_FIELD
    {
        NULL = 0x00,
        SOH = 0x01,
        STX = 0x02,
        ETC = 0x03,
        EOT = 0x04,
        ENQ = 0x05,
        ACK = 0x06,
        BEL = 0x07,
        BS = 0x08,
        HT = 0x09,
        LF = 0x0a,
        VT = 0x0b,
        FF = 0x0c,
        CR = 0x0d,
        SO = 0x0e,
        SI = 0x0f,
        DLE = 0x10,
        DC1 = 0x11,
        DC2 = 0x12,
        DC3 = 0x13,
        DC4 = 0x14,
        NAK = 0x15,
        SYN = 0x16,
        ETB = 0x17,
        CAN = 0x18,
        EM = 0x19,
        SUB = 0x1a,
        ESC = 0x1b,
        FS = 0x1c,
        GS = 0x1d,
        RS = 0x1e,
        US = 0x1f,
        DEL = 0x7f
    
    };


    public interface IProtocole
    {
        ushort Size { get; set; }
        ushort OwnSize { get; }
        ushort Write(ref byte[] buf, ref ushort Index);
        ushort Read(byte[] buf, ref ushort Index);
        ushort Write(ref StreamWriter SW, ref ushort Index);
    }

    public abstract class CProtocole : IProtocole
    {
        protected ushort _size;
        protected bool _IsSize = false;
        public virtual ushort Size { get; set; }
        public abstract ushort OwnSize { get; }
        public abstract ushort Write(ref byte[] buf, ref ushort Index);
        public abstract ushort Read(byte[] buf, ref ushort Index);
        public abstract ushort Write(ref StreamWriter SW, ref ushort Index);
    }


    interface IData
    {

    }


 

}
