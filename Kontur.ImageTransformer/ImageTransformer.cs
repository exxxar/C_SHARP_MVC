using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    class ImageTransformer:iImageTransformer
    {
        public Stream inputStream { get; set; }
        public ImageTransformer(Stream inputStream)
        {
            this.inputStream = inputStream;
        }
    }
}
