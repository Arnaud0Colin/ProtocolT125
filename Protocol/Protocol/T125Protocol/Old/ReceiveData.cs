using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ReseauxProtocol.T125Protocol.Old;

namespace ReseauxProtocol.T125Protocol
{

    public class ReceiveData : CProtocole
    {

        public CODE_FIELD Head;

        public ReceiveData()
        {
            _Entete = new List<byte>();
        }

        public ReceiveData(List<byte> entete)
        {
            _Entete = entete;
        }


        public List<byte> _Entete = null;
        public List<Field> chant = new List<Field>();

        public byte controle()
        {
            int var = 0;
            bool bcount = false;
            foreach (Field fld in chant)
            {
                if (bcount)
                    var += fld.Size;

                if (fld.IsControl)
                {
                    bcount = true;
                    var += fld.TestSize;
                }            
            }
           // var -= 3; TEST ARNAUD 

            return (byte)var;
        }


        public string GetData(short x, short y)
        {

            for (int i = 0; i < chant.Count; i++)
                if ((x == chant[i].X) && (y == chant[i].Y))
                {
                    if (chant[i].Text != null)
                        return chant[i].Text;
                    else
                    {
                        var chant2 = (Field)null;
                        if ( (chant2 = chant[i + 1]) != null)
                        {
                            if ((chant2.X == null) && (chant2.Y == null))
                            {
                                string stp = chant[i + 1].Text != null ? chant[i + 1].Text.Trim() : null;
                                if ((stp != null) && (stp.Length > 0))
                                    return stp;                          
                            }
                        }

                           return null;
                    }
                }
            
            return null;
        }

        public int Count
        {
            get
            {
                return chant.Count;
            }
        }

        public override ushort Size
        {
            get
            {
                if (_IsSize)
                    return _size;
                else
                    return (ushort)(OwnSize);
            }
            set
            {
                _IsSize = true;
                _size = value;
            }
        }
        public override ushort OwnSize { get { return (ushort)(_Entete.Count + chant.Sum(p => p.Size) ); } }
        public override ushort Write(ref byte[] buf, ref ushort Index)
        {

            for (int i = 0; i < _Entete.Count; i++)
                buf[++Index] = _Entete[i];

            foreach (Field v in chant)
                v.Write(ref  buf, ref  Index);

            return Index;

        }


        public override ushort Write(ref StreamWriter SW, ref UInt16 Index)
        {
            SW.Write("{ ");
            for (int i = 0; i < _Entete.Count; i++)
            {
                if(i > 0 )
                    SW.Write(", ");
                SW.Write((_Entete[i]).ToString("X2"));
            }            
            SW.WriteLine("} ");

            foreach (Field v in chant)
                v.Write(ref  SW, ref  Index);


            return Index;
        }


        public override ushort Read(byte[] buf, ref ushort Index)
        {
            Index++;
            Field oo = null;
            bool bEntete = true;
            ushort ind = 5;
            ushort Read = 0;
            for (int i = 0; i <= Size; i++)
            {
                Read++;
                byte cur = buf[Index + i];

                if (((cur == (byte)CODE_FIELD.CR) || 
                     (cur == (byte)CODE_FIELD.US) ||
                     (cur == (byte)CODE_FIELD.EM) || 
                     (cur == (byte)0xf2) || 
                     (cur == (byte)0xf1) || 
                     (cur == (byte)CODE_FIELD.ESC)) && (oo == null || !oo.IsCommand))
                {
                    bEntete = false;
                    if (oo != null)
                        chant.Add(oo);
                    oo = new Field();
                    ind = 0;
                    oo.Add2(cur, ind);
                }
                else
                    if (bEntete)
                        _Entete.Add(cur);
                    else
                        oo.Add2(cur, ind);

                ind++;
            }

            if (oo != null)
                chant.Add(oo);

            Index += Size;
            Index++;

            return Read;

        }

   
    }
}
