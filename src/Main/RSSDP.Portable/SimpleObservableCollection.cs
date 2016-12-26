using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rssdp
{
	/// <summary>
	/// Represents a collection with events for observing some simple &amp; common changes.
	/// </summary>
	/// <remarks>
	/// <para>This class is used because the obserable collection provided by the .Net framework is not avaiable on all supported platforms.</para>
	/// </remarks>
	/// <typeparam name="T">The of item stored in the collection.</typeparam>
	public class SimpleObservableCollection<T> : ICollection<T>
	{

		#region Fields

		private List<T> _Items;

		#endregion

		#region Events

		/// <summary>
		/// Raised when the <see cref="Add(T)"/> method is called.
		/// </summary>
		public event EventHandler<SimpleObservableCollectionEventArgs<T>> ItemAdded;
		/// <summary>
		/// Raised when the <see cref="Remove(T)"/> method is called if it actually removes an item.
		/// </summary>
		public event EventHandler<SimpleObservableCollectionEventArgs<T>> ItemRemoved;
		/// <summary>
		/// Raised when the <see cref="Clear"/> method is called.
		/// </summary>
		public event EventHandler Cleared;

		#endregion

		#region ICollection Implementation

		/// <summary>
		/// Default constructor.
		/// </summary>
		public SimpleObservableCollection()
		{
			_Items = new List<T>();
		}

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <remarks>
		/// <para>An enumerable set of items to initialize the collection with.</para>
		/// </remarks>
		public SimpleObservableCollection(IEnumerable<T> items)
		{
			_Items = new List<T>(items);
		}

		/// <summary>
		/// Returns an integer specifying the number of items in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				return _Items.Count;
			}
		}

		/// <summary>
		/// Returns false, this type of collection is never read only.
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Adds the specified item to the collection.
		/// </summary>
		/// <remarks>
		/// <para>Raises the <see cref="ItemAdded"/> event.</para>
		/// </remarks>
		/// <param name="item">The item to be added.</param>
		/// <seealso cref="ItemAdded"/>
		public void Add(T item)
		{
			_Items.Add(item);
			OnItemAdded(item);
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		/// <remarks>
		/// <para>Raises the <see cref="Cleared"/> event.</para>
		/// </remarks>
		/// <seealso cref="Cleared"/>
		public void Clear()
		{
			_Items.Clear();
			OnCleared();
		}

		/// <summary>
		/// Returns a boolean indicating if this collection contains the specified item.
		/// </summary>
		/// <param name="item">The item to check for the existence of.</param>
		/// <returns>True if the specified item exists in the collection, otherwise false.</returns>
		public bool Contains(T item)
		{
			return _Items.Contains(item);
		}

		/// <summary>
		/// Copies the entire collection to the specified array starting at the specified index.
		/// </summary>
		/// <param name="array">The destination array.</param>
		/// <param name="arrayIndex">The index to copy the first item to.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			_Items.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Returns an enumerator that iterates through this collection.
		/// </summary>
		/// <returns>An implementation of <see cref="IEnumerator{T}"/>.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return _Items.GetEnumerator();
		}

		/// <summary>
		/// Removes the specified item from the collection if it exists, and returns a boolean indicating if the remove actually occurred.
		/// </summary>
		/// <param name="item">The item to be removed.</param>
		/// <remarks>
		/// <para>Raises the <see cref="ItemRemoved"/> event if an item was actually removed.</para>
		/// </remarks>
		/// <returns>True if an item was removed, or false if the item was not found in the collection.</returns>
		/// <seealso cref="ItemRemoved"/>
		public bool Remove(T item)
		{
			var retVal = _Items.Remove(item);
			if (retVal)
				OnItemRemoved(item);

			return retVal;
		}

		/// <summary>
		/// Returns an enumerator that iterates through this collection.
		/// </summary>
		/// <returns>An implementation of <see cref="IEnumerable"/>.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _Items.GetEnumerator();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Raises the <seealso cref="ItemAdded"/> event.
		/// </summary>
		/// <param name="item">The item was that added.</param>
		/// <seealso cref="ItemAdded"/>
		/// <seealso cref="Add(T)"/>
		protected virtual void OnItemAdded(T item)
		{
			ItemAdded?.Invoke(this, new Rssdp.SimpleObservableCollectionEventArgs<T>(item));
		}

		/// <summary>
		/// Raises the <see cref="ItemRemoved"/> event.
		/// </summary>
		/// <param name="item">The item that was removed.</param>
		/// <seealso cref="ItemRemoved"/>
		/// <seealso cref="Remove(T)"/>
		protected virtual void OnItemRemoved(T item)
		{
			ItemRemoved?.Invoke(this, new Rssdp.SimpleObservableCollectionEventArgs<T>(item));
		}

		/// <summary>
		/// Raises the <see cref="Cleared"/> event.
		/// </summary>
		/// <seealso cref="Clear"/>
		/// <seealso cref="Cleared"/>
		protected virtual void OnCleared()
		{
			Cleared?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Returns the item at the specified (zero based) index.
		/// </summary>
		/// <param name="index">The index of the item to return.</param>
		/// <returns>The item from the collection at the specified index.</returns>
		public T this[int index]
		{
			get { return _Items[index]; }
		}

		#endregion
	}

	/// <summary>
	/// Event arguments class for <seealso cref="SimpleObservableCollection{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of item the event is about (sub type of related collection).</typeparam>
	public class SimpleObservableCollectionEventArgs<T> : EventArgs
	{
		private readonly T _Item;

		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="item">The item the event is about.</param>
		public SimpleObservableCollectionEventArgs(T item)
		{
			_Item = item;
		}

		/// <summary>
		/// Returns the item associated with this event.
		/// </summary>
		public T Item
		{
			get { return _Item; }
		}
	}
}