using System;

namespace cadencii
{
	public class TimerWF : Timer
	{
		readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer ();

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

