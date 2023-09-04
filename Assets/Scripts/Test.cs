using Proxy;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        ProxyServerConfiguration configuration = new ProxyServerConfiguration(
            "localhost", 9090,
            "localhost", 9091
        );
        configuration.AddPathMapping("/hi", "/hello");
        ProxyServerLifetimeHolder.Create(configuration);
    }
}