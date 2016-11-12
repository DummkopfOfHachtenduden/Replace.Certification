namespace Replace.Common.Billing.Http
{
    internal abstract class IHttpHandler
    {
        public abstract bool Handle(System.Net.HttpListenerContext context);

        protected void SendResult(System.Net.HttpListenerResponse response, string responseString)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;

            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }
    }
}