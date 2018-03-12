using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReseauxProtocol.NetworkManagement;

namespace ReseauxProtocol
{
    public class NetIDManager
    {
        List<NetID> idList;

        public NetIDManager()
        {
            idList = new List<NetID>();
        }

        public NetID GenerateID()
        {
            if (idList.Count < 10000 && idList.Count >= 0)
            {
                Random rGen = new Random();
                int num = rGen.Next(0, 10000);

                while (idList.Contains(new NetID(num)))
                {
                    num = rGen.Next(0, 10000);
                }

                NetID tmp = new NetID(num);
                idList.Add(tmp);

                return tmp;
            }

            else
            {
                return new NetID(-1);
            }
        }

        public NetID[] List { get { return idList.ToArray(); } }
    }
}
