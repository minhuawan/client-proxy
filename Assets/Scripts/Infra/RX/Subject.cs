using System;
using System.Collections.Generic;

namespace Infra.RX
{
    public class Subject<T> : IDisposable
    {
        private List<Subscription<T>> subscriptions = new List<Subscription<T>>();
        public string debugId { get; }

        public Subject(string debugId = "")
        {
            this.debugId = debugId;
        }

        public IDisposable Subscribe(Subscription<T>.function func)
        {
            Subscription<T> sub = new Subscription<T>(this, func);
            subscriptions.Add(sub);
            return sub;
        }

        public void Unsubscribe(Subscription<T> sub)
        {
            if (subscriptions.Remove(sub))
            {
                sub.Dispose();
            }
        }

        public void Dispose()
        {
            for (var i = 0; i < subscriptions.Count; i++)
            {
                subscriptions[i].Dispose();
            }

            subscriptions.Clear();
        }
    }
}