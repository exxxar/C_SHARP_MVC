using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    
    class Chunk
    {
        public Int32 length { get; set; }       
        public byte [] byteArray { get; set; }
        public String type { get; set; }
        public Int32 crc { get; set; } 
        
        public Chunk(byte [] byteArray) {
            this.byteArray = byteArray;
            this.length = (Int32)((UInt32)BitConverter.ToInt32(this.byteArray,0)).ReverseBytes();//4
            Console.WriteLine("content lenth=>"+length);
            this.type = this.byteArray.toString(4, 4);//4
            Console.WriteLine(type);
            this.crc = (Int32)((UInt32)BitConverter.ToInt32(this.byteArray, 8 + this.length)).ReverseBytes();
        }

        public byte[] getFragment()
        {
            return this.byteArray;
        }
       
    }
}
