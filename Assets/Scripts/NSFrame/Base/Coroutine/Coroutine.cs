using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NSFrame
{
    public   class Coroutine
    {
        private Stack<IEnumerator> enumerators;
		//private EventHandler<CoroutineExceptionEventArgs> exceptionHandler;
		public bool IsFinished
		{
			get;
			protected set;
		}
		public bool IsSuspended
		{
			get;
			protected set;
		}
		public Exception LastException
		{
			get;
			private set;
		}
		protected Coroutine(IEnumerator enumerator, EventHandler<CoroutineExceptionEventArgs> exceptionHandler)
		{
			this.enumerators = new Stack<IEnumerator>();
			this.enumerators.Push(enumerator);
			//this.exceptionHandler = exceptionHandler;
		}
        public static IEnumerator WaitForNumberOfFrames(uint frames)
		{
            while (frames > 0)
            {
                yield return null;
                --frames;
            }
		}
		public static IEnumerator WaitForSeconds(float seconds)
		{
           
		     while( seconds > 0.0f )
             {
                 yield return null;
                 seconds -= Time.deltaTime;
             }

           
           //  Debug.Log(" Coroutine WaitForSeconds end... ");
		}
		public static Coroutine StartCoroutine(IEnumerator enumerator, EventHandler<CoroutineExceptionEventArgs> exceptionHandler = null)
		{
			if (enumerator == null)
			{
				return null;
			}
			return new Coroutine(enumerator, exceptionHandler);
		}

		public virtual void Run()
		{
            UnityEngine.Profiling.Profiler.BeginSample("Coroutine WaitForSeconds begin");
			while (!this.IsFinished && !this.IsSuspended)
			{
				IEnumerator enumerator = this.enumerators.Peek();
				if (enumerator != null)
				{
					bool flag = enumerator.MoveNext();
					if (flag)
					{
						if (enumerator.Current is IEnumerator)
						{
							this.enumerators.Push((IEnumerator)enumerator.Current);
							//Debug.Log( "enumerator  Current is IEnumerator" );
						}
						else
						{
							if (enumerator.Current is Coroutine)
							{
								this.enumerators.Push(((Coroutine)enumerator.Current).EnumerableRun());
							}
							else
							{
								if (enumerator.Current == null)
								{
									//Debug.Log( "enumerator.Current == null" );
									break;
								}
								else
								{
									//Debug.Log( "wait for some thing" +   enumerator.Current.GetType());
								}
							}
						}
					}
					else
					{
						this.enumerators.Pop();
						this.IsFinished = (this.enumerators.Count == 0);
					}
				}
				else
				{
					this.IsFinished = true;
				}
			}
            UnityEngine.Profiling.Profiler.EndSample();
		}
		public virtual void RunUntilIsFinished()
		{
			while (!this.IsFinished)
			{
				this.Run();
			}
		}


		protected virtual IEnumerator EnumerableRun()
		{
			while (!this.IsFinished && !this.IsSuspended)
			{
				IEnumerator enumerator = this.enumerators.Peek();
				if (enumerator != null)
				{
					bool flag = enumerator.MoveNext();
					if (flag)
					{
						if (enumerator.Current is IEnumerator)
						{
							this.enumerators.Push((IEnumerator)enumerator.Current);
							//Debug.Log( "enumerator  Current is IEnumerator" );
						}
						else
						{
							if (enumerator.Current is Coroutine)
							{
								this.enumerators.Push(((Coroutine)enumerator.Current).EnumerableRun());
							}
							else
							{
								if (enumerator.Current == null)
								{
									//Debug.Log( "enumerator.Current == null" );
									yield return null;
								}
								else
								{
									yield return enumerator.Current;
									//Debug.Log( "wait for some thing" +   enumerator.Current.GetType());
								}
							}
						}
					}
					else
					{
						this.enumerators.Pop();
						this.IsFinished = (this.enumerators.Count == 0);
					}
				}
				else
				{
					this.IsFinished = true;
				}
			}

		}
    }
}
