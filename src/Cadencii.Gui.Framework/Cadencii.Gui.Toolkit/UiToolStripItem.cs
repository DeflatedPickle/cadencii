using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiToolStripItem : UiComponent, IDisposable
	{
		Font Font {
			get;
			set;
		}

		bool Enabled { get; set; }
		bool Visible { get; set; }
		int Height { get; }
		string Name { get; set; }
		object Tag { get; set; }
		string Text { get; set; }
		string ToolTipText { get; set; }
		Dimension Size { get; set; }

		void PerformClick ();

		event EventHandler MouseEnter;
		event EventHandler Click;
	}
}
