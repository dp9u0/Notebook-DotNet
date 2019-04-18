using Common;
using System;

namespace Type
{
  public class PropertyRunner : Runner
  {
    protected override void RunCore()
    {
    }

    public class SomeType
    {
      private string _field1;
      private string _field2;

      public String Prop1
      {
        get { return _field1; }
        set { _field1 = value; }
      }

      public String Prop2
      {
        get;
        set;
      }
    }
    
    public sealed class BitArray
    {
      // Private array of bytes that hold the bits 
      private Byte[] m_byteArray;
      private Int32 m_numBits;
      // Constructor that allocates the byte array and sets all bits to 0 
      public BitArray(Int32 numBits)
      {// Validate arguments first. 
        if (numBits <= 0)
          throw new ArgumentOutOfRangeException("numBits must be > 0");
        // Save the number of bits. 
        m_numBits = numBits;
        // Allocate the bytes for the bit array. 
        m_byteArray = new Byte[(numBits + 7) / 8];
      }
      // This is the indexer (parameterful property). 
      public Boolean this[Int32 bitPos]
      {
        // This is the indexer's get accessor method. 
        get
        {
          // Validate arguments first 
          if ((bitPos < 0) || (bitPos >= m_numBits))
            throw new ArgumentOutOfRangeException("bitPos");
          // Return the state of the indexed bit. 
          return (m_byteArray[bitPos / 8] & (1 << (bitPos % 8))) != 0;
        }
        // This is the indexer's set accessor method. 
        set
        {
          if ((bitPos < 0) || (bitPos >= m_numBits))
            throw new ArgumentOutOfRangeException("bitPos", bitPos.ToString());
          if (value)
          {
            // Turn the indexed bit on. 
            m_byteArray[bitPos / 8] = (Byte)
            (m_byteArray[bitPos / 8] | (1 << (bitPos % 8)));
          }
          else
          {
            // Turn the indexed bit off. 
            m_byteArray[bitPos / 8] = (Byte)
            (m_byteArray[bitPos / 8] & ~(1 << (bitPos % 8)));
          }
        }
      }
    }
  }
}
