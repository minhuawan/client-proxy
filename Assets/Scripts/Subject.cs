using System;
using System.Collections.Generic;

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

public class Subject<T> : IDisposable
{
    private List<Subscription<T>> subscriptions = new List<Subscription<T>>();
    public string debugId { get; }

    public Subject(string debugId = "")
    {
        this.debugId = debugId;
    }

    public IDisposable Subscribe(Subscription<T>.function func)
    {
        Subscription<T> sub = new Subscription<T>(this, func);
        subscriptions.Add(sub);
        return sub;
    }

    public void Unsubscribe(Subscription<T> sub)
    {
        if (subscriptions.Remove(sub))
        {
            sub.Dispose();
        }
    }

    public void Dispose()
    {
        for (var i = 0; i < subscriptions.Count; i++)
        {
            subscriptions[i].Dispose();
        }

        subscriptions.Clear();
    }
}


// public class Observable : IDisposable
// {
//     private Subject subject = new Subject();
//
//     public IDisposable Subscribe()
//     {
//         return subject.Subscribe();
//     }
//
//     public void Dispose()
//     {
//         subject.Dispose();
//     }
// }

public class Program
{
    public static void Main()
    {
        Subject<int> subject = new Subject<int>();
        subject.Subscribe();
    }
}