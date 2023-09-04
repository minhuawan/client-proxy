using System;
using UnityEngine;
using Logger = Infra.Log.Logger;

namespace App
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            Logger.GameLogger.Log("123");
        }
    }
}