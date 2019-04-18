using Common;
using System;
namespace Type
{
  class MethodRunner2 : Runner
  {
    protected override void RunCore()
    {
      var first = new SomeType();
      var second = new SomeType();
      // Console.WriteLine(first + second);
      // Console.WriteLine(first - second);
      SomeType val3 = 1;
      SomeType val4 = (SomeType)"1.4";
      double val5 = 1.1;
      SomeType val6 = (int)val5;
      // Console.WriteLine(val3);
      // Console.WriteLine(val4); 
      // Console.WriteLine(val5);
    }

    public class SomeType
    {

      static SomeType() { }

      public SomeType()
      {

      }

      public static SomeType operator +(SomeType first, SomeType second)
      {
        return first;
      }

      public static SomeType operator -(SomeType first, SomeType second)
      {
        return first;
      }

      public static implicit operator SomeType(int val)
      {
        return new SomeType();
      }

      public static explicit operator SomeType(string val)
      {
        return new SomeType();
      }
    }
  }
}
