using System;
using System.Reflection;

namespace Infra.MVP.ViewLoader
{
    public class ViewPathAttribute : Attribute
    {
        public string viewPath { get; protected set; }

        public ViewPathAttribute(string viewPath)
        {
            this.viewPath = viewPath;
        }

        public static string GetPath<T>() where T : View
        {
            ViewPathAttribute attribute = typeof(T).GetCustomAttribute<ViewPathAttribute>();
            if (attribute == null)
            {
                return null;
            }

            return attribute.viewPath;
        }
    }
}