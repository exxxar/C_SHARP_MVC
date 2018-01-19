using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    internal class AsyncHttpServer : IDisposable
    {
        public AsyncHttpServer()
        {
            listener = new HttpListener();
        }

        public void Start(string prefix)
        {
            lock (listener)
            {
                if (!isRunning)
                {
                    listener.Prefixes.Clear();
                    listener.Prefixes.Add(prefix);
                    listener.Start();

                    listenerThread = new Thread(Listen)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.Highest
                    };
                    listenerThread.Start();

                    isRunning = true;
                }
            }
        }

        public void Stop()
        {
            lock (listener)
            {
                if (!isRunning)
                    return;

                listener.Stop();

                listenerThread.Abort();
                listenerThread.Join();

                isRunning = false;
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            Stop();

            listener.Close();
        }

        private void Listen()
        {
            while (true)
            {
                try
                {
                    if (listener.IsListening)
                    {
                        var context = listener.GetContext();
                        Task.Run(() => HandleContextAsync(context));
                    }
                    else Thread.Sleep(0);
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception error)
                {
                    // TODO: log errors
                }
            }
        }

        private async Task HandleContextAsync(HttpListenerContext listenerContext)
        {
            // TODO: implement request handling

            Console.WriteLine(listenerContext.Request.RawUrl);


            String buf = "";
            Type t = typeof(PageController);
            MethodInfo[] attrs = t.GetMethods();
        
            int errorCode = 404;
        
            foreach (MethodInfo m in attrs)
            {
                foreach (CustomAttributeData cd in m.CustomAttributes)
                {
                    String path = "";
                    PageAttribute.HttpMethod method = PageAttribute.HttpMethod.GET;

                    Console.WriteLine(cd.AttributeType == typeof(PageAttribute) ? "мой атрибут" : "хз что");
                    PageAttribute tx = (PageAttribute)Attribute.GetCustomAttribute(m, typeof(PageAttribute));
                    if (tx != null)
                    {
                        path = tx.path;
                        method = tx.method;
                    }

                    if (cd.AttributeType == typeof(PageAttribute)
                        && path.Equals(listenerContext.Request.RawUrl)
                        && method.ToString().Equals(listenerContext.Request.HttpMethod))
                    {
                        try
                        {
                            buf = (string)m.Invoke(Activator.CreateInstance(typeof(PageController)), new object[] { listenerContext.Request });
                            errorCode = -1;
                        }catch(TargetInvocationException e)
                        {
                           errorCode = ((PageException)e.InnerException).code;                           
                           break;
                        }
                    }
                }

             
            }

            if (errorCode!=-1)
            {
                Type t2 = typeof(ErrorController);
                MethodInfo[] errors = t2.GetMethods();
                foreach (MethodInfo m in errors)
                {
                    foreach (CustomAttributeData cd in m.CustomAttributes)
                    {

                        int localErrorCode = 0;
                        ErrorAttribute tx = (ErrorAttribute)Attribute.GetCustomAttribute(m, typeof(ErrorAttribute));
                        if (tx != null)
                            localErrorCode = tx.errorCode;                                

                        if (cd.AttributeType == typeof(ErrorAttribute)
                            && errorCode == localErrorCode)                          
                        {                            
                              buf = (string)m.Invoke(Activator.CreateInstance(typeof(ErrorController)), new object[] { listenerContext.Request });                            
                        }
                    }


                }
            }

            listenerContext.Response.StatusCode = (int)HttpStatusCode.OK;
            using (var writer = new StreamWriter(listenerContext.Response.OutputStream))
                writer.WriteLine(buf);
        }

        private readonly HttpListener listener;

        private Thread listenerThread;
        private bool disposed;
        private volatile bool isRunning;
    }
}