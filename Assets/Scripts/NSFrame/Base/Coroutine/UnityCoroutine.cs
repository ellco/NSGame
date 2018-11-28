using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSFrame
{
	public class UnityCoroutine : Coroutine
	{
		public UnityEngine.Coroutine Coroutine
		{
			get;
			private set;
		}
		public UnityCoroutine(IEnumerator enumerator, EventHandler<CoroutineExceptionEventArgs> exceptionHandler = null) : base(enumerator, exceptionHandler)
		{
		}

		public static UnityCoroutine StartCoroutine(MonoBehaviour behaviour, IEnumerator enumerator, EventHandler<CoroutineExceptionEventArgs> exceptionHandler = null)
		{
			UnityCoroutine unityCoroutine = new UnityCoroutine(enumerator, exceptionHandler);
			unityCoroutine.Coroutine = behaviour.StartCoroutine(unityCoroutine.EnumerableRun());
			return unityCoroutine;
		}

        private static IEnumerator NextFrameTaskImp(Action task, int frameDelay=1)
        {
            while (frameDelay-->0) yield return null;
            if (task != null)
            {
                task();
            }
        }

        private static IEnumerator DelayTimeTaskImp(Action task, float delay_time)
        {
            yield return new WaitForSeconds(delay_time);

            if (task != null)
            {
                task();
            }
        }

        //延迟指定帧之后执行，默认下一帧
        public static void ProcessNextFrame(MonoBehaviour behaviour,  Action task, int frameDelay=1)
        {
            if (behaviour)
            {
                behaviour.StartCoroutine(NextFrameTaskImp(task, frameDelay));
            }
        }

        //指定时间之后执行（秒）
        public static void ProcessDelay(MonoBehaviour behaviour, float delay_time, Action task)
        {
            if (task == null)
                return;

            if (behaviour)
            {
                behaviour.StartCoroutine(DelayTimeTaskImp(task, delay_time));
            }
        }

        //执行直到条件满足退出
        public static void ProcessUntil(MonoBehaviour behaviour,  Func<bool> task)
        {
            if (task == null)
                return;

            if (behaviour)
            {
                behaviour.StartCoroutine(DoUntilImp(task));
            }
        }

        private static IEnumerator DoUntilImp(Func<bool> task)
        {
            while(true)
            {
                if (task() == true)
                    break;
                yield return null;             
            }
        }
	}
}

