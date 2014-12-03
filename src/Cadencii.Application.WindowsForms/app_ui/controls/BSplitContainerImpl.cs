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
		event EventHandler<PaintEventArgs> BSplitContainer.Paint {
			add { this.Paint += (o, e) => value (o, e.ToAwt ()); }
			remove { throw new NotImplementedException (); }
		}

		int BSplitContainer.SplitterWidth {
			get { return model.SplitterWidth; }
			set { model.SplitterWidth = value; }
		}

		int BSplitContainer.SplitterDistance {
			get { return model.SplitterDistance; }
			set { model.SplitterDistance = value; }
		}

		Orientation BSplitContainer.Orientation {
			get { return model.Orientation; }
			set { model.Orientation = value; }
		}

		bool BSplitContainer.SplitterFixed {
			get { return model.SplitterFixed; }
			set { model.SplitterFixed = value; }
		}

		int BSplitContainer.Panel1MinSize {
			get { return model.Panel1MinSize; }
			set { model.Panel1MinSize = value; }
		}

		FixedPanel BSplitContainer.FixedPanel {
			get { return model.FixedPanel; }
			set { model.FixedPanel = value; }
		}

		int BSplitContainer.Panel2MinSize {
			get { return model.Panel2MinSize; }
			set { model.Panel2MinSize = value; }
		}

		Cadencii.Gui.Dimension BSplitContainer.MinimumSize {
			get { return MinimumSize.ToAwt (); }
			set { MinimumSize = value.ToWF (); }
		}

		bool BSplitContainer.Panel2Hidden {
			set { model.setPanel2Hidden (value); }
		}

		int BSplitContainer.DividerLocation {
			get { return model.getDividerLocation (); }
			set { model.setDividerLocation (value); }
		}

		int BSplitContainer.DividerSize {
			get { return model.getDividerSize (); }
			set { model.setDividerSize (value); }
		}

		bool BSplitContainer.Panel1Hidden {
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
