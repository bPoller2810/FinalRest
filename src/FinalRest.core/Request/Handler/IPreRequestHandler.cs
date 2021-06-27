namespace FinalRest.core
{
    public interface IPreRequestHandler
    {
        void HandlePreRequest(FinalRestHeaderCollection headers);
    }
}
