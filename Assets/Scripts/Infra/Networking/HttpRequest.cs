using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Infra.Networking
{
    // Without process Header data
    public static class HttpRequest
    {
        public class HttpRequestEvent
        {
            public UnityWebRequest webRequest;
            public UnityWebRequest.Result Result => webRequest.result;
            public string url => webRequest.url;
            public byte[] data => webRequest.downloadHandler.data;
            public TimeSpan timeSpan;
        }

        public delegate void OnHttpRequestDone(HttpRequestEvent e);

        public static OnHttpRequestDone doneEvent;
        public static OnHttpRequestDone errorEvent;

        private static Dictionary<int, UnityWebRequest> fetchingRequests = new Dictionary<int, UnityWebRequest>();
        private static Dictionary<int, DateTime> requestBeginTimes = new Dictionary<int, DateTime>();


        public static int DoGet(string path)
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            www.SendWebRequest().completed += OnRequestCompleted;
            int key = www.GetHashCode();
            fetchingRequests[www.GetHashCode()] = www;
            requestBeginTimes[key] = DateTime.Now;
            return key;
        }

        private static TimeSpan GetRequestFetchTimeOffset(int hash)
        {
            if (requestBeginTimes.TryGetValue(hash, out DateTime begin))
            {
                return DateTime.Now - begin;
            }

            return TimeSpan.Zero;
        }

        private static void OnRequestCompleted(AsyncOperation operation)
        {
            UnityWebRequestAsyncOperation wop = operation as UnityWebRequestAsyncOperation;
            if (wop == null)
            {
                return;
            }

            int key = wop.webRequest.GetHashCode();
            if (!wop.isDone)
            {
                DispatchError(key, "request not complete corrently");
                return;
            }


            if (!string.IsNullOrEmpty(wop.webRequest.error) || wop.webRequest.result != UnityWebRequest.Result.Success)
            {
                DispatchError(key, $"request get error: {wop.webRequest.error}");
                return;
            }

            DispatchDone(key);
        }

        private static void DispatchError(int hash, string message)
        {
            if (fetchingRequests.TryGetValue(hash, out UnityWebRequest webRequest))
            {
                doneEvent(new HttpRequestEvent()
                {
                    webRequest = webRequest
                });
                fetchingRequests.Remove(hash);
            }
        }

        private static void DispatchDone(int hash)
        {
            if (fetchingRequests.TryGetValue(hash, out UnityWebRequest webRequest))
            {
                errorEvent(new HttpRequestEvent()
                {
                    webRequest = webRequest,
                    timeSpan = GetRequestFetchTimeOffset(hash),
                });
                fetchingRequests.Remove(hash);
            }
        }
    }
}