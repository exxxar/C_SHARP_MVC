using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            //throw new PageException("ERROR!!", 500);
            helper.SendText("test");
        }

        [Page("/process/main2")]
        public String main2(HttpListenerRequest request)
        {

           // throw new PageException("ERROR!!", 500);
            return "main2";
        }


    }
}
