using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class CustomNumericUpDown : NumericUpDown
{
	private string _003CInputString_003Ek__BackingField;

	private decimal _003CReplaceValue_003Ek__BackingField;

	private string _003CReplaceString_003Ek__BackingField;

	public void SetMaxTextLength(int int_0)
	{
		if (base.Controls[1] is TextBox textBox)
		{
			textBox.MaxLength = int_0;
		}
	}

	[CompilerGenerated]
	public string GetInputString()
	{
		return _003CInputString_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void SetInputString(string string_0)
	{
		_003CInputString_003Ek__BackingField = string_0;
	}

	[CompilerGenerated]
	public decimal GetReplaceValue()
	{
		return _003CReplaceValue_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void SetReplaceValue(decimal decimal_0)
	{
		_003CReplaceValue_003Ek__BackingField = decimal_0;
	}

	[CompilerGenerated]
	public string GetReplaceString()
	{
		return _003CReplaceString_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void SetReplaceString(string string_0)
	{
		_003CReplaceString_003Ek__BackingField = string_0;
	}

	public override void UpButton()
	{
		decimal num = base.Value;
		num += base.Increment;
		if (num > base.Maximum)
		{
			num = base.Minimum;
		}
		else if (num % base.Increment != 0m)
		{
			num -= num % base.Increment;
		}
		base.Value = num;
	}

	public override void DownButton()
	{
		decimal num = base.Value;
		num -= base.Increment;
		if (num < base.Minimum)
		{
			num = base.Maximum;
		}
		else if (num % base.Increment != 0m)
		{
			num += base.Increment - num % base.Increment;
		}
		base.Value = num;
	}

	protected override void OnLostFocus(EventArgs e)
	{
		decimal value = base.Value;
		if (value < base.Minimum)
		{
			decimal num = (base.Value = base.Maximum);
			value = num;
		}
		else if (value % base.Increment != 0m)
		{
			decimal num = (base.Value = value - value % base.Increment);
			value = num;
		}
		base.OnLostFocus(e);
	}

	protected virtual void vmethod_0(object sender, EventArgs e)
	{
	}

	protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
	{
		if (!string.IsNullOrEmpty(GetInputString()) && GetInputString().IndexOf(e.KeyChar) < 0)
		{
			e.Handled = true;
		}
		else
		{
			base.OnTextBoxKeyPress(source, e);
		}
	}

	protected override void UpdateEditText()
	{
		if (GetReplaceString() != null && GetReplaceString().Length != 0 && base.Value == GetReplaceValue())
		{
			Text = GetReplaceString();
		}
		else
		{
			base.UpdateEditText();
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (e is HandledMouseEventArgs e2)
		{
			e2.Handled = true;
		}
		if (e.Delta < 0)
		{
			SendKeys.Send("{DOWN}");
		}
		else
		{
			SendKeys.Send("{UP}");
		}
	}
}
