using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Infra.Log.Logger;

namespace Infra.MVP
{
    public interface IViewLoader
    {
        public T LoadView<T>() where T : View;
        public Task<T> LoadViewAsync<T>() where T : View;
    }

    public static class ViewPathFinder
    {
        public static string GetViewPath<T>() where T : View
        {
            ViewPathAttribute attr = typeof(T).GetCustomAttribute(typeof(ViewPathAttribute), false) as ViewPathAttribute;
            if (attr == null)
            {
                return null;
            }

            return attr.path;
        }
    }

    public class ViewLoader : IViewLoader
    {
        public T LoadView<T>() where T : View
        {
            throw new System.NotImplementedException();
        }

        public Task<T> LoadViewAsync<T>() where T : View
        {
            throw new System.NotImplementedException();
        }
    }

#if UNITY_EDITOR
    public class EditorViewLoader : IViewLoader
    {
        public T LoadView<T>() where T : View
        {
            string path = ViewPathFinder.GetViewPath<T>();
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return null;
        }

        public Task<T> LoadViewAsync<T>() where T : View
        {
            throw new System.NotImplementedException();
        }
    }
#endif
}