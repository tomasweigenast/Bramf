using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Bramf.Collections
{
    /// <summary>
    /// Represents a dynamic way to store sorted objects and get notified when a change is made
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    public class SortedObservableCollection<T> : SortedSet<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Private Members

        private SimpleMonitor mMonitor = new SimpleMonitor(); // A simple monitor
        private const string CountString = "Count";
        private const string IndexerName = "Item[]"; // This must agree with Binding.IndexerName. Its delcared separately here so as to avoid a dependency on PresentationFramework.dll
        private List<T> mItems => this.ToList(); // A list containing the current items

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor used to create a new <see cref="SortedObservableCollection{T}"/>
        /// </summary>
        public SortedObservableCollection() : base() { }

        /// <summary>
        /// Creates a new <see cref="SortedObservableCollection{T}"/> copying the elements of an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="collection">The collection whose elements will be copied to the new list.</param>
        public SortedObservableCollection(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Creates a new <see cref="SortedObservableCollection{T}"/> with a specific comparer
        /// </summary>
        /// <param name="comparer">The comparer to use</param>
        public SortedObservableCollection(IComparer<T> comparer) : base(comparer) { }

        /// <summary>
        /// Creates a new <see cref="SortedObservableCollection{T}"/> with a specific comparer copying the elements of an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="collection">The collection whose items will be copied to the new list.</param>
        /// <param name="comparer">The comparer to use</param>
        public SortedObservableCollection(IEnumerable<T> collection, IComparer<T> comparer) : base(collection, comparer) { }

        #endregion

        #region INotifyPropertyChanged implementation

        /// <summary>
        /// Property changed event
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChanged += value;
            remove => PropertyChanged -= value;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the collection changes, either by adding or removing an item
        /// </summary>
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// PropertyChanged event
        /// </summary>
        protected virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes an item from the collection
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void RemoveItem(T item)
        {
            CheckReentrancy();

            // Base class remove
            base.Remove(item);

            // Fire property changed events
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, mItems.IndexOf(item));
        }

        /// <summary>
        /// Removes an item from the collection
        /// </summary>
        /// <param name="index">The index where the item is</param>
        public void RemoveItem(int index)
        {
            CheckReentrancy();

            // Get item
            T item = mItems.ElementAt(index);

            // Base class remove
            base.Remove(item);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        }

        /// <summary>
        /// Adds an item to the collection
        /// </summary>
        /// <param name="item">The item to add</param>
        public void AddItem(T item)
        {
            CheckReentrancy();

            // Base class add
            base.Add(item);

            // Fire property changed events
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, mItems.IndexOf(item));
        }

        /// <summary>
        /// Returns the index of an item
        /// </summary>
        /// <param name="item">The item to get its index</param>
        public int IndexOf(T item) => mItems.IndexOf(item);

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises a PropertyChanged event
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            => PropertyChanged?.Invoke(this, e);

        /// <summary>
        /// Raised CollectionChanged event to any listeners.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                using (BlockReentrancy())
                    CollectionChanged(this, e);
        }

        /// <summary>
        /// Disallow reentrant attempts to change this collection.
        /// </summary>
        /// /// <remarks>
        /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
        /// <code>
        ///         using (BlockReentrancy())
        ///         {
        ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
        ///         }
        /// </code>
        /// </remarks>
        protected IDisposable BlockReentrancy()
        {
            mMonitor.Enter();
            return mMonitor;
        }

        /// <summary>
        /// Check and assert for reentrant attemps to change this collection.
        /// </summary>
        protected void CheckReentrancy()
        {
            if (mMonitor.Busy)
                if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList().Length > 1))
                    throw new InvalidOperationException("Cannot change ObservableCollection during a CollectionChanged event.");
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Called by base class to clear the items 
        /// </summary>
        public override void Clear()
        {
            CheckReentrancy();
            base.Clear(); // Base class clear
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionReset();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Helper to raise a PropertyChanged event
        /// </summary>
        private void OnPropertyChanged(string propertyName)
            => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
            => OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
            => OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
            => OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));

        /// <summary>
        /// Helper to raise CollectionChanged event with action == Reset to any listeners
        /// </summary>
        private void OnCollectionReset()
            => OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

        #endregion

        #region Private Types

        /// <summary>
        /// This class helps prevent reentrant calls
        /// </summary>
        private class SimpleMonitor : IDisposable
        {
            private int mBusyCount;

            public bool Busy => mBusyCount > 0;

            public void Enter()
                => ++mBusyCount;

            public void Dispose()
                => --mBusyCount;
    
        }

        #endregion
    }
}
