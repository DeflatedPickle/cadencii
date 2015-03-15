using System;
using System.ComponentModel;

namespace Cadencii.Gui
{
	public class TimerXwt : Timer
	{
		readonly System.Timers.Timer timer;

		public TimerXwt ()
		{
			timer = new System.Timers.Timer ();
		}

		public TimerXwt (IContainer components)
		{
			timer = new System.Timers.Timer ();
		}

		#region implemented abstract members of Timer

		public override void Dispose ()
		{
			timer.Dispose ();
		}

		public override void Start ()
		{
			timer.Enabled = true;
		}

		public override void Stop ()
		{
			timer.Enabled = false;
		}

		public override bool Enabled {
			get { return timer.Enabled; }
			set { timer.Enabled = value; }
		}

		public override int Interval {
			get { return (int) timer.Interval; }
			set { timer.Interval = value; }
		}

		public override event EventHandler Tick;

		void OnTimer (object o, System.Timers.ElapsedEventArgs a)
		{
			if (Tick != null)
				Tick (o, a);
		}

		#endregion
	}
}

