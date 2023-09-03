using System;

namespace Infra.MVP.ViewLoader
{
    public interface IViewLoader
    {
        public void LoadView<T>(Action<T> action) where T : View;
    }
}