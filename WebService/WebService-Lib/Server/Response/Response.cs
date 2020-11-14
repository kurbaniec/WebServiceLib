using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebService_Lib.Server
{
    /// <summary>
    /// Used to return responses in REST endpoints.
    /// </summary>
    public class Response
    {
        public bool IsStatus;
        public bool IsText;
        public bool IsJson;

        public uint StatusCode;
        public string StatusName;
        public string? Payload;
        public string? ContentType;

        private Response(uint status)
        {
            this.IsStatus = true;
            this.StatusCode = status;
            this.StatusName = ((Status) this.StatusCode).ToString().ToUpper();
        }

        private Response(Status status)
        {
            var code = (uint)status;
            this.IsStatus = true;
            this.StatusCode = code;
            this.StatusName = status.ToString().ToUpper();
        }

        private Response(Dictionary<string, object> json)
        {
            this.StatusCode = 200;
            this.StatusName = ((Status) this.StatusCode).ToString().ToUpper();
            this.IsJson = true;
            // Deserialize json
            // See: https://www.newtonsoft.com/json/help/html/SerializeDictionary.htm
            this.Payload = JsonConvert.SerializeObject(json);
            this.ContentType = "application/json";
        }

        private Response(string plainText)
        {
            this.StatusCode = 200;
            this.StatusName = ((Status) this.StatusCode).ToString().ToUpper();
            this.IsText = true;
            this.Payload = plainText;
            this.ContentType = "text/plain";
        }

        public static Response Status(Status status)
        {
            return new Response(status);
        }

        public static Response Json(Dictionary<string, object> json)
        {
            return new Response(json);
        }

        public static Response PlainText(string plainText)
        {
            return new Response(plainText);
        }
    }
}