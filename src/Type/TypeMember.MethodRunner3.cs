using Common;
using System;
namespace Type
{
  class MethodRunner3 : Runner
  {
    protected override void RunCore()
    {
      string val = "111";
      /* 0x000008B8 1200         IL_0008: ldloca.s  val*/
      /* 0x000008BA 2821000006   IL_000A: call      instance void Type.MethodRunner3::RefMethd(string&)*/
      RefMethd(ref val);
    }

    private void RefMethd(ref string val)
    {
      val = "123";
    }
  }
}
