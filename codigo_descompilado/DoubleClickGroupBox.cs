using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class DoubleClickGroupBox : GroupBox
{
	private bool _DoubleClickSelectCheckBox;

	private bool _ClickFocus;

	[CompilerGenerated]
	public bool method_0()
	{
		return _DoubleClickSelectCheckBox;
	}

	[CompilerGenerated]
	public void method_1(bool bool_0)
	{
		_DoubleClickSelectCheckBox = bool_0;
	}

	[CompilerGenerated]
	public bool method_2()
	{
		return _ClickFocus;
	}

	[CompilerGenerated]
	public void method_3(bool bool_0)
	{
		_ClickFocus = bool_0;
	}

	public DoubleClickGroupBox()
	{
		method_1(bool_0: false);
		method_3(bool_0: true);
	}

	protected override void OnClick(EventArgs e)
	{
		if (method_2())
		{
			Focus();
		}
		base.OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		if (method_0() && e is MouseEventArgs e2)
		{
			foreach (object control in base.Controls)
			{
				if (control is CheckBox checkBox && checkBox.Enabled)
				{
					if (e2.Button == MouseButtons.Left)
					{
						checkBox.Checked = true;
					}
					else if (e2.Button == MouseButtons.Right)
					{
						checkBox.Checked = false;
					}
					else
					{
						checkBox.Checked = !checkBox.Checked;
					}
				}
			}
		}
		base.OnDoubleClick(e);
	}
}
