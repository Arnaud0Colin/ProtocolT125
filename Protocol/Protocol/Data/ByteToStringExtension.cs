using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauxProtocol.Data
{
    public static class ByteToStringExtension
    {
       public static byte[] GetArray( this string p)
        {
            byte[] buf = new byte[p.Length / 2];
            int count = 0;
            for (int i = 0; i < p.Length; i = i + 2)
            {
                int h = 0;
                int l = 0;
                if (p[i] >= 'a')
                    h = (p[i] - 'a') + 10;
                else
                    h = (p[i] - '0');

                if (p[i + 1] >= 'a')
                    l = (p[i + 1] - 'a') + 10;
                else
                    l = (p[i + 1] - '0');

                byte g = (byte)((h << 4) + l);

                buf[count] = g;
                count++;

            }
            return buf;
        }


     

        public static string GetString( this byte[] buf)
        {
            string res = string.Empty;

            for (int i = 0; i < res.Length; i++)
            {
                res += buf[i].ToString("x2");
            }
            return res;
        }

        }

    }
