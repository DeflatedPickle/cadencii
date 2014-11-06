using System;

namespace Cadencii.Gui
{
	public abstract class Timer
	{
		public abstract event EventHandler Tick;

		public abstract bool Enabled { get; set; }

		public abstract int Interval { get; set; }

		public abstract void Start ();

		public abstract void Stop ();
	}
}

