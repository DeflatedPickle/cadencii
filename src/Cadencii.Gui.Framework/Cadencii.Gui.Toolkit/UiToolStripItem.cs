using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiToolStripItem : UiComponent, IDisposable
	{
		// FIXME: should be removed, cannot be implemented.
		// only FormMainImpl.updateMenuFonts() uses this (indirectly).
		Font Font {
			get;
			set;
		}

		bool Enabled { get; set; }
		bool Visible { get; set; }
		// FIXME: should be removed, cannot be implemented.
		int Height { get; }
		string Name { get; set; }
		object Tag { get; set; }
		string Text { get; set; }
		// FIXME: should be removed, cannot be implemented.
		string ToolTipText { get; set; }
		// FIXME: should be removed, cannot be implemented.
		Size Size { get; set; }

		// FIXME: should be removed, cannot be implemented.
		void PerformClick ();

		// FIXME: should be removed, cannot be implemented.
		event EventHandler MouseEnter;
		event EventHandler Click;
	}
}

