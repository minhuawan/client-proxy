using System;
using UnityEngine;

namespace Infra.MVP
{
    [ViewPath("")]
    public abstract class View : MonoBehaviour, IDisposable
    {
        public abstract void Dispose();
    }

    public class ViewPathAttribute : Attribute
    {
        public string path { get; private set; }

        public ViewPathAttribute(string path)
        {
            this.path = path;
        }
    }
}