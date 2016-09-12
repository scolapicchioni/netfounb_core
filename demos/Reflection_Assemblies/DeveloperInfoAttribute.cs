using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reflection_Assemblies
{
    [AttributeUsage(AttributeTargets.All)]
    public class DeveloperInfo : Attribute 
    {
        
        public DeveloperInfo(string emailAddress, int revision) {
            this.EmailAddress = emailAddress;
            this.Revision = revision;
        }

        public string EmailAddress { get; }
        public int Revision { get; }
    }
}
