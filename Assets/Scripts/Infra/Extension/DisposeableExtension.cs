using System;
using System.Collections.Generic;

namespace Infra.Extension
{
    public static class DisposeableExtension
    {
        public static T AddTo<T>(this T disposable, ICollection<IDisposable> container)
            where T : IDisposable
        {
            if (disposable == null) throw new ArgumentNullException("disposable");
            if (container == null) throw new ArgumentNullException("container");

            container.Add(disposable);

            return disposable;
        }
    }

    public abstract class DisposableContainer : IDisposable
    {
        protected List<IDisposable> disposables = new List<IDisposable>();
        public bool disposed { get; protected set; }

        public virtual void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            for (var i = 0; i < disposables.Count; i++)
            {
                disposables[i].Dispose();
            }

            disposables.Clear();
        }
    }
}