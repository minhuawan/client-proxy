using Infra.Extension;

namespace Infra.MVP
{
    public abstract class Presenter : DisposableContainer
    {
        public abstract View view { get; protected set; }

        public Presenter()
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual void LoadView<T>() where T : View
        {
            View.LoadView<T>(OnViewLoaded);
        }

        protected virtual void OnViewLoaded(View v)
        {
            if (v == null)
            {
                return;
            }

            view = v;
        }

        protected virtual void Appear()
        {
            view.Appear();
        }

        protected virtual void Disappear()
        {
            view.Disappear();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}