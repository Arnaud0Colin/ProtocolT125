using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReseauxProtocol.T125Protocol;

namespace ReseauxProtocol
{
   static class qTermForm
    {
        static public Trame GetAppel(string Ecran)
        {
            COTP cotp3 = new COTP();
            cotp3.PDU = PDU_TYPE.DATA_INDICATION;
            cotp3.Size = 3;
            cotp3.Class = 0x80;

            List<byte> ett = new List<byte>();
            ett.Add(0x8e);
            ett.Add(0x81);
            ett.Add(0xdb);
            ett.Add(0x02);
            ett.Add(0x03);
            ett.Add(0x01);
            ett.Add(0xe0);
            ett.Add(0x02);
            ett.Add(0x02);
            ett.Add(0x01);
            ett.Add(0xc3);
            ett.Add(0x07);

            ReceiveData fff = new ReceiveData(ett);

            fff.chant.Add(new Field().Cursor(2, 1, 0x00));

            Field dfrr = new Field();
            dfrr.Before.Add((byte)CODE_FIELD.RS);
            dfrr.Before.Add((byte)0xc0);
            dfrr.Before.Add((byte)Ecran.Length);
            dfrr.Text = Ecran;
         //   dfrr.After.Add(0x93);

         //   dfrr.After.Add(0x95);
         //   dfrr.After.Add(0x90);

            fff.chant.Add(dfrr);

            return new Trame(cotp3, fff, new byte[3] { 0x93, 0x95, 0x90 });     
        }

        static public Trame GetClean(List<byte> entete)
        {
            COTP cotp4 = new COTP();
            cotp4.PDU = PDU_TYPE.DATA_INDICATION;
            cotp4.Size = 3;
            cotp4.Class = 0x80;


            List<byte> ett = new List<byte>();
            ett.Add(0x8e);
            ett.Add(0x80);
            // for (int i = 2; i < 6; i++)
            for (int i = 0; i < 4; i++)
                ett.Add(entete[i]);
         //   ett.Add(0x95);
//            ett.Add(0x90);

            ReceiveData fff = new ReceiveData(ett);
            return new Trame(cotp4, fff, new byte[2] { 0x95, 0x90 });
        }

        static public Trame GetTIPInit(string pid)
        {

            Request_COTP cotp = new Request_COTP();

            cotp.PDU = PDU_TYPE.CONNECT_REQUEST;
            cotp.Destination = 0x0000;
            cotp.Source = 0x0100;
            cotp.Class = 0x00;
            cotp.SourceParameter = TSAP_TYPE.SRC_TSAP;
            cotp.SourceTSAP = "QUICKW";
            cotp.DestinationParameter = TSAP_TYPE.DST_TSAP;
            cotp.DestinationTSAP = "TIPCSU";


            List<byte> ett = new List<byte>();
            ett.Add(0x01);
            ett.Add(0x01);
            ReceiveData fff = new ReceiveData(ett);

            fff.chant.Add(new Field().CallInit(0xf1, "TIP", null));
            fff.chant.Add(new Field().CallInit(0xf2, pid, 0x8e));

            //return new Trame(cotp, fff, new byte[1] { 0x8e });                
            return new Trame(cotp, fff, null);
        }
     
    }


