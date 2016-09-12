/*
Add System.Runtime.Serialization.Xml package in order to find DataContract 
"dependencies": {
    "System.Runtime.Serialization.Xml": "4.1.1"
  }
  */

using System.Runtime.Serialization;

namespace ConsoleApplication{
    [DataContract(Name = "person", Namespace = "http://schemas.infosupport.com")]
    public class PersonForDataContractXmlSerialization{
        public int Id {get; set;}
        [DataMember(Name = "name")]
        public string Name {get; set;}
        public string Surname {get; set;}
    }
}