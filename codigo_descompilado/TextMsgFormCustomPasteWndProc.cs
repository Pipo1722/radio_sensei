using System;
using System.Windows.Forms;

internal class TextMsgFormCustomPasteWndProc : NativeWindow
{
	private const int WM_CHAR = 258;

	private const int WM_PASTE = 770;

	protected override void WndProc(ref Message m)
	{
		if (m.Msg == 770)
		{
			method_0();
		}
		else
		{
			base.WndProc(ref m);
		}
	}

	private void method_0()
	{
		string text = Clipboard.GetText();
		foreach (char c in text)
		{
			Message m = new Message
			{
				HWnd = base.Handle,
				Msg = 258,
				WParam = (IntPtr)c,
				LParam = IntPtr.Zero
			};
			base.WndProc(ref m);
		}
	}
}
