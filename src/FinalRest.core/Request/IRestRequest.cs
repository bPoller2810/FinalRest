using System.Collections.Generic;

namespace FinalRest.core
{
    public interface IRestRequest
    {

        ERestMethod Method { get; }
        string Route { get; }
        EBodyType BodyType { get; }

        FinalRestHeaderCollection Headers { get; }

        IPreRequestHandler[] PreRequestHandler { get; }
        IPostRequestHandler[] PostRequestHandler { get; }

        AsyncResultBehaviourDefinition[] AsyncResultBehaviours { get; }
        AsyncResponseBehaviourDefinition[] AsyncResponseBehaviours { get; }
        ResultBehaviourDefinition[] ResultBehaviours { get; }
        ResponseBehaviourDefinition[] ResponseBehaviours { get; }


        IRestRequest Copy();

    }
}
