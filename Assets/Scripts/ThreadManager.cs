using System;
using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    public static ThreadManager Instance;

    private static readonly object Lock = new object();
    private static readonly Queue<Action> MainThreadQueue = new Queue<Action>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateMain();
    }

    public static void ExecuteOnMainThread(Action action)
    {
        if (action == null) return;

        lock (Lock)
        {
            MainThreadQueue.Enqueue(action);
        }
    }

    /// <summary>
    /// Must be called regularly from the main thread (e.g., inside your game/server loop).
    /// This will execute all queued actions safely.
    /// </summary>
    public static void UpdateMain()
    {
        Queue<Action> actionsToRun;

        // Swap queues to avoid locking during execution
        lock (Lock)
        {
            if (MainThreadQueue.Count == 0) return;

            actionsToRun = new Queue<Action>(MainThreadQueue);
            MainThreadQueue.Clear();
        }

        while (actionsToRun.Count > 0)
        {
            try
            {
                actionsToRun.Dequeue()?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Threading] Error executing action: {ex.Message}");
            }
        }
    }
}