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
        public void badRequest(HttpListenerRequest request, HttpHelper helper)
        {
            helper.SetStatus(HttpStatusCode.BadRequest);
            helper.SendText("400");
        }

        [Error(404)]
        public void pageNotFound(HttpListenerRequest request, HttpHelper helper)
        {
            helper.SetStatus(HttpStatusCode.NotFound);
            helper.SendText("404");
        }


        [Error(500)]
        public void networkError(HttpListenerRequest request, HttpHelper helper)
        {
            helper.SetStatus(HttpStatusCode.InternalServerError);
            helper.SendText("500");
        }
    }
}
