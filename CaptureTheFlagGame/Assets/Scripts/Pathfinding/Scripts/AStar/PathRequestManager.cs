using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Pathfinding.Scripts.Grid;
using UnityEngine;

namespace Assets.Scripts.Pathfinding.Scripts.AStar
{
    /// <summary>
    /// Responsible to process the pathfinding in a separated thread and then return the result to the main thread
    /// </summary>
    public class PathRequestManager : MonoBehaviour, ISubscriber
    {
        /// <summary>
        /// Singleton reference
        /// </summary>
        public static PathRequestManager Instance;
        /// <summary>
        /// The pathfinding to be used
        /// </summary>
        [Tooltip("The pathfinding to be used")]
        public AStar AStar;

        /// <summary>
        /// The results waiting to be returned to the main thread
        /// </summary>
        private readonly Queue<PathResult> _results = new Queue<PathResult>();
        /// <summary>
        /// The requests pending process in a separated thread
        /// </summary>
        private readonly HashSet<PathRequest> _requests = new HashSet<PathRequest>();

        /// <summary>
        /// The thread that will process the requests
        /// </summary>
        private Thread _processRequestThread;

        /// <summary>
        /// Indicates if the thread is still running
        /// </summary>
        private bool _threadRunning;


        /// <summary>
        /// The grid from the used A*. This allows different implementations of the A* and different versions of the grid to be used by the manager to create paths
        /// </summary>
        public PathfindingGrid Grid => AStar.Grid;

        private void Awake()
        {
            Instance = this;
            Listeners = new HashSet<IListener>();
        }

        private void Start()
        {
            _processRequestThread = new Thread(ProcessRequests){IsBackground = true};
            _threadRunning = true;
            _processRequestThread.Start();
        }

        /// <summary>
        /// Every frame return all the results to the mainthread, calling their CallBack from here.
        /// </summary>
        private void Update()
        {
            lock (_results)
            {
                if (_results.Count <= 0) return;

                var itemsInQueue = _results.Count;
                for (var i = 0; i < itemsInQueue; i++)
                {
                    var result = _results.Dequeue();
                    result.Invoke();
                }
            }
        }

        /// <summary>
        /// Method running in a separated thread from the main thread. Keeps cycliong the requests and executing them.
        /// </summary>
        private void ProcessRequests()
        {
            try
            {
                while (_threadRunning)
                {
                    while (_requests.Count > 0)
                    {
                        if (_threadRunning == false)
                            break;

                        PathRequest nextRequest;
                        lock (_requests)
                            nextRequest = _requests.FirstOrDefault();


                        ThreadStart threadStart = delegate
                        {
                            Instance.AStar.FindPath(nextRequest, Instance.FinishedProcessingPath);
                        };

                        threadStart.Invoke();
                        if (_requests.Count > 0)
                        {
                            lock (_requests)
                                _requests.Remove(nextRequest);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e + e.StackTrace);
            }
        }

        /// <summary>
        /// Add a new request
        /// </summary>
        public void RequestPath(PathRequest request)
        {
            lock (_requests)
            {
                _requests.Remove(request);
                _requests.Add(request);
            }
        }

        /// <summary>
        /// Add a new result. Runs outside the mainthread.
        /// </summary>
        public void FinishedProcessingPath(PathResult result)
        {
            lock (_results)
            {
                _results.Enqueue(result);
            }
        }

        /// <summary>
        /// Stops the thread
        /// </summary>
        private void OnApplicationQuit()
        {
            _threadRunning = false;
            _processRequestThread.Abort();
        }

        private void OnDestroy()
        {
            _threadRunning = false;
            _processRequestThread.Abort();
        }

        public HashSet<IListener> Listeners { get; set; }
        public void RegisterListener(IListener listener)
        {
            if (Listeners.Contains(listener) == false)
                Listeners.Add(listener);
        }

        public void RemoveListener(IListener listener)
        {
            if (Listeners.Contains(listener))
                Listeners.Remove(listener);
        }

        public void Notify()
        {
            foreach (var listener in Listeners)
                listener.Notify();
        }
    }
}
