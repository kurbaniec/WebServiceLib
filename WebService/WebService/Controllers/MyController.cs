using WebService.Components;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace WebService.Controllers
{
    [Controller]
    class MyController
    {
        [Autowired]
        private Logger logger;

        [Get("/hi")]
        public Response Hi()
        {
            return Response.PlainText("Hi!");
        }
    }
}
