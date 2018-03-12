using ReseauxProtocol.T125Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTest
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var mainfraime = new ConnectionT125("127.0.0.1", 102);

            mainfraime.Connect2("pid807", "05600", 170, 4, null, ' ');


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());






        }
    }
}
