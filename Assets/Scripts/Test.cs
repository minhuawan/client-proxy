// using System;
// using System.Collections;
// using Infra.RX;
// using UnityEngine;
//
// public class Test : MonoBehaviour
// {
//     Subject<int> timeEvent = new Subject<int>();
//     private ISubject<int> TimerEvent => timeEvent;
//
//     private IDisposable disposable = null;
//
//     private void Start()
//     {
//         TimerEvent.Subscribe(i =>
//         {
//             Debug.Log("check");
//             if (i > 2)
//             {
//                 timeEvent.Dispose();
//             }
//         });
//         StartCoroutine(UpdateSecond());
//         disposable = TimerEvent.Subscribe(Print);
//     }
//
//     private void Print(int i)
//     {
//         Debug.Log($"s: {i}");
//         if (i > 5)
//         {
//             disposable.Dispose();
//             disposable = null;
//         }
//     }
//
//     private IEnumerator UpdateSecond()
//     {
//         int s = 0;
//         while (true)
//         {
//             yield return new WaitForSeconds(1);
//             timeEvent.OnNext(s++);
//         }
//     }
// }