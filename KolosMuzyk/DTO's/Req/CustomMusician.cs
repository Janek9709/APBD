using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolosMuzyk.DTO_s.Req
{
    public class CustomMusician
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nickname { get; set; }
        public IEnumerable<CustomTrack> track { get; set; }
    }
}
