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
        {
            this.chunk = chunk;
            this.takeData();
        }

        private void takeData()
        {
           
            //this.height = (Int32)((UInt32)BitConverter.ToInt32(this.getByteArray(), 4)).ReverseBytes();
            //this.width = (Int32)((UInt32)BitConverter.ToInt32(this.getByteArray(), 8)).ReverseBytes();

            this.bitDepth = this.getByteArray()[12];
            this.color = this.getByteArray()[13];
            this.methodCompression = this.getByteArray()[14];
            this.methodFiltration = this.getByteArray()[15];
            this.methodInterlace = this.getByteArray()[16];          
        }

    }
}
