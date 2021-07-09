using System.Diagnostics;
using System.Threading.Tasks;

namespace FinalRest.sample.console
{
    public class Nav
    {
        public static Task HandleAuthFailAsync(string response)
        {
            Debug.WriteLine($"Auth failes with: {response}");
            return Task.CompletedTask;
        }
    }
}
