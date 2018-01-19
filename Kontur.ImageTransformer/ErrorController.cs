using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    public class ErrorController
    {
        [Error(400)]
        public String badRequest(HttpListenerRequest request)
        {
            return "404";
        }

        [Error(404)]
        public String pageNotFound(HttpListenerRequest request)
        {
            return "404";
        }


        [Error(500)]
        public String networkError(HttpListenerRequest request)
        {
            return "500";
        }
    }
}
