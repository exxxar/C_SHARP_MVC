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
        private byte [] byteArray { get; set; }
        public String type { get; set; }
        public Int32 crc { get; set; } 
        
        public Chunk() {
            this.type = "";
            this.crc = 0;
            }

        public byte[] getByteArray()
        {
            return this.byteArray;
        }
        public void setByteArray(byte [] byteArray)
        {
            this.byteArray = byteArray;
            Console.WriteLine("byteArrayLen = " + this.byteArray.Length);
            this.type = this.byteArray.toString(4, 4);
            Console.WriteLine("type= " + this.type);
            this.crc = (Int32)((UInt32)BitConverter.ToInt32(this.byteArray, 4+this.length)).ReverseBytes();
        }      
    }
}
