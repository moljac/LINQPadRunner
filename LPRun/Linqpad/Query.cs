using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LPRun.Linqpad
{
    [Serializable]
    public class Query
    {
        public string Kind { get; set; }
        public List<string> Namespaces { get; set; }
        public List<string> GACReferences { get; set; }
        public List<string> RelativeReferences { get; set; }

    }
}
