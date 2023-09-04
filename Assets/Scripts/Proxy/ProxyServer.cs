using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Proxy
{
    public class ProxyServerConfiguration
    {
        public readonly string host;
        public readonly int port;

        public readonly string proxyHost;
        public readonly int proxyPort;

        private readonly Dictionary<string, string> mapping = new Dictionary<string, string>();

        public ProxyServerConfiguration(string host, int port, string proxyHost, int proxyPort)
        {
            this.host = host;
            this.port = port;
            this.proxyHost = proxyHost;
            this.proxyPort = proxyPort;
        }

        public void AddPathMapping(string path, string proxyPath)
        {
            mapping[path] = proxyPath;
        }

        public Uri GetPathMapping(Uri uri)
        {
            string path = uri.LocalPath;
            string query = uri.Query;
            string proxyPath = HasPathMapping(uri) ? mapping[path] : path;
            string full = $"{uri.Scheme}://{proxyHost}:{proxyPort}{proxyPath}{query}";
            return new Uri(full);
        }

        public bool HasPathMapping(Uri uri)
        {
            return mapping.ContainsKey(uri.LocalPath);
        }
    }

    public class ProxyServerLifetimeHolder : MonoBehaviour
    {
        private ProxyServer server;

        public static ProxyServerLifetimeHolder Create(ProxyServerConfiguration configuration)
        {
            GameObject go = new GameObject(nameof(ProxyServerLifetimeHolder));
            DontDestroyOnLoad(go);

            ProxyServerLifetimeHolder holder = go.AddComponent<ProxyServerLifetimeHolder>();
            ProxyServer server = new ProxyServer(configuration);
            holder.server = server;

            return holder;
        }

        private void Start()
        {
            server.Startup();
        }

        private void OnDestroy()
        {
            server.Shutdown();
            server = null;
        }
    }

    public class ProxyServer
    {
        private ProxyServerConfiguration configuration;
        private HttpListener server;
        private Thread httpThread;


        public ProxyServer(ProxyServerConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Startup()
        {
            httpThread = new Thread(HandleIncoming);
            httpThread.Start();
        }

        private void HandleIncoming()
        {
            server = new HttpListener();
            server.Prefixes.Add($"http://localhost:{configuration.port}/");
            server.Start();
            while (true)
            {
                try
                {
                    var httpListenerContext = server.GetContext(); // block call
                    HttpListenerRequest request = httpListenerContext.Request;
                    HttpListenerResponse response = httpListenerContext.Response;
                    if (!configuration.HasPathMapping(request.Url))
                    {
                        HandleUnmappedPath(request, response);
                    }
                    else
                    {
                        if (request.HttpMethod == "GET")
                        {
                            HandleHttpGet(request, response);
                        }
                        else if (request.HttpMethod == "POST")
                        {
                            HandleHttpPost(request, response);
                        }
                        else
                        {
                            HandleUnsupportedMethod(request, response);
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignored
                    Debug.LogException(e);
                }
            }
        }

        private void WriteTextToResponse(HttpListenerResponse response, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            WriteBytesToResponse(response, data);
        }

        private void WriteBytesToResponse(HttpListenerResponse response, byte[] data)
        {
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = data.Length;
            response.OutputStream.Write(data, 0, data.Length);
        }

        private void HandleUnsupportedMethod(HttpListenerRequest request, HttpListenerResponse response)
        {
            string message = $"Unsupported http method: {request.HttpMethod} of url: {request.Url}";
            WriteTextToResponse(response, message);
            response.Close();
            Debug.LogError(message);
        }

        private void HandleUnmappedPath(HttpListenerRequest request, HttpListenerResponse response)
        {
            string message;
            // ignore if browser request favicon
            if (request.Url.LocalPath == "/favicon.ico")
            {
                message = "";
            }
            else
            {
                message = $"Unmapped path: {request.Url.LocalPath}";
                Debug.LogError(message);
            }

            WriteTextToResponse(response, message);
            response.Close();
        }

        private void HandleHttpGet(HttpListenerRequest request, HttpListenerResponse response)
        {
            HttpClient client = new HttpClient();
            Uri proxyUri = configuration.GetPathMapping(request.Url);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, proxyUri);
            HttpResponseMessage responseMessage = client.SendAsync(requestMessage).GetAwaiter().GetResult();
            byte[] bytes = responseMessage.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            WriteBytesToResponse(response, bytes);
            response.Close();
        }

        private void HandleHttpPost(HttpListenerRequest request, HttpListenerResponse response)
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            if (server != null)
            {
                server.Stop();
                server = null;
            }

            if (httpThread != null)
            {
                httpThread.Abort();
                httpThread = null;
            }
        }
    }
}