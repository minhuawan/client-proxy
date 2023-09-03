using System;
using Infra.MVP.ViewLoader;
using UnityEngine;

namespace Infra.MVP
{
    public abstract class View : MonoBehaviour, IDisposable
    {
        private static readonly IViewLoader viewLoader = new EditorViewLoader();

        public static void LoadView<T>(Action<T> action) where T : View
        {
            viewLoader.LoadView<T>(action);
        }


        public virtual void Appear()
        {
            gameObject.SetActive(true);
        }

        public virtual void Disappear()
        {
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
        }
    }
}