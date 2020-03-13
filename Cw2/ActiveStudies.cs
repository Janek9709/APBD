using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Cw2
{
    [XmlType("studies")]
    public class ActiveStudies
    {
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public int numberOfStudents { get; set; }
    }
}