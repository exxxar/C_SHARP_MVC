using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    public class PageController
    {
        [Page("/process/grayscale",PageAttribute.HttpMethod.POST)]
        public String grayscale(HttpListenerRequest request)
        {
            Console.WriteLine("test"+request.HttpMethod);
            return "2 "+ request.RawUrl;
        }

        [Page("/process/sepia", PageAttribute.HttpMethod.POST)]
        public String sepia(HttpListenerRequest request)
        {
            return "1 "+ request.QueryString;
        }


        [Page("/process/threshold([{n}])/[{x},{y},{w},{h}]", PageAttribute.HttpMethod.POST)]
        public String threshold(HttpListenerRequest request)
        {
            return "1 " + request.QueryString;
        }

        [Page("/process/main")]
        public String main(HttpListenerRequest request) 
        {

            throw new PageException("ERROR!!", 500);
            //return "main";
        }

        [Page("/process/main2")]
        public String main2(HttpListenerRequest request)
        {

           // throw new PageException("ERROR!!", 500);
            return "main2";
        }


    }
}
