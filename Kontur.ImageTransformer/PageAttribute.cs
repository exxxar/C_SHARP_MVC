using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    [AttributeUsage(AttributeTargets.Method)]
    class PageAttribute : System.Attribute
    {
        public enum HttpMethod
        {
            GET ,
            POST
        }
        public string path { get; set; }
        public HttpMethod method { get; set; }

        public PageAttribute(string path)
        {
            this.path = path;
            this.method = HttpMethod.GET;
        }
        public PageAttribute(string path,HttpMethod method)
        {
            this.path = path;
            this.method = method;
        }
    }
}