    static class INTECM
    {
        static public Trame GetAction(string NumCde, short depot, short ste, char key)
        {
            COTP cotp5 = new COTP();
            //T125 t1255 = new T125();
            cotp5.PDU = PDU_TYPE.DATA_INDICATION;
            cotp5.Size = 3;
            cotp5.Class = 0x80;


            List<byte> ett = new List<byte>();
            ett.Add(0x8e);
            ett.Add(0x81);
            ett.Add(0xdb);
            ett.Add(0x02);
            ett.Add(0x03);
            ett.Add(0x01);
            ett.Add(0xe0);
            ett.Add(0x02);
            ett.Add(0x01);
            ett.Add(0x01);
            ett.Add(0xc3);
             ett.Add(0x10);
           // ett.Add(0x0B);

            ReceiveData fff = new ReceiveData(ett);

            fff.chant.Add(new Field().Cursor(1, 1, 0x00));

            fff.chant.Add(new Field().Sequence(1, 5, 0x64, 0x40));


            Field fld = new Field();
            //fld.Before.Add(0xC0);
            //fld.Text = "     ";
            //fld.IsControl = true;
            //Field controle = fld;
            //fff.chant.Add(fld);

            //Field fld = new Field();
            //fld.Before.Add(0xC0);
            //fld.Text = "     ";
            //fld.IsControl = true;
            //Field controle = fld;
            //fff.chant.Add(fld);


            //fff.chant.Add(new Field().Sequence(1, 12, 0x64, 0x41));
            ////fff.chant.Add(new Field().Sequence(1, 27, 0x60, 0x42));

            //fld = new Field().Sequence(1, 18, 0x60, 0x42);
            //fld.Text = " 4";
            //fff.chant.Add(fld);


            /*
            fld = new Field();
            fld.Before.Add(0xC0);
            fld.Text = Article;
            fld.IsControl = true;*/
            //Field controle = fld;
            //fff.chant.Add(fld);
        

            fld = new Field().Sequence(1, 18, 0x64, 0x42);
            fld.Before.Add(0xc0);
            fld.Before.Add(0x0c);
            fld.Text = ste.ToString();
            fff.chant.Add(fld);


            //fld = new Field().Sequence(1, 28, 0x60, 0x42);
            //fld.Text = depot.ToString();
            //fff.chant.Add(fld);


            fld = new Field().Sequence(1, 40, 0x60, 0x42);
            fld.Text = NumCde;
            fff.chant.Add(fld);

            //if (client != null)
            //{
            //    fld = new Field().Sequence(1, 61, 0x60, 0x42);
            //    fld.Text = client;
            //    fff.chant.Add(fld);


            //    fld = new Field().Sequence(1, 73, 0x60, 0x40);
            //    fld.Text = key.ToString();
            //    fld.After.Add(0x20);
            //    // fld.After.Add(0x93);
            //    fff.chant.Add(fld);
            //}
            //else
            //{
            //    fld = new Field().Sequence(1, 61, 0x64, 0x42);
            //    fld.After.Add(0x20);
            //    //   fld.After.Add(0x93);
            //    fff.chant.Add(fld);
            //}
            /*
                       fld = new Field();
                       fld.Before.Add(0x95);
                       fld.Before.Add(0x90);
                       fff.chant.Add(fld);
                       */
           // controle.Before.Add(fff.controle());

            return new Trame(cotp5, fff, new byte[3] { 0x93, 0x95, 0x90 });

        }
    }

    static class INSTOCK
   {
       static public Trame GetAction(string Article, short depot, short ste, string client, char key)
       {
           COTP cotp5 = new COTP();
           //T125 t1255 = new T125();
           cotp5.PDU = PDU_TYPE.DATA_INDICATION;
           cotp5.Size = 3;
           cotp5.Class = 0x80;


           List<byte> ett = new List<byte>();
           ett.Add(0x8e);
           ett.Add(0x81);
           ett.Add(0xdb);
           ett.Add(0x02);
           ett.Add(0x03);
           ett.Add(0x01);
           ett.Add(0xe0);
           ett.Add(0x02);
           ett.Add(0x01);
           ett.Add(0x01);
           ett.Add(0xc3);
          // ett.Add(0x15);
           ett.Add(0x0B);

           ReceiveData fff = new ReceiveData(ett);


           fff.chant.Add(new Field().Cursor(1, 1, 0x00));

           fff.chant.Add(new Field().Sequence(1, 6, 0x60, 0x40));

           Field fld = new Field();
           fld.Before.Add(0xC0);
           fld.Text = "     ";
           fld.IsControl = true;
           Field controle = fld;
           fff.chant.Add(fld);


           fff.chant.Add(new Field().Sequence(1, 12, 0x64, 0x41));
           //fff.chant.Add(new Field().Sequence(1, 27, 0x60, 0x42));

           fld = new Field().Sequence(1, 24, 0x60, 0x42);
           fld.Text = Article;
           fff.chant.Add(fld);


           /*
           fld = new Field();
           fld.Before.Add(0xC0);
           fld.Text = Article;
           fld.IsControl = true;
           Field controle = fld;
           fff.chant.Add(fld);*/

           fld = new Field().Sequence(1, 38, 0x60, 0x42);
           fld.Text = ste.ToString();
           fff.chant.Add(fld);


           fld = new Field().Sequence(1, 47, 0x60, 0x42);
           fld.Text = depot.ToString();
           fff.chant.Add(fld);

           if (client != null)
           {
               fld = new Field().Sequence(1, 61, 0x60, 0x42);
               fld.Text = client;
               fff.chant.Add(fld);


               fld = new Field().Sequence(1, 73, 0x60, 0x40);
               fld.Text = key.ToString();
               fld.After.Add(0x20);
              // fld.After.Add(0x93);
               fff.chant.Add(fld);
           }
           else
           {
               fld = new Field().Sequence(1, 61, 0x64, 0x42);
               fld.After.Add(0x20);
            //   fld.After.Add(0x93);
               fff.chant.Add(fld);
           }
/*
           fld = new Field();
           fld.Before.Add(0x95);
           fld.Before.Add(0x90);
           fff.chant.Add(fld);
           */
           controle.Before.Add(fff.controle());

           return new Trame(cotp5, fff, new byte[3] { 0x93, 0x95, 0x90 });

       }
   }
}
