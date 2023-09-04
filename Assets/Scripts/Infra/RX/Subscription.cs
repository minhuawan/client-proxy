using System;

namespace Infra.RX
{
    public class Subscription<T> : IDisposable
    {
        private Subject<T> subject;

        public delegate T function();

        private function func;

        public Subscription(Subject<T> subject, function func)
        {
            this.func = func;
            this.subject = subject;
        }

        public void Dispose()
        {
            if (subject != null)
            {
                subject.Unsubscribe(this);
            }
        }
    }
}