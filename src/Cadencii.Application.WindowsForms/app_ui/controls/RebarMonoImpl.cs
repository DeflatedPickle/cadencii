#if !UI_WIN32API_DEP
using System;
using System.Linq;
using Cadencii.Gui.Toolkit;
using System.Collections.Generic;
using System.ComponentModel;
using Cadencii.Gui;
using System.Collections.ObjectModel;

namespace Cadencii.Application.Controls
{
	public class RebarMonoImpl : UserControlImpl, Rebar
	{
		public RebarMonoImpl ()
		{
			Height = 40;
		}

		public IntPtr RebarHwnd {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool ToggleDoubleClick { get; set; }

		public RebarBandCollection Bands {
			get { return bands ?? (bands = new RebarBandCollectionMonoImpl (this)); }
		}

		RebarBandCollection bands;

		protected override void OnControlAdded (System.Windows.Forms.ControlEventArgs e)
		{
			var tb = e.Control as RebarBand;
			var uc = e.Control as UiControl;
			if (tb != null) {
				uc.Location = new Point (((int) ((Bands.Count - 1) / 2)) * 300 + 11, Bands.Count % 2 * 30);
			}
			base.OnControlAdded (e);
		}

		public class RebarBandCollectionMonoImpl : Collection<RebarBand>, RebarBandCollection
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
				get { return (RebarBand) this.FirstOrDefault (p => p.Key == name); }
			}

			protected override void InsertItem (int index, RebarBand item)
			{
				((UiControl) item).Location = new Point (2, 2); // force override.
				base.InsertItem (index, item);
				owner.Controls.Add ((System.Windows.Forms.Control) item);
			}

			protected override void SetItem (int index, RebarBand item)
			{
				base.SetItem (index, item);
				owner.Controls.SetChildIndex ((System.Windows.Forms.Control) item, index);
			}
		}

		public class RebarBandMonoImpl : PanelImpl, RebarBand
		{
			public RebarBandMonoImpl ()
			{
				AllowVertical = true;
				FixedBackground = true;
				Header = -1;
				Integral = 1;
				Key = "";
				MaxHeight = 40;
				MinHeight = 26;
				NewRow = true;
				UseCoolbarPicture = true;
				Visible = true;
				UseChevron = true;

				BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				ForeColor = Colors.Red.ToNative ();
				Width = 300;
				Height = 26;
				AutoSize = true;
			}

			int id = -1;

			private string _caption = "";
			private bool _embossPicture = true;
			//private GripperSettings _gripSettings = GripperSettings.Auto;
			private int _image = -1;
			private int _minWidth = 24;
			private bool _showCaption = true;
			private bool _throwExceptions = true;
			private bool _useCoolbarColors = true;

			private const int SPACE_CHEVRON_MENU = 6;

			public void CreateBand ()
			{
				throw new NotSupportedException ();
			}
			public void DestroyBand ()
			{
				throw new NotSupportedException ();
			}
			public void Show (UiControl control, Cadencii.Gui.Rectangle chevronRect)
			{
				throw new NotSupportedException ();
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
				get { return base.Location.ToGui (); }
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
				get { return base.BackgroundImage.ToGui (); }
				set { base.BackgroundImage = value.ToWF (); }
			}
			public Cadencii.Gui.Rectangle Bounds {
				get { return base.Bounds.ToGui (); }
			}
			public int ID {
				get { return id; }
			}

			// ignore XML settings. That is only for Windows winforms.
			protected override void OnControlAdded (System.Windows.Forms.ControlEventArgs e)
			{
				var tb = e.Control as UiToolBar;
				if (tb != null) {
					tb.Dock = DockStyle.Top;
					tb.Location = new Point (0, 0);
					tb.Width = 300;
					tb.Wrappable = true;
				}
				base.OnControlAdded (e);
			}
		}
	}
}

#endif
