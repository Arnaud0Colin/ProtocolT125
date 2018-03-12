using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolT125.Data
{
    /// <summary>
    /// Class for convert string to array of bytes
    /// <example>  </example>/// 
    /// </summary>
    public static class ByteToString
    {

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static byte calcul(char x, char y)
        {
            int h = 0;
            int l = 0;
            if (x >= 'a')
                h = (x - 'a') + 10;
            else
                h = (x - '0');

            if (y >= 'a')
                l = (y - 'a') + 10;
            else
                l = (y - '0');

            return (byte)((h << 4) + l);
        }



#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if !WindowsCE
        private static int? ValidIndex(int Size, bool NeedOd, int Start = 0, int? length = null)
#else
        private static int? ValidIndex(int Size, bool NeedOd, int Start, int? length)
#endif
        {
            int _Length;

            if (length.HasValue)
                if (Start > Size || (NeedOd && (length % 2 != 0)) || (Size - Start - length.Value) <= 0)
                    return null;
                else
                    _Length = length.Value;
            else
                if (Start > Size || ((_Length = (Size - Start)) <= 0) || (NeedOd && (_Length % 2 != 0)))
                return null;

            return _Length;
        }



        /// <summary>
        /// <para>
        /// <param name="chaine">string</param>
        /// <param name="Start">int</param>
        /// <param name="length">int?</param>
        /// </para>
        /// convert the string in Byte[] 
        /// <example> ByteToString.GetArray("0104",0,4); </example>/// 
        /// <returns>Byte[]</returns>
        /// </summary>
#if !WindowsCE
        public static byte[] GetArray(string chaine, int Start = 0, int? length = null)
#else
         public static byte[] GetArray(string chaine, int Start , int? length )
#endif

        {
            int? _Length = null;

            if ((_Length = ValidIndex(chaine.Length, true, Start, length)) == null)
                return null;

            byte[] buf = new byte[_Length.Value / 2];

            int count = 0;
            for (int i = Start; i < _Length.Value; i = i + 2)
            {
                buf[count] = calcul(chaine[i], chaine[i + 1]);
                count++;
            }

            return buf;
        }


        /// <summary>
        /// <para>
        /// <param name="chaine">string</param>
        /// </para>
        /// convert the string in Byte[] 
        /// <example> ByteToString.GetArray("0104"); </example>/// 
        /// <returns>Byte[]</returns>
        /// </summary>
        public static byte[] GetArray(string chaine)
        {
            return GetArray(chaine, 0, null);
        }


        /// <summary>
        /// <para>
        /// <param name="buf">string</param>
        /// <param name="Start">int</param>
        /// <param name="length">int?</param>
        /// </para>
        /// convert a Byte[] in  string
        /// <example> ByteToString.GetString(new byte() { 0x01, 0x04 }, 0 , null ); </example>/// 
        /// <returns>string</returns>
        /// </summary>
#if !WindowsCE
        public static string GetString(this byte[] buf, int Start = 0, int? length = null)
#else
         public static string GetString(byte[] buf, int Start, int? length )
#endif

        {
            int? _Length = null;

            if ((_Length = ValidIndex(buf.Length, false, Start, length)) == null)
                return null;

            string res = string.Empty;

            for (int i = Start; i < _Length.Value; i++)
            {
                res += buf[i].ToString("x2");
            }
            return res;

        }


        /// <summary>
        /// <para>
        /// <param name="buf">string</param>
        /// </para>
        /// convert a Byte[] in  string
        /// <example> ByteToString.GetString(new byte() { 0x01, 0x04 }); </example>/// 
        /// <returns>string</returns>
        /// </summary>
        public static string GetString(byte[] buf)
        {
            string res = string.Empty;

            for (int i = 0; i < buf.Length; i++)
            {
                res += buf[i].ToString("x2");
            }
            return res;
        }


        /// <summary>
        /// <para>
        /// <param name="buf">string</param>
        /// <param name="length">int?</param>
        /// </para>
        /// convert a Byte[] in  string
        /// <example> ByteToString.GetString(new byte() { 0x01, 0x04 }, 2 ); </example>/// 
        /// <returns>string</returns>
        /// </summary>
        public static string GetString(byte[] buf, int length)
        {
            return GetString(buf, 0, length);
        }

    }
}
