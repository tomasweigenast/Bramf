using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bramf.Tasks
{
    /// <summary>
    /// Provides a safe way to execute multiple tasks at once
    /// </summary>
    public class TaskParallelExecution
    {
        #region Members

        private Dictionary<Task, bool> mPendingTasks = new Dictionary<Task, bool>();
        private Task[] mTaskPool;
        private int mExecutedTasks = 0;
        private int mMaxParallelExecution;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if the task execution has finished
        /// </summary>
        public bool Finished => mPendingTasks.Where(x => !x.Value).Count() <= 0;

        /// <summary>
        /// Action that gets executed when all pending tasks are executed
        /// </summary>
        public Action OnFinish { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskParallelExecution(int maxParallelExecution, Task[] tasks, Action onFinish)
        {
            OnFinish = onFinish;
            mTaskPool = new Task[mMaxParallelExecution];
            mMaxParallelExecution = maxParallelExecution;

            foreach (var task in tasks)
                mPendingTasks.Add(task, false);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskParallelExecution(int maxParallelExecution, Action onFinish)
        {
            mMaxParallelExecution = maxParallelExecution;
            mTaskPool = new Task[mMaxParallelExecution];
            OnFinish = onFinish;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskParallelExecution(int maxParallelExecution)
        {
            mMaxParallelExecution = maxParallelExecution;
            mTaskPool = new Task[mMaxParallelExecution];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a task
        /// </summary>
        public void AddTask(Task task)
            => mPendingTasks.Add(task, false);

        /// <summary>
        /// Executes tasks and waits them to finish
        /// </summary>
        public async Task Execute()
        {
            // Get tasks to complete
            var tasksToComplete = mPendingTasks.Where(x => !x.Value).Select(x => x.Key).Take(mMaxParallelExecution).ToArray();

            // Set task to thread pool
            for (int i = 0; i < mMaxParallelExecution; i++)
                mTaskPool[i] = tasksToComplete[i];

            await Task.WhenAny(mTaskPool);
            Console.WriteLine("Task finished?");

            OnFinish?.Invoke();
        }

        #endregion

        #region Private Helpers

        // Returns a pending task
        Task GetFromPending() => mPendingTasks.Where(x => !x.Value).First().Key;

        Task[] GetPendings(int max) => mPendingTasks.Where(x => !x.Value).Take(max).Select(x => x.Key).ToArray();

        #endregion
    }

}
