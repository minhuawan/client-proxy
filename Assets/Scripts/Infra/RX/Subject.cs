using System;
using System.Collections.Generic;

namespace Infra.RX
{
    public class Subject<TSource> : ISubject<TSource>, IDisposable
    {
        private List<Subscription<TSource>> subscriptions = new List<Subscription<TSource>>();
        public string debugId { get; protected set; }
        public bool disposed { get; protected set; }

        public Subject(string debugId = "")
        {
            this.debugId = debugId;
        }

        public IDisposable Subscribe(Subscription<TSource>.function func)
        {
            if (disposed)
                throw new Exception("Subscribe to disposed subject");

            Subscription<TSource> sub = new Subscription<TSource>(this, func);
            subscriptions.Add(sub);
            return sub;
        }

        public void Unsubscribe(IDisposable sub) => Unsubscribe(sub as Subscription<TSource>);

        public void Unsubscribe(Subscription<TSource> sub)
        {
            if (!disposed && sub != null && subscriptions.Remove(sub))
            {
                sub.Dispose();
            }
        }

        public void OnNext(TSource t)
        {
            if (disposed)
                return;

            for (var i = 0; i < subscriptions.Count; i++)
            {
                if (disposed)
                {
                    // break if dispose in OnNext loop, 
                    break;
                }

                subscriptions[i].OnNext(t);
            }
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            for (var i = 0; i < subscriptions.Count; i++)
            {
                subscriptions[i].Dispose();
            }

            subscriptions.Clear();
        }
    }
}