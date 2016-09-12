using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Threads
{
    public class Buffer
    {
        public Buffer()
        {
        }
        private string[] _buffer = new string[10];
         
        private int index = 0;
        public void Add(string toAdd){
            Monitor.Enter(this);
            if(index>=10)
                Monitor.Wait(this);
            System.Console.WriteLine($"adding {toAdd} at position {index}");
            _buffer[index] = toAdd;
            index++;
            Monitor.Pulse(this);
            Monitor.Exit(this);
        }
        public string Remove(){
            Monitor.Enter(this);
            if(index<=0)
                Monitor.Wait(this);
            System.Console.WriteLine($"removing {_buffer[0]}");
            string result = _buffer[0];
            for (int i = 1; i < index; i++)
            {
                _buffer[i-1] = _buffer[i];
            }
            index--;
            Monitor.Pulse(this);
            Monitor.Exit(this);
            return result;
        }
    }
}
