using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace PanelWork;

public sealed class PanelSynchronizationContext : SynchronizationContext {
    readonly Action<Task> onFaulted;

    readonly ConcurrentQueue<CallbackState> callbacks = [];

    Exception exception;

    public PanelSynchronizationContext() {
        onFaulted = OnFaulted;
    }

    public override void Post(SendOrPostCallback callback, object state) {
        callbacks.Enqueue(new(callback, state));
    }

    public void Run(Task task) {
        task.ContinueWith(onFaulted, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
    }

    public void Update() {
        while(callbacks.TryDequeue(out CallbackState callback))
            callback.Callback(callback.State);

        if(exception is not null)
            ExceptionDispatchInfo.Throw(exception);
    }

    void OnFaulted(Task task) {
        exception ??= task.Exception.GetBaseException();
    }

    readonly struct CallbackState(SendOrPostCallback callback, object state) {
        public readonly SendOrPostCallback Callback = callback;

        public readonly object State = state;
    }
}
