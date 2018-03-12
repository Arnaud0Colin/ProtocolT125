using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.Data
{
    public static class MemoryStreamExtension // : DataReadFormat
    {

        public static void init()
        {
            ReseauxProtocol.PacketManagement.PacketRegistrator.Instance.RegisterPackets(System.Reflection.Assembly.GetExecutingAssembly().GetTypes());
        }

        public static MemoryStream InitializeStream(this MemoryStream Stream)
        {
            Stream.Seek(0, SeekOrigin.Begin);
            return Stream;
        }

        public static ushort Toushort(this MemoryStream Stream)
        {
            return 0;// (ushort)ToInt16(ToBytes(Stream, 2));
        }

        public static UInt16 ToUInt16(this MemoryStream Stream)
        {
            return 0;//  (UInt16)ToInt16(ToBytes(Stream, 2));
        }

        public static UInt32 ToUInt32(this MemoryStream Stream)
        {
            return 0;//  (UInt32)ToUInt32(ToBytes(Stream, 4));
        }

        public static UInt64 ToUInt64(this MemoryStream Stream)
        {
            return 0;//  (UInt64)ToInt64(ToBytes(Stream, 8));
        }

        public static String ToString(this MemoryStream Stream, UInt16 Length)
        {
            return System.Text.Encoding.UTF8.GetString(ToBytes(Stream, Length), 0, Length);
        }

        public static Byte ToByte(this MemoryStream Stream)
        {
             return ToBytes(Stream, 1)[0];
        }

        public static Byte[] ToBytes(this MemoryStream Stream, UInt16 Length)
        {
            var buffer = new byte[Length];
            Stream.Read(buffer, 0, Length);
            return buffer;
        }

    }
}
