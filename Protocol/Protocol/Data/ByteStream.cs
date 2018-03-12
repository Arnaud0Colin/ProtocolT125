using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.Data
{
    public class ByteStream : ArrayStream<byte> 
    {
      
        public ByteStream InitializeStream()
        {
            _Position = 0;
            return this;
        }

        public void Seek(long pos)
        {
            _Position = pos;
        }

        public byte Tobyte(int? pos = null)
        {
             long pTemp = _Position;
            if (pos.HasValue)
                pTemp = pos.Value;

            if (!pos.HasValue)
                _Position += 1;
            return _Array[pTemp];
        }

        public bool ToBool(int? pos = null)
        {
            return (Tobyte(pos) != 0 );
        }

        public short ToInt16(int? pos = null)
        {
            return ToInt16(ToBytes(2, pos));
        }

        public Int32 ToInt32(int? pos = null)
        {
            return ToInt32(ToBytes(4, pos));
        }

        public Int64 ToInt64(int? pos = null)
        {
            return ToInt64(ToBytes(8, pos));
        }

    
        public  UInt16 ToUInt16( int? pos = null)
        {
            return (ushort)ToInt16(ToBytes(2, pos));
        }

        public UInt32 ToUInt32(int? pos = null)
        {
            return (UInt32)ToInt32(ToBytes(4, pos)); 
        }

        public UInt64 ToUInt64(int? pos = null)
        {
            return (UInt64)ToInt64(ToBytes(8, pos)); 
        }

        public String ToString(UInt16 Length, int? pos = null)
        {
            return System.Text.Encoding.UTF8.GetString(ToBytes(Length, pos), 0, Length);
        }

        public Byte[] ToBytes(UInt16 Length, int? pos = null)
        {
            long pTemp = _Position;
            if (pos.HasValue)
                pTemp = pos.Value;

            var buffer = new byte[Length];
            for (int i = 0; i < Length; i++)
                buffer[i] = _Array[pTemp + i];

            if (!pos.HasValue)
                _Position += Length;
            return buffer;
        }

        int SetAtCursor<T>(T rr)
        {
            return 0;
        }

        T GetAtCursor<T>()
        {
            return default(T);
        }

    }

   


}
