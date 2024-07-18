using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityAsyncAwaitUtil
{
    public static class TaskUtil
    {
        public static Action<ExceptionDispatchInfo> Exceptionhandler = delegate (ExceptionDispatchInfo ex)
        {
            Debug.LogException(ex.SourceException);
        };

        public static void Run(Func<Task> p)
        {
            p().Run();
        }

        public static async Task<bool> WaitTill(Func<bool> func, float timeout, float interval = 0.25f)
        {
            while (!func() && timeout > 0f)
            {
                await Awaiters.SecondsRealtime(interval);
                timeout -= interval;
            }
            return timeout > 0f;
        }

        public static async void Run(this Task task)
        {
            try
            {
                await Awaiters.NextFrame;
                await task;
            }
            catch (Exception source)
            {
                ExceptionDispatchInfo obj = ExceptionDispatchInfo.Capture(source);
                TaskUtil.Exceptionhandler(obj);
            }
        }
    }
}
