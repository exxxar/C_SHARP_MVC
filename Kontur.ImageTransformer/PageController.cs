using Kontur.ImageTransformer.PNGFormat;
using System;
using System.IO;
using System.Net;

namespace Kontur.ImageTransformer
{
    public class PageController
    {
        [Page(@"/process/grayscale/([0-9]+),([0-9]+),([0-9]+),([0-9]+)", PageAttribute.HttpMethod.POST)]
        public String grayscale(HttpListenerRequest request)
        {
            Console.WriteLine("test"+request.HttpMethod);
            return "2 "+ request.RawUrl;
        }

        [Page(@"/process/sepia/([0-9]+),([0-9]+),([0-9]+),([0-9]+)", PageAttribute.HttpMethod.POST)]
        public void sepia(HttpListenerRequest request,HttpHelper helper)
        {
            byte[] imageBytes = new byte[Math.Min(request.InputStream.Length, 100000)];


            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Console.WriteLine(imageBytes.Length);
            }
            helper.SendText("test");
            //return "1 "+ request.QueryString;
        }


        [Page(@"/process/threshold.([0-9]+)./([0-9]+),([0-9]+),([0-9]+),([0-9]+)", PageAttribute.HttpMethod.GET)]
        public String threshold(HttpListenerRequest request, HttpHelper helper, object n, object x, object y, object h, object w)
        {
          
            Console.WriteLine(n + " " + x + " " + y + " " + h + " " + w);
            return "1 " + request.QueryString;
        }

        [Page("/process/main")]
        public void main(HttpListenerRequest request, HttpHelper helper) 
        {

            Console.WriteLine(request.InputStream.Length);
            byte[] imageBytes = new byte[Math.Min(request.InputStream.Length, 100000)];


            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                
            }

            //throw new PageException("ERROR!!", 500);
            helper.SetStatus(HttpStatusCode.OK);
            helper.SendText("test");
           

        }

        [Page("/process/main3",PageAttribute.HttpMethod.POST)]
        public void main3(HttpListenerRequest request, HttpHelper helper)
        {
            using (BinaryReader reader = new BinaryReader(request.InputStream))
            {

                // Read file 
              
                
                Console.WriteLine(request.ContentLength64+"["+request.ContentType+"]") ;
                Byte[] bytes =  reader.ReadBytes((int)request.ContentLength64);

              
                var pattern = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
                int offset = bytes.Locate(pattern)[0];

                Chunk chunk = new Chunk();
                chunk.length = (Int32)((UInt32)BitConverter.ToInt32(bytes, offset + 8)).ReverseBytes();
                Console.WriteLine(chunk.length);
                
                byte[] buf = new byte[chunk.length+8];
                Array.Copy(bytes, offset+8, buf, 0, 4 + chunk.length + 4);
                chunk.setByteArray(buf);
                Console.WriteLine(chunk.type);
                IHDRChunk ihdr = new IHDRChunk(chunk);

                Console.WriteLine(ihdr.bitDepth);
                Console.WriteLine(ihdr.color);
                //4+LEN+4

                //Int32  length= (Int32)((UInt32)BitConverter.ToInt32(bytes, offset+8)).ReverseBytes();
                //string title = bytes.toString(offset + 12, 4);
                //if (title.Equals("IHDR"))
                //{
                //    Int32 width = (Int32)((UInt32)BitConverter.ToInt32(bytes, offset + 16)).ReverseBytes(),
                //        height = (Int32)((UInt32)BitConverter.ToInt32(bytes, offset + 20)).ReverseBytes();
                //    byte 
                //        bitDepth = bytes[offset + 24],
                //        color = bytes[offset + 25],
                //        methodCompression = bytes[offset + 26],
                //        methodfiltration = bytes[offset + 27],
                //        methodInterlace = bytes[offset + 28];
                //    Console.WriteLine("width=>" + width);
                //    Console.WriteLine("height=>" + height);
                //    Console.WriteLine("bitDepth=>" + bitDepth);
                //    Console.WriteLine("color=>" + color);
                //    Console.WriteLine("methodCompression=>" + methodCompression);
                //    Console.WriteLine("methodfiltration=>" + methodfiltration);
                //    Console.WriteLine("methodInterlace=>" + methodInterlace);


                //}
                //Console.WriteLine(length);
                //Console.WriteLine((BitConverter.IsLittleEndian?"true":"false")+" "+title);


                Console.WriteLine("pos=>"+ offset);               
                BinaryWriter bw = new BinaryWriter(helper.response.OutputStream);
                bw.Write(bytes);
             
            
            }

            //byte[] imageBytes = new byte[Math.Min(request.InputStream.Length, 100000)];


            //using (MemoryStream ms = new MemoryStream(imageBytes))
            //{
            //    Console.WriteLine(imageBytes.Length);
            //}

            //throw new PageException("ERROR!!", 500);
           // helper.SetStatus(HttpStatusCode.OK);
           // helper.SendText("test");


        }

        [Page("/process/main2")]
        public String main2(HttpListenerRequest request)
        {

           // throw new PageException("ERROR!!", 500);
            return "main2";
        }


    }
}
