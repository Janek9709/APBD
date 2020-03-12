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

        [XmlElement(ElementName = "fname")]
        public string imie { get; set; }
        [XmlElement(ElementName = "lname")]
        public string nazwisko { get; set; }

        [XmlAttribute(AttributeName = "indexNumber")]
        public string numer { get; set; }
        [XmlElement(ElementName = "birthdate")]
        public string data { get; set; }
        [XmlElement(ElementName = "email")]
        public string mail { get; set; }
        [XmlElement(ElementName = "mothersName")]
        public string imieMatki { get; set; }
        [XmlElement(ElementName = "fathersName")]
        public string imieOjca { get; set; }
        public Studies studies { get; set; }
    }
}
