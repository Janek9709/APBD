using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2
{
    public class Studies
    {
        [XmlElement(ElementName = "name")]
        [JsonProperty("name")]
        public string kierunek { get; set; }
        [XmlElement(ElementName = "mode")]
        [JsonProperty("mode")]
        public string tryb { get; set; }
    }
}
