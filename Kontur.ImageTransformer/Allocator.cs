using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    public static class Allocator
    {
        static readonly int[] Empty = new int[0];


        public static Stream toStream(this byte[] data)
        {
            Stream s = new MemoryStream();
            s.Write(data, 0, data.Length);
            return s;
        }

        public static UInt32 ReverseBytes(this UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public static string toString(this byte[] value,int offset, int len)
        {
            byte[] bytes = new byte[len];
            Array.Copy(value, offset, bytes,0, len);
            StringBuilder builder = new StringBuilder();
            foreach(byte b in bytes)           
                builder.Append(Convert.ToChar(b));
            
            return builder.ToString();
        }

        public  static int[] Locate(this byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate))
                return Empty;

            var list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? Empty : list.ToArray();
        }

        public static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        public static  bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }

    }
}
