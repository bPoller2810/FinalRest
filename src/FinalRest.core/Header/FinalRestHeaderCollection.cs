using System;
using System.Collections.Generic;
using System.Text;

namespace FinalRest.core
{
    public class FinalRestHeaderCollection : List<FinalRestHeader>
    {

        public FinalRestHeaderCollection(IEnumerable<FinalRestHeader> headers) : base(headers)
        {
        }

        public void Add(string key, string value)
        {
            base.Add(new FinalRestHeader(key, value));
        }

        public FinalRestHeaderCollection Copy()
        {
            return new FinalRestHeaderCollection(this.ToArray());
        }

    }

    public static class FinalRequestHeaderCollectionExtensions
    {

        public static FinalRestHeaderCollection ToFinalRequestHeaderCollection(this IEnumerable<FinalRestHeader> self)
        {
            return new FinalRestHeaderCollection(self);
        }

    }

}
