using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace WebService_Lib.Server
{
    /// <summary>
    /// Used to return responses in REST endpoints.
    /// </summary>
    public class Response
    {
        private bool isStatus;
        private bool isText;
        private bool isJSON;

        private uint status;
        private string? payload;

        private Response(uint status)
        {
            this.isStatus = true;
            this.status = status;
        }

        private Response(Status status)
        {
            var code = (uint)status;
            this.isStatus = true;
            this.status = code;
        }

        private Response(Dictionary<string, object> json)
        {
            this.isJSON = true;
            // Deserialize json
            // See: https://www.newtonsoft.com/json/help/html/SerializeDictionary.htm
            this.payload = JsonConvert.SerializeObject(json);
        }

        private Response(string plainText)
        {
            this.isText = true;
            this.payload = plainText;
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