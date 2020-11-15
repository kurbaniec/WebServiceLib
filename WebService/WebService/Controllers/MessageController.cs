using WebService.Components;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace WebService.Controllers
{
    /// <summary>
    /// <c>Controller</c> class that manages message endpoints.
    /// </summary>
    [Controller]
    public class MessageController
    {
        // Automatically get Component 'MessageHandler'
        [Autowired] private readonly MessageHandler messages = null!;
        
        /// <summary>
        /// Used to list messages.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>
        /// If no path variable is given all messages will be listed.
        /// If one is given and is valid, the corresponding message will be listed.
        /// If one faulty is given, a empty string will be returned.
        /// </returns>
        [Get("/messages")]
        public Response ListMessages(PathVariable<string> messageId)
        {
            return Response.PlainText(
                messageId.Ok ? messages.GetMessage(messageId.Value!) : messages.GetAllMessages());
        }

        /// <summary>
        /// Used to add new messages.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>
        /// With a valid payload, the newly created message id will be returned.
        /// A faulty request returns status code 400 (Bad Request).
        /// </returns>
        [Post("/messages")]
        public Response AddMessage(string? payload)
        {
            if (payload == null) return Response.Status(Status.BadRequest);
            var id = messages.AddMessage(payload);
            return Response.PlainText(id);
        }

        /// <summary>
        /// Used to update messages.
        /// If the messageId does not exist, no changes will be made.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="payload"></param>
        /// <returns>
        /// Returns status code 204 (No Content) on a valid request.
        /// A faulty request returns status code 400 (Bad Request).
        /// </returns>
        [Put("/messages")]
        public Response UpdateMessage(PathVariable<string> messageId, string? payload)
        {
            if (!messageId.Ok || payload == null) return Response.Status(Status.BadRequest);
            messages.UpdateMessage(messageId.Value!, payload);
            return Response.Status(Status.NoContent);

        }
        
        /// <summary>
        /// Used to delete messages.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>
        /// Returns status code 204 (No Content) on a valid request.
        /// A faulty request returns status code 400 (Bad Request).
        /// </returns>
        [Delete("/messages")]
        public Response DeleteMessage(PathVariable<string> messageId)
        {
            if (!messageId.Ok) return Response.Status(Status.BadRequest);
            messages.DeleteMessage(messageId.Value!);
            return Response.Status(Status.NoContent);
        }

    }
}