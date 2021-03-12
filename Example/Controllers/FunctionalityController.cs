using System;
using System.Collections.Generic;
using System.IO;
using WebService_Lib;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace WebService.Controllers
{
    /// <summary>
    /// Controller used to test <c>WebService_Lib</c> functionality.
    /// </summary>
    [Controller]
    public class FunctionalityController
    {
        [Autowired]
        private readonly AuthCheck auth = null!;
        
        [Post("/register")]
        public Response Register(Dictionary<string, object>? payload)
        {
            if (payload == null) return Response.Status(Status.BadRequest);
            if (!payload.ContainsKey("username") || !payload.ContainsKey("password") ||
                !(payload["username"] is string) ||  !(payload["password"]is string))
                return Response.Status(Status.BadRequest);
            var result 
                = auth.Register((payload["username"] as string)!, (payload["password"] as string)!);
            return !result.Item1 ? Response.Status(Status.Conflict) : Response.PlainText(result.Item2, Status.Created);
        }

        [Get("/secret")]
        public Response SecretToEverything()
        {
            return Response.PlainText("42");
        }

        [Get("/json")]
        public Response SomeJson()
        {
            return Response.Json(new Dictionary<string, object>()
            {
                {"int", 1},
                {"float", 1.1},
                {"string", "Hi"}
            });
        }

        [Get("/img")]
        public Response Img()
        {
            // Get example image
            string runningPath = AppDomain.CurrentDomain.BaseDirectory!;
            string imgPath =
                $"{Path.GetFullPath(Path.Combine(runningPath!, @$"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}"))}res{Path.DirectorySeparatorChar}doge.jpg";
            return Response.File(imgPath) ?? Response.Status(Status.NotFound);
        }
    }
}