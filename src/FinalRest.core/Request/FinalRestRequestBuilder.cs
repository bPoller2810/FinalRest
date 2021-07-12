using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FinalRest.core
{
    public sealed class FinalRestRequestBuilder
    {

        #region base settings
        /// <summary>
        /// The Rest Method used for this Request
        /// On Build this will default to ERestMethod.GET if not set
        /// </summary>
        internal ERestMethod Method { get; set; }

        /// <summary>
        /// The encoding used for StringContent bodies
        /// </summary>
        internal Encoding Encoding { get; set; }

        /// <summary>
        /// The MediaType for StringContent bodies
        /// </summary>
        internal string MediaType { get; set; }

        /// <summary>
        /// The Route to the called Endpoint
        /// </summary>
        internal string Route { get; set; }

        /// <summary>
        /// In Case of a Post request the used Content-Type
        /// On Build this will default to EBodyType.JSON if not set
        /// </summary>
        internal EBodyType BodyType { get; set; }
        #endregion

        /// <summary>
        /// A List of all Headers applied to this Request
        /// </summary>
        internal Dictionary<string, string> Headers { get; } = new();

        #region handlers
        /// <summary>
        /// A List of Handlers that act before the Request is made
        /// Here you can add headers to this Request
        /// </summary>
        internal List<Type> PreRequestHandler { get; } = new();

        /// <summary>
        /// A List of Handlers that act after the Request is made
        /// Here you can extract Data from the Result
        /// </summary>
        internal List<Type> PostRequestHandler { get; } = new();
        #endregion

        #region behaviours
        /// <summary>
        /// A List of async Behaviours that will be invoked on a certain StatusCode
        /// Here you have access to the result
        /// </summary>
        internal List<(HttpStatusCode StatusCode, Func<HttpStatusCode, object, Task> Behaviour)> AsyncResultBehaviours { get; } = new();
        /// <summary>
        /// A List of async Behaviours that will be invoked on a certain StatusCode
        /// </summary>
        internal List<(HttpStatusCode StatusCode, Func<HttpStatusCode, Task> Behaviour)> AsyncResponseBehaviour { get; } = new();

        /// <summary>
        /// A List of Behaviours that will be invoked on a certain StatusCode
        /// Here you have access to the result
        /// </summary>
        internal List<(HttpStatusCode StatusCode, Action<HttpStatusCode, object> Behaviour)> ResultBehaviours { get; } = new();
        /// <summary>
        /// A List of Behaviours that will be invoked on a certain StatusCode
        /// </summary>
        internal List<(HttpStatusCode StatusCode, Action<HttpStatusCode> Behaviour)> ResponseBehaviours { get; } = new();
        #endregion

        #region ctor
        /// <summary>
        /// Instanciates a new Builder for a Rest Request
        /// </summary>
        public FinalRestRequestBuilder()
        {
        }
        #endregion

    }

}
