using System.Runtime.CompilerServices;

public class NumericRange
{
	private int _minVal;

	private int _maxVal;

	private int _incVal;

	private decimal _SdVal;

	private int _MaxLen;

	public NumericRange()
	{
	}

	public NumericRange(int int_0, int int_1, int int_2, decimal decimal_0, int int_3)
	{
		SetMin(int_0);
		SetMax(int_1);
		SetIncrement(int_2);
		SetScale(decimal_0);
		SetMaxLength(int_3);
	}

	[CompilerGenerated]
	public int GetMin()
	{
		return _minVal;
	}

	[CompilerGenerated]
	public void SetMin(int int_0)
	{
		_minVal = int_0;
	}

	[CompilerGenerated]
	public int GetMax()
	{
		return _maxVal;
	}

	[CompilerGenerated]
	public void SetMax(int int_0)
	{
		_maxVal = int_0;
	}

	[CompilerGenerated]
	public int GetIncrement()
	{
		return _incVal;
	}

	[CompilerGenerated]
	public void SetIncrement(int int_0)
	{
		_incVal = int_0;
	}

	[CompilerGenerated]
	public decimal GetScale()
	{
		return _SdVal;
	}

	[CompilerGenerated]
	public void SetScale(decimal decimal_0)
	{
		_SdVal = decimal_0;
	}

	[CompilerGenerated]
	public int GetMaxLength()
	{
		return _MaxLen;
	}

	[CompilerGenerated]
	public void SetMaxLength(int int_0)
	{
		_MaxLen = int_0;
	}
}
