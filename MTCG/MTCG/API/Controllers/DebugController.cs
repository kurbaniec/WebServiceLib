using MTCG.Components.DataManagement.DB;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace MTCG.API.Controllers
{
    [Controller]
    public class DebugController
    {
        [Autowired] private PostgresDatabase db = null!;

        [Get("/drop")]
        public Response Rebuild()
        {
            db.DropMTCG();
            return Response.Status(Status.Ok);
        }
    }
}