using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReseauxProtocol.T125Protocol
{

    public class BitEffect
    {
        protected BitArray _bit = null;


        protected BitEffect(byte Val)
        {
            _bit = new BitArray(new byte[] { Val });
        }


        public virtual byte Value
        {
            get
            {
                  int count = 0;
                  byte currentByte = 0;
                  foreach (bool b in _bit) 
                  {
                    if (b) currentByte |= (byte)(1 << count);
                  }

                  return currentByte;
            }
            
        }


    }

    public class FonctionBit : BitEffect
    {

        protected FonctionBit(byte val)
            : base(val)
        {
        }

        public bool Expanded
        {
            get
            {
                return _bit[6];
            }
            set
            {
                _bit[6] = value;
            }
        }

        public override byte Value
        {
            get
            {
                if (!Expanded)
                    return (byte)(base.Value | ((1 << 4) + (1 << 5)));
                else
                    return (byte)(base.Value | (1 << 6));
            }

        }

    }


    public class FCC_N : FonctionBit
    {
        public FCC_N(byte val)
            : base(val)
        {
        }

        public bool Blinking
        {
            get
            {
                return _bit[3];
            }
            set
            {
                _bit[3] = value;
            }
        }

        public bool Reverse
        {
            get
            {
                return _bit[4];
            }
            set
            {
                _bit[4] = value;
            }
        }

        public bool Kanji
        {
            get
            {
                return _bit[5];
            }
            set
            {
                _bit[5] = value;
            }
        }


        public bool Justification
        {
            get
            {
                if (_bit[0] && _bit[1])
                    return false;
                else
                    return _bit[2];
            }
            set
            {
                _bit[2] = value;
            }
        }

        public DataEntryCode DataEntry
        {
            get
            {
                byte res = 0;
                if (_bit[0])
                    res += 1;
                if (_bit[1])
                    res += 2;

                return (DataEntryCode)res;
            }
            set
            {
                byte res = (byte)value;
                if ((res == 1) || (res == 3))
                    _bit[0] = true;
                else
                    _bit[0] = false;

                if ((res == 3) || (res == 2))
                    _bit[1] = true;
                else
                    _bit[1] = false;
            }
        }
    }

    public class FCC_M : FonctionBit
    {
        public FCC_M(byte val)
            : base(val)
        {
        }

        public bool TabStop
        {
            get
            {
                return !_bit[3];
            }
            set
            {
                _bit[3] = !value;
            }
        }

        public bool ChangedData
        {
            get
            {
                return !_bit[2];
            }
            set
            {
                _bit[2] = !value;
            }
        }


        /// <summary>
        /// normal
        /// </summary>
        public IntensityCode Intensity
        {
            get
            {
                byte res = 0;
                if (_bit[0])
                    res += 1;
                if (_bit[1])
                    res += 2;

                return (IntensityCode)res;
            }
            set
            {
                byte res = (byte)value;
                _bit[0] = ((res == 1) || (res == 3));
                _bit[1] = ((res == 3) || (res == 2));
            }
        }

        /// <summary>
        /// Extended
        /// </summary>

        public bool Video
        {
            get
            {
                return !_bit[0];
            }
            set
            {
                _bit[0] = !value;
            }
        }


        public bool IntensityNormal
        {
            get
            {
                return !_bit[1];
            }
            set
            {
                _bit[1] = !value;
            }
        }

        public bool Protected
        {
            get
            {
                return _bit[5];
            }
            set
            {
                _bit[5] = value;
            }
        }

        public bool Kanji
        {
            get
            {
                return _bit[4];
            }
            set
            {
                _bit[4] = value;
            }
        }

    }


    public class FCC_Color
    {
        private Color _CT;
        private Color _CB;
        private bool _Valid = false;

        public bool IsValid
        {
            get
            {
                return _Valid;
            }
        }

        public FCC_Color()
        {
            _CT = Color.White;
            _CB = Color.Black;        
        }

        public FCC_Color(byte C)
        {

            _Valid = (((((C >> 6) & 0x01) == 1) && (((C >> 7) & 0x01) == 0)) || C == 0x3F);

            _CT = (Color)(C & 0x07);
            _CB = (Color)((C >> 3) & 0x07);
        }

        public byte Value
        {
            get
            {   
                byte currentByte = 0;
                if( !((_CT == Color.White) && (_CT == Color.White)))
                    currentByte += (1 << 6);

                currentByte += (byte)_CT;
                currentByte +=(byte) (((byte)_CB) << 3);
                return currentByte;
            }
        }

        public Color TextColor
        {
            get
            {
                return _CT;
            }
            set
            {
                _CT = value;
            }
        }

        public Color BackGroundColor
        {
            get
            {
                return _CB;
            }
            set
            {
                _CB = value;
            }
        }

   

    }
}
