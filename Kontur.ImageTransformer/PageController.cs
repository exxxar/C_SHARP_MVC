using Kontur.ImageTransformer.PNGFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Management;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Kontur.ImageTransformer
{
    public class PageController
    {
        [Page(@"/process/grayscale/([0-9]+),([0-9]+),([0-9]+),([0-9]+)", PageAttribute.HttpMethod.POST)]
        public String grayscale(HttpListenerRequest request)
        {
            Console.WriteLine("test" + request.HttpMethod);
            return "2 " + request.RawUrl;
        }

        [Page(@"/process/sepia/([0-9]+),([0-9]+),([0-9]+),([0-9]+)", PageAttribute.HttpMethod.POST)]
        public void sepia(HttpListenerRequest request, HttpHelper helper)
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

        [Page("/process/main3", PageAttribute.HttpMethod.POST)]
        public void main3(HttpListenerRequest request, HttpHelper helper)
        {


            //PerformanceCounter cpuCounter;
            //PerformanceCounter ramCounter;

            //cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //ramCounter = new PerformanceCounter("Memory", "Available KBytes");

            //Console.WriteLine("cpu=>" + cpuCounter.NextValue() + "%");
            //Console.WriteLine("ramCounter=>" + ramCounter.NextValue() + "KB");
            List<Chunk> list = new List<Chunk>();

            BinaryReader reader = new BinaryReader(request.InputStream);
            lock (reader)
            {
                Byte[] bytes = reader.ReadBytes((int)request.ContentLength64);
                var pattern = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
                int offset = bytes.Locate(pattern)[0];

                for (int step = 8; step < bytes.Length - offset;)
                {
                    int size = (Int32)((UInt32)BitConverter.ToInt32(bytes, offset + step)).ReverseBytes() + 12;
                    Console.WriteLine("size=>" + size);
                    byte[] buf = new byte[size];
                    Array.Copy(bytes, offset + step, buf, 0, size);
                    Chunk chunk = new Chunk(buf);
                    step += size;
                    if (chunk.type.Equals("IDAT"))
                    {

                    }
                    list.Add(chunk);
                    if (chunk.type.Equals("IHDR"))
                    {
                        IHDRChunk ihdr = new IHDRChunk(chunk);
                    }
                   
                    if (chunk.type.Equals("IEND"))
                        break;

                }
                reader.Dispose();
                reader.Close();               
            }


            BinaryWriter bw = new BinaryWriter(helper.response.OutputStream);
            lock (bw)
            {
                bw.Write(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });
                foreach (Chunk c in list)
                {

                    Console.WriteLine(c.type);
                    bw.Write(c.getFragment());
                }
                bw.Flush();
                bw.Close();
            }



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
