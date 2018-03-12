using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReseauxProtocol.T125Protocol
{
   
    public class BackField
    {
        public bool IsColor
        {
            get
            {
                return _IsColor;
            }
            set
            {
                if (value)
                    _IsColor = true;
            }
        }

        protected bool _IsColor = false;

        public bool IsCommand = false;
        public bool IsNeedFin = false;
        public ushort nbCommand = 0;
    }

    public class Field : BackField
    {
        CODE_FIELD code;
        public List<byte> Before = null;
        public List<byte> After = null;

        public string Text;
        public ushort TestSize { get { return (ushort)((Text != null) ? Text.Length : 0); } }
        public ushort Size { get { return (ushort)(((Text != null) ? Text.Length : 0) + ((Before != null) ? Before.Count : 0) + ((After != null) ? After.Count : 0)); } }

        public bool IsControl = false;


        public Field()
            {
                Text = null;
                Before = new List<byte>();
                After = new List<byte>();
            }

        public FCC_Color C1
        {
            get
            {
                if (IsColor && O != 22)
                {
                    switch (code)
                    {
                        case CODE_FIELD.US:
                            return new FCC_Color(Before[6]);
                        case CODE_FIELD.EM:
                            return new FCC_Color(Before[4]);
                    }                
                }

                return null;
            }
        }

        public FCC_Color C2
        {
            get
            {
                if (IsColor && O > 21 & O < 24)
                {
                    switch (code)
                    {
                        case CODE_FIELD.US:
                            return new FCC_Color(Before[7]);
                        case CODE_FIELD.EM:
                            return new FCC_Color(Before[5]);
                    }
                }

                return null;
            }
        }

        public FCC_M M
        {
            get
            {
                switch (code)
                {
                    case CODE_FIELD.US:
                        if( IsColor)
                            return new FCC_M(Before[4]);
                        else
                            return new FCC_M(Before[3]);
                    case CODE_FIELD.EM:
                        if (IsColor)
                            return new FCC_M(Before[2]);
                        else
                            return new FCC_M(Before[1]);
                }
                return null;
            }
        }

        public byte O
        {
            get
            {
                switch (code)
                {
                    case CODE_FIELD.US:
                        if (IsColor)
                            return Before[3];
                        break;
                    case CODE_FIELD.EM:
                        if (IsColor)
                            return Before[1];
                        break;
                }
                return 0;
            }
        }

        public FCC_N N
        {
            get
            {
                switch (code)
                {
                    case CODE_FIELD.US:
                        if (IsColor)
                            return new FCC_N(Before[5]);
                        else
                            return new FCC_N(Before[4]);
                    case CODE_FIELD.EM:
                        if (IsColor)
                            return new FCC_N(Before[3]);
                        else
                            return new FCC_N(Before[2]);
                }
                return null;
            }
        }


        public short? X
        {
            get
            {
                switch( code)
                {
                    case CODE_FIELD.US:
                        return (short)(Before[2] - 0x1F);
                    case CODE_FIELD.ESC:
                        if (Before.Count >= 5 && Before[1] == (byte)CODE_FIELD.VT)
                            return (short)(Before[3] - 0x1F);
                        break;
                }
                return null;
            }
        }

        public short? Y
        {
            get
            {
                switch (code)
                {
                    case CODE_FIELD.US:
                        return (short)(Before[1] - 0x1F);
                    case CODE_FIELD.ESC:
                        if (Before.Count >= 5 && Before[1] == (byte)CODE_FIELD.VT)
                            return (short)(Before[2] - 0x1F);
                        break;
                }
                return null;
            }
        }


        public void Entete(byte[] arrbyte)
        {
            foreach (byte b in arrbyte)
                Before.Add(b);
        }

        public ushort Write(ref byte[] buf, ref ushort Index)
        {

            if (Before != null)
                for (int i = 0; i < Before.Count; i++)
                    buf[++Index] = (byte)Before[i];


            if (Text != null)
                for (int i = 0; i < Text.Length; i++)
                    buf[++Index] = (byte)Text[i];


            if (After != null)
                for (int i = 0; i < After.Count; i++)
                    buf[++Index] = (byte)After[i];

            return Index;
        }


        public  ushort Write(ref StreamWriter SW, ref UInt16 Index)
        {
            if (Before != null)
            {
                SW.Write("Before = { ");
                for (int i = 0; i < Before.Count; i++)
                {
                    if (i > 0)
                        SW.Write(", ");
                    SW.Write((Before[i]).ToString("X2"));
                }
                SW.Write("} ");

                if ((X > 0) && (Y > 0))
                {
                    string stp = " x=" + X.ToString() + " y=" + Y.ToString() ;
                    SW.Write(stp);
                }
            }

              if (Text != null)
                {
                    SW.Write(" => ");
                    SW.WriteLine(Text);
                }
              else
                  SW.WriteLine("");

              if (Before != null)
              {
                if (M != null)
                {
                    string stp;
                    if(M.Expanded)
                    {
                        
                        stp = " M => Changed =" + M.ChangedData.ToString() + 
                            " Video =" + M.Video.ToString()+
                            " IntensityNormal =" + M.IntensityNormal.ToString() +
                            " Protected =" + M.Protected.ToString();
                    }
                    else
                    {
                        stp = " M => Changed =" + M.ChangedData.ToString() +  " " + M.Intensity.ToString();
                    }
                    SW.WriteLine(stp);
                }
                if (N != null)
                {
                    string stp;
                    if (N.Expanded)
                    {
                         stp = " N => Justification =" + N.Justification.ToString() +
                             " Blinking =" + N.Blinking.ToString() +
                             " Reverse =" + N.Reverse.ToString() +
                             " " + N.DataEntry.ToString();
                    }
                    else
                    {
                        stp = " N => Justification =" + N.Justification.ToString() + " " + N.DataEntry.ToString();
                    }
                    SW.WriteLine(stp);
               }
                SW.WriteLine("");
            }

          

            if (After != null)
            {
                SW.Write(" After = { ");
                for (int i = 0; i < After.Count; i++)
                {
                    if (i > 0)
                        SW.Write(", ");
                    SW.Write((After[i]).ToString("X2"));
                }
                SW.WriteLine("} ");
            }

            return Index;
        }

        public static bool IsNewField(byte cur,  Field oo)
        {
            bool bStartCode = 
                 ((cur == (byte)CODE_FIELD.CR) ||
                  (cur == (byte)CODE_FIELD.US) ||
                  (cur == (byte)CODE_FIELD.EM) ||
                  (cur == (byte)0xf2) ||
                  (cur == (byte)0xf1) ||
                  (cur == (byte)CODE_FIELD.ESC));

            return (bStartCode && (oo == null || !oo.IsCommand));
        }

        public void Add2(byte cur, ushort ind)
        {
             if (ind == 0)
                code = (CODE_FIELD)cur;

            int iAction = -2;
            switch (code)
            {
                case CODE_FIELD.CR:
                    iAction = IsEnteteCR(cur, ind);
                    break;
                case CODE_FIELD.US:
                    iAction = IsEnteteUS( cur, ind);
                    break;
                case CODE_FIELD.EM:
                    iAction = IsEnteteEM(cur, ind);
                    break;
                case CODE_FIELD.ESC:
                    iAction = IsEnteteESC(cur, ind);
                    break;
                case CODE_FIELD.SO:
                    iAction = IsEnteteSO(cur, ind);
                    break;
                case CODE_FIELD.DC1:
                case CODE_FIELD.DC2:
                case CODE_FIELD.DC3:
                case CODE_FIELD.DC4:
                    iAction = IsEnteteDC(cur, ind);
                    break;
            }

            if (iAction == -1)
                Before.Add(cur);
            else
            {
                if (cur > (byte)CODE_FIELD.SI && cur < (byte)CODE_FIELD.DEL)
                    Text += (char)cur;
                else
                    if ((Text != null) && (Text.Length > 0))
                        After.Add(cur);
                    else
                        Before.Add(cur);
            }
        }

        private int IsEnteteDC(byte cur, ushort ind)
        {
            if (ind == 0)
            {
                nbCommand = 2;
                IsCommand = true;
                return -1;
            }
            else
                if (IsCommand && (ind >= nbCommand))
                {
                    IsCommand = false;
                    nbCommand = 0;
                    return -1;
                }
                else
                {
                    return 0;
                }
        }

        private int IsEnteteSO(byte cur, ushort ind)
        {
            if (ind == 0)
            {
                nbCommand = 3;
                IsCommand = true;
                return -1;
            }
            else
            if (IsCommand && (ind >= nbCommand))
            {
                IsCommand = false;
                nbCommand = 0;
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private int IsEnteteESC(byte cur, ushort ind)
        {
            if (ind == 0)
            {
                nbCommand = 1;
                IsCommand = true;
                return -1;
            }
            else if (ind == 1 && ((cur == (byte)CODE_FIELD.VT) || (cur == (byte)CODE_FIELD.HT)))
            {
                IsNeedFin = true;
                IsCommand = true;             
                return -1;
            }
            else if (cur == (byte)CODE_FIELD.SI)
                        {
                            IsCommand = false;
                            IsNeedFin = false;
                            nbCommand = 0;
                            return -1;
                        }
            else if (ind == 1)
                 {
                    switch (cur)
                    {
                        case (byte)'Z':
                        case (byte)'Y':
                            IsCommand = true;
                            nbCommand = 2;
                            break;
                        case (byte)CODE_FIELD.SO:
                            IsCommand = true;
                            nbCommand = 4;
                            break;
                        case (byte)'X':
                            IsCommand = true;
                            nbCommand = 2;
                            break;
                        case (byte)'W':
                            IsCommand = true;
                            nbCommand = 4;
                            break;
                        default:
                            IsCommand = false;
                            nbCommand = 0;
                            break;
                    }
                    return -1;
                }
            else
                if (IsNeedFin)
                {
                    return -1;
                }
                else
                if (IsCommand && (ind >= nbCommand))
                {
                    IsCommand = false;
                    nbCommand = 0;
                    return -1;
                }
                else
                {
                  return 0;
                }
           
        }

        private int IsEnteteCR(byte cur, ushort ind)
        {
            nbCommand = 0;
            IsCommand = false;

            if (ind == 0)
                return -1;
            else
                return 0;

        }

        int IsEnteteUS(byte cur, ushort ind)
        {
            if (ind == 0)
            {
                nbCommand = 4;
                IsCommand = true;
                return -1;
            }
            else
            if (IsCommand && (ind <= nbCommand))
            {
                if ((cur > 0x1F) && (cur < 0x30)  && ind == 3)
                {
                    nbCommand = 7;
                    IsColor = true;
                }

                if (ind == nbCommand)
                {
                    IsCommand = false;
                    nbCommand = 0;
                }

                return -1;
            }

            return 0;
        }

        int IsEnteteEM(byte cur, ushort ind)
        {
            if (ind == 0)
            {
                nbCommand = 2;
                IsCommand = true;
                return -1;
            }
            else
                if (IsCommand && (ind <= nbCommand))
                {
                    if ((cur > 0x1F) && (cur < 0x30) && ind == 1)
                    {
                        nbCommand = 4;
                        IsColor = true;
                    }

                    if (ind == nbCommand)
                    {
                        IsCommand = false;
                        nbCommand = 0;
                    }

                    return -1;
                }

            return 0;
        }


        /*
        public void Addd(byte cur, ushort ind)
        {
           
            if (ind == 0)
            {
                code = (CODE_FIELD)cur;
                if (code == CODE_FIELD.US)
                    IsCommand = true;
                if (code == CODE_FIELD.EM)
                    IsCommand = true;
                Before.Add(cur);
                return;
            }
            else               
                if (code == CODE_FIELD.US && IsCommand && ((ind < 5 && !IsColor) || (ind < 8 && IsColor) ))
                {
                    if(cur == 'O' && ind== 3 )
                        IsColor = true;

                    if (((ind == 4 && !IsColor) || (ind == 7 && IsColor)))
                        IsCommand = false;

                    Before.Add(cur);
                    return;
                }
                else
                    if (ind == 1 && ((cur == (byte)CODE_FIELD.VT) || (cur == (byte)CODE_FIELD.HT)))
                    {
                        if (code == CODE_FIELD.ESC)
                            IsNeedFin = true;

                        IsCommand = true;
                        IsColor = false;
                    }
                    else
                        if (cur == (byte)CODE_FIELD.SI)
                        {
                            IsCommand = false;
                            IsNeedFin = false;
                             IsColor = false;
                        }
                        else
                        if (code == CODE_FIELD.EM && IsCommand && ((ind < 5 && !IsColor) || (ind < 6 && IsColor) ))
                        {
                             if(cur == 'O' && ind== 1 )
                                IsColor = true;

                            Before.Add(cur);
                            return;
                        }

            if (code == CODE_FIELD.ESC && cur == 'Z' && ind == 1)
                IsCommand = true;

            if (code == CODE_FIELD.ESC && cur == 'Y' && ind == 1)
                IsCommand = true;

            if (code == CODE_FIELD.ESC && (cur == (byte)CODE_FIELD.SO) && ind == 1)
                IsCommand = true;

            if (code == CODE_FIELD.ESC && ind > 2 && !IsNeedFin)
                IsCommand = false;

            if (IsCommand || (code == CODE_FIELD.ESC && ind == 1)) // ||  (ind == 1 && ((cur == 0x65) || (cur == 0x66) || (cur == 0x67) || (cur == 0x68) || (cur == 0x69) || (cur == 0x7a))))
                Before.Add(cur);
            else
                if (cur > (byte)CODE_FIELD.SI && cur < (byte)CODE_FIELD.DEL)
                    Text += (char)cur;
                else
                    if ((Text != null) && (Text.Length > 0))
                        After.Add(cur);
                    else
                        Before.Add(cur);


            

        }
        */

        public Field CallInit(byte func, string Name, byte? End)
        {
            Before.Add(func);
            Before.Add((byte)Name.Length);
            Text = Name;
            if(End.HasValue)
                After.Add(End.Value);

            return this;
        }

        public Field Cursor(byte y, byte x, int p4)
        {
            Before.Add((byte)CODE_FIELD.ESC);
            Before.Add((byte)CODE_FIELD.VT);
            Before.Add((byte)(y + 0x1F));
            Before.Add((byte)(x + 0x1F));
            Before.Add((byte)(0x00));
            Before.Add((byte)CODE_FIELD.SI);

            return this;
        }

        public Field Sequence( byte m, byte n, FCC_Color C1, FCC_Color C2)
        {
            Before.Add((byte)CODE_FIELD.EM);
            Before.Add(m);
            Before.Add(n);
            Before.Add(C1.Value);
            Before.Add(C2.Value);
            return this;
        }

        public Field Sequence(byte m, byte n)
        {
            Before.Add((byte)CODE_FIELD.EM);
            Before.Add(m);
            Before.Add(n);
            return this;
        }

        public Field Sequence(FCC_M m, FCC_N n, FCC_Color C1, FCC_Color C2)
        {
            Before.Add((byte)CODE_FIELD.EM);
            Before.Add(m.Value);
            Before.Add(n.Value);
            Before.Add(C1.Value);
            Before.Add(C2.Value);
            return this;
        }

        public Field Sequence(FCC_M m, FCC_N n)
        {
            Before.Add((byte)CODE_FIELD.EM);
            Before.Add(m.Value);
            Before.Add(n.Value);
            return this;
        }


        public Field Sequence(byte y, byte x, byte m, byte n, FCC_Color C1, FCC_Color C2)
        {
            Before.Add((byte)CODE_FIELD.US);
            Before.Add((byte)(y + 0x1F));
            Before.Add((byte)(x + 0x1F));
            Before.Add((byte)0x4F);
            Before.Add(m);
            Before.Add(n);
            Before.Add(C1.Value);
            Before.Add(C2.Value);
            return this;
        }

        public Field Sequence(byte y, byte x, FCC_M m, FCC_N n, FCC_Color C1, FCC_Color C2)
        {
            Before.Add((byte)CODE_FIELD.US);
            Before.Add((byte)(y + 0x1F));
            Before.Add((byte)(x + 0x1F));
            Before.Add((byte)0x4F);
            Before.Add(m.Value);
            Before.Add(n.Value);
            Before.Add(C1.Value);
            Before.Add(C2.Value);
            return this;
        }


        public Field Sequence(byte y, byte x, byte m, byte n)
        {
            Before.Add((byte)CODE_FIELD.US);
            Before.Add((byte)(y + 0x1F));
            Before.Add((byte)(x + 0x1F));
            Before.Add(m);
            Before.Add(n);
            return this;
        }

        public Field Sequence(byte y, byte x, FCC_M m, FCC_N n)
        {
            Before.Add((byte)CODE_FIELD.US);
            Before.Add((byte)(y + 0x1F));
            Before.Add((byte)(x + 0x1F));
            Before.Add(m.Value);
            Before.Add(n.Value);
            return this;
        }


        public void Cursofr(int p1, CODE_FIELD cODE_FIELD1, int p2, int p3, int p4, CODE_FIELD cODE_FIELD2)
        {
            throw new NotImplementedException();
        }

    }
  
}
