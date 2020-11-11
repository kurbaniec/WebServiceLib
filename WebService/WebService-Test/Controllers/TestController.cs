using System.Collections.Generic;
using WebService_Lib;
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

        private string test;

        public TestLogger Logger => logger;

        public TestController()
        {
            test = "Test";
        }

        [Get("/hi")]
        public Response Hi()
        {
            return Response.PlainText("Hi!");
        }

        [Get("/secured")]
        public Response Secure(AuthDetails? authDetails)
        {
            return Response.PlainText("Secured");
        }

        [Post("/secured2")]
        public Response Secure2(AuthDetails? authDetails, string? payload)
        {
            return Response.PlainText("Secured2");
        }

        [Put("/insert")]
        public Response Insert(Dictionary<string, object>? json)
        {
            return Response.PlainText("Json");
        }

        [Patch("/patch")]
        public Response SomePatch(string? payload, AuthDetails? authDetails)
        {
            return Response.PlainText("Patch");
        }

        [Delete("/delete")]
        public Response DeleteId(PathVariable<int> id)
        {
            return Response.Status(Status.Ok);
        }
        
        [Delete("/delete2")]
        public Response DeleteIdByRequest(RequestParam id)
        {
            return Response.Status(Status.Ok);
        }
    }
}