using System;
using System.Runtime.Serialization;

namespace CustomerLibrary
{
    
    public class Customer
    {
        public int CustomerID { get; private set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TelephoneNumber { get; set; }//later toevoegen.

        public Customer(int customrID, string name, string address)
        {
            CustomerID = customrID;
            Name = name;
            Address = address;
        }

        [OnDeserializing]
        private void SetTelephoneNumber(StreamingContext streamingContext)
        {
           TelephoneNumber = "onbekend";
        }

        public override string ToString()
        {
            return $"CustomerID: {CustomerID}\tName: {Name}\tAddress: {Address}\tTelephoneNumber: {TelephoneNumber}";
            //return $"CustomerID: {CustomerID}\tName: {Name}\tAddress: {Address}";
        }
    }
}
