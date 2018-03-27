using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
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

        private async void Listen()
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
            
            Type t = typeof(PageController);
            MethodInfo[] attrs = t.GetMethods();
        
            int errorCode = 404;
        
            foreach (MethodInfo m in attrs)
            {
                foreach (CustomAttributeData cd in m.CustomAttributes)
                {
                
                    PageAttribute.HttpMethod method = PageAttribute.HttpMethod.GET;
                    PageAttribute tx = (PageAttribute)Attribute.GetCustomAttribute(m, typeof(PageAttribute));
                    MatchCollection mc = null;

                    if (tx != null)
                    {                       
                        method = tx.method;
                        Regex reg = new Regex(tx.path);
                        mc = reg.Matches(listenerContext.Request.RawUrl);                        
                    }

               
                    if (cd.AttributeType == typeof(PageAttribute)
                        && mc.Count!=0 
                        && method.ToString().Equals(listenerContext.Request.HttpMethod) )
                    {
                        try
                        {
                            List<object> obj = new List<object>();
                            obj.Add(listenerContext.Request);
                            obj.Add(new HttpHelper(listenerContext.Response));
                            foreach (Match item in mc)
                            {
                                for (int i = 1; i < item.Groups.Count;i++) {                                  
                                    obj.Add(item.Groups[i]);
                                }                                  
                            }

                            m.Invoke(Activator.CreateInstance(typeof(PageController)), obj.ToArray());
                            errorCode = -1;
                        }catch(TargetInvocationException e)
                        {
                           errorCode = ((PageException)e.InnerException).code;                                                   
                           break;
                        }
                    }
                   
                }

             
            }

            var fileUrl = listenerContext.Request.RawUrl.Replace("/","\\");
            if (File.Exists($"sites\\{fileUrl}"))
            {
                errorCode = -1;
                listenerContext.Response.StatusCode = (int)HttpStatusCode.OK;

                var f = File.OpenRead($"sites\\{fileUrl}");
                Console.WriteLine($"sites\\{fileUrl}");
                using (FileStream fstream = File.OpenRead($"sites\\{fileUrl}"))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);

                    using (var writer = listenerContext.Response.OutputStream)
                        writer.Write(array, 0, array.Length);
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
                              m.Invoke(Activator.CreateInstance(typeof(ErrorController)), new object[] { listenerContext.Request, new HttpHelper(listenerContext.Response) });                            
                        }
                    }


                }
            }
          
            
        }

        private readonly HttpListener listener;

        private Thread listenerThread;
        private bool disposed;
        private volatile bool isRunning;
    }
}