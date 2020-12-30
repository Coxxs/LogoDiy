using System;

[AttributeUsage(AttributeTargets.Field)]
public class EnumDisplayAttribute : Attribute
{
	public string Display
	{
		get;
		private set;
	}

	public EnumDisplayAttribute(string displayStr)
	{
		Display = displayStr;
	}
}
