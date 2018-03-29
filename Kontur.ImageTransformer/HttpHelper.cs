using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

        public void SendImage(byte[] imgByteArray)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            using (var writer = response.OutputStream)
                writer.Write(imgByteArray, 0, imgByteArray.Length);
        }

        public void view(String htmlName)
        {
            var html = File.ReadAllText($"sites\\{htmlName}.html");
            StreamWriter sw = new StreamWriter(response.OutputStream);
            sw.Write(html);
            sw.Close();
        }

        public void view(String htmlName, Dictionary<string, string> variables)
        {
            var html = File.ReadAllText($"sites\\{htmlName}.html");
            MatchCollection mc = null;
            Regex reg = new Regex("{{([0-9a-zA-Z]+)}}");
            mc = reg.Matches(html);

            foreach (Match match in mc)
            {
                try
                {
                    html = html.Replace(match.Groups[0].Value, variables[match.Groups[1].Value]);
                }
                catch
                {
                    html = html.Replace(match.Groups[0].Value, $"undefined[{match.Groups[1].Value}]");
                }
            }
            StreamWriter sw = new StreamWriter(response.OutputStream);
            sw.Write(html);
            sw.Close();
        }

    }
}
