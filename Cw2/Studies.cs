using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2
{
    public class Studies
    {
        [XmlElement(ElementName = "name")]
        public string kierunek { get; set; }
        [XmlElement(ElementName = "mode")]
        public string tryb { get; set; }
    }
}
