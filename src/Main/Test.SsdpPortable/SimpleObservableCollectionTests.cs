using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rssdp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.RssdpPortable
{
	[TestClass]
	public class SimpleObservableCollectionTests
	{

		#region Constructor Tests

		[TestMethod]
		public void SimpleObservableCollection_Constructor_ConstructsOk()
		{
			var collection = new SimpleObservableCollection<int>();
			Assert.AreEqual(0, collection.Count);
		}

		[TestMethod]
		public void SimpleObservableCollection_Constructor_ConstructsWithContent()
		{
			var collection = new SimpleObservableCollection<int>(new int[] { 1, 2 });
			Assert.AreEqual(2, collection.Count);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[TestMethod]
		public void SimpleObservableCollection_Constructor_ThrowsOnNullContent()
		{
			var collection = new SimpleObservableCollection<int>(null);
			Assert.AreEqual(2, collection.Count);
		}

		#endregion

		#region Add Tests

		[TestMethod]
		public void SimpleObservableCollection_Add_AddsItem()
		{
			var collection = new SimpleObservableCollection<int>();
			collection.Add(5);
			Assert.AreEqual(1, collection.Count);
			Assert.AreEqual(5, collection[0]);
		}

		[TestMethod]
		public void SimpleObservableCollection_Add_AddsNull()
		{
			var collection = new SimpleObservableCollection<object>();
			collection.Add(null);
			Assert.AreEqual(1, collection.Count);
			Assert.AreEqual(null, collection[0]);
		}

		[TestMethod]
		public void SimpleObservableCollection_Add_RaisesItemAddedEvent()
		{
			var signal = new System.Threading.ManualResetEvent(false);
			var collection = new SimpleObservableCollection<int>();

			int addedValue = 0;
			collection.ItemAdded += (s, e) => { addedValue = e.Item; signal.Set(); };
			collection.Add(2);

			if (!signal.WaitOne(1000))
				Assert.Fail("Event not raised.");

			Assert.AreEqual(2, addedValue);
		}

		#endregion

		#region Remove Tests

		[TestMethod]
		public void SimpleObservableCollection_Remove_RemovesItem()
		{
			var collection = new SimpleObservableCollection<int>();
			collection.Add(5);

			collection.Remove(5);
			Assert.AreEqual(0, collection.Count);
		}

		[TestMethod]
		public void SimpleObservableCollection_Remove_RemovesNull()
		{
			var collection = new SimpleObservableCollection<object>();
			collection.Add(null);

			collection.Remove(null);
			Assert.AreEqual(0, collection.Count);
		}

		[TestMethod]
		public void SimpleObservableCollection_Add_RaisesItemRemovedEvent()
		{
			var signal = new System.Threading.ManualResetEvent(false);
			var collection = new SimpleObservableCollection<int>();
			int removedValue = 0;

			collection.ItemAdded += (s, e) => { removedValue = e.Item; signal.Set(); };
			collection.Add(2);

			collection.Remove(2);
			if (!signal.WaitOne(1000))
				Assert.Fail("Event not raised.");

			Assert.AreEqual(2, removedValue);
		}

		#endregion

		#region Clear Tests

		public void SimpleObservableCollection_Clear_ClearsItems()
		{
			var collection = new SimpleObservableCollection<int>(new int[] { 1, 2, 3, 4, 5});

			collection.Clear();
			Assert.AreEqual(0, collection.Count);
		}

		public void SimpleObservableCollection_Clear_RaisesClearedEvent()
		{
			var signal = new System.Threading.ManualResetEvent(false);
			var collection = new SimpleObservableCollection<int>(new int[] { 1, 2, 3, 4, 5 });
			collection.Cleared += (s, e) => signal.Set();

			collection.Clear();
			if (!signal.WaitOne(1000))
				Assert.Fail("Cleared event not raised.");

			Assert.AreEqual(0, collection.Count);
		}

		#endregion

	}
}