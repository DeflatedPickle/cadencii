/*
 * BSplitContainer.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.apputil.
 *
 * cadencii.apputil is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.apputil is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.ComponentModel;
using cadencii;

using AnchorStyles = Cadencii.Gui.Toolkit.AnchorStyles;
using UiControl = Cadencii.Gui.Toolkit.UiControl;
using Orientation = Cadencii.Gui.Toolkit.Orientation;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Models;

namespace Cadencii.Application.Controls
{
    [Serializable]
	public partial class BSplitContainerImpl : ContainerControlImpl, BSplitContainer
    {
		public event EventHandler<PaintEventArgs> Paint {
			add { base.Paint += (o, e) => value (o, e.ToGui ()); }
			remove { throw new NotImplementedException (); }
		}

		public int SplitterWidth {
			get { return model.SplitterWidth; }
			set { model.SplitterWidth = value; }
		}

		public int SplitterDistance {
			get { return model.SplitterDistance; }
			set { model.SplitterDistance = value; }
		}

		public Orientation Orientation {
			get { return model.Orientation; }
			set { model.Orientation = value; }
		}

		public bool SplitterFixed {
			get { return model.SplitterFixed; }
			set { model.SplitterFixed = value; }
		}

		public int Panel1MinSize {
			get { return model.Panel1MinSize; }
			set { model.Panel1MinSize = value; }
		}

		public FixedPanel FixedPanel {
			get { return model.FixedPanel; }
			set { model.FixedPanel = value; }
		}

		public int Panel2MinSize {
			get { return model.Panel2MinSize; }
			set { model.Panel2MinSize = value; }
		}

		Cadencii.Gui.Size BSplitContainer.MinimumSize {
			get { return base.MinimumSize.ToGui (); }
			set { base.MinimumSize = value.ToWF (); }
		}

		public bool Panel2Hidden {
			set { model.setPanel2Hidden (value); }
		}

		public int DividerLocation {
			get { return model.getDividerLocation (); }
			set { model.setDividerLocation (value); }
		}

		public int DividerSize {
			get { return model.getDividerSize (); }
			set { model.setDividerSize (value); }
		}

		public bool Panel1Hidden {
			set { model.setPanel1Hidden (value); }
		}

        private System.ComponentModel.IContainer components = null;

		BSplitContainerModel model;

        public BSplitContainerImpl()
        {
			model = new BSplitContainerModel (this);

			InitializeComponent ();

			model.Initialize ();
        }

		/// <summary> 
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			ApplicationUIHost.Instance.ApplyXml (this, "BSplitContainer.xml");
			this.ResumeLayout(false);
		}

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BSplitterPanel Panel1 { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BSplitterPanel Panel2 { get; private set; }

		public Cadencii.Gui.Toolkit.UiPictureBox Splitter { get; private set; }

		public void SplitterMouseMove (object sender, MouseEventArgs e)
		{
			base.OnMouseMove(e.ToWF ());
			model.ProcessSplitterMouseMove ();
        }
    }

}
