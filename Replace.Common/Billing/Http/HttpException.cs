using System;
using System.Net;
using System.Runtime.Serialization;

namespace Replace.Common.Billing.Http
{
    [Serializable]
    public class HttpException : Exception
    {
        public HttpListenerContext Context
        {
            get;
            protected set;
        }

        public HttpException(HttpListenerContext context)
        {
            this.Context = context;
        }

        public HttpException(HttpListenerContext context, string message) : base(message)
        {
            this.Context = context;
        }

        public HttpException(HttpListenerContext context,string message, Exception inner) : base(message, inner)
        {
            this.Context = context;
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}