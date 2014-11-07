using System;

namespace Cadencii.Gui
{
	public abstract class Timer : IDisposable
	{
		public abstract event EventHandler Tick;

		public abstract bool Enabled { get; set; }

		public abstract int Interval { get; set; }

		public abstract void Dispose ();

		public abstract void Start ();

		public abstract void Stop ();
	}
}

