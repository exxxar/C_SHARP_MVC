using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    class PageException:Exception
    {
        public int code { get; set; }
        public PageException(String message, int code):base(message)
        {
            this.code = code;
        }
    }
}
