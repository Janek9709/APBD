using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2
{
    [Serializable]
    public class SummaryArray
    {
        [XmlAttribute]
        public string createdAt { get; set; }
        [XmlAttribute]
        public string author { get; set; }
        
        public HashSet<Student> studenci;
        
        public HashSet<ActiveStudies> activeStudents;
    }
}
