using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cw2
{
    
    [Serializable]
    public class SummaryArray
    {
        
        [XmlAttribute]
        [JsonProperty(Order = 1)]
        public string createdAt { get; set; }
        
        [XmlAttribute]
        [JsonProperty(Order = 2)]
        public string author { get; set; }
        //[JsonProperty]
        [JsonProperty(Order = 3)]
        public HashSet<Student> studenci;
        //[JsonProperty]
        [JsonProperty(Order = 4)]
        public HashSet<ActiveStudies> activeStudents;
    }
}
