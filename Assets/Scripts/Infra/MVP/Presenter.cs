using System;
using System.Collections.Generic;

namespace Infra.MVP
{
    public abstract class Presenter : IDisposable
    {
#if UNITY_EDITOR
        protected static readonly IViewLoader viewLoader = new EditorViewLoader();
#else
        protected static readonly IViewLoader viewLoader = new ViewLoader();
#endif


        protected List<IDisposable> disposables = new List<IDisposable>();

        public virtual void Dispose()
        {
            for (var i = 0; i < disposables.Count; i++)
            {
                disposables[i].Dispose();
            }

            disposables.Clear();
        }
    }
}