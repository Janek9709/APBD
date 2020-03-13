using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2
{
    
    [Serializable]
    [XmlType("student")]
    public class Student
    {
        [XmlAttribute(AttributeName = "indexNumber")]
        [JsonProperty("indexNumber")]
        public string numer { get; set; }
        //[JsonProperty]
        [XmlElement(ElementName = "fname")]
        [JsonProperty("fname")]
        public string imie { get; set; }
        [JsonProperty("lname")]
        //[JsonProperty]
        [XmlElement(ElementName = "lname")]
        public string nazwisko { get; set; }
        //[JsonProperty]
        //[JsonProperty]
        [XmlElement(ElementName = "birthdate")]
        [JsonProperty("birthdate")]
        public string data { get; set; }
        //[JsonProperty]
        [XmlElement(ElementName = "email")]
        [JsonProperty("email")]
        public string mail { get; set; }
        //[JsonProperty]
        [XmlElement(ElementName = "mothersName")]
        [JsonProperty("mothersName")]
        public string imieMatki { get; set; }
        //[JsonProperty]
        [XmlElement(ElementName = "fathersName")]
        [JsonProperty("fathersName")]
        public string imieOjca { get; set; }
        //[JsonProperty]
        public Studies studies { get; set; }
    }
}
