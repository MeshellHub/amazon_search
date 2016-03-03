using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AmazonApp.Models.Networking.SOAPSigning
{

    public class AmazonSigningMessageInspector : IClientMessageInspector
    {
        private string accessKeyId = "";
        private string secretKey = "";

        public AmazonSigningMessageInspector(string accessKeyId, string secretKey)
        {
            this.accessKeyId = accessKeyId;
            this.secretKey = secretKey;
        }

        public Object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            string operation = Regex.Match(request.Headers.Action, "[^/]+$").ToString();
            DateTime now = DateTime.UtcNow;
            String timestamp = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            String signMe = operation + timestamp;
            Byte[] bytesToSign = Encoding.UTF8.GetBytes(signMe);

            Byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            HMAC hmacSha256 = new HMACSHA256(secretKeyBytes);
            Byte[] hashBytes = hmacSha256.ComputeHash(bytesToSign);
            String signature = Convert.ToBase64String(hashBytes);

            request.Headers.Add(new AmazonHeader("AWSAccessKeyId", accessKeyId));
            request.Headers.Add(new AmazonHeader("Timestamp", timestamp));
            request.Headers.Add(new AmazonHeader("Signature", signature));
            return null;
        }

        void IClientMessageInspector.AfterReceiveReply(ref System.ServiceModel.Channels.Message Message, Object correlationState)
        {
        }
    }
}