using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace PanelWork;

public sealed class PanelSynchronizationContext : SynchronizationContext {
    readonly Action<Task> onFaulted;

    readonly Queue<(SendOrPostCallback Callback, object State)> callbacks = [];

    Exception exception;

    public PanelSynchronizationContext() {
        onFaulted = OnFaulted;
    }

    public override void Post(SendOrPostCallback callback, object state) {
        callbacks.Enqueue((callback, state));
    }

    public void Run(Task task) {
        task.ContinueWith(onFaulted, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
    }

    public void Update() {
        while(callbacks.TryDequeue(out (SendOrPostCallback Callback, object State) callback)) {
            callback.Callback(callback.State);

            Console.WriteLine("ctx");
        }

        if(exception is not null)
            ExceptionDispatchInfo.Throw(exception);
    }

    void OnFaulted(Task task) {
        exception ??= task.Exception.GetBaseException();
    }
}
