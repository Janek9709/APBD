using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2
{
    [Serializable]
    public class SummaryArray
    {
        [XmlElement(ElementName = "createdAt")]
        public string createdAt { get; set; }
        [XmlElement(ElementName = "author")]
        public string author { get; set; }
        
        public HashSet<Student> studenci;
        
        public HashSet<ActiveStudies> activeStudents;
    }
}
