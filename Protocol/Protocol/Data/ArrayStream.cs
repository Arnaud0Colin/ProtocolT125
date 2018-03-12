using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.Data
{
    public class ArrayStream<T> : DataReadFormat
    {
        protected T[] _Array;
        protected long _Position = 0;
        public T[] MyArray
        {
            get
            {
                return _Array;
            }
            set
            {
                _Array = value;
            }
        }

        public T this[int index]
        {
            get
            {
                return _Array[index];
            }
            set
            {
                _Array[index] = value;
            }
        }

        public int Length
        {
            get
            {
                if (_Array != null)
                    return _Array.Length;
                else
                    return 0;
            }         
        }

    }
}
