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

        AsyncResponseBehaviourDefinition[] AsyncResponseBehaviours { get; }
        ResponseBehaviourDefinition[] ResponeBehaviours { get; }


        IRestRequest Copy();

    }
}
