using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Management;
using System.Diagnostics;
using ZXing;
using System.Drawing;

using NPOI.XWPF.UserModel;

namespace Kontur.ImageTransformer
{
    [Controller]
    public class PageController
    {
        
        private BarcodeGenerator bGenerator = new BarcodeGenerator();
              

        [Page(@"/barcode/gen/ean13/([0-9]+)")]
        public void main(HttpListenerRequest request, HttpHelper helper,object data)
        {
            
            helper.SetStatus(HttpStatusCode.OK);
            try
            {
                helper.SendImage(bGenerator.getEan13("" + data));
            }
            catch(ArgumentException ae)
            {
                throw new PageException(ae.Message, 500);
            }

        }


        [Page(@"/barcode/test")]
        public void main3(HttpListenerRequest request, HttpHelper helper)
        {
            helper.SetStatus(HttpStatusCode.OK);
            helper.view("index");
        }

        [Page(@"/barcode/gen/ean12")]
        public void main2(HttpListenerRequest request, HttpHelper helper)
        {

            helper.SetStatus(HttpStatusCode.OK);

            //   XWPFDocument doc = new XWPFDocument();
            //   XWPFParagraph p2 = doc.CreateParagraph();
            //   XWPFRun r2 = p2.CreateRun();
            //   r2.SetText("test");
            //   XWPFTable table = doc.CreateTable();
            //   table.CreateRow();
            //   table.CreateRow();
            //   table.AddNewCol();





            //   var widthEmus = (int)(400.0 * 9525);
            //   var heightEmus = (int)(300.0 * 9525);

            //// r2.AddPicture(bGenerator.getEan13("123456789012").toStream(), (int)PictureType.DIB, "image1", widthEmus, heightEmus);


            //   using (Stream sw = helper.response.OutputStream)
            //   {
            //       doc.Write(sw);
            //   }
            helper.SendText("test");

        }

    }
}
