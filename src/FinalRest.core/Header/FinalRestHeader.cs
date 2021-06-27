using System;
using System.Collections.Generic;
using System.Text;

namespace FinalRest.core
{
    public class FinalRestHeader
    {

        public string Key { get; }
        public string Value { get;  }

        public FinalRestHeader(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
