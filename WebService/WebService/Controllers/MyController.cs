using System;
using System.Collections.Generic;
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
        
        [Get("/hey")]
        public Response Hey(PathVariable<int> p)
        {
            return Response.PlainText(p.Ok ? $"Hey {p.Value}!" : "Hey!");
        }
        
        [Get("/heyParam")]
        public Response Hey(RequestParam p)
        {
            if (!p.Empty)
            {
                var vals = p.Value.Values;
                return Response.PlainText(vals.ToString()!);
            }

            return Response.PlainText("Baum");
        }
        
        [Get("/json")]
        public Response Json()
        {
            return Response.Json(new Dictionary<string, object>() {["a"]="b"});
        }
        
        [Post("/json")]
        public Response GetJson(Dictionary<string, object>? json)
        {
            Console.WriteLine(json);
            return Response.PlainText("json received");
        }
        
    }
}
