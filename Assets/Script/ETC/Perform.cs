using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public delegate void Task();
public class Perform : MonoBehaviour
{
    private static Queue<Task> TaskQueue = new Queue<Task>();
    private static object _queueLock = new object();

    // Update is called once per frame
    void Update()
    {
        lock (_queueLock)
        {
            if (TaskQueue.Count > 0)
            {
                TaskQueue.Dequeue()();
            }
        }
    }

    // schedule task
    public static void OnMainThread(Task newTask)
    {
        lock (_queueLock)
        {
            if (TaskQueue.Count < 100)
                TaskQueue.Enqueue(newTask);
        }
    }
}
