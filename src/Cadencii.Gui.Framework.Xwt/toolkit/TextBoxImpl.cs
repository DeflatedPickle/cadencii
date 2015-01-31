using System;

namespace Cadencii.Gui.Toolkit
{
	public partial class TextBoxImpl : TextBoxBase, UiTextBox
	{
		// FIXME: this cannot work on Xwt.
		ImeMode UiTextBox.ImeMode { get; set; }

		event EventHandler UiTextBox.TextChanged {
			add { base.Changed += value; }
			remove { base.Changed -= value; }
		}

		void UiTextBox.SelectAll ()
		{
			base.SelectionStart = 0;
			base.SelectionLength = base.Text.Length;
		}

		int UiTextBox.GetFirstCharIndexFromLine (int line)
		{
			var texts = Text.Split (new String [] {Environment.NewLine}, StringSplitOptions.None);
			if (line >= texts.Length)
				throw new ArgumentOutOfRangeException ("line", string.Format ("Requested line #{0}, but current max line is {0}.", line, texts.Length));

			int ret = 0;
			for (int i = 0; i < line; i++)
				ret += texts [i].Length + Environment.NewLine.Length;
			return ret;
		}

		// False is not really expected in the app... so, ignored.
		bool UiTextBox.AcceptsReturn { get; set; }

		// In Xwt no way to distinguish FixedSingle and Fixed3D (are they worthy anyways? I don't think so.)
		BorderStyle UiTextBox.BorderStyle {
			get { return base.ShowFrame ? BorderStyle.FixedSingle : BorderStyle.None; }
			set { base.ShowFrame = value != BorderStyle.None; }
		}

		HorizontalAlignment UiTextBox.TextAlign {
			get {
				switch (base.TextAlignment) {
				default:
				case Xwt.Alignment.Start:
					return HorizontalAlignment.Left;
				case Xwt.Alignment.Center:
					return HorizontalAlignment.Center;
				case Xwt.Alignment.End:
					return HorizontalAlignment.Right;
				}
			}
			set {
				base.TextAlignment =
					value == HorizontalAlignment.Left ? Xwt.Alignment.Start :
					value == HorizontalAlignment.Center ? Xwt.Alignment.Center :
					Xwt.Alignment.End;
			}
		}

		string[] UiTextBox.Lines {
			get { return base.Text.Split (new String [] {Environment.NewLine}, StringSplitOptions.None); }
		}

		// FIXME: this won't work on Xwt. Do not use it.
		bool UiTextBox.HideSelection { get; set; }
	}
}

