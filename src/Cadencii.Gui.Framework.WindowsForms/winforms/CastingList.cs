using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using cadencii.java.awt;
using System.Collections;

namespace cadencii
{
	public static class ListExtensions
	{
		public static void AddRange<T> (this IList<T> list, IEnumerable<T> items)
		{
			foreach (var a in items)
				list.Add (a);
		}
	}

	class CastingList<A,W> : IList<A>
	{
		IList source;
		Func<W,A> forward;
		Func<A,W> backward;

		public CastingList (IList source, Func<W,A> forward, Func<A,W> backward)
		{
			this.source = source;
			this.forward = forward ?? new Func<W,A> (w => (A) (object) w);
			this.backward = backward ?? new Func<A,W> (a => (W) (object) a);
		}

		#region IList implementation

		public int IndexOf (A item)
		{
			return source.IndexOf (backward (item));
		}

		public void Insert (int index, A item)
		{
			source.Insert (index, backward (item));
		}

		public void RemoveAt (int index)
		{
			source.RemoveAt (index);
		}

		public A this [int index] {
			get { return forward ((W) source [index]); }
			set { source [index] = backward (value); }
		}

		#endregion

		#region ICollection implementation

		public void Add (A item)
		{
			source.Add (backward (item));
		}

		public void Clear ()
		{
			source.Clear ();
		}

		public bool Contains (A item)
		{
			return source.Contains (backward (item));
		}

		public void CopyTo (A[] array, int arrayIndex)
		{
			throw new NotImplementedException ();
		}

		public bool Remove (A item)
		{
			var w = backward (item);
			if (!source.Contains (w))
				return false;
			source.Remove (w);
			return true;
		}

		public int Count {
			get { return source.Count; }
		}

		public bool IsReadOnly {
			get { return source.IsReadOnly; }
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<A> GetEnumerator ()
		{
			return source.Cast<W> ().Select (w => forward (w)).GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion
	}
}
