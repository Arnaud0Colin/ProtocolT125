using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.NetworkManagement
{
      public class NetID
    {
        public int Value { get; private set; }

        public NetID(int Value)
        {
            this.Value = Value;
        }
    }
}
