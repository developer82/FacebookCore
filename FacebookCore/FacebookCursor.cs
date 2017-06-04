using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookCore
{
    public class FacebookCursor
    {
        public string Before { get; internal set; }
        public string After { get; internal set; }

        public FacebookCursor()
        {
            
        }

        public FacebookCursor(string before, string after)
        {
            Before = before;
            After = after;
        }
    }
}
