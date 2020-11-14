using WebService.Components;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace WebService.Controllers
{
    [Controller]
    public class MessageController
    {
        [Autowired] private readonly MessageHandler messages = null!;
        
        [Get("/messages")]
        public Response ListMessages(PathVariable<string> messageId)
        {
            return Response.PlainText(
                messageId.Ok ? messages.GetMessage(messageId.Value!) : messages.GetAllMessages());
        }

        [Post("/messages")]
        public Response AddMessage(string? payload)
        {
            if (payload == null) return Response.Status(Status.BadRequest);
            var id = messages.AddMessage(payload);
            return Response.PlainText(id);

        }

        [Put("/messages")]
        public Response UpdateMessage(PathVariable<string> messageId, string? payload)
        {
            if (!messageId.Ok || payload == null) return Response.Status(Status.BadRequest);
            messages.UpdateMessage(messageId.Value!, payload);
            return Response.Status(Status.NoContent);

        }
        
        [Delete("/messages")]
        public Response DeleteMessage(PathVariable<string> messageId)
        {
            if (!messageId.Ok) return Response.Status(Status.BadRequest);
            messages.DeleteMessage(messageId.Value!);
            return Response.Status(Status.NoContent);
        }
        
    }
}