using System.Web.Http;

namespace KatanaConsoleHost
{
    public class GreetingController : ApiController
    {
        public Greeting Get()
        {
            return new Greeting { Text = "Hello you awesome world" };
        }
    }
}
