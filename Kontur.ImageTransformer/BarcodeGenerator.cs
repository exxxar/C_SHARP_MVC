using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace Kontur.ImageTransformer
{
    class BarcodeGenerator
    {

        public BarcodeGenerator()
        {

        }

        public byte[] getEan13(string data)
        {
            if (data.Length != 12)
                throw new ArgumentException("Неправильные параметры: 12 цифр без чексуммы!");

            IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.EAN_13};
            
            return getBytes(new Bitmap(writer.Write(data)));
        }

       

        public byte[] getEan8(string data)
        {
            if (data.Length != 7)
                throw new ArgumentException("Неправильные параметры: 7 цифр без чексуммы!");

            IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.EAN_13 };
            return getBytes(new Bitmap(writer.Write(data)));
        }

        public byte[] getQRCode(string data)
        {   
            IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
            return getBytes(new Bitmap(writer.Write(data)));
        }

        private byte[] getBytes(Bitmap bitmap)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bitmap, typeof(byte[]));
        }
    }
}
