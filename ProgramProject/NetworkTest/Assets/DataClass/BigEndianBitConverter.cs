using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Main.Script.DataClass
{
    public class BigEndianBitConverter 
    {
        public static byte[] CopyBytesImpl(int value, int bytes, int index) 
        {
            byte[] buffer=new byte[bytes];
            int endOffset = index + bytes - 1;
            for (int i = 0; i < bytes; i++)
            {
                buffer[endOffset - i] = unchecked((byte)(value & 0xff));
                value = value >> 8;
            }
            return buffer;
        }
        public static byte[] CopyBytesImpl(long value, int bytes, int index)
        {
            byte[] buffer = new byte[bytes];
            int endOffset = index + bytes - 1;
            for (int i = 0; i < bytes; i++)
            {
                buffer[endOffset - i] = unchecked((byte)(value & 0xff));
                value = value >> 8;
            }
            return buffer;
        }
        public static byte[] CopyBytesImpl(uint value, int bytes, int index)
        {
            byte[] buffer = new byte[bytes];
            int endOffset = index + bytes - 1;
            for (int i = 0; i < bytes; i++)
            {
                buffer[endOffset - i] = unchecked((byte)(value & 0xff));
                value = value >> 8;
            }
            return buffer;
        }


        public static long FromBytes(byte[] buffer, int startIndex, int bytesToConvert)
        {
            long ret = 0;
            for (int i = 0; i < bytesToConvert; i++)
            {
                ret = unchecked((ret << 8) | buffer[startIndex + i]);
            }
            return ret;
        }
    }
}
