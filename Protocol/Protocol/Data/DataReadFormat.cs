using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.Data
{
   public class DataReadFormat
    {

        private  bool _IsLittleEndian = true;

        public  bool IsLittleEndian
        {
            get
            {
                return _IsLittleEndian;
            }
             set
            {
                _IsLittleEndian = value;
            }
        }


        protected  unsafe Int64 ToInt64(byte[] value)
        {
            int startIndex = 0;
            fixed (byte* numPtr = &value[startIndex])
            {
                if (IsLittleEndian)
                    return (long)(uint)((int)*numPtr | (int)numPtr[1] << 8 | (int)numPtr[2] << 16 | (int)numPtr[3] << 24) | (long)((int)numPtr[4] | (int)numPtr[5] << 8 | (int)numPtr[6] << 16 | (int)numPtr[7] << 24) << 32;
                int num = (int)*numPtr << 24 | (int)numPtr[1] << 16 | (int)numPtr[2] << 8 | (int)numPtr[3];
                return (long)((uint)((int)numPtr[4] << 24 | (int)numPtr[5] << 16 | (int)numPtr[6] << 8) | (uint)numPtr[7]) | (long)num << 32;
            }
        }

        protected unsafe Int32 ToInt32(byte[] value)
        {
            int startIndex = 0;
            fixed (byte* numPtr = &value[startIndex])
            {
                if (IsLittleEndian)
                    return (int)*numPtr | (int)numPtr[1] << 8 | (int)numPtr[2] << 16 | (int)numPtr[3] << 24;
                else
                    return (int)*numPtr << 24 | (int)numPtr[1] << 16 | (int)numPtr[2] << 8 | (int)numPtr[3];
            }
        }

        protected unsafe Int16 ToInt16(byte[] value)
        {
            int startIndex = 0;
            fixed (byte* numPtr = &value[startIndex])
            {
                if (IsLittleEndian)
                    return (short)((int)*numPtr | (int)numPtr[1] << 8);
                else
                    return (short)((int)*numPtr << 8 | (int)numPtr[1]);
            }
        }

    }
}
