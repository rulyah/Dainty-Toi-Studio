using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.ProcessesTool
{
    public abstract partial class Process
    {
        public bool isRunning { get; protected set; }
        public bool IsRunning() => isRunning;
        
        protected IProcessRunner _runner;
        protected Coroutine _coroutine;
        
        private Action _continueCallback;
        private bool _autoRelease;

        public virtual Process Start()
        {
            if (isRunning)
                throw new UnityException("Process already running.");
            _coroutine = _runner.StartCoroutine(Processing());
            isRunning = true;
            return this;
        }

        public virtual void Stop()
        {
            if (isRunning)
            {
                _runner.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _autoRelease = false;
            _continueCallback = null;
            isRunning = false;
        }

        public Process ContinueWith(Action callback)
        {
            _continueCallback = callback;
            return this;
        }
        
        public Process ReleaseAfterComplete()
        {
            _autoRelease = true;
            return this;
        }

        protected void OnCompleted()
        {
            _continueCallback?.Invoke();
            
            if (_autoRelease)
                Release(this);
        }

        protected virtual IEnumerator Processing()
        {
            yield return null;
            OnCompleted();
        }
    }
    
    public abstract partial class Process
    {
        private static readonly List<Process> _cache;

        static Process() => _cache = new List<Process>(15);
        
        public static T Create<T>() where T : Process, new()
        {
            for (int i = 0; i < _cache.Count; i++)
            {
                if (_cache[i] is T tProcess)
                {
                    _cache.Remove(tProcess);
                    return tProcess;
                }
            }

            return new T();
        }

        public static void Release(Process process)
        {
            process.Stop();
            _cache.Add(process);
        }
    }
}