using System;
using System.Net;

namespace FinalRest.core
{
    public interface IRestResponse
    {

        HttpStatusCode StatusCode { get; }
        object Data { get; }

        string Reason { get; }
        Exception Exception { get; }

    }

}
