using System;
using System.Net;

namespace Replace.Common.Billing.Http
{
    [Serializable]
    public class AccessDeniedException : HttpException
    {
        public AccessDeniedException(HttpListenerContext context) : base(context)
        {
        }

        public AccessDeniedException(HttpListenerContext context, string message) : base(context, message)
        {
        }

        public AccessDeniedException(HttpListenerContext context, string message, Exception inner) : base(context, message, inner)
        {
        }

        protected AccessDeniedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}