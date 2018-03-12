# ProtocolT125
implement T125 Protocol for communicate with a mainframe like Qterm. { Alpha version }

The purpose of this project is to make request to a mainframe or to emulate a mainframe.
I put this alpha version who working fine but need a lot of work. All help are welcome.
This code was test on Unysis mainframe.

```C#
 CodeId flag = FreePid.GetFree();
  var mainfraime = new ConnectionT125("127.0.0.1", 102);
  ReceiveData data= mainfraime.Connect2(flag.ToString(), "", "", "",  null,  ' ');
  string str;
  if (_data != null)
    /*     screen coordinate (x, y)     */
    str = _data.GetData(x, y));
   

       Socket ss = null;
       byte[] RcpBuf = new byte[2048];
       ReceiveData Result = null;

       if ((ss = Open()) == null)
             return Result;

       ss.ReceiveTimeout = 3000;
      List<Trame> tRecu;
      int iRecu = 0;
      byte[] buf = null;
      ushort index = 0;
      qTermForm.GetTIPInit(pid).Write(ref buf, ref index);

      if (!Send(ref ss, ref  buf, SocketFlags.None))
             return Result;

     if ((iRecu = ReceiveFrom(ref ss, ref RcpBuf, SocketFlags.None)) == 0)
             goto Exit;

     if ((tRecu = ReceiveField<Trame>(ref RcpBuf, iRecu )) == null)
            goto Exit;

     if (tRecu.First().PDU == PDU_TYPE.CONNECT_CONFIRMATION)
     {
     }
```
