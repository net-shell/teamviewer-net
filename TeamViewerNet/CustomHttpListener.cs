using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TeamViewerNet
{
    class CustomHttpListener
    {
        public Exception Exception;
        private HttpListener listener;

        private HttpListenerContext context;
        public HttpListenerContext Context
        {
            get { return context; }
            set { context = value; }
        }

        public event OnRequestHandler OnRequest;
        public delegate void OnRequestHandler(CustomHttpListener sender, EventArgs e);

        public bool Start(string prefix, int port)
        {
            if (!HttpListener.IsSupported)
            {
                Exception = new Exception("Not supported");
                return false;
            }

            listener = new HttpListener();
            listener.Prefixes.Add(string.Format(prefix, port));
            listener.Start();

            Context = listener.GetContext();

            return true;
        }

        public void Stop()
        {
            if (listener == null)
                return;

            listener.Stop();
        }
    }
}
