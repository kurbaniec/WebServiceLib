using WebService_Test.Components;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace WebService_Test.Controllers
{

    [Controller]
    public class TestController
    {
        [Autowired]
        private TestLogger logger;

        public TestLogger Logger => logger;

        [Get("/hi")]
        public Response Hi()
        {
            return Response.PlainText("hi!");
        }
    }
}