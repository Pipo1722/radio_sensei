using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

internal class CustomNumericUpDown : NumericUpDown
{
	private string _003CInputString_003Ek__BackingField;

	private decimal _003CReplaceValue_003Ek__BackingField;

	private string _003CReplaceString_003Ek__BackingField;

	public void method_0(int int_0)
	{
		if (base.Controls[1] is TextBox textBox)
		{
			textBox.MaxLength = int_0;
		}
	}

	[CompilerGenerated]
	public string method_1()
	{
		return _003CInputString_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void method_2(string string_0)
	{
		_003CInputString_003Ek__BackingField = string_0;
	}

	[CompilerGenerated]
	public decimal method_3()
	{
		return _003CReplaceValue_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void method_4(decimal decimal_0)
	{
		_003CReplaceValue_003Ek__BackingField = decimal_0;
	}

	[CompilerGenerated]
	public string method_5()
	{
		return _003CReplaceString_003Ek__BackingField;
	}

	[CompilerGenerated]
	public void method_6(string string_0)
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
		if (!string.IsNullOrEmpty(method_1()) && method_1().IndexOf(e.KeyChar) < 0)
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
		if (method_5() != null && method_5().Length != 0 && base.Value == method_3())
		{
			Text = method_5();
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
