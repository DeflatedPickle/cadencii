using System;
using Cadencii.Gui;

namespace Cadencii.Gui.Toolkit
{
	public interface UiUserControl : UiControl
	{
		event EventHandler Load;

		BorderStyle BorderStyle { get; set; }
		void AddControl (UiControl control);

		// actually ContainerControl member.
		AutoScaleMode AutoScaleMode { get; set; }

		// actually ContainerControl member and SizeF.
		Dimension AutoScaleDimensions { get; set; }

		// The same as UiForm property.
		// This is not a form specific feature, but since Control.DoubleBuffered is protected,
		// it cannot be an interface member (technically it can, but it's annoying to add impl.
		// all around) so expose it here.
		bool DoubleBuffered { get; set; }

		// custom property that accesses "Styles"
		bool UserPaint { get; set; }
		bool AllPaintingInWmPaint { get; set; }

		void OnPaint (PaintEventArgs e);
	}
	
}
