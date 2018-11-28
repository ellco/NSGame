using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace NSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public class ThreadManager : MonoBehaviour
    {
        private static ThreadManager _instance;
        public static ThreadManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    UnityEngine.Debug.LogError("【Error】【ThreadManager】Get instance Fail!!!");
                }
                return _instance;
            }
        }

        private List<Thread> _threadList = new List<Thread>();

        private Action _mainThreadAction;
        public event Action MainThreadAction
        {
            remove
            {
                lock(this)
                {
                    if(_mainThreadAction != null)
                    {
                        _mainThreadAction -= value;
                    }
                }
            }
            add
            {
                lock(this)
                {
                    if(_mainThreadAction == null)
                    {
                        _mainThreadAction = value;
                    }
                    else
                    {
                        _mainThreadAction += value;
                    }
                }
            }
        }
        
        public void Awake()
        {
            if(_instance != null)
            {
                UnityEngine.Debug.LogError("【Error】【ThreadManager】Instance dumplicate!!!");
            }
            else
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            _instance = this.GetComponent<ThreadManager>();
            UnityEngine.Debug.Log("【ThreadManager】Initialize!!!");
        }

        private void Update()
        {
            RunMainThreadAction();
        }

        private void RunMainThreadAction()
        {
            lock(this)
            {
                if (_mainThreadAction != null)
                {
                    Action actionList = _mainThreadAction;//保存任务列表，防止中途被添加删除;
                    if (actionList != null)
                    {
                        try
                        {
                            //UnityEngine.Debug.Log(string.Format("【ThreadManager】RunMainThreadAction @thread {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
                            actionList();
                        }
                        catch (Exception e)
                        {
                            UnityEngine.Debug.LogError("【Error】【ThreadManager】RunMainThreadAction exception:" + e.ToString());
                        }
                    }
                    MainThreadAction -= _mainThreadAction;
                }
            }
            
        }

        public Thread CreateThread(ThreadStart start)
        {
            Thread thread = new Thread(start);
            thread.IsBackground = true;
            _threadList.Add(thread);
            return thread;
        }

        public void OnDestroy()
        {
            for (int i = 0; i < _threadList.Count; i++)
            {
                _threadList[i].Abort();
            }
            _threadList.Clear();
        }

    }
}
