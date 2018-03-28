using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    [Controller]
    class TestController
    {

        [Page(@"/testss")]
        public void main323(HttpListenerRequest request, HttpHelper helper)
        {

            helper.SetStatus(HttpStatusCode.OK);
            helper.SendText("test");

        }
    }
}
