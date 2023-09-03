using System;
using Gameplay.Main;
using UnityEngine;

namespace Gameplay
{
    public class Startup : MonoBehaviour
    {
        private void Start()
        {
            var main = new MainPresenter();
            main.Initialize();
            main.LoadView<MainView>();
        }
    }
}