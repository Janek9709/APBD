using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace KolosProbny2.DTO_s.Req
{
    public class CustomOrder
    {
        public DateTime dataPrzyjecia { get; set; }
        public string uwagi { get; set; }
        public IEnumerable<WyrobCustom> wyroby { get; set; }
    }
}
