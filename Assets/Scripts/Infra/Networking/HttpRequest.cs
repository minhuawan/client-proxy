using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Infra.Networking
{
    // Without process Header data
    public static class HttpRequest
    {
        public class HttpRequestResult
        {
            public UnityWebRequest webRequest;
            public string url;
            public byte[] data;
            public TimeSpan timeSpan;
        }

        public delegate void OnHttpRequestDone(HttpRequestResult result);

        public static OnHttpRequestDone httpRequestDoneEvent;

        private static Dictionary<int, UnityWebRequest> fetchingRequests = new Dictionary<int, UnityWebRequest>();
        private static Dictionary<int, DateTime> reqeustBeginTime = new Dictionary<int, DateTime>();


        public static int DoGet(string path)
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            www.SendWebRequest().completed += OnRequestCompleted;
            int key = www.GetHashCode();
            fetchingRequests[www.GetHashCode()] = www;
            reqeustBeginTime[key] = DateTime.Now;
            return key;
        }

        public static TimeSpan GetRequestFetchTimeOffset(int hash)
        {
            if (reqeustBeginTime.TryGetValue(hash, out DateTime begin))
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
                DispathDoneError(key, "request not complete corrently");
                httpRequestDoneEvent(HttpRequestResult.Error);
                return;
            }


            if (!string.IsNullOrEmpty(wop.webRequest.error) || wop.webRequest.result != UnityWebRequest.Result.Success)
            {
                DispathDoneError(key, $"request get error: {wop.webRequest.error}");
                return;
            }

            var data = wop.webRequest.downloadHandler.data;
            httpRequestDoneEvent(new HttpRequestResult()
            {
                data = data,
                timeSpan = GetRequestFetchTimeOffset(key),
                url = wop.webRequest.url
            });
            fetchingRequests.Remove(key);
        }

        private static void DispathDoneError(int hash, string message)
        {
            httpRequestDoneEvent(HttpRequestResult.Error);
        }
    }
}