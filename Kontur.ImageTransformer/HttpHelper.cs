using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    public class HttpHelper
    {
        public HttpListenerResponse response { get; }
        public HttpHelper(HttpListenerResponse response)
        {
            this.response = response;
        }
        public void SetStatus(HttpStatusCode code)
        {
            response.StatusCode = (int)code;
        }
        public void SendText(String text)
        {
            
            using (var writer = new StreamWriter(response.OutputStream))
                writer.WriteLine(text);
        }

        public void SendImage(Stream stream)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            using (var writer = new StreamWriter(response.OutputStream))
                writer.Write(stream);
        }
    }
}
