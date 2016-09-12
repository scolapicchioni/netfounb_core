using System;

namespace MetaData
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple=true)]
   public class CodeChangesAttribute : Attribute
    {
        public string Developer { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }

        public CodeChangesAttribute()
        {
        }

        public CodeChangesAttribute(string description  )
        {
            Description = description;
        }

        public override string ToString()
        {
            return $"{Description} by {Developer} on {Date}";
        }
    }
}
