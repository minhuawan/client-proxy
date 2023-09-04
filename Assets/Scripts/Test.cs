using System;
using System.Text;
using Infra.Networking;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        HttpRequest.httpRequestDoneEvent += (r) =>
        {
            if (r == HttpRequest.HttpRequestResult.Error)
            {
                return;
            }
            Debug.Log($"uri : {r.url}");
            var text = Encoding.UTF8.GetString(r.data);
            Debug.Log($"data: {text}");
        };
        // HttpRequest.DoGet("http://baidu.com");
        HttpRequest.DoGet("http://localhost:9090/test.txt");
        HttpRequest.DoGet("http://localhost:9090/test123.txt");
    }
}