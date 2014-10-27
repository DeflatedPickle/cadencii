using System;
using cadencii.java.awt;

namespace cadencii
{
	public interface RebarBand : IDisposable
	{
		RebarBandCollection Bands { get; set; }
		string Key { get; set; }
		int BandSize { get; set; }
		Point Location { get; }
		bool VariantHeight { get; set; }
		bool AllowVertical { get; set; }
		UiControl Child { get; set; }
		int Header { get; set; }
		int Integral { get; set; }
		int MaxHeight { get; set; }
		int MinHeight { get; set; }
		bool UseChevron { get; set; }
		int IdealWidth { get; set; }
		bool NewRow { get; set; }
		bool UseCoolbarPicture { get; set; }
		bool FixedBackground { get; set; }
		Image BackgroundImage { get; set; }
		Rectangle Bounds { get; set; }
		int ID { get; }

		void CreateBand ();
		void DestroyBand ();
		void Show (UiControl control, Rectangle chevronRect);

		void OnResize (EventArgs e);
		void OnMouseDown (MouseEventArgs e);
		void OnMouseMove (MouseEventArgs e);
		void OnMouseUp (MouseEventArgs e);
		void OnMouseWheel (MouseEventArgs e);
		event EventHandler Resize;
	}
}

