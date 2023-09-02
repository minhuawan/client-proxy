using System;

namespace Infra.RX
{
    public class Subscription<T> : IDisposable
    {
        public delegate void function(T t);

        protected Subject<T> subject;
        protected function func;

        public Subscription(Subject<T> subject, function func)
        {
            this.subject = subject;
            this.func = func;
        }

        public void OnNext(T t)
        {
            func(t);
        }

        public void Dispose()
        {
            if (subject != null)
            {
                subject.Unsubscribe(this);
            }

            subject = null;
            func = null;
        }
    }
}