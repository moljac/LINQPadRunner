using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LPRun
{
    [Serializable]
    public class Query
    {
        [XmlAttribute]
        public string Kind { get; set; }
    }
}
