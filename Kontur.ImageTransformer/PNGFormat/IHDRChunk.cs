using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.PNGFormat
{
    class IHDRChunk:Chunk
    {
        public Int32 width { get; set; }
        public Int32 height { get; set; }
        
        public byte bitDepth { get; set; }
        public byte color { get; set; }
        public byte methodCompression { get; set; }
        public byte methodFiltration { get; set; }
        public byte methodInterlace { get; set; }

        public Chunk chunk { get; set; }
       
        public IHDRChunk(Chunk chunk)
            :base(chunk.byteArray)
        {
            this.chunk = chunk;
            this.takeData();
        }

        private void takeData()
        {

            this.height = (Int32)((UInt32)BitConverter.ToInt32(this.byteArray, 8)).ReverseBytes();
            this.width = (Int32)((UInt32)BitConverter.ToInt32(this.byteArray, 12)).ReverseBytes();

            this.bitDepth = this.byteArray[16];
            this.color = this.byteArray[17];
            this.methodCompression = this.byteArray[18];
            this.methodFiltration = this.byteArray[19];
            this.methodInterlace = this.byteArray[20];


            Console.WriteLine("height=>" + height);
            Console.WriteLine("width=>" + width);
            Console.WriteLine("bit depth=>" + bitDepth);
            Console.WriteLine("color=>" + color);
            Console.WriteLine("methodCompression=>" + methodCompression);
            Console.WriteLine("methodFiltration=>" + methodFiltration);
            Console.WriteLine("methodInterlace=>" + methodInterlace);

        }

    }
}
