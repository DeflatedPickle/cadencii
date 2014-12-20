#if MONO
using System;
using Cadencii.Gui.Toolkit;
using System.Collections.Generic;
using System.ComponentModel;
using Cadencii.Gui;

namespace Cadencii.Application.Controls
{
	public class RebarMonoImpl : UserControlImpl, Rebar
	{
		public IntPtr RebarHwnd {
			get {
				throw new NotImplementedException ();
			}
		}

		public Image BackgroundImage {
			get { return base.BackgroundImage.ToAwt (); }
			set { base.BackgroundImage = value.ToWF (); }
		}

		public bool ToggleDoubleClick { get; set; }

		public RebarBandCollection Bands {
			get { return bands ?? (bands = new RebarBandCollectionMonoImpl (this)); }
		}

		RebarBandCollection bands;

		public class RebarBandCollectionMonoImpl : List<RebarBand>, RebarBandCollection
		{
			RebarMonoImpl owner;

			public RebarBandCollectionMonoImpl (RebarMonoImpl owner)
			{
				this.owner = owner;
			}

			public Rebar Rebar {
				get { return owner; }
			}
			public RebarBand this [int i] {
				get { return (RebarBand)this [i]; }
			}
			public RebarBand this [string name] {
				get { return (RebarBand)this.Find (p => p.Key == name); }
			}
		}

		public class RebarBandMonoImpl : ToolBarImpl, RebarBand
		{
			public RebarBandMonoImpl ()
			{
			}

			int id = new Random ().Next ();

			public void CreateBand ()
			{
			}
			public void DestroyBand ()
			{
			}
			public void Show (UiControl control, Cadencii.Gui.Rectangle chevronRect)
			{
			}
			public void OnMouseDown (MouseEventArgs e)
			{
				base.OnMouseDown (e.ToWF ());
			}
			public void OnMouseMove (MouseEventArgs e)
			{
				base.OnMouseMove (e.ToWF ());
			}
			public void OnMouseUp (MouseEventArgs e)
			{
				base.OnMouseUp (e.ToWF ());
			}
			public void OnMouseWheel (MouseEventArgs e)
			{
				base.OnMouseWheel (e.ToWF ());
			}
			public void OnResize (EventArgs e)
			{
				base.OnResize (e);
			}
			public RebarBandCollection Bands { get; set; }
			public string Key { get; set; }
			public int BandSize { get; set; }
			public Cadencii.Gui.Point Location {
				get { return base.Location.ToAwt (); }
				set { base.Location = value.ToWF (); }
			}
			public bool VariantHeight { get; set; }
			public bool AllowVertical { get; set; }
			public UiControl Child {
				get { return Controls.Count > 0 ? (UiControl)Controls [0] : null; }
				set {
					Controls.Clear ();
					Controls.Add ((System.Windows.Forms.Control)value.Native);
				}
			}
			public int Header { get; set; }
			public int Integral { get; set; }
			public int MaxHeight { get; set; }
			public int MinHeight { get; set; }
			public bool UseChevron { get; set; }
			public int IdealWidth { get; set; }
			public bool NewRow { get; set; }
			public bool UseCoolbarPicture { get; set; }
			public bool FixedBackground { get; set; }
			public Cadencii.Gui.Image BackgroundImage {
				get { return base.BackgroundImage.ToAwt (); }
				set { base.BackgroundImage = value.ToWF (); }
			}
			public Cadencii.Gui.Rectangle Bounds {
				get { return base.Bounds.ToAwt (); }
			}
			public int ID {
				get { return id; }
			}
		}
	}
}

#endif
