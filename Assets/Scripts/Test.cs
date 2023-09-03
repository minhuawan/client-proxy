using Infra.MVP;
using UnityEngine;

public class MainPresenter : Presenter
{
}

public class Test : MonoBehaviour
{
    private void Start()
    {
        Presenter p = new MainPresenter();
        p.Initialize();
    }
}