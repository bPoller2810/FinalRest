namespace FinalRest.core
{
    public sealed class FinalRestRequest : IRestRequest
    {
        public ERestMethod Method { get; internal set; }

        public string Route { get; internal set; }

        public EBodyType BodyType { get; internal set; }

        public FinalRestHeaderCollection Headers { get; internal set; }

        public IPreRequestHandler[] PreRequestHandler { get; internal set; }

        public IPostRequestHandler[] PostRequestHandler { get; internal set; }

        public AsyncResponseBehaviourDefinition[] AsyncResponseBehaviours { get; internal set; }

        public ResponseBehaviourDefinition[] ResponeBehaviours { get; internal set; }


        public IRestRequest Copy()
        {
            return new FinalRestRequest
            {
                Method = Method,
                Route = Route,
                BodyType = BodyType,
                PreRequestHandler = PreRequestHandler,
                PostRequestHandler = PostRequestHandler,
                AsyncResponseBehaviours = AsyncResponseBehaviours,
                ResponeBehaviours = ResponeBehaviours,
                Headers = Headers.Copy(),
            };
        }

    }
}
