using System;

namespace Infra.RX
{
    public interface ISubject<TSource>
    {
        public IDisposable Subscribe(Subscription<TSource>.function obs);
        public void Unsubscribe(IDisposable sub);
    }
}