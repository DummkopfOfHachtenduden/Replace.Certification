using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Replace.Common.Billing.Http
{
    internal class HttpServer
    {
        private static Logger BillingLogger = LogManager.GetLogger(nameof(BillingManager));

        private HttpListener m_listener = new HttpListener();
        private Thread m_workthread;

        public HttpListenerPrefixCollection Prefixes => m_listener.Prefixes;

        public List<IHttpHandler> Handlers { get; private set; }

        public HttpServer()
        {
            this.Handlers = new List<IHttpHandler>();
        }

        private static void worker(object state)
        {
            var server = state as HttpServer;

            while (server.m_listener.IsListening)
            {
                HttpListenerContext context = server.m_listener.GetContext();

                if (BillingLogger.IsTraceEnabled)
                    BillingLogger.Trace("REQUEST: {0}", context.Request.Url.ToString());

                bool handled = false;
                try
                {
                    foreach (IHttpHandler handler in server.Handlers)
                    {
                        if (handler.Handle(context))
                        {
                            handled = true;
                            break;
                        }
                    }
                }
                catch (AccessDeniedException ex)
                {
                    // Access Denied
                    // Send Status 403
                    BillingLogger.Error("Access Denied: {0} from {1}", ex.Context.Request.Url, ex.Context.Request.RemoteEndPoint);

                    string responseString = "<HTML><BODY>Access denied</BODY></HTML>";
                    var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                    HttpListenerResponse response = context.Response;

                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    response.StatusCode = 403;
                    response.StatusDescription = "Access denied";
                    using (var output = response.OutputStream)
                        output.Write(buffer, 0, buffer.Length);

                    continue;
                }

                // Check is the request was handled
                if (!handled)
                {
                    // Unhandler Request Handler
                    // Send Status 500
                    BillingLogger.Error("Unhandled Request");

                    string responseString = "<HTML><BODY>Unhandled Request</BODY></HTML>";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                    HttpListenerResponse response = context.Response;

                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    response.StatusCode = 500;
                    response.StatusDescription = "Internal Server error";
                    using (var output = response.OutputStream)
                        output.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public void Start()
        {
            m_listener.Start();

            foreach (var prefix in m_listener.Prefixes)
                BillingLogger.Info("Listening on {0}", prefix);

            m_workthread = new Thread(new ParameterizedThreadStart(worker));

            m_workthread.Start(this);
        }

        public void Stop()
        {
            m_listener.Stop();
        }
    }
}