using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauxProtocol.T125Protocol.Old
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal sealed class EndSize : Attribute
    {
           /// <summary>
        /// <para>
        /// <param name="nb">ushort</param>
        /// </para>
        /// Constructor
        /// </summary>
        public EndSize(ushort size)
        {
            this.Size = size;
        }

        /// <summary>
        /// Number of digit
        /// <example>  </example>/// 
        /// <returns>ushort</returns>
        /// </summary>
        public ushort Size { get; private set; }
    }
}
