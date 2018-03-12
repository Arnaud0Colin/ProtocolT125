using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.T125Protocol
{
    public class Debug
    {
        public static string Filename { get; set; }

        public static void rr(string truc, bool append = true)
        {
            if (Filename != null)
            {
                StreamWriter _fs = null;

                if (_fs == null)
                {
                    _fs = new StreamWriter(Filename, append);
                }

                _fs.WriteLine("Size = {0} => {1}", truc.Length / 2, truc);

                _fs.Flush();

                _fs.Close();
                _fs = null;
            }
        }

        public static void rr2(string truc, bool append = true)
        {
            if (Filename != null)
            {
                StreamWriter _fs = null;

                if (_fs == null)
                {
                    _fs = new StreamWriter(Filename, append);
                }

                _fs.WriteLine(truc);

                _fs.Flush();

                _fs.Close();
                _fs = null;
            }
        }
    }
}
