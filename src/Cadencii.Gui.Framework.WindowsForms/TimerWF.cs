using System;
using System.ComponentModel;
using Cadencii.Gui;

namespace cadencii
{
	public class TimerWF : Timer
	{
		readonly System.Windows.Forms.Timer timer;

		public TimerWF ()
		{
			timer = new System.Windows.Forms.Timer ();
		}

		public TimerWF (IContainer components)
		{
			timer = new System.Windows.Forms.Timer (components);
		}

		public override void Dispose ()
		{
			timer.Dispose ();
		}

		public override event EventHandler Tick {
			add { timer.Tick += value; }
			remove { timer.Tick -= value; }
		}

		public override void Start ()
		{
			timer.Start ();
		}

		public override void Stop ()
		{
			timer.Stop ();
		}

		public override bool Enabled {
			get { return timer.Enabled; }
			set { timer.Enabled = value; }
		}

		public override int Interval {
			get { return timer.Interval; }
			set { timer.Interval = value; }
		}
	}
}

