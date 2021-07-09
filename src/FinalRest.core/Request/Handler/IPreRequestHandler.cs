namespace FinalRest.core
{
    public interface IPreRequestHandler
    {

        /// <summary>
        /// A callback that acts before the request is made.
        /// Here you can add headers to this request
        /// </summary>
        /// <param name="headers"></param>
        void HandlePreRequest(FinalRestHeaderCollection headers);
    }
}
