using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    [AttributeUsage(AttributeTargets.Method)]
    class ErrorAttribute : System.Attribute
    {
        public int errorCode { get; set; }
      
        public ErrorAttribute(int errorCode)
        {
            this.errorCode = errorCode;
        }
    }
}
